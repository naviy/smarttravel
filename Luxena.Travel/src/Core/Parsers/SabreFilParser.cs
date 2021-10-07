using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Parsers
{



	//===g






	public class SabreFilParser
	{

		//---g



		private static readonly string[] _robots = { "S1OI-WS", };



		//---g



		public static IEnumerable<Entity2> Parse(string content, string defaultCurrency, GdsOriginator originator = GdsOriginator.Sabre)
		{

			if (content.No())
				throw new GdsImportException("Empty SabreFil");


			return new SabreFilParser
			{
				Content = content,
				DefaultCurrency = defaultCurrency,
				Originator = originator
			}.Parse();

		}



		//---g



		public string Content;
		public string DefaultCurrency { get; set; }
		public GdsOriginator Originator { get; set; }

		private string _parseLine;
		private int _productIndex;
		private int _productNo;
		private Match _m0, _m1, _m2;
		private Match[] _m1s, _m2s, _m3s, _m4s, _m5s, _m8s, _mGs;
		private static Regex _reM0, _reM1, _reM2, _reM3, _reM4, _reM5, _reM8, _reMG;

		private static Regex NewRegex(string pattern)
			=> new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline);


		//---g



		private IEnumerable<Entity2> Parse()
		{

			if (Content.Contains("\n\n"))
				Content = Regex.Replace(Content, "(?<!\r)\n", "\r\n");


			if (_reM0 == null)
			{
				_reM0 = NewRegex(@"^[\d\w]{11}M0.+?$");
				_reM1 = NewRegex(@"^M1(?<rowNo>\d\d).+?$");
				_reM2 = NewRegex(@"^M2(?<rowNo>\d\d).+?[\r\n]{1,2}(^.+?[\r\n]{1,2})(^.+?[\r\n]{1,2})(^.+?[\r\n]{1,2})(?<IU2ORG>.+?[\r\n]{1,2})");
				_reM3 = NewRegex(@"^M3(?<rowNo>\d\d).+?$");
				_reM4 = NewRegex(@"^M4(?<rowNo>\d\d).+?$");
				_reM5 = NewRegex(@"^M5(?<rowNo>\d\d).+?$");
				_reM8 = NewRegex(@"^M8(?<rowNo>\d\d).*?TTL(?<total>\d+(\.\d+)?)/.+?/S(?<segs>[\d,]+)/(?<psgNo>\d+).1\s*$");
				//_reM8 = NewRegex(@"^M8(?<rowNo>\d\d).*?TTL(?<total>\d+)\/.+?(?<psgNo>\d+)\.1\s*$");
				_reMG = NewRegex(@"^MG(?<rowNo>\d\d).+?$");
			}


			_m0 = _reM0.Match(Content);

			if (_m0.No())
				throw new FormatException();


			_m1s = _reM1.Matches(Content).Cast<Match>().ToArray();
			_m2s = _reM2.Matches(Content).Cast<Match>().ToArray();
			_m3s = _reM3.Matches(Content).Cast<Match>().ToArray();
			_m4s = _reM4.Matches(Content).Cast<Match>().ToArray();
			_m5s = _reM5.Matches(Content).Cast<Match>().ToArray();
			_m8s = _reM8.Matches(Content).Cast<Match>().ToArray();
			_mGs = _reMG.Matches(Content).Cast<Match>().ToArray();


			_parseLine = _m0.Value;

			var type = Part(14, 1);
			_productIndex = -1;


			if (type == "1" || type == "2" || type == "3" || type == "A" || type == "B")
			{

				foreach (var m1 in _m1s)
				{

					_productIndex++;
					var rowNo = m1.Groups["rowNo"].Value;
					_productNo = rowNo.As().Int;
					_m1 = m1;


					_m2 = _m2s.By(a => a.Groups["rowNo"].Value == rowNo);// ?? _m2s.LastOrDefault();

					if (_m2 == null && _m2s.Yes())
						continue;


					if (type == "1" || type == "3" || type == "B")
					{
						yield return ParseAviaTicket();
					}
					else if (type == "2")
					{
						yield return ParseAviaRefund();
					}
					else if (type == "A")
					{
						yield return ParseAviaMco();
					}

				}

			}

			else if (type == "5")
			{
				yield return ParseAviaDocumentVoiding5();
			}

			else if (type == "F")
			{
				yield return ParseAviaDocumentVoidingF();
			}

		}



		private void ParseProduct(Product r)
		{

			r.Originator = Originator;
			r.Origin = ProductOrigin.SabreFil;

			_parseLine = _m0.Value;
			PartDate(226).Do(a => r.IssueDate = a);

			r.PnrCode = Part(54, 8);

			r.BookerOffice = Part(89, 5);
			r.BookerCode = Part(96, 3);
			r.TicketerOffice = Part(127, 5);
			r.TicketerCode = Part(134, 3);

			r.IsTicketerRobot = _robots.Contains(r.TicketerOffice + "-" + r.TicketerCode);

			_parseLine = _m1.Value;
			r.PassengerName = Part(5, 64);


			if (_m2 != null)
			{

				_parseLine = _m2.Value;


				Part(35, 3).Do(cur =>
					r.Fare = new Money(cur, PartSign(34) * (PartFloat(38) ?? 0))
				);


				Part(77, 3).Do(cur =>
				{
					r.Total = new Money(cur, PartFloat(80) ?? 0);

					(PartFloat(88, 11) ?? PartFloat(99, 11)).Do(a =>
						r.CancelCommission = new Money(cur, a)
					);
				});


				Part(117, 3).Do(cur =>
				{
					var equalFare = PartFloat(120) ?? 0;
					if (equalFare == 0)
						equalFare = PartFloat(146) ?? 0;

					r.EqualFare = new Money(cur, equalFare);
					r.Commission = new Money(cur, PartFloat(136, 9) ?? 0);
					//r.ServiceFee = new Money(cur, PartFloat(157) ?? 0);
				});


				r.CommissionPercent = PartFloat(128);

				if (r.EqualFare.Yes() && r.CommissionPercent != null)
					r.Commission = r.EqualFare * (r.CommissionPercent.Value / 100m);

			}


			if (_m8s.Yes() && r.Total.No() && r.EqualFare.No())
			{

				var total = 0m;

				var m3s = _m3s?.Select(a => a.Groups["rowNo"].Value.As().Int).ToArray();


				foreach (var m8 in _m8s)
				{

					if (m8.Groups["psgNo"].Value.As().Int != _productNo)
						continue;


					if (m3s.Yes())
					{

						var segs = m8.Groups["segs"].Value.Split(',').Select(a => a.As().Int);

						if (!segs.Any(seg => m3s.Any(m3 => m3 == seg)))
							continue;

					}


					total += m8.Groups["total"].Value.As().Decimal;

				}

				r.EqualFare = new Money(DefaultCurrency, total);

			}


			//r.Note = _m8s?.Select(a => a.Value.AsSubstring(4).Trim()).Join("\r\n");

		}



		//---g



		#region Parse Avia

		private void ParseAviaDocument(AviaDocument r)
		{

			_parseLine = _m0.Value;
			r.TicketingIataOffice = Part(44, 10)?.Replace(" ", null);

			if (_m2 == null)
				return;


			_parseLine = _m2.Value;


			r.Total?.Currency?.Code.Do(cur =>
			{

				var feeTotal = r.Total.Yes() && r.EqualFare.Yes() ? r.Total.Amount - r.EqualFare.Amount : 0;

				var noFeeTotal = feeTotal <= 0;


				var fee3 = PartFloat(66) ?? 0;

				if (fee3 > 0 && (noFeeTotal || feeTotal >= fee3))
				{
					feeTotal -= fee3;
					r.AddFee(Part(74, 2), new Money(cur, fee3), ignoreOtherCode: false);
				}


				var fee2 = PartFloat(56) ?? 0;

				if (fee2 > 0 && (noFeeTotal || feeTotal >= fee2))
				{
					feeTotal -= fee2;
					r.AddFee(Part(64, 2), new Money(cur, fee2), ignoreOtherCode: false);
				}


				var fee1 = PartFloat(46) ?? 0;

				if (fee1 > 0 && (noFeeTotal || feeTotal >= fee1))
				{
					r.AddFee(Part(54, 2), new Money(cur, fee1), ignoreOtherCode: false);
				}

			});


			r.AirlineIataCode = Part(232, 2).Do(a =>
				r.Producer = new Organization { AirlineIataCode = a }
			);


			r.Number = Part(234, 10);

		}



		private AviaTicket ParseAviaTicket()
		{

			var r = new AviaTicket();


			ParseProduct(r);
			ParseAviaDocument(r);


			if (_m2 != null)
			{
				_m2.By("IU2ORG").Clip().Do(a =>
				{
					r.ReissueFor = new AviaTicket
					{
						AirlinePrefixCode = a.Substring(0, 3),
						Number = a.Substring(3, 10).Clip()
					};
				});

				r.Domestic = Part(156, 1) == "D";
			}


			ParseAviaTicketSegments(r);


			return r;

		}



		private void ParseAviaTicketSegments(AviaTicket ticket)
		{

			if (ticket == null)
				return;


			var segmentCount = _m3s.Length;


			for (var i = 0; i < segmentCount; i++)
			{

				_parseLine = _m3s[i].Value;

				var r = new FlightSegment();

				r.Position = PartInt(3, 2) ?? 0;

				r.FromAirportCode = Part(19, 3);
				r.FromAirportName = Part(22, 17);
				r.FromAirport = new Airport
				{
					Code = r.FromAirportCode,
					Name = r.FromAirportName,
				};


				r.ToAirportCode = Part(39, 3);
				r.ToAirportName = Part(42, 17);

				r.ToAirport = new Airport
				{
					Code = r.ToAirportCode,
					Name = r.ToAirportName,
				};


				r.DepartureTime = PartDate(10) + PartTime(68);
				r.CheckInTerminal = Part(130, 26)?.TrimStart("TERMINAL ");
				r.ArrivalTerminal = Part(160, 26)?.TrimStart("TERMINAL ");

				if (r.DepartureTime != null)
				{
					r.ArrivalTime = r.DepartureTime.Value.Date + PartTime(73);
					if (r.ArrivalTime != null && r.ArrivalTime < r.DepartureTime)
						r.ArrivalTime = r.ArrivalTime.Value.AddDays(1);
				}


				r.CarrierIataCode = Part(59, 2);

				if (r.CarrierIataCode == ticket.AirlineIataCode)
					r.Carrier = ticket.Provider;


				r.FlightNumber = Part(61, 5);
				r.ServiceClassCode = Part(66, 2);

				r.Duration = Part(78, 8)?.Replace(".", ":");

				r.NumberOfStops = PartInt(92, 1);
				r.Stopover = Part(93, 18) != null;
				//r.FareBasis


				Part(113, 3).Do(a =>
					r.Equipment = new AirplaneModel { IataCode = a }
				);


				if (_m4s.Yes())
				{
					_parseLine = (
						_m4s.By(_productIndex * segmentCount + i) ??
						_m4s.By(i) ??
						_m4s.Last()
					)?.Value;

					r.Luggage = Part(22, 3);
					r.FareBasis = Part(25, 13);
				}


				ticket.AddSegment(r);

			}

		}



		private AviaRefund ParseAviaRefund()
		{

			var r = new AviaRefund();

			ParseProduct(r);
			ParseAviaDocument(r);


			_m5s.Last().Do(match =>
			{

				_parseLine = match.Value;
				r.AirlineIataCode = Part(9, 2);
				r.Number = Part(12, 10);

				if (r.EqualFare.No())
				{
					var parts = _parseLine.Split('/');
					var equalFare = parts[2].Clip();

					if (equalFare.Yes() && equalFare != "0")
					{
						var feesTotal = parts[3].Clip();
						var cur = DefaultCurrency;

						if (equalFare.Length > 3 && char.IsLetter(equalFare[0]))
						{
							cur = equalFare.Substring(0, 3);
						}

						r.EqualFare = new Money(cur, ParseFloat(equalFare) ?? 0);
						r.FeesTotal = new Money(cur, ParseFloat(feesTotal) ?? 0);
					}
				}


				r.RefundedProduct = new AviaTicket
				{
					AirlineIataCode = r.AirlineIataCode,
					Number = r.Number
				};

			});


			return r;

		}



		private Entity2 ParseAviaMco()
		{

			var r = new AviaMco();

			ParseProduct(r);
			ParseAviaDocument(r);


			var mg = _mGs.By(a => a.Groups["rowNo"]?.Value.As().Intn == _productNo) ?? _mGs.One();


			if (mg.Success)
			{

				_parseLine = mg.Value;

				if (r.AirlinePrefixCode.No())
					r.AirlinePrefixCode = Part(26, 3);

				if (r.Number.No())
					r.Number = Part(29, 10);


				Part(89, 3).Do(cur =>
				{
					r.EqualFare = new Money(cur, PartFloat(92, 18) ?? 0);
				});

			}


			return r;

		}


		private Entity2 ParseAviaDocumentVoidingF()
		{

			var r = new AviaDocumentVoiding
			{
				Originator = GdsOriginator.Sabre,
				Origin = ProductOrigin.SabreFil,
			};


			PartDate(117).Do(a => r.TimeStamp = a);


			r.AgentOffice = Part(127, 5);
			r.AgentCode = Part(134, 3);
			//r.IataOffice = airRecordHeader[10];


			_parseLine = _m2s[0].Value;

			r.Document = new AviaTicket
			{
				AirlineIataCode = Part(232, 2),
				Number = Part(234, 10)
			};


			r.IsVoid = true;


			return r;

		}



		private Entity2 ParseAviaDocumentVoiding5()
		{

			var r = new AviaDocumentVoiding
			{
				Originator = GdsOriginator.Sabre,
				Origin = ProductOrigin.SabreFil,
				Document = new AviaTicket
				{
					AirlinePrefixCode = Part(27, 3),
					Number = Part(30, 10)
				},
				IsVoid = true,
			};


			//r.AgentOffice = Part(127, 5);
			//r.AgentCode = Part(134, 3);

			_parseLine = Content;
			PartDate(3).Do(a => r.TimeStamp = a);


			return r;

		}

		#endregion



		//---g



		private string Part(string str, int startIndex, int length)
		{
			return str != null && startIndex >= 1 && startIndex <= str.Length - length// - 1
				? str.Substring(startIndex - 1, length).Clip()
				: null;
		}



		private string Part(int startIndex, int length)
		{
			return Part(_parseLine, startIndex, length);
		}



		private DateTime? PartDate(int startIndex)
		{

			var s = Part(_parseLine, startIndex, 5);
			if (s.No())
				return null;


			var day = s.Left(2).As().Int;

			var month = DateTime.ParseExact(s.Right(3), "MMM", CultureInfo.InvariantCulture).Month;

			var today = DateTime.Today;


			var year = today.Year;

			if (month < today.Month)
				year++;


			var date = new DateTime(year, month, day);


			return date;

		}



		private decimal? PartFloat(int startIndex, int length = 8)
		{
			var s = Part(_parseLine, startIndex, length);
			return s.No() ? null : ParseFloat(s);
		}



		private decimal? ParseFloat(string s)
		{

			s = s.Replace(" ", null);
			var sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			s = s.Replace(sep == "." ? "," : ".", sep);
			s = Regex.Replace(s, @"[A-Za-z]+", "");


			return decimal.TryParse(s, out var result) ? result : 0;

		}



		private int? PartInt(int startIndex, int length)
		{
			var s = Part(_parseLine, startIndex, length);
			if (s.No())
				return null;

			return int.Parse(s);
		}



		private TimeSpan? PartTime(int startIndex)
		{
			var s = Part(_parseLine, startIndex, 5);
			if (s.No()) return null;

			var hours = s.Left(2).As().Int;
			var minutes = s.Substring(2, 2).As().Int;

			if (s.Right(1) == "P")
			{
				if (hours < 12)
					hours += 12;
			}
			else if (s.Right(1) == "A")
			{
				if (hours >= 12)
					hours -= 12;
			}

			var time = new TimeSpan(hours, minutes, 0);

			return time;
		}



		private int PartSign(int startIndex)
		{
			var s = Part(_parseLine, startIndex, 1);
			return s == "-" ? -1 : 1;
		}



		//---g

	}






	//===g



}
