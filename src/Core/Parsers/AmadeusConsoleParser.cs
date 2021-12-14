using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Parsers
{



	//===g






	public class AmadeusConsoleParser
	{

		//---g



		private AmadeusConsoleParser(string content, Currency defaultCurrency)
		{
			Content = content;
			DefaultCurrency = defaultCurrency;
		}



		public static IEnumerable<AviaDocument> Parse(string content, Currency defaultCurrency)
		{
			if (content.No())
				throw new GdsImportException("Empty content");

			return new AmadeusConsoleParser(content, defaultCurrency).ParseTickets();
		}



		//---g



		public Currency DefaultCurrency { get; }

		public string Content { get; }



		//---g



		private IEnumerable<AviaDocument> ParseTickets()
		{


			//var baseMatch1 = _reBase1.Match(Content);
			//if (!baseMatch1.Success)
			//	return Documents;


			var baseMatch = _reBase.Match(Content);

			if (!baseMatch.Success)
				yield break;



			var passengerGroupsMatches = _rePassengerGroups.Matches(Content);

			var fareMatches = _reFare.Matches(Content);

			var equalFareMatches = _reEqualFare.Matches(Content);

			var totalMatches = _reTotal.Matches(Content);

			var commissionMatches = _reCommission.Matches(Content);

			var paymentTypeMatches = _rePaymentType.Matches(Content);



			if (passengerGroupsMatches.Count == 0 || totalMatches.Count == 0 || totalMatches.Count != fareMatches.Count)
			{
				yield break;
			}



			var bookingOffice = baseMatch.Groups["bookingOffice"].Value;

			var issuingOffice = baseMatch.Groups["issuingOffice"].Value;

			var ticketerCode = baseMatch.Groups["ticketerCode"].Value;

			var issueDate = DateTime.ParseExact(baseMatch.Groups["issueDate"].Value, "dMMMyy", CultureInfo.InvariantCulture);

			var pnrCode = baseMatch.Groups["pnrCode"].Value;


			var tickets = _reTicket.Matches(Content);

			var airlines = _reAirline.Matches(Content).Cast<Match>().Select(a => a.Groups["iataCode"].Value.Clip()).ToArray();


			var segments2 = _reSegments2.Matches(Content).Cast<Match>().Select(a => a.Groups["line"].Value).ToArray();


			var ticketBlocks = _reTicketBlocks.Matches(Content + "\nTST99999").Cast<Match>().ToArray();

			var segmentBlocks0 = ticketBlocks.Select(a => a.Groups["segments"].Value).ToArray();

			var segmentBlocks = segmentBlocks0.Select(g1 => _reSegments.Matches(g1).Cast<Match>().Select(a => a.Groups["line"].Value.Clip()).ToArray()).ToArray();


			var feesBlocks0 = ticketBlocks.Select(a => a.Groups["sums"].Value).ToArray();


			var feeBlocks = feesBlocks0

				.Select(a =>
					_reFees.Matches(a).Cast<Match>()
					.Select(m => new
					{
						Code = m.Groups["code"].Value,
						Amount = m.Groups["amount"].Value.As().Decimal,
						Currency = m.Groups["currency"].Value,
					})
					.ToArray()
				)

				.ToArray()

			;


			//var passengerNames = baseMatch.Groups["passenger"].Captures.Select(a => a.Value.Trim()).ToArray();

			var passengerGroups = passengerGroupsMatches.Cast<Match>().Select(a =>
				a.Groups["passenger"].Captures.Select(b => b.Value.Trim()).ToArray()
			).ToArray();


			Tuple<string, decimal>[] SelectSum(MatchCollection matches)
			{
				return matches
					.Select(a => new Tuple<string, decimal>(
						a.Groups["currency"].Value,
						decimal.Parse(a.Groups["amount"].Value, CultureInfo.InvariantCulture)
					))
					.ToArray()
				;
			}


			var fares = SelectSum(fareMatches);

			var equalFares = SelectSum(equalFareMatches);

			var totals = SelectSum(totalMatches);

			var commissions = commissionMatches.Select(a => new
			{
				Amount = a.Groups["amount"].Value.As().Decimaln,
				IsMoney = a.Groups["ismoney"].Success,
			}).ToArray();


			var paymentTypes = paymentTypeMatches.Select(a => AirParser.ParsePaymentType(a.Groups["type"].Value)).ToArray();


			var ticketIndex = 0;


			for (int i = 0, len = passengerGroups.Length; i < len; i++)
			{

				var passengerGroup = passengerGroups[i];

				var fareCurrency = fares.By(i).As(a => a.Item1);
				var fareAmount = fares.By(i).As(a => a.Item2);
				var equalFareCurrency = equalFares.By(i).As(a => a.Item1);
				var equalFareAmount = equalFares.By(i).As(a => a.Item2);
				var totalCurrency = totals.By(i).As(a => a.Item1);
				var totalAmount = totals.By(i).As(a => a.Item2);
				var paymentType = paymentTypes.By(i);
				var segments = segmentBlocks.By(i);
				var fees = feeBlocks.By(i);
				var commission = commissions.By(i);
				var airlineIataCode = airlines.By(i);// ?? "PS";


				foreach (var passengerName in passengerGroup)
				{

					AviaDocument doc = null;

					if (ticketIndex < tickets.Count)
					{
						tickets[ticketIndex].Groups["dt"].Do(g =>
						{
							if (g.Value.StartsWith("DT"))
								doc = new AviaMco();
						});
					}


					doc = doc ?? new AviaTicket();

					doc.Originator = GdsOriginator.Amadeus;
					doc.Origin = ProductOrigin.AmadeusAir;
					doc.BookerOffice = bookingOffice;
					doc.TicketerOffice = issuingOffice;
					doc.TicketerCode = ticketerCode;


					doc.IssueDate = issueDate;
					doc.PnrCode = pnrCode;
					doc.PassengerName = passengerName;
					doc.AirlineIataCode = airlineIataCode;

					doc.Fare = fareCurrency != null ? new Money(fareCurrency, fareAmount) : null;

					doc.EqualFare = equalFareCurrency != null ? new Money(equalFareCurrency, equalFareAmount) : null;

					doc.FeesTotal = totalCurrency == equalFareCurrency && totalAmount >= equalFareAmount
						? new Money(totalCurrency, totalAmount - equalFareAmount)
						: null;

					doc.Total = totalCurrency != null ? new Money(totalCurrency, totalAmount) : null;

					doc.PaymentType = paymentType;


					if (commission != null)
					{

						if (commission.IsMoney)
						{
							doc.Commission = new Money(totalCurrency, commission.Amount ?? 0);
						}
						else if (commission.Amount.HasValue)
						{
							doc.CommissionPercent = commission.Amount;
							doc.Commission = new Money(totalCurrency, Math.Round(equalFareAmount * commission.Amount.Value / 100m, 2));
						}
					}


					if (ticketIndex < tickets.Count)
					{

						tickets[ticketIndex].Groups["airline"].Do(g =>
							doc.AirlinePrefixCode = g.Value
						);

						tickets[ticketIndex].Groups["number"].Do(g =>
							doc.Number = g.Value
						);

						tickets[ticketIndex].Groups["iataOffice"].Do(g =>
							doc.TicketingIataOffice = g.Value.Replace("\r", null).Replace("\n", null).Replace("\t", null).Replace(" ", null)
						);

					}


					if (fees.Yes())
					{
						foreach (var f in fees)
						{
							doc.AddFee(f.Code, new Money(f.Currency, f.Amount), updateTotal: false);
						}
					}


					var ticket = doc as AviaTicket;

					if (ticket && segments.Yes())
					{
						ParseSegments(ticket, segments);
					}


					if (doc.AirlineIataCode.No())
						doc.AirlineIataCode = "PS";


					ticketIndex++;

					yield return doc;

				}

			}




			void ParseSegments(AviaTicket ticket, string[] segments)
			{

				for (int j = 0, jlen = segments.Length; j < jlen; j++)
				{

					var ss = segments[j];

					if (ss.No() || ss.Length < 16)
						continue;


					var ss2 = segments2.By(ticket.Segments.Count);


					var time1 = ss.AsSubstring(14, 10).As().ToDateTimen("ddMMM HHmm");
					var time2 = time1?.Date + ss2.AsSubstring(42, 4).As().ToTimeSpann("hhmm");


					if (time1 != null && time1 < issueDate)
					{
						time1 = time1.Value.AddYears(issueDate.Year - time1.Value.Year + 1);
					}


					if (time2 != null && time2 < issueDate)
					{
						time2 = time2.Value.AddYears(issueDate.Year - time2.Value.Year + 1);
					}


					if (ss2.AsSubstring(46, 1) == "+")
					{
						time2 = time2?.AddDays(ss2.AsSubstring(47, 1).As().Int32);
					}



					var seg = new FlightSegment
					{

						Position = ticket.Segments.Count + 1,
						FromAirportCode = ss.AsSubstring(0, 3).Clip(),
						ToAirportCode = (segments.By(j + 1) ?? segments[0]).AsSubstring(0, 3).Clip(),
						CarrierIataCode = ss.AsSubstring(4, 2).Clip(),
						FlightNumber = ss.AsSubstring(7, 4).Clip(),
						ServiceClassCode = ss.AsSubstring(12, 1).Clip(),
						FareBasis = ss.AsSubstring(29, 15).Clip(),
						DepartureTime = time1,
						ArrivalTime = time2,
						//Duration = time1 != null && time2 != null ? (time2.Value - time1.Value).ToString("hh\\:mm") : null,
						Luggage = ss.AsSubstring(56, 3).Clip(),

					};


					ticket.AddSegment(seg);


					if (ticket.AirlineIataCode.No())
						ticket.AirlineIataCode = seg.CarrierIataCode;

				}

			}

		}



		//---g



		static readonly Regex _reBase = new Regex(
			@"(^|\n)\s*RP/(?'bookingOffice'[\w\d]+)/(?'issuingOffice'[\w\d]+)\s+(?'ticketerCode'[\w\d]+)/(?:[\w\d]+).*?\s+(?'issueDate'\d+\w\w\w\d\d)/(?'issueTime'\d\d\d\d)Z\s*(?'pnrCode'[\dA-Z]{6})\s*\n+",
			RegexOptions.Compiled
		);


		static readonly Regex _rePassengerGroups = new Regex(
			@"(^|\n)\s*T-\w?\s*\n+" +
			@"(?:.+?\n+)??" +
			@"(?:\s*\d+\.(?'passenger'[A-Z/\s]+)(\([A-Z\d/\s]+\))*?)+\s*\n",
			RegexOptions.Compiled
		);



		static readonly Regex _reTicket = new Regex(
			@"\n\s+\d+\s+FA PAX\s+(?:(?'airline'\d{3})-(?'number'\d{10,})(?:-\d\d)?/(?'dt'.+?)/(?:.+?/)?(?:\d\d\w\w\w\d\d)/(?:[A-Z\d]+)/(?'iataOffice'[\d\s\n]+?)/)?",
			RegexOptions.Compiled
		);

		static readonly Regex _reFare = new Regex(
			@"\n\s*FARE\s+\w*\s+(?'currency'[A-Z]{3})\s+(?'amount'\d+(?:.\d\d)?)\s*\n",
			RegexOptions.Compiled
		);

		static readonly Regex _reEqualFare = new Regex(
			@"\n\s*EQUIV\s+(?'currency'[A-Z]{3})\s+(?'amount'\d+(?:.\d\d)?)\s*\n",
			RegexOptions.Compiled
		);

		static readonly Regex _reTotal = new Regex(
			@"\n\s*GRAND TOTAL\s+(?'currency'[A-Z]{3})\s+(?'amount'\d+(?:.\d\d)?)\s*\n",
			RegexOptions.Compiled
		);

		static readonly Regex _reCommission = new Regex(
			@"\n\s*\d+\.\s*FM \*[A-Z]\*(?'amount'\d+(?:\.\d+?)?)(?'ismoney'A)?\s*(?:\n|$)",
			RegexOptions.Compiled
		);

		static readonly Regex _rePaymentType = new Regex(
			@"\n\s*\d+\.\s*FP (/O)?(?'type'CASH|INVOICE|INV|CHECK).*?(?:\n|$)",
			RegexOptions.Compiled
		);

		static readonly Regex _reAirline = new Regex(
			@"\n\s*\d+\.\s*FV\s+(\*\w\*)?(?'iataCode'[A-Z\d]{2})\s*(?:\n|$)",
			RegexOptions.Compiled
		);

		static readonly Regex _reTicketBlocks = new Regex(
			@"\n\s*T-(.|\n)*?\n(?'segments'\s*\d+\s+(.|\n)*?)\n\s*(?'sums'FARE (.|\n)+?)TST\d+",
			RegexOptions.Compiled | RegexOptions.Multiline
		);

		static readonly Regex _reSegments = new Regex(
			@"(^|\n)(\s+(\d+)\s+[OX]?\s+|(\t\s|\s|\t)+?)(?'line'.+?)($|\n)",
			RegexOptions.Compiled | RegexOptions.Multiline
		);

		static readonly Regex _reSegments2 = new Regex(
			@"(^|\n)\s\s\d\s\s(?'line'\w[\w\d][\s\d]\d\d\d\s\w\s\d\d\w\w\w.+?)($|\n)",
			RegexOptions.Compiled | RegexOptions.Multiline
		);

		static readonly Regex _reFees = new Regex(
			@"TX\d{3} X (?'currency'\w{3})\s+(?'amount'\d+\.?\d*)-?(?'code'\w\w)",
			RegexOptions.Compiled | RegexOptions.Multiline
		);



		//---g

	}






	//===g



}
