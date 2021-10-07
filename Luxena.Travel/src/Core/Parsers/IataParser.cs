using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{
	public class IataParser
	{
		public static void ParseTicketFares(AviaTicket ticket, string ticketFare)
		{
			if (ticketFare == null)
				return;

			var match = _mainPattern.Match(ticketFare);

			if (!match.Success)
				return;

			var fareSegments = ticket.Segments.Where(a => a.Type == 0).ToList();

			var carriers = match.Groups["carrier"].Captures;
			
			//if (carriers.Count > fareSegments.Count)
			//	return;

			var sideTrips = match.Groups["sideTrip"].Captures;
			var surcharges = match.Groups["surcharges"].Captures;
			var fares = match.Groups["fare"].Captures;
			var stopovers = match.Groups["stopover"].Captures;
			var currency = match.Groups["currency"].Value;
			var total = match.Groups["total"].Value;


			var fareSegmentIndex = 0;
			for (var i = 0; i < carriers.Count; ++i)
			{
				var carrier = carriers[i].Value;

				if (carrier == "//" || carrier == "/-")
					continue;

				if (fareSegmentIndex >= fareSegments.Count) return;
				var segment = fareSegments[fareSegmentIndex];

				var surcharge = _extractAmountsPattern.Matches(surcharges[i].Value);

				if (surcharge.Count != 0)
				{
					segment.Surcharges = 0;
					for (var j = 0; j < surcharge.Count; ++j)
						segment.Surcharges += decimal.Parse(surcharge[j].Value, CultureInfo.InvariantCulture);
				}

				var fare = fares[i].Value.Trim();

				segment.IsInclusive = fare.StartsWith("M");
				if (!segment.IsInclusive && fare.Yes())
					segment.Fare = decimal.Parse(fare, CultureInfo.InvariantCulture);

				if (stopovers[i].Value.Yes())
				{
					var stopoverMatch = _extractAmountsPattern.Match(stopovers[i].Value);
					if (stopoverMatch.Success)
						segment.StopoverOrTransferCharge = decimal.Parse(stopoverMatch.Value, CultureInfo.InvariantCulture);
				}

				fareSegmentIndex++;

				if (sideTrips[i].Value.Yes())
					ParseFareSegmentSideTrips(sideTrips[i].Value, fareSegments, ref fareSegmentIndex);

			}

			if (currency.Yes())
			{
				if (currency == "NU" || currency == "UC") 
					currency = "NUC";
				var dtotal = total.No() ? 0m : decimal.Parse(total, CultureInfo.InvariantCulture);

				ticket.FareTotal = new Money(currency, dtotal);
			}
		}


		private static void ParseFareSegmentSideTrips(string inputValue, List<FlightSegment> fareSegments, ref int fareSegmentIndex)
		{
			var match = _sideTripPattern.Match(inputValue);
			if (!match.Success) return;

			var carriers = match.Groups["carrier"].Captures;
			var surcharges = match.Groups["surcharges"].Captures;
			var fares = match.Groups["fare"].Captures;

			for (var i = 0; i < carriers.Count; ++i)
			{
				var carrier = carriers[i].Value;

				if (carrier == "//" || carrier == "/-")
					continue;

				if (fareSegmentIndex >= fareSegments.Count) return;
				var segment = fareSegments[fareSegmentIndex++];
				segment.IsSideTrip = true;

				var surcharge = _extractAmountsPattern.Matches(surcharges[i].Value);

				if (surcharge.Count != 0)
				{
					segment.Surcharges = 0;
					for (var j = 0; j < surcharge.Count; ++j)
						segment.Surcharges += decimal.Parse(surcharge[j].Value, CultureInfo.InvariantCulture);
				}

				var fare = fares[i].Value.Trim();
				if (fare.Yes())
					segment.Fare = decimal.Parse(fare, CultureInfo.InvariantCulture);
			}
		}


		private static readonly Regex _mainPattern = new Regex(
			@"(?:I[- ])?"
			+ @"(?'from'(?:[A-Z]{3})+)"
			+ @"(?:"
				+ @"\s*"
				+ @"(?:"
					+ @"(?'carrier'/[-/])|"
					+ @"(?'carrier'[A-Z\d]{2})(?:\([^)]{2,}\))?"
				+ @")"
				+ @"\s*"
				+ @"(?'flags'(?:[A-Z]/)*)"
				+ @"(?'destination'(?:[A-Z]{3})+)"
				+ @"(?'nonMandatoryExtraMilage'(?: E/XXX)?)"
				+ @"(?'sideTrip'(?:\(.*?\))?)"
				+ @"(?'surcharges'(?:\s*Q\d+\.\d\d|\s*Q\d{3,})*)"
				+ @"(?'miles'(?:\s*\d*M)?)"
				+ @"(?'fare'(?:\s*M/(?:IT|BT)?|\s*\d+\.\d\d)?|\s*\d{3,})"
				+ @"(?'fareCode'(?:[A-Z]{2}[A-Z/\d]+ )?)"
				+ @"(?'stopover'(?:\s*\d*S\d+\.\d\d)?)"
			+ @")+"
			+ @"(?:"
				+ @"\s*(?'currency'[A-Z]{3}|NU|UC)"
				+ @"\s*(?'total'\d+\.\d\d|\d{3,})"
			+ @")?"
			+ @"\s*END"
			, RegexOptions.Compiled
		);

		private static readonly Regex _sideTripPattern = new Regex(
			@"\(?(?:"
				+ @" ?"
				+ @"(?:"
					+ @"(?'carrier'/[-/])|"
					+ @"(?'carrier'[A-Z\d]{2})(?:\([^)]{2,}\))?"
				+ @")"
				+ @" ?"
				+ @"(?'flags'(?:[A-Z]/)*)"
				+ @"(?'destination'(?:[A-Z]{3})+)"
				+ @"(?'surcharges'(?: ?Q\d+\.\d\d)*)"
				+ @"(?'fare'(?: ?M/(?:IT|BT)?| ?\d+\.\d\d)?)"
			+ @")*\)?"
			, RegexOptions.Compiled
		);

		private static readonly Regex _extractAmountsPattern = new Regex(@"(\d+\.\d\d|\d{3,})", RegexOptions.Compiled);
	}
}