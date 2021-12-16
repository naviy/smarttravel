using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Parsers
{



	//===g






	public class AirParser
	{

		//---g



		private AirParser(
			string air,
			AmadeusRizUsingMode rizUsingMode,
			Currency defaultCurrency,
			Money defaultConsolidatorCommission
		)
		{

			_lines = new LinesEnumerator(new StringReader(air));

			_documents = new List<Entity2>();

			_rizUsingMode = rizUsingMode;

			_defaultCurrency = defaultCurrency;

			_defaultConsolidatorCommission = defaultConsolidatorCommission;

		}



		//---g



		public static IList<Entity2> Parse(string air, Currency defaultCurrency)
		{
			return Parse(air, AmadeusRizUsingMode.All, defaultCurrency);
		}



		public static IList<Entity2> Parse(
			string air,
			AmadeusRizUsingMode rizUsingMode,
			Currency defaultCurrency,
			Money defaultConsolidatorCommission = null
		)
		{

			if (air.No())
				throw new GdsImportException("Empty AIR");


			return new AirParser(
				air, 
				rizUsingMode,
				defaultCurrency,
				defaultConsolidatorCommission
			).Parse();

		}



		//---g



		private IList<Entity2> Parse()
		{

			ReadLine("AIR-BLK");

			var airOption = _lines.Current.Split(';')[1];

			switch (airOption)
			{
				case "1A":
				case "5A":
				case "5B":
				case "6A":
				case "7A":
				case "8A":
					ParseTicket();
					break;

				case "IM":
					ParseReservation();
					break;

				case "1M":
				case "5M":
				case "5N":
				case "6M":
				case "8M":
					ParseMco(false);
					break;

				case "7M":
					try
					{
						ParseMco(false);
					}
					catch (Exception ex1)
					{
						try
						{
							ParseMco(true);
						}
						catch
						{
							throw ex1;
						}
					}
					break;

				case "7D":
					ParseMco(true);
					break;

				case "RF":
					ParseRefund();
					break;

				case "MA":
					ParseVoid();
					break;

				default:
					throw CreateAirException("Unknown AIR option");
			}


			return _documents;

		}



		private void ParseTicket()
		{

			var templateTicket = new AviaTicket { Originator = GdsOriginator.Amadeus, Origin = ProductOrigin.AmadeusAir };

			var parser = new LinesSequenceParser(_ticketPattern, _lines, null);

			parser.Parse();


			var airRecordHeader = (string[])parser.Result[0];
			var validatingAirline = (string[])parser.Result[1];
			var servicingCarrier = (string[])parser.Result[2];
			var pnrDate = (string[])parser.Result[3];
			var segmentsGroup = (object[][])parser.Result[4];
			var fare = (string[])parser.Result[5];
			var taxData = (string[])parser.Result[6];
			var sellingFare = (string[])parser.Result[7];
			var sellingTaxData = (string[])parser.Result[8];
			var fareBasisCodes = (string[])parser.Result[9];
			var couponAmounts = (string[])parser.Result[10];
			var ticketFares = (string[])parser.Result[12];
			var passengersGroup = (object[][])parser.Result[13];
			var rizGroup = (object[][])parser.Result[14];


			templateTicket.PnrCode = airRecordHeader[1].Substring(0, 6).TrimOrNull();

			templateTicket.BookerOffice = airRecordHeader[3];
			templateTicket.BookerCode = servicingCarrier[1].Substring(6, 6).TrimOrNull();

			templateTicket.TicketerOffice = airRecordHeader[9];
			templateTicket.TicketingIataOffice = airRecordHeader[10];
			templateTicket.TicketerCode = servicingCarrier[1].Substring(15, 6).TrimOrNull();

			templateTicket.IsTicketerRobot = _robots.Contains(templateTicket.TicketerCode);

			templateTicket.AirlinePnrCode = airRecordHeader[32].TrimOrNull();


			var airline = new Organization
			{
				Name = validatingAirline[1],
				AirlineIataCode = validatingAirline[2].Substring(0, 3).Trim(),
				AirlinePrefixCode = validatingAirline[2].Substring(3, 3)
			};


			templateTicket.IssueDate = Utility.ParseExactDateTime(pnrDate[3], "yyMMdd");


			var segments = ParseSegments(templateTicket, segmentsGroup, fareBasisCodes, couponAmounts, airline);


			ParseTicketFare(templateTicket, servicingCarrier[1][24], fare, taxData, sellingFare, sellingTaxData);


			if (ticketFares != null && ticketFares.Length >= 2)
			{
				var fares = ticketFares[1];
				IataParser.ParseTicketFares(templateTicket, fares);
			}


			if (passengersGroup.No())
				return;


			for (var i = 0; i < passengersGroup.Length; ++i)
			{

				var passengerName = (string[])passengersGroup[i][1];
				var seatNumbers = (string[])passengersGroup[i][2];
				var gdsPassport = (string[])passengersGroup[i][3];
				var ticketNumber = (string[])passengersGroup[i][4];
				var endorsement = (string[])passengersGroup[i][5];
				var fareCommission = (string[])passengersGroup[i][6];
				var originalIssue = (string[])passengersGroup[i][7];
				var formOfPayment = (string[])passengersGroup[i][8];
				var tourCode = (string[])passengersGroup[i][9];
				var remarks = (object[])passengersGroup[i][10];


				var ticket = i == passengersGroup.Length - 1 
					? templateTicket 
					: (AviaTicket)templateTicket.Clone()
				;


				ticket.PassengerName = passengerName[2].Substring(2);


				if (gdsPassport != null && gdsPassport.Length > 1)
				{
					ticket.GdsPassport = gdsPassport[1].Substring(4);
				}


				ticket.AirlinePrefixCode = ticketNumber[1].Substring(1, 3);


				var numbers = ticketNumber[1].Substring(5).Split(new[] { '-' }, 2);
				
				ticket.Number = numbers[0].Clip();

				if (numbers.Length > 1)
				{
					ticket.ConjunctionNumbers = numbers[1];
				}


				if (ticket.AirlinePrefixCode == airline.AirlinePrefixCode)
				{
					ticket.AirlineName = airline.Name;
					ticket.AirlineIataCode = airline.AirlineIataCode;
					ticket.Producer = airline;
				}


				ParseSeats(ticket, seatNumbers, segments);


				ticket.PaymentForm = formOfPayment[1];

				ticket.PaymentType = ParsePaymentType(formOfPayment[1]);


				if (remarks != null)
					ticket.Remarks = ParseRemarks(remarks);


				ProcessRizData(ticket, rizGroup, i);


				if (tourCode != null)
					ticket.TourCode = tourCode[1];


				if (originalIssue != null)
				{
					ticket.ReissueFor = new AviaTicket
					{
						AirlinePrefixCode = originalIssue[1].Substring(0, 3),
						Number = originalIssue[1].Substring(4, 10).Clip()
					};
				}


				ProcessCommission(ticket, fareCommission);


				if (endorsement != null)
					ticket.Endorsement = endorsement[1].Trim('*');


				_documents.Add(ticket);

			}

		}



		private void ParseReservation()
		{

			var templateTicket = new AviaTicket { Originator = GdsOriginator.Amadeus, Origin = ProductOrigin.AmadeusAir };

			var parser = new LinesSequenceParser(_reservationPattern, _lines, null);

			parser.Parse();

			var airRecordHeader = (string[])parser.Result[0];
			var servicingCarrier = (string[])parser.Result[1];
			var pnrDate = (string[])parser.Result[2];
			var segmentsGroup = (object[][])parser.Result[3];
			var passengersGroup = (object[][])parser.Result[4];
			var rizGroup = (object[][])parser.Result[5];

			templateTicket.PnrCode = airRecordHeader[1].Substring(0, 6).TrimOrNull();

			templateTicket.BookerOffice = airRecordHeader[3];
			templateTicket.BookerCode = servicingCarrier[1].Substring(6, 6).TrimOrNull();

			templateTicket.TicketerOffice = airRecordHeader[9];
			templateTicket.TicketingIataOffice = airRecordHeader[10];
			templateTicket.TicketerCode = servicingCarrier[1].Substring(15, 6).TrimOrNull();

			templateTicket.AirlinePnrCode = airRecordHeader[32].TrimOrNull();

			templateTicket.IssueDate = Utility.ParseExactDateTime(pnrDate[3], "yyMMdd");

			var segments = ParseSegments(templateTicket, segmentsGroup, null, null, null);


			for (var i = 0; i < passengersGroup.Length; ++i)
			{

				var passengerName = (string[])passengersGroup[i][1];

				var fare = (string[])passengersGroup[i][2];
				var taxData = (string[])passengersGroup[i][3];
				var sellingFare = (string[])passengersGroup[i][4];
				var sellingTaxData = (string[])passengersGroup[i][5];
				//var fareBasisCodes = (string[])passengersGroup[i][6];

				var seatNumbers = (string[])passengersGroup[i][7];
				var ticketNumbers = (string[])passengersGroup[i][8];
				var endorsement = (string[])passengersGroup[i][9];
				var fareCommission = (string[])passengersGroup[i][10];
				var originalIssue = (string[])passengersGroup[i][11];
				var formOfPayment = (string[])passengersGroup[i][12];
				var tourCode = (string[])passengersGroup[i][13];
				var carrier = (string[])passengersGroup[i][14];
				var remarks = (object[])passengersGroup[i][15];


				var ticket = i == passengersGroup.Length - 1 ? templateTicket : (AviaTicket)templateTicket.Clone();

				ticket.PassengerName = passengerName[2].Substring(2);


				if (carrier != null)
					ticket.AirlineIataCode = carrier[1].Substring(carrier[1].Length - 2);


				var pricingCode = servicingCarrier[1].Split('-')[2];


				ParseTicketFare(ticket, pricingCode.No() ? (char?)null : pricingCode[0], fare, taxData, sellingFare, sellingTaxData);


				ParseSeats(ticket, seatNumbers, segments);


				if (ticketNumbers != null)
				{

					var number = ticketNumbers[1].Split('-');

					ticket.AirlinePrefixCode = number[0];
					ticket.Number = number[1].Clip();
					ticket.IsManual = true;


					foreach (var segment in ticket.Segments)
					{
						if (segment.Type == FlightSegmentType.Unticketed)
							segment.Type = FlightSegmentType.Ticketed;
					}

				}


				if (formOfPayment != null)
				{
					ticket.PaymentForm = formOfPayment[1];
					ticket.PaymentType = ParsePaymentType(formOfPayment[1]);
				}


				ProcessRizData(ticket, rizGroup, i);


				if (tourCode != null)
					ticket.TourCode = tourCode[1];


				if (remarks != null)
					ticket.Remarks = ParseRemarks(remarks);


				if (originalIssue != null)
				{
					ticket.ReissueFor = new AviaTicket
					{
						AirlinePrefixCode = originalIssue[1].Substring(0, 3),
						Number = originalIssue[1].Substring(4, 10).Clip()
					};
				}


				ProcessCommission(ticket, fareCommission);


				if (endorsement != null)
					ticket.Endorsement = endorsement[1].Trim('*');


				_documents.Add(ticket);

			}

		}



		private static List<FlightSegment> ParseSegments(
			AviaTicket templateTicket,
			IEnumerable<object[]> segmentsGroup,
			IList<string> fareBasisCodes,
			IList<string> couponAmounts,
			Organization airline
		)
		{

			var segments = new List<FlightSegment>();

			if (segmentsGroup == null)
				return segments;


			var ticketedSegments = new List<FlightSegment>();

			string couponCurrency = null;


			if (couponAmounts != null && couponAmounts.Count > 1)
			{
				var couponAmount1 = couponAmounts[1];
				if (couponAmount1.Yes() && char.IsLetter(couponAmount1[0]))
				{
					couponCurrency = couponAmount1.AsSubstring(0, 3);
					couponAmounts[1] = couponAmount1.AsSubstring(3);
				}
				else
					couponCurrency = "USD";

				//if (couponCurrency == "NUC")
				//	couponCurrency = "USD";
			}


			foreach (var t in segmentsGroup)
			{

				var segmentLine = (string[])t[0];


				if (segmentLine[2].Length < AviaSegmentNumberPartMinLength || 
					segmentLine[2].Length > AviaSegmentNumberPartMaxLength
				)
				{
					continue;
				}


				var segment = new FlightSegment();

				segments.Add(segment);


				segment.Position = int.Parse(segmentLine[2].Substring(0, 3));

				segment.Stopover = segmentLine[2][3] == 'O';


				segment.FromAirportCode = segmentLine[2].Substring(4, 3);
				segment.FromAirportName = segmentLine[3].Trim();

				segment.FromAirport = new Airport
				{
					Code = segment.FromAirportCode,
					Name = segment.FromAirportName
				};


				segment.ToAirportCode = segmentLine[4];
				segment.ToAirportName = segmentLine[5].Trim();

				segment.ToAirport = new Airport
				{
					Code = segment.ToAirportCode,
					Name = segment.ToAirportName
				};


				if (segment.Position == VoidSegmentNumber)
					continue;


				if (segmentLine[0] == "H-")
				{

					ticketedSegments.Add(segment);

					segment.Type = FlightSegmentType.Ticketed;

					//if (fareBasisCodes != null)
					//	segment.FareBasis = fareBasisCodes[ticketedSegments.Count].Trim();
					segment.FareBasis = fareBasisCodes.By(ticketedSegments.Count).Clip();

					if (couponCurrency.Yes())
					{
						var couponAmount = couponAmounts.By(segments.Count);
						if (couponAmount.Yes())
						{
							var amount = couponAmount.As().Decimal;
							if (amount != 0)
								segment.CouponAmount = new Money(couponCurrency, amount);
						}
					}
				}
				else
				{
					segment.Type = FlightSegmentType.Unticketed;
				}


				var segmentPartsLine = segmentLine[6];

				var segmentParts =
					segmentPartsLine.Length == 43 ? segmentPartsLine.Split(6, 5, 2, 2, 5, 8, 5, 6) :
					segmentPartsLine.Split(6, 5, 2, 2, 5, 5, 5, 5)
				;


				segment.CarrierIataCode = segmentParts[0];

				if (airline != null && segment.CarrierIataCode == airline.AirlineIataCode)
					segment.Carrier = airline;


				segment.FlightNumber = segmentParts[1];

				segment.ServiceClassCode = segmentParts[3].Trim();


				if (segment.FlightNumber == OpenSegment)
				{

					if (segmentLine[7].Length != 0)
					{
						segment.DepartureTime = Utility.ParseSegmentDateTime(segmentLine[7] + "0000", templateTicket.IssueDate);
					}


					segment.Luggage = segmentLine.TrimOrNull(13);


					templateTicket.AddSegment(segment);

				}

				else
				{

					segment.DepartureTime = Utility.ParseSegmentDateTime(segmentParts[4] + segmentParts[5], templateTicket.IssueDate);

					segment.ArrivalTime = Utility.ParseSegmentDateTime(segmentParts[7] + segmentParts[6], templateTicket.IssueDate);

					segment.MealCodes = segmentLine[9].Trim();

					segment.NumberOfStops = int.Parse(segmentLine[10]);

					segment.Luggage = segmentLine[14].TrimOrNull();

					segment.CheckInTerminal = segmentLine[15].TrimOrNull();


					if (segmentLine[16].Length != 0)
					{
						segment.CheckInTime = segmentLine[16].Substring(0, 2) + ":" + segmentLine[16].Substring(2, 2);
					}

					if (segmentLine[18].Length != 0)
					{
						segment.Duration = segmentLine[18].Substring(0, 2) + ":" + segmentLine[18].Substring(2, 2);
					}


					segment.ArrivalTerminal = segmentLine.TrimOrNull(segment.Type == FlightSegmentType.Ticketed ? 23 : 25);

				}


				int j;


				for (j = 0; j < templateTicket.Segments.Count; ++j)
				{
					if (segment.Position < templateTicket.Segments[j].Position)
						break;
				}


				if (j < templateTicket.Segments.Count)
				{
					templateTicket.InsertSegment(j, segment);
				}
				else
				{
					templateTicket.AddSegment(segment);
				}

			}


			for (var i = 0; i < templateTicket.Segments.Count; ++i)
			{

				templateTicket.Segments[i].Position = i;


				if (!templateTicket.Segments[i].Stopover && i > 0)
				{
					templateTicket.Segments[i - 1].Stopover = false;
					templateTicket.Segments[i].Stopover = true;
				}

			}


			return segments;

		}



		private void ParseTicketFare(AviaTicket ticket, char? pricingCode, string[] fare, string[] taxData, string[] sellingFare, string[] sellingTaxData)
		{

			string[] fareToUse, taxDataToUse;

			if (fare[1].Yes() && pricingCode != 'B' && pricingCode != 'F')
			{
				fareToUse = fare;
				taxDataToUse = taxData;
			}
			else if (sellingFare != null)
			{
				fareToUse = sellingFare;
				taxDataToUse = sellingTaxData;
			}
			else
			{
				return;
			}


			ticket.Fare = ParseMoney(fareToUse[1].Substring(1));
			ticket.EqualFare = ParseMoney(fareToUse[2]);
			ticket.Total = ParseMoney(fareToUse[13]) + ticket.ConsolidatorCommission;


			if (taxDataToUse != null)
			{

				ticket.FeesTotal = new Money(ticket.Total.Currency, 0);

				for (var index = 2; index < taxDataToUse.Length; ++index)
				{
					ParseAndAddFee(ticket, taxDataToUse[index]);
				}

			}


			if (ticket.EqualFare == null && ticket.Total != null)
				ticket.EqualFare = ticket.Total - ticket.FeesTotal;


			ticket.ConsolidatorCommission = _defaultConsolidatorCommission;
			
			ticket.Total += ticket.ConsolidatorCommission;


		}



		private static void ParseSeats(AviaTicket ticket, string[] seatNumbers, List<FlightSegment> segments)
		{

			if (seatNumbers == null)
				return;


			for (var j = 1; j < seatNumbers.Length; ++j)
			{

				var seat = seatNumbers[j].Split('/')[1].Split('.')[0];

				if (seat.No())
					continue;

				var previous = segments[j - 1];

				var segment = ticket.Segments.Find(seg => seg.Position == previous.Position);

				segment.Seat = seat.Substring(1).Split('+')[0];

			}

		}



		public static PaymentType ParsePaymentType(string str)
		{

			if (str.StartsWith("O/"))
				str = str.Substring(str.LastIndexOf('/') + 1);

			if (str == "CASH")
				return PaymentType.Cash;

			if (str == "INVOICE" || str == "INV")
				return PaymentType.Invoice;

			if (str == "CHECK")
				return PaymentType.Check;


			return PaymentType.Unknown;

		}



		private void ParseMco(bool emd)
		{

			var templateMco = new AviaMco
			{
				Originator = GdsOriginator.Amadeus,
				Origin = ProductOrigin.AmadeusAir
			};


			var parser = new LinesSequenceParser(_mcoPattern, _lines, null);

			parser.Parse();


			var airRecordHeader = (string[])parser.Result[0];
			var servicingCarrier = (string[])parser.Result[1];
			var pnrDate = (string[])parser.Result[2];
			var segmentsGroup = (object[][])parser.Result[3];
			var passengersGroup = (object[][])parser.Result[4];


			templateMco.PnrCode = airRecordHeader[1].Substring(0, 6).TrimOrNull();

			templateMco.BookerOffice = airRecordHeader[3];
			templateMco.BookerCode = servicingCarrier[1].Substring(6, 6).TrimOrNull();

			templateMco.TicketerOffice = airRecordHeader[9];
			templateMco.TicketingIataOffice = airRecordHeader[10];
			templateMco.TicketerCode = servicingCarrier[1].Substring(15, 6).TrimOrNull();

			templateMco.AirlinePnrCode = airRecordHeader[32].TrimOrNull();

			templateMco.IssueDate = Utility.ParseExactDateTime(pnrDate[3], "yyMMdd");

			var mcoMap = new Dictionary<int, AviaMco>();


			for (var i = 0; i < segmentsGroup.Length; ++i)
			{

				var segmentLine = (string[])segmentsGroup[i][0];

				var connectedWith = (string[])segmentsGroup[i][1];


				var lineNumber = emd 
					? int.Parse(segmentLine[6].Substring(1)) 
					: int.Parse(segmentLine[2].Substring(0, 3))
				;


				var mco = i == segmentsGroup.Length - 1 
					? templateMco 
					: (AviaMco)templateMco.Clone()
				;


				mco.AirlineIataCode = segmentLine[2].Substring(3).TrimOrNull();

				mco.AirlinePrefixCode = segmentLine[3].Substring(0, 3);

				mco.AirlineName = segmentLine[4].TrimOrNull();


				mco.Producer = new Organization
				{
					AirlineIataCode = mco.AirlineIataCode,
					AirlinePrefixCode = mco.AirlinePrefixCode,
					Name = mco.AirlineName
				};


				if (emd)
				{
					mco.Fare = ParseMoney(segmentLine[29]);

					if (segmentLine[31].Length != 0)
						mco.EqualFare = ParseMoney(segmentLine[31]);

					mco.Total = ParseMoney(segmentLine[133]);

					mco.FeesTotal = new Money(mco.Total.Currency, 0);

					for (var j = 33; j < 133; ++j)
						ParseAndAddFee(mco, segmentLine[j]);

					//mco.ServiceFee = ParseMoney(segmentLine[]);

					if (connectedWith != null)
					{
						mco.InConnectionWith = new AviaTicket
						{
							AirlinePrefixCode = connectedWith[1].Substring(0, 3),
							Number = connectedWith[1].Substring(3, 10).Clip()
						};
					}
				}

				else
				{

					mco.Fare = ParseMoney(segmentLine[15]);

					if (segmentLine[17].Length != 0)
						mco.EqualFare = ParseMoney(segmentLine[17]);

					mco.Total = ParseMoney(segmentLine[49]);

					mco.FeesTotal = mco.Total != null ? new Money(mco.Total.Currency, 0) : null;

					for (var j = 18; j < 48; ++j)
						ParseAndAddFee(mco, segmentLine[j]);

					mco.ServiceFee = ParseMoney(segmentLine[58]);

				}


				mcoMap[lineNumber] = mco;

			}


			foreach (var psgGroup in passengersGroup)
			{

				var mcoNumbers = (object[][])psgGroup[2];

				if (mcoNumbers == null)
					continue;


				var passengerName = (string[])psgGroup[1];
				var fareCommissions = (object[][])psgGroup[3];
				var originalIssues = (object[][])psgGroup[4];
				var formOfPayments = (object[][])psgGroup[5];
				var tourCodes = (object[][])psgGroup[6];

				var passenger = passengerName[2].Substring(2);


				foreach (var mcoNumber_ in mcoNumbers)
				{

					var mcoNumber = (string[])mcoNumber_[0];

					var lineNumber = mcoNumber[3].AsSubstring(1).As().Int;

					var mco = mcoMap.By(lineNumber);

					if (mco == null) 
						continue;


					mco.Number = mcoNumber[1].Substring(5).Clip();
					mco.PassengerName = passenger;


					if (mcoNumber[2].Length != 0)
					{
						mco.InConnectionWith = new AviaTicket
						{
							AirlinePrefixCode = mcoNumber[2].Substring(0, 3),
							Number = mcoNumber[2].Substring(4, 10).Clip()
						};
					}


					_documents.Add(mco);

				}


				if (formOfPayments != null)
				{

					foreach (var formOfPayment_ in formOfPayments)
					{

						var formOfPayment = (string[])formOfPayment_[0];

						var lineNumber = formOfPayment[emd ? 4 : 2].AsSubstring(1).As().Int;

						mcoMap.By(lineNumber).Do(mco =>
						{
							mco.PaymentForm = formOfPayment[1];
							mco.PaymentType = ParsePaymentType(formOfPayment[1]);
						});

					}

				}


				if (fareCommissions != null)
				{

					foreach (var fareCommission_ in fareCommissions)
					{

						var fareCommission = (string[])fareCommission_[0];

						if (fareCommission[2].Length <= 0) 
							continue;


						var lineNumber = fareCommission[2].AsSubstring(1).As().Int;

						mcoMap.By(lineNumber).Do(mco => ProcessCommission(mco, fareCommission));

					}

				}


				if (tourCodes != null)
				{

					foreach (var tourCode_ in tourCodes)
					{

						var tourCode = (string[])tourCode_[0];

						var lineNumber = tourCode[2].AsSubstring(1).As().Int;

						mcoMap.By(lineNumber).Do(mco =>
						{
							mco.TourCode = tourCode[1];
						});

					}

				}


				if (originalIssues != null)
				{

					foreach (var originalIssue_ in originalIssues)
					{

						var originalIssue = (string[])originalIssue_[0];

						var lineNumber = originalIssue[2].AsSubstring(1).As().Int;

						mcoMap.By(lineNumber).Do(mco =>
						{
							mco.ReissueFor = new AviaMco
							{
								AirlinePrefixCode = originalIssue[1].AsSubstring(0, 3),
								Number = originalIssue[1].AsSubstring(4, 10).Clip()
							};
						});

					}

				}

			}

		}



		private void ParseRefund()
		{

			var r = new AviaRefund
			{
				Originator = GdsOriginator.Amadeus,
				Origin = ProductOrigin.AmadeusAir
			};

			var parser = new LinesSequenceParser(_refundPattern, _lines, null);

			parser.Parse();

			var airRecordHeader = (string[])parser.Result[0];
			var servicingCarrier = (string[])parser.Result[1];
			var pnrDate = (string[])parser.Result[2];
			var refundNotice = (string[])parser.Result[3];
			var refundableTaxes = (string[])parser.Result[4];
			var passengersGroup = (object[][])parser.Result[5];

			r.PnrCode = airRecordHeader[1].Substring(0, 6).TrimOrNull();

			r.BookerOffice = airRecordHeader[3];
			r.BookerCode = servicingCarrier[1].Substring(6, 6).TrimOrNull();

			r.TicketerOffice = airRecordHeader[9];
			r.TicketingIataOffice = airRecordHeader[10];
			r.TicketerCode = servicingCarrier[1].Substring(15, 6).TrimOrNull();

			r.AirlinePnrCode = airRecordHeader[32].TrimOrNull();

			r.IssueDate = Utility.ParseExactDateTime(pnrDate[3], "yyMMdd");


			if (refundNotice != null)
			{

				var currency = new Currency(refundNotice[4].Substring(0, 3));

				var fare = refundNotice[6];
				r.Fare = new Money(currency, fare == "" ? 0 : Utility.ParseDecimal(fare));

				var cancelFee = refundNotice[9];
				r.CancelFee = new Money(currency, cancelFee == "" ? 0 : Utility.ParseDecimal(cancelFee));

				var cancelCommission = refundNotice[10];
				r.CancelCommission = new Money(currency, cancelCommission == "" ? 0 : Utility.ParseDecimal(cancelCommission));

				var feesTotal = refundNotice[12];
				r.FeesTotal = new Money(currency, feesTotal == "" ? 0 : Utility.ParseDecimal(feesTotal.Substring(2)));
			

				var total = refundNotice[13];

				if (total.Length == 0)
					r.Total = r.Fare - r.CancelFee.Amount + r.FeesTotal.Amount;
				else
					r.Total = new Money(currency, Utility.ParseDecimal(total));

			}


			if (refundableTaxes != null)
			{

				for (var i = 2; i < refundableTaxes.Length; ++i)
				{
					if (refundableTaxes[i].Length == 0)
						continue;

					var tax = refundableTaxes[i].Split(true, 1, 3, 9, 3, 2);

					if (tax[0] == "H")
						continue;

					r.AddFee(
						new AviaDocumentFee
						{
							Code = tax[3],
							Amount = new Money(tax[1], Utility.ParseDecimal(tax[2]))
						},
						false);
				}

			}


			for (var i = 0; i < passengersGroup.Length; ++i)
			{

				var passengerName = (string[])passengersGroup[i][1];
				var refundNumber = (string[])passengersGroup[i][2];
				var fareCommission = (string[])passengersGroup[i][3];
				//var originalIssue = (string[]) passengersGroup[i][4];
				var formOfPayment = (string[])passengersGroup[i][5];
				var tourCode = (string[])passengersGroup[i][6];
				var remarks = (object[])passengersGroup[i][7];

				var refund = i == passengersGroup.Length - 1 ? r : (AviaRefund)r.Clone();

				refund.PassengerName = passengerName[2].Substring(2);

				refund.AirlinePrefixCode = refundNumber[1].Substring(0, 3);
				refund.Number = refundNumber[1].Substring(4, 10).Clip();

				ProcessRefundCommission(refund, fareCommission);

				if (formOfPayment != null)
				{
					refund.PaymentForm = formOfPayment[1];
					refund.PaymentType = ParsePaymentType(formOfPayment[1].Split('/')[0]);
				}

				if (tourCode != null)
					refund.TourCode = tourCode[1];

				if (remarks != null)
					refund.Remarks = ParseRemarks(remarks);

				_documents.Add(refund);

			}

		}



		private void ParseVoid()
		{

			var parser = new LinesSequenceParser(_voidPattern, _lines, null);

			parser.Parse();

			var amdHeader = (string[])parser.Result[0];
			var airRecordHeader = (string[])parser.Result[1];
			var passengersGroup = (object[][])parser.Result[2];

			var type = amdHeader[3].Substring(0, 4);

			if (type != "VOID" && type != "RSTD" && type != "TRFX")
				throw new GdsImportException($"Unknown voiding type '{type}'");

			var date = Utility.ParseExactDateTime(amdHeader[3].Substring(4), "ddMMM");

			foreach (var passengersGroup_ in passengersGroup)
			{
				var ticketNumber = (string[])passengersGroup_[2];
				var refundNumber = (string[])passengersGroup_[3];
				var mcoNumber = (string[])passengersGroup_[4];

				var voiding = new AviaDocumentVoiding
				{
					Origin = ProductOrigin.AmadeusAir,
					Originator = GdsOriginator.Amadeus
				};

				if (mcoNumber != null)
				{
					voiding.Document = new AviaMco
					{
						AirlinePrefixCode = mcoNumber[1].Substring(1, 3),
						Number = mcoNumber[1].Substring(5, 10).Clip()
					};
				}
				else if (refundNumber != null)
				{
					voiding.Document = new AviaRefund
					{
						AirlinePrefixCode = refundNumber[1].Substring(0, 3),
						Number = refundNumber[1].Substring(4, 10).Clip()
					};
				}
				else
				{
					voiding.Document = new AviaTicket
					{
						AirlinePrefixCode = ticketNumber[1].Substring(1, 3),
						Number = ticketNumber[1].Substring(5, 10).Clip()
					};
				}

				voiding.IsVoid = type != "RSTD";

				voiding.TimeStamp = date;

				voiding.AgentOffice = airRecordHeader[9];
				voiding.IataOffice = airRecordHeader[10];
				voiding.AgentCode = amdHeader[4].Substring(0, 2);

				_documents.Add(voiding);
			}
		}



		private static void ParseAndAddFee(AviaDocument document, string tax)
		{

			if (tax.Length == 0 || tax[0] == OldTaxIndicator)
				return;


			document.AddFee(new AviaDocumentFee
			{
				Code = tax.AsSubstring(13, 3)?.Trim(),
				Amount = ParseMoney(tax.AsSubstring(1, 12))
			});

		}



		private void ProcessCommission(AviaDocument document, string[] commissionLine)
		{

			if (commissionLine == null)
				return;


			var valueParts = commissionLine[1].Split('*');

			var value = valueParts[valueParts.Length - 1].TrimEnd('N');

			var documentCurrency = document.Total == null ? _defaultCurrency : document.Total.Currency;


			if (value.Length == 0)
			{
				document.Commission = new Money(documentCurrency, 0);
			}

			else if (value.EndsWith("A"))
			{
				document.Commission = new Money(documentCurrency, Utility.ParseDecimal(value.TrimEnd('A')));
			}

			else
			{

				document.CommissionPercent = Utility.ParseDecimal(value);


				if ((document.IsAviaTicket || document.IsAviaMco) && document.ReissueFor != null)
				{
					document.Commission = ((document.Total - document.FeesTotal) * document.CommissionPercent.Value) / 100;
				}
				else
				{
					document.Commission = ((document.EqualFare ?? document.Fare) * document.CommissionPercent.Value) / 100;
				}

			}

		}



		private static void ProcessRefundCommission(AviaDocument document, string[] commissionLine)
		{

			if (commissionLine == null)
				return;


			var valueParts = commissionLine[1].Split('/');


			if (valueParts[0].EndsWith("A"))
			{
				var valueAmount = valueParts[0].TrimEnd('A');
				if (valueAmount.Length != 0)
					document.Commission = new Money(document.Fare.Currency, Utility.ParseDecimal(valueAmount));
			}

			else if (valueParts[0].EndsWith("P") || valueParts[0].EndsWith("N"))
			{
				var valuePercent = valueParts[0].TrimEnd('P', 'N');
				if (valuePercent.Length != 0)
					document.CommissionPercent = Utility.ParseDecimal(valuePercent);
			}


			if (valueParts.Length > 1)
			{
				document.Commission = new Money(document.Fare.Currency, Utility.ParseDecimal(valueParts[1].TrimEnd('A')));
			}
			else if (document.CommissionPercent.HasValue)
			{
				document.Commission = (document.Fare * document.CommissionPercent.Value) / 100;
			}

		}



		private void ProcessRizData(AviaDocument document, object[][] rizGroup, int i)
		{

			if (_rizUsingMode == AmadeusRizUsingMode.None || rizGroup == null)
				return;


			if (i >= rizGroup.Length)
				i = rizGroup.Length - 1;


			var rizFare = (string[])rizGroup[i][0];
			var rizTax = (string[])rizGroup[i][1];
			var rizTotal = (string[])rizGroup[i][2];
			var rizServiceFee = (string[])rizGroup[i][3];
			var rizGrandTotal = (string[])rizGroup[i][4];


			if (rizServiceFee != null)
			{
				var parts = rizServiceFee[1].Split('+', '/');

				document.ServiceFee = ParseRizMoney(parts[0]);

				if (document.ServiceFee != null && parts.Length > 1)
				{
					var vat = Utility.ParseDecimal(parts[1].Substring(4));

					document.ServiceFee += vat;
				}
			}


			if (_rizUsingMode == AmadeusRizUsingMode.All)
			{

				if (rizFare != null)
					document.EqualFare = ParseRizMoney(rizFare[1]);

				if (rizTax != null)
					document.FeesTotal = ParseRizMoney(rizTax[1]);

				if (rizTotal != null)
					document.Total = ParseRizMoney(rizTotal[1]);

				if (rizGrandTotal != null)
					document.GrandTotal = ParseRizMoney(rizGrandTotal[1]);

			}

		}



		private static string ParseRemarks(IEnumerable<object> remarks)
		{

			var builder = new StringBuilder();
			var separator = string.Empty;


			foreach (object[] remark in remarks)
			{
				builder
					.Append(separator)
					.Append(((string[])remark[0])[1]);

				separator = Environment.NewLine;
			}


			return builder.ToString();

		}



		private static Money ParseMoney(string value)
		{

			if (value.No()) 
				return null;


			return Utility.TryParseDecimal(value.AsSubstring(3), out var amount)
				? new Money(value.Substring(0, 3), amount)
				: null
			;

		}



		private static Money ParseRizMoney(string value)
		{

			var parts = value.Split(' ', '/');

			if (parts.Length == 1)
				return null;


			if (!_rizCurrencyMisprints.TryGetValue(parts[0], out var currency))
				currency = parts[0];


			if (Utility.TryParseDecimal(parts[1], out var amount))
				return new Money(currency, amount);


			return null;

		}



		private void ReadLine(string type)
		{

			if (!_lines.MoveNext())
				throw new GdsImportException("Unexpected end of AIR");


			if (!_lines.Current.StartsWith(type))
				throw CreateAirException(type + " missing");

		}



		private GdsImportException CreateAirException(string message)
		{
			return new GdsImportException(_lines.Number, message);
		}



		private static readonly object[] _ticketPattern = 
		{
			"MUC1A ",
			"A-",
			"C-",
			"D-",
			new object[]
			{
				"H-|U-"
			},
			"K-",
			"?KFT",
			"?KS-",
			"?KST",
			"M-",
			"?N-",
			"O-",
			"?Q-",
			new object[]
			{
				"?I-GRP",
				"I-",
				"?S-",
				"?SSR DOCS ",
				"T-",
				"?FE",
				"?FM",
				"?FO",
				"FP",
				"?FT",
				new object[]
				{
					"?RM*"
				}
			},
			new object[]
			{
				"RIZTICKET FARE ",
				"?RIZTICKET TAX ",
				"?RIZTICKET TTL ",
				"?RIZSERVICE FEE ",
				"?RIZGRAND TOTAL "
			}
		};



		private static readonly object[] _reservationPattern = 
		{
			"MUC1A ",
			"C-",
			"D-",
			new object[]
			{
				"H-|U-"
			},
			new object[]
			{
				"?I-GRP",
				"I-",
				"K-",
				"?KFT",
				"?KS-",
				"?KST",
				"M-",
				"?S-",
				"?FH",
				"?FE",
				"?FM",
				"?FO",
				"?FP",
				"?FT",
				"?FV",
				new object[] { "?RM*" }
			},
			new object[]
			{
				"RIZTICKET FARE ",
				"?RIZTICKET TAX ",
				"?RIZTICKET TTL ",
				"?RIZSERVICE FEE ",
				"?RIZGRAND TOTAL "
			}
		};

		private static readonly object[] _mcoPattern = 
		{
			"MUC1A ",
			"?C-",
			"D-",
			new object[]
			{
				"MCO|EMD",
				"?ICW"
			},
			new object[]
			{
				"?I-GRP",
				"I-",
				new object[] { "TMC" },
				new object[] { "MFM" },
				new object[] { "MFO" },
				new object[] { "MFP" },
				new object[] { "MFT" }
			}
		};



		private static readonly object[] _refundPattern = 
		{
			"MUC1A ",
			"?C-",
			"D-",
			"?RFD",
			"?KRF",
			new object[]
			{
				"?I-GRP",
				"I-",
				"R-",
				"?FM",
				"?FO",
				"?FP",
				"?FT",
				new object[] { "?RM*" }
			}
		};



		private static readonly object[] _voidPattern = 
		{
			"AMD",
			"MUC1A ",
			new object[]
			{
				"?I-GRP",
				"I-",
				"?T-",
				"?R-",
				"?TMC"
			}
		};



		private const int VoidSegmentNumber = 0;
		private const int AviaSegmentNumberPartMinLength = 7;
		private const int AviaSegmentNumberPartMaxLength = 9;
		private const string OpenSegment = "OPEN";
		private const char OldTaxIndicator = 'O';


		private static readonly Dictionary<string, string> _rizCurrencyMisprints = new Dictionary<string, string>
		{
			{ "UA", "UAH" },
			{ "UH", "UAH" },
			{ "USH", "UAH" },
			{ "UAJ", "UAH" }
		};


		private static readonly string[] _robots = { "9997WS", "9998WS", "9999WS", "2017SS" };

		private readonly LinesEnumerator _lines;

		private readonly List<Entity2> _documents;

		private readonly AmadeusRizUsingMode _rizUsingMode;
		private readonly Currency _defaultCurrency;
		private readonly Money _defaultConsolidatorCommission;



		//---g

	}






	//===g



}