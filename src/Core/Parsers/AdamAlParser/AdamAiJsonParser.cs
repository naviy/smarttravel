using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Parsers
{



	//===g






	public class AdamAiJsonParser
	{

		//---g



		public static IEnumerable<Entity2> Parse(string json, Currency defaultCurrency/*, string[] robotCodes = null*/)
		{

			if (json.No())
				throw new GdsImportException("Empty AiJson");


			return new AdamAiJsonParser(defaultCurrency/*, robotCodes*/).Parse(json);

		}



		public static IEnumerable<Entity2> Parse(AdamAlJsonData r, Currency defaultCurrency/*, string[] robotCodes = null*/)
		{

			if (r == null)
				throw new GdsImportException("Empty AiJson");


			return new AdamAiJsonParser(defaultCurrency/*, robotCodes*/).Parse(r);

		}



		private AdamAiJsonParser(Currency defaultCurrency/*, string[] robotCodes*/)
		{
			_defaultCurrency = defaultCurrency;
			//_robotCodes = robotCodes ?? Array.Empty<string>();
		}



		//---g



		private IEnumerable<Entity2> Parse(string json)
		{

			var r = JsonConvert.DeserializeObject<AdamAlJsonData>(json);

			if (r == null)
				return null;


			var docs = Parse(r);


			return docs.ToArray();

		}



		private IEnumerable<Entity2> Parse(AdamAlJsonData d)
		{

			var type = d.TypeOfIssue;


			if (type == "Ticket")
			{

				if (IsPasteboardNumber(d.BookingNumber))
				{
					yield return ParsePasteboard(d);
				}
				else
				{
					yield return ParseAviaTicket(d);
				}

			}

			else if (type == "EMD")
			{
				yield return ParseMco(d);
			}

			else if (type == "HotelBooking")
			{
				yield return ParseAccommodation(d);
			}

			else if (type == "Insurance")
			{
				yield return ParseInsurance(d);
			}

			else
			{
				throw new NotImplementedException();
			}

		}



		//---g



		private void ParseProduct<TProduct>(AdamAlJsonData d, TProduct r)
			where TProduct : Product
		{

			r.Originator = GdsOriginator.AdamAI;
			r.Origin = ProductOrigin.AdamAiJson;

			r.IssueDate = ParseDate(d.DateOfIssue);
			r.PnrCode = d.BookingNumber.Clip();
			r.PassengerName = (d.PassengerSurname + "/" + d.PassengerName + d.PassengerTitle.As(a => " " + a))?.ToUpperInvariant();

			r.Fare = ParseMoney(d.GrandTotal);
			r.EqualFare = r.EqualFare.Clone();





		}



		private TProduct ResolveProduct<TProduct>(TProduct r)
			where TProduct : Product
		{
			r.Total = r.GetTotal();
			r.GrandTotal = r.GetGrandTotal();
			return r;
		}



		//---g



		private void ParseAviaDocument<TAviaDocument>(AdamAlJsonData d, TAviaDocument r)
			where TAviaDocument : AviaDocument, new()
		{


			ParseProduct(d, r);

		}



		private AviaTicket ParseAviaTicket(AdamAlJsonData d)
		{

			var r = new AviaTicket();


			ParseAviaDocument(d, r);


			ParseFlightSegment(d).ForEach(r.AddSegment);


			return ResolveProduct(r);

		}



		private IEnumerable<FlightSegment> ParseFlightSegment(AdamAlJsonData d)
		{

			var routes = parseRoutes();


			var flightNumbers = d.FlightNumber.Split(',').Clip();


			for (int i = 0, len = Math.Max(routes.Length, flightNumbers.Length); i < len; i++)
			{

				var route = routes.By(i);

				var seg = new FlightSegment()
				{
					Position = i + 1,
					Type = FlightSegmentType.Ticketed,

					FlightNumber = flightNumbers.By(i),
					FromAirportCode = route.from,
					ToAirportCode = route.to,
				};

				if (i == 0)
				{
					seg.Seat = d.Seat;
					seg.DepartureTime = ParseDateTime(d.DepartureDate);
					seg.ArrivalTime = ParseDateTime(d.ArrivalDate);
				}


				yield return seg;

			}


			(string from, string to)[] parseRoutes()
			{

				var mm = _reRoutes1.Matches(d.Route).ToArray(a => a.Groups[2].Value);

				if (mm.No())
				{
					mm = _reRoutes2.Matches(d.Route).ToArray(a => a.Value);
				}


				if (mm.No())
				{
					return new (string, string)[] { };
				}


				return mm.Skip(1).ToArray((a, i) => (mm[i], a));

			}

		}



		private static readonly Regex _reRoutes1 = new Regex(
			@"(^|-)([A-Z]{3})",
			RegexOptions.Compiled
		);

		private static readonly Regex _reRoutes2 = new Regex(
			@"[A-Z]{3}",
			RegexOptions.Compiled
		);



		private AviaMco ParseMco(AdamAlJsonData d)
		{

			var r = new AviaMco();

			ParseAviaDocument(d, r);


			return ResolveProduct(r);

		}



		//---g



		private Pasteboard ParsePasteboard(AdamAlJsonData d)
		{

			var r = new Pasteboard();

			ParseProduct(d, r);


			return ResolveProduct(r);

		}



		//---g



		private Accommodation ParseAccommodation(AdamAlJsonData d)
		{

			var r = new Accommodation();

			ParseProduct(d, r);


			return ResolveProduct(r);

		}



		private Insurance ParseInsurance(AdamAlJsonData d)
		{

			var r = new Insurance();

			ParseProduct(d, r);


			return ResolveProduct(r);

		}



		//---g




		public Money ParseMoney(string str)
		{
			return ParseMoney(str, _defaultCurrency);
		}


		public static Money ParseMoney(string str, string defaultCurrency)
		{

			if (parseA(_reMoneyGrn, "UAH", out var m))
				return m;

			if (parseA(_reMoneyEUR, "EUR", out m))
				return m;

			if (parseA(_reMoneyPLN, "PLN", out m))
				return m;

			if (parseA(_reMoneyUSD, "USD", out m))
				return m;

			if (parse(_reMoney1, out m))
				return m;

			if (parse(_reMoney2, out m))
				return m;

			if (parseA(_reMoneyA, defaultCurrency, out m))
				return m;


			return new Money(defaultCurrency, 0);


			bool parseA(Regex re, string currency, out Money result)
			{

				var mm = re.Match(str);

				if (!mm.Success)
				{
					result = null;
					return false;
				}


				result = new Money(
					currency,
					mm.Groups["amount"].Value.As().DecimalSafe
				);

				return true;

			}


			bool parse(Regex re, out Money result)
			{

				var mm = re.Match(str);

				if (!mm.Success)
				{
					result = null;
					return false;
				}


				result = new Money(
					mm.Groups["currency"].Value,
					mm.Groups["amount"].Value.As().DecimalSafe
				);

				return true;
			}


		}


		private static readonly Regex _reMoney1 = new Regex(
			@"(?<currency>[A-Z]{3})\s*(?<amount>\d+([.,]\d+)?)",
			RegexOptions.Compiled
		);

		private static readonly Regex _reMoney2 = new Regex(
			@"(?<amount>\d+([.,]\d+)?)\s*(?<currency>[A-Z]{3})",
			RegexOptions.Compiled
		);

		private static readonly Regex _reMoneyA = new Regex(
			@"(?<amount>\d+([.,]\d+)?)",
			RegexOptions.Compiled
		);

		private static readonly Regex _reMoneyGrn = new Regex(
			@"(?<amount>\d+([.,]\d+)?)\s*ГРН",
			RegexOptions.Compiled
		);

		private static readonly Regex _reMoneyPLN = new Regex(
			@"(?<amount>\d+([.,]\d+)?)\s*zł",
			RegexOptions.Compiled
		);

		private static readonly Regex _reMoneyEUR = new Regex(
			@"€\s*(?<amount>\d+([.,]\d+)?)",
			RegexOptions.Compiled
		);

		private static readonly Regex _reMoneyUSD = new Regex(
			@"\$\s*(?<amount>\d+([.,]\d+)?)",
			RegexOptions.Compiled
		);



		//---g



		public DateTime ParseDate(string str)
		{
			return DateTime.ParseExact(str, "dd.MM.yyyy", CultureInfo.InvariantCulture);
		}

		public DateTime ParseDateTime(string str)
		{
			return DateTime.ParseExact(str, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
		}



		//---g



		public bool IsPasteboardNumber(string number)
		{
			return _reIsPasteboardNumber1.IsMatch(number) || _reIsPasteboardNumber2.IsMatch(number);
		}


		private static readonly Regex _reIsPasteboardNumber1 = new Regex(
			@"(\w\d)+-(\w\d)+-(\w\d)",
			RegexOptions.Compiled
		);

		private static readonly Regex _reIsPasteboardNumber2 = new Regex(
			@"(\w)+-(\d)+",
			RegexOptions.Compiled
		);



		//---g



		//private readonly string[] _robotCodes;
		private readonly Currency _defaultCurrency;



		//---g

	}






	//===g



}