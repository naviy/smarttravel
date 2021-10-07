using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{

	public class MirParser
	{
		private MirParser(string mir, Currency defaultCurrency)
		{
			_lines = new LinesEnumerator(new StringReader(mir));

			_documents = new List<Entity2>();

			_defaultCurrency = defaultCurrency;
		}

		public static IList<Entity2> Parse(string mir, Currency defaultCurrency)
		{
			if (mir.No())
				throw new GdsImportException("Empty MIR");

			if (!mir.StartsWith(BasicId + GscId))
				throw new GdsImportException("Unsupported format");

			return new MirParser(mir, defaultCurrency).Parse();
		}

		private IList<Entity2> Parse()
		{
			_header = new[]
			{
				_lines.ReadLine(),
				_lines.ReadLine(),
				_lines.ReadLine(),
				_lines.ReadLine(),
				_lines.ReadLine()
			};

			var command = _header[3].Substring(11, 1);

			if (command == TicketingCommandSpoiled)
				return _documents;

			if (command == TicketingCommandCancelledRefund)
				return _documents;

			PrepareSections();

			if (command == TicketingCommandRegular || command == TicketingCommandAvoided || command == TicketingCommandEmd)
				ParseTicketsOrMcos(command == TicketingCommandEmd);
			else if (command == TicketingCommandRefund)
				ParseRefund();
			else if (command == TicketingCommandVoid)
				ParseVoid(true);
			else if (command == TicketingCommandUnvoid)
				ParseVoid(false);
			else
				throw new GdsImportException($"Unsupported ticketing command: '{command}'");

			return _documents;
		}

		private void PrepareSections()
		{
			_sections = new Dictionary<string, List<List<string>>>();
			_fares = new Dictionary<string, List<string>>();
			_basises = new Dictionary<string, List<string>>();
			_reissues = new Dictionary<string, List<string>>();
			_seats = new Dictionary<string, List<string>>();
			_passports = new Dictionary<int, string>();
			_coupons = new Dictionary<int, List<string>>();

			List<List<string>> section = null;
			List<string> items = null;

			while (_lines.MoveNext())
			{
				if (string.IsNullOrWhiteSpace(_lines.Current))
				{
					section?.Add(items);

					section = null;
					items = null;
				}
				else if (_headerRegex.IsMatch(_lines.Current))
				{
					if (section == null)
					{
						section = new List<List<string>>();

						var sectionId = _lines.Current.Substring(0, SectionIdLength);

						_sections[sectionId] = section;
					}

					if (items != null)
						section.Add(items);

					items = new List<string> { _lines.Current };

					if (_lines.Current.StartsWith("A07"))
						_fares[_lines.Current.Substring(3, 2)] = items;
					else if (_lines.Current.StartsWith("A08"))
						_basises[_lines.Current.Substring(3, 4)] = items;
					else if (_lines.Current.StartsWith("A10"))
						_reissues[_lines.Current.Substring(3, 2)] = items;
					else if (_lines.Current.StartsWith("A22"))
						_seats[_lines.Current.Substring(3, 4)] = items;
					else if (_lines.Current.StartsWith("A30"))
					{
						var passengerNumber = int.Parse(_lines.Current.Substring(3, 2));
						var coupons = _coupons.By(passengerNumber);
						if (coupons == null)
							_coupons[passengerNumber] = coupons = new List<string>();
						coupons.Add(items[0]);
					}
				}
				else
				{
					items?.Add(_lines.Current);
				}
			}
		}

		private void ParseTicketsOrMcos(bool isEmd)
		{
			var isMco = _sections.ContainsKey("A19");
			var isTicket = !isMco && !isEmd;

			var template = isTicket ? (AviaDocument)new AviaTicket() : new AviaMco();

			ParseHeader(template);

			decimal commissionAmount = Utility.ParseInt(_header[2].Substring(69, 8)); // Utility.ParseDecimal(_header[2].Substring(69, 8)) / 100;

			if (commissionAmount == 0)
				template.CommissionPercent = Utility.ParseDecimal(_header[2].Substring(77, 4)) / 100;
			else
				template.Commission = new Money(_taxCurrency, commissionAmount);

			if (isTicket)
			{
				ParseSegments((AviaTicket)template);
				ParseTicketFares((AviaTicket)template);
			}

			ParseFormOfPayment(template);

			ParseRemarks(template);

			ParseGdsPassports();

			var passengers = _sections["A02"];
			var passengers2 = _sections.By("A29");

			for (var i = 0; i < passengers.Count; ++i)
			{
				var passengerNumber = i + 1;

				var passenger = passengers[i];

				var number = passenger[0].Substring(48, 10).Trim();

				var document = i < passengers.Count - 1 ? (AviaDocument)template.Clone() : template;

				document.PassengerName = passenger[0].Substring(3, 33).Clip();

				var passenger2 = passengers2.By(a => a.By(0).AsSubstring(14).StartsWith(document.PassengerName));


				if (number.Yes())
					document.Number = number;
				else if (passenger2 != null)
					document.Number = passenger2[0].Substring(72, 10).Clip();


				string fareId = null;

				List<string> fare = null;

				if (isEmd)
				{
					var coupon = _coupons.By(passengerNumber);

					if (coupon != null)
					{
						document.Fare = document.EqualFare = coupon.Sum(a => ParseMoney(a.Substring(85, 15)));
						document.Total = coupon.Sum(a => ParseMoney(a.Substring(148, 15)));
					}

					if (document.Fare == null && passenger2 != null)
					{
						document.Fare = document.EqualFare = ParseMoney(passenger2.By(0)?.Substring(97, 15));
						document.Total = ParseMoney(passenger2.By(0)?.Substring(142, 15));
					}
				}
				else
				{
					fareId = passenger[0].Substring(75, 2);

					if (_fares.TryGetValue(fareId, out fare))
					{
						document.Fare = ParseMoney(fare[0].Substring(5, 15));
						document.Total = ParseMoney(fare[0].Substring(20, 15));
						document.EqualFare = ParseMoney(fare[0].Substring(35, 15));
					}
				}

				ParseFareAndFees(document, fare);

				if (isTicket)
				{
					var ticket = (AviaTicket)document;

					var reissueId = passenger[0].Substring(77, 2);
					var seatId = passengerNumber.ToString("D2");

					ParseFareBasises(ticket, fareId);

					ParseReissueInfo(ticket, reissueId);

					ParseSeats(ticket, seatId);
				}

				ParseAgencyData(document);

				document.GdsPassport = _passports.GetOrDefault(passengerNumber);

				_documents.Add(document);
			}
		}

		private void ParseGdsPassports()
		{
			ParseGdsPassports("A14", _passportA14Pattern);

			ParseGdsPassports("A15", _passportA15Pattern);
		}

		private void ParseGdsPassports(string sectionName, Regex pattern)
		{
			var section = _sections.GetOrDefault(sectionName);

			if (section == null)
				return;

			foreach (var lines in _sections.GetOrDefault(sectionName))
			{
				foreach (var line in lines)
				{
					var match = pattern.Match(line);

					if (!match.Success)
						continue;

					_passports[int.Parse(match.Groups["passenger"].Value)] = match.Groups["passport"].Value.Replace('-', '/');
				}
			}
		}

		private void ParseTicketFares(AviaTicket ticket)
		{
			var section = _sections.GetOrDefault("A09");

			if (section == null)
				return;

			string s = null;

			foreach (var line in section.SelectMany(lines => lines))
				s += line.StartsWith("A09010") ? line.Substring(6) : line;

			if (s != null)
				IataParser.ParseTicketFares(ticket, s);
		}

		private void ParseRefund()
		{
			if (!_sections.ContainsKey("A23"))
				return;

			var template = new AviaRefund();

			ParseHeader(template);

			ParseRemarks(template);

			var section = _sections["A23"];

			for (var i = 0; i < section.Count; ++i)
			{
				var items = section[i];

				var refund = (i == section.Count - 1) ? template : (AviaRefund)template.Clone();

				refund.AirlinePrefixCode = items[0].Substring(3, 3);
				refund.Number = items[0].Substring(6, 10).Clip();

				refund.PassengerName = items[0].Substring(48, 33).TrimOrNull();

				Currency currency = null;

				for (var j = 1; j < items.Count; ++j)
				{
					var item = items[j];

					switch (item.Substring(0, 3))
					{
						case "BF:":
							{
								var match = _refundFareAndFeesPattern.Match(item);

								if (match.Success)
								{
									currency = new Currency(match.Groups["currency"].Value);

									refund.FeesTotal = new Money(currency);

									ParseFees(match, refund, currency);
								}

								break;
							}

						case "IT:":
							ParseItFees(item, refund, currency);
							break;

						case "CR:":
							{
								var commissionAmount = Utility.ParseDecimal(item.Substring(8));

								if (commissionAmount == 0)
									refund.CommissionPercent = Utility.ParseDecimal(item.Substring(3, 5));
								else
									refund.Commission = new Money(currency, commissionAmount);

								break;
							}

						case "PF:":
							{
								var match = _refundPenaltyPattern.Match(item);

								if (match.Success)
								{
									refund.CancelFee = new Money(currency, Utility.ParseDecimal(match.Groups["penalty"].Value));

									var commissionPercentGroup = match.Groups["commissionPercent"];
									if (commissionPercentGroup.Success)
										refund.CancelCommissionPercent = Utility.ParseDecimal(commissionPercentGroup.Value);

									var commissionAmountGroup = match.Groups["commissionAmount"];
									if (commissionAmountGroup.Success)
										refund.CancelCommission = new Money(currency, Utility.ParseDecimal(commissionAmountGroup.Value));
									else if (refund.CancelCommissionPercent.HasValue)
										refund.CancelCommission = refund.CancelFee * refund.CancelCommissionPercent.Value / 100;
									else
										refund.CancelCommission = new Money(currency, 0);
								}

								break;
							}

						case "RA:":
							{
								var match = _refundAmountPattern.Match(item);

								if (match.Success)
								{
									refund.Total = new Money(currency, Utility.ParseDecimal(match.Groups["amount"].Value));

									refund.Fare = refund.Total - refund.FeesTotal;

									if (refund.CancelFee != null)
										refund.Fare += refund.CancelFee;

									if (refund.CommissionPercent.HasValue)
										refund.Commission = refund.Fare * refund.CommissionPercent.Value / 100;
								}

								break;
							}
					}
				}

				ParseAgencyData(refund);
			}

			_documents.Add(template);
		}

		private void ParseVoid(bool isVoid)
		{
			var template = new AviaDocumentVoiding();

			var prefixCode = _header[0].Substring(34, 3);

			template.IsVoid = isVoid;

			template.TimeStamp = ParseDateTime(_header[0].Substring(20, 12));

			template.Originator = GdsOriginator.Galileo;
			template.Origin = ProductOrigin.GalileoMir;

			template.AgentOffice = _header[1].Substring(4, 4);
			template.IataOffice = _header[1].Substring(8, 9).TrimEnd();
			template.AgentCode = _header[1].Substring(39, 2);

			var passengers = _sections["A02"];

			for (var i = 0; i < passengers.Count; ++i)
			{
				var voiding = (i == passengers.Count - 1) ? template : (AviaDocumentVoiding)template.Clone();

				template.Document = new AviaTicket
				{
					AirlinePrefixCode = prefixCode,
					Number = passengers[i][0].Substring(48, 10).Clip()
				};

				_documents.Add(voiding);
			}
		}

		private void ParseHeader(AviaDocument document)
		{
			document.IssueDate = ParseDateTime(_header[0].Substring(20, 12));

			document.Originator = GdsOriginator.Galileo;
			document.Origin = ProductOrigin.GalileoMir;

			var airline = ParseAirline(_header[0].Substring(32, 29));
			document.AirlineIataCode = airline.AirlineIataCode;
			document.AirlinePrefixCode = airline.AirlinePrefixCode;
			document.AirlineName = airline.Name;
			if (airline.AirlinePrefixCode != null)
				document.Producer = airline;

			document.BookerOffice = _header[1].Substring(0, 4).TrimOrNull();
			document.BookerCode = _header[1].Substring(36, 2).TrimOrNull();

			document.TicketerOffice = _header[1].Substring(4, 4).TrimOrNull();
			document.TicketingIataOffice = _header[1].Substring(8, 9).TrimOrNull();
			document.TicketerCode = _header[1].Substring(39, 2).TrimOrNull();

			document.PnrCode = _header[1].Substring(17, 6).TrimOrNull();
			document.AirlinePnrCode = _header[1].Substring(23, 6).TrimOrNull();

			document.TourCode = _header[2].Substring(82).TrimOrNull();

			_taxCurrency = new Currency(_header[2].Substring(16, 3));
		}

		private void ParseSegments(AviaTicket ticket)
		{
			_firstTravelDate = Utility.ParseExactDateTime(_header[0].Substring(61, 7), "ddMMMyy");

			var segments = _sections["A04"];//.AsConcat(_sections.By("A05"));
			foreach (var segment in segments)
				ticket.AddSegment(ParseSegment(segment));
		}

		private FlightSegment ParseSegment(IList<string> item)
		{
			var segment = new FlightSegment
			{
				Position = int.Parse(item[0].Substring(3, 2)),
				Type = FlightSegmentType.Ticketed
			};

			var carrier = ParseAirline(item[0].Substring(5, 17));
			segment.CarrierIataCode = carrier.AirlineIataCode;
			segment.CarrierPrefixCode = carrier.AirlinePrefixCode;
			segment.CarrierName = carrier.Name;
			if (carrier.AirlineIataCode != null && carrier.AirlinePrefixCode != null && carrier.Name != null)
				segment.Carrier = carrier;

			segment.FlightNumber = item[0].Substring(22, 4).TrimOrNull();

			segment.ServiceClassCode = item[0].Substring(26, 2).TrimOrNull();

			var departureTime = item[0].Substring(30, 10).TrimOrNull();

			if (departureTime != null)
			{
				segment.DepartureTime = Utility.ParseSegmentDateTime(departureTime, _firstTravelDate);

				var arrivalTime = item[0].Substring(40, 5).TrimOrNull();

				if (arrivalTime != null)
				{
					var time = ParseDateTime(arrivalTime);

					segment.ArrivalTime = new DateTime(
						segment.DepartureTime.Value.Year, segment.DepartureTime.Value.Month, segment.DepartureTime.Value.Day,
						time.Hour, time.Minute, 0);

					var arrivalIndicator = item[0][45];

					if (arrivalIndicator == PreviousDayArrival)
						segment.ArrivalTime = segment.ArrivalTime.Value.AddDays(-1);
					else if (arrivalIndicator == NextDayArrival)
						segment.ArrivalTime = segment.ArrivalTime.Value.AddDays(1);
					else if (arrivalIndicator == TwoDaysLaterArrival)
						segment.ArrivalTime = segment.ArrivalTime.Value.AddDays(2);
					else if (arrivalIndicator != SameDayArrival)
						throw new GdsImportException(_lines.Number, $"Invalid arrival day indicator: '{arrivalIndicator}'");
				}
			}

			var fromInfo = ParseAirport(item[0].Substring(46, 16));
			segment.FromAirport = (Airport)fromInfo[0];
			segment.FromAirportCode = segment.FromAirport.Code;
			segment.FromAirportName = (string)fromInfo[1];

			var toInfo = ParseAirport(item[0].Substring(62, 16));
			segment.ToAirport = (Airport)toInfo[0];
			segment.ToAirportCode = segment.ToAirport.Code;
			segment.ToAirportName = (string)toInfo[1];

			segment.MealCodes = item[0].Substring(80, 4).TrimOrNull();

			segment.Stopover = item[0][84] == 'O';

			if (item[0][85] != ' ')
				segment.NumberOfStops = item[0][85] - '0';

			segment.CheckInTerminal = item[0].Substring(93, 3).TrimOrNull();

			var pos = item[0].IndexOf("JT:", 103, StringComparison.InvariantCulture);

			if (pos > 0)
				segment.Duration = item[0].Substring(pos + 3, 5).Replace('.', ':').TrimOrNull();

			return segment;
		}

		private void ParseFareAndFees(AviaDocument document, List<string> fare)
		{
			if (document.Commission == null && document.CommissionPercent.HasValue)
			{
				if (document.ReissueFor != null && (document.IsAviaTicket || document.IsAviaMco))
				{
					if (document.Total != null)
						document.Commission =
							(document.Total - (document.FeesTotal ?? new Money(document.Total.Currency, 0)))
							* document.CommissionPercent.Value / 100;
				}
				else if (document.EqualFare != null)
					document.Commission = document.EqualFare * document.CommissionPercent.Value / 100;
				else if (document.Fare != null)
					document.Commission = document.Fare * document.CommissionPercent.Value / 100;
			}

			if (fare == null)
				return;

			var match = _fareAndFeesPattern.Match(fare[0]);

			if (!match.Success)
			{
				document.FeesTotal = new Money(_taxCurrency);
				return;
			}

			var taxesCurrency = new Currency(match.Groups["currency"].Value);

			document.FeesTotal = new Money(taxesCurrency);

			ParseFees(match, document, taxesCurrency);

			if (fare.Count > 1)
				ParseItFees(fare[1], document, taxesCurrency);
		}

		private static void ParseItFees(string line, AviaDocument document, Currency taxesCurrency)
		{
			ParseFees(_itFeesPattern.Match(line), document, taxesCurrency);
		}

		private static void ParseFees(Match match, AviaDocument document, Currency taxesCurrency)
		{
			if (!match.Success)
				return;

			var codes = match.Groups["code"].Captures;
			var amounts = match.Groups["amount"].Captures;

			for (var i = 0; i < codes.Count; ++i)
			{
				document.AddFee(new AviaDocumentFee
				{
					Code = codes[i].Value,
					Amount = new Money(taxesCurrency, Utility.ParseDecimal(amounts[i].Value))
				});
			}
		}

		private void ParseFareBasises(AviaTicket ticket, string fareId)
		{
			foreach (var segment in ticket.Segments)
			{
				var segmentId = segment.Position.ToString("D2");

				if (_basises.ContainsKey(fareId + segmentId))
				{
					var basis = _basises[fareId + segmentId];
					segment.FareBasis = basis[0].Substring(7, 8).TrimOrNull();

					if (basis[0].Length >= 127)
					{
						segment.Luggage = basis[0].Substring(124, 3).TrimOrNull();
					}
					else
					{
						var index = basis[0].LastIndexOf("B:", StringComparison.InvariantCulture) + 2;
						if (index > 1)
						{
							segment.Luggage = index + 3 < basis[0].Length ?
								basis[0].Substring(index, 3) :
								basis[0].Substring(index);
							segment.Luggage.TrimOrNull();
						}
					}
				}
			}
		}

		private void ParseReissueInfo(AviaTicket ticket, string reissueId)
		{
			if (reissueId == "  ")
				return;

			var reissueNumber = _reissues[reissueId][1];

			ticket.ReissueFor = new AviaTicket
			{
				AirlinePrefixCode = reissueNumber.Substring(3, 3),
				Number = reissueNumber.Substring(6, 10).Clip()
			};

			if (_actualAmount != null && ticket.EqualFare != null)
			{
				ticket.EqualFare.Amount =
					_actualAmount.Value
					- (ticket.FeesTotal?.Amount ?? 0)
					+ (ticket.CancelFee?.Amount ?? 0);

				ticket.Total.Amount = _actualAmount.Value;
			}

			//Total = EqualFare + FeesTotal - CancelFee
		}

		private void ParseFormOfPayment(AviaDocument document)
		{
			List<List<string>> fops;

			if (!_sections.TryGetValue("A11", out fops))
				return;

			var fop = fops[0][0];

			var fopMatch = _fopPattern.Match(fop);

			if (!fopMatch.Success)
				return;

			var code = fopMatch.Groups["code"].Value;

			document.PaymentForm = _paymentFormMap.ContainsKey(code) ? _paymentFormMap[code] : code;

			document.PaymentType = _paymentTypeMap.ContainsKey(code) ? _paymentTypeMap[code] : PaymentType.Unknown;

			if (document.PaymentType == PaymentType.CreditCard)
			{
				var match = _creditCardDetailsRegex.Match(fop);

				if (match.Success)
				{
					var value = match.Groups["type"].Value;
					string type;
					if (!_creditCardTypes.TryGetValue(value, out type))
						type = value;

					var number = match.Groups["number"].Value;
					number = number.Substring(Math.Max(0, number.Length - 4));

					var expiry = match.Groups["expiry"].Value;

					document.PaymentDetails =
						$"{type} *{number} {expiry.Substring(0, 2)}/{expiry.Substring(2, 2)} - {match.Groups["authCode"].Value}";
				}
			}

			_actualAmount = Utility.ParseDecimal(fopMatch.Groups["amount"].Value);
		}

		private void ParseAgencyData(AviaDocument document)
		{
			List<List<string>> section;

			if (!_sections.TryGetValue("A13", out section))
				return;

			string line = null;

			foreach (var sectionItem in section)
			{
				line = sectionItem[0];

				var match = _passengerRegex.Match(line);

				if (!match.Success)
					break;

				if (Utility.MatchPassengers(match.Groups["passenger"].Value, document.PassengerName))
					break;

				line = null;
			}

			if (line.No())
				return;

			ParseAgencyDataString(document, line);

			if (document.ServiceFee != null && document.Customer != null)
			{
				if (document.Discount == null)
					document.Discount = new Money(_defaultCurrency);

				//document.GrandTotal = document.Total + document.ServiceFee - document.Discount;

				var refund = document as AviaRefund;

				if (refund != null)
				{
					refund.RefundServiceFee = new Money(_defaultCurrency);
					refund.ServiceFeePenalty = new Money(_defaultCurrency);
				}
			}
		}

		private void ParseAgencyDataString(AviaDocument document, string line)
		{

			var strings = line.Substring(5).Split('*');

			foreach (var str in strings)
			{
				if (str.StartsWith("XP") && document.ServiceFee == null)
				{
					decimal serviceFee;
					if (Utility.TryParseDecimal(str.Substring(2), out serviceFee))
						document.ServiceFee = new Money(_defaultCurrency, serviceFee);
				}
				else if (str.StartsWith("DC"))
				{
					decimal discount;
					if (Utility.TryParseDecimal(str.Substring(2), out discount) && document.Discount == null)
						document.Discount = new Money(_defaultCurrency, discount);
				}
				else if (str.StartsWith("CL"))
					document.SetCustomer(null, new Organization
					{
						LegalName = str.Substring(2).Trim('.')
					});
			}

		}

		private void ParseRemarks(AviaDocument document)
		{
			List<List<string>> section;

			if (_sections.TryGetValue("A14", out section))
			{
				document.IsTicketerRobot = section.Any(item => _robotRegex.IsMatch(item[0]));

				ParseAirlinePnrCode(document, section);

				ParseRemarkSection(document, section);
			}

			if (_sections.TryGetValue("A15", out section))
				ParseRemarkSection(document, section);
		}

		private static void ParseAirlinePnrCode(AviaDocument document, IEnumerable<List<string>> section)
		{
			var matchs = section.Select(item => _airlineLocatorRegex.Match(item[0])).Where(m => m.Success);

			foreach (var match in matchs)
			{
				if (document.AirlinePnrCode.Yes())
					document.AirlinePnrCode += ", ";

				if (match.Groups[1].Value != "1A")
					document.AirlinePnrCode = match.Groups[1].Value + ' ';

				document.AirlinePnrCode += match.Groups[2].Value;
			}
		}

		private static void ParseRemarkSection(AviaDocument document, IEnumerable<List<string>> section)
		{
			var notes = new StringBuilder();

			foreach (var sectionItem in section)
			{
				var startIndex = sectionItem[0].IndexOf("X*", StringComparison.InvariantCulture);

				if (startIndex == -1)
					continue;

				var remarks = sectionItem[0].Substring(startIndex + 2).Split('/');

				foreach (var remark in remarks)
				{
					if (_remarkRegex.IsMatch(remark))
						notes.AppendLine(remark.Substring(1));
				}
			}

			if (notes.Length > 0)
				document.Remarks = (document.Remarks ?? string.Empty) + notes;
		}

		private void ParseSeats(AviaTicket ticket, string seatId)
		{
			foreach (var segment in ticket.Segments)
			{
				var segmentId = segment.Position.ToString("D2");

				if (_seats.ContainsKey(seatId + segmentId))
				{
					var seat = _seats[seatId + segmentId];

					segment.Seat = seat[0].Substring(8, 4).TrimStart('0');
				}
			}
		}

		private static Organization ParseAirline(string str)
		{
			var prefixCode = str.Substring(2, 3).TrimOrNull();

			return new Organization
			{
				AirlineIataCode = str.Substring(0, 2).TrimOrNull(),
				AirlinePrefixCode = prefixCode == "000" ? null : prefixCode,
				Name = str.Substring(5).TrimOrNull()
			};
		}

		private static object[] ParseAirport(string airportInfo)
		{
			var cityInfo = airportInfo.Substring(3).TrimEnd();

			var cityInfoParts = cityInfo.Split('/');

			var airport = new Airport
			{
				Code = airportInfo.Substring(0, 3),
				Settlement = cityInfoParts[0],
				Name = cityInfoParts.Length == 1 ? cityInfoParts[0] : cityInfoParts[1]
			};

			return new object[] { airport, cityInfo };
		}

		private static DateTime ParseDateTime(string s)
		{
			var pattern = s.Length == 12 ? "ddMMMyy" : string.Empty;

			if (s.EndsWith("A") || s.EndsWith("P"))
			{
				pattern += "hhmmt";

				s = s.Replace(" ", "0");

				if (s.Length == 4)
					s = "0" + s;
			}
			else
				pattern += "HHmm";


			return Utility.ParseExactDateTime(s, pattern);
		}

		private static Money ParseMoney(string value)
		{
			if (value.Yes())
			{
				decimal amount;

				if (Utility.TryParseDecimal(value.Substring(3), out amount))
					return new Money(value.Substring(0, 3), amount);
			}

			return null;
		}

		private const string BasicId = "T5";
		private const string GscId = "1G";

		private const string TicketingCommandAvoided = "A";
		private const string TicketingCommandRegular = "H";
		private const string TicketingCommandEmd = "F";
		private const string TicketingCommandCancelledRefund = "C";
		private const string TicketingCommandRefund = "R";
		private const string TicketingCommandSpoiled = "S";
		private const string TicketingCommandUnvoid = "U";
		private const string TicketingCommandVoid = "V";

		private const int SectionIdLength = 3;

		private const char PreviousDayArrival = '1';
		private const char SameDayArrival = '2';
		private const char NextDayArrival = '3';
		private const char TwoDaysLaterArrival = '4';

		public static readonly Dictionary<string, string> _paymentFormMap = new Dictionary<string, string>
		{
			{ "S", "CASH" },
			{ "CK", "CHEQUE" },
			{ "CC", "CREDIT CARD" },
			{ "EX", "EXCHANGE" },
			{ "MS", "MISCELLANEOUS" },
			{ "IN", "INVOICE" },
			{ "NO", "NON REFUNDABLE" },
			{ "MR", "MULTIPLE" },
			{ "FR", "FREE" }
		};

		private static readonly Dictionary<string, PaymentType> _paymentTypeMap = new Dictionary<string, PaymentType>
		{
			{ "S", PaymentType.Cash },
			{ "CK", PaymentType.Check },
			{ "CC", PaymentType.CreditCard },
			{ "IN", PaymentType.Invoice }
		};

		private static readonly Dictionary<string, string> _creditCardTypes = new Dictionary<string, string>
		{
			{ "VI", "Visa" }
		};

		private static readonly Regex _headerRegex = new Regex(@"^A\d\d", RegexOptions.Compiled);
		private static readonly Regex _passengerRegex = new Regex(@"^A13W(-|(-.+\*))N\.?(?<passenger>[^\*]+)", RegexOptions.Compiled);
		private static readonly Regex _airlineLocatorRegex = new Regex(@"^A14VL-.{14}(..)(.+)", RegexOptions.Compiled);
		private static readonly Regex _robotRegex = new Regex(@"^A14SA-PCC:[\d|\w]{4}-NAME:.+", RegexOptions.Compiled);
		private static readonly Regex _remarkRegex = new Regex(@"^R\d+", RegexOptions.Compiled);
		private static readonly Regex _fopPattern = new Regex(@"^A11(?'code'\w\w?) *(?'amount'\d+(?:\.\d\d)?)", RegexOptions.Compiled);
		private static readonly Regex _creditCardDetailsRegex = new Regex(@"^A11CC.{13}(?<type>..)(?<number>\d{1,20})\s*(?<expiry>\d{4})\s*(?<authCode>\d{1,8})", RegexOptions.Compiled);
		private static readonly Regex _fareAndFeesPattern = new Regex(@"(?'currency'[A-Z]{3})(?:T\d+: *(?'amount'\d+(?:\.\d\d)?)(?'code'\w\w))+", RegexOptions.Compiled);
		private static readonly Regex _refundFareAndFeesPattern = new Regex(@"(?'currency'[A-Z]{3}) *(?'fare'\d+(?:\.\d\d)?)(?:T\d+: *(?'amount'\d+(?:\.\d\d)?)(?'code'\w\w))*", RegexOptions.Compiled);
		private static readonly Regex _refundPenaltyPattern = new Regex(@"PF: *(?'penalty'\d+(?:\.\d\d)?)(?:PC:(?'commissionPercent'\d\d\.\d\d) *(?'commissionAmount'\d+(?:\.\d\d)?))?", RegexOptions.Compiled);
		private static readonly Regex _refundAmountPattern = new Regex(@"RA:.*(?'currency'[A-Z]{3}) *(?'amount'\d+(?:\.\d\d)?)", RegexOptions.Compiled);
		private static readonly Regex _itFeesPattern = new Regex(@"^IT:(?: *(?:(?:(?'amount'\d+(?:\.\d\d)?)(?'code'\w\w))|EXEMPT))*", RegexOptions.Compiled);
		private static readonly Regex _passportA14Pattern = new Regex(@"^A14X\*//-P(?'passenger'\d+)/SSRDOCS[A-Z]{2}(?'passport'.*)-//", RegexOptions.Compiled);
		private static readonly Regex _passportA15Pattern = new Regex(@"^A15\d\dP(?'passenger'\d+)/SSRDOCS[A-Z]{2}(?'passport'.*)", RegexOptions.Compiled);

		private readonly LinesEnumerator _lines;

		private string[] _header;
		private Dictionary<string, List<List<string>>> _sections;
		private Dictionary<string, List<string>> _fares;
		private Dictionary<string, List<string>> _basises;
		private Dictionary<string, List<string>> _reissues;
		private Dictionary<string, List<string>> _seats;
		private Dictionary<int, string> _passports;
		private Dictionary<int, List<string>> _coupons;

		private DateTime _firstTravelDate;

		private Currency _taxCurrency;
		private decimal? _actualAmount;

		private readonly List<Entity2> _documents;
		private readonly Currency _defaultCurrency;
	}

}