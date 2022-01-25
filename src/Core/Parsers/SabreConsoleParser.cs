using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Parsers
{



	//===g






	public class SabreConsoleParser
	{

		//---g




		public static IEnumerable<AviaDocument> Parse(string content, Currency defaultCurrency)
		{

			if (content.No())
				throw new GdsImportException("Empty content");


			var parser = new SabreConsoleParser
			{
				Content = content,
				DefaultCurrency = defaultCurrency,
			};


			return parser.ParseTickets();

		}



		//---g



		public Currency DefaultCurrency { get; set; }

		public string Content { get; set; }



		//---g



		private IEnumerable<AviaDocument> ParseTickets()
		{

			var headerAndMaskListMatch = _reHeaderAndMaskList.Match(Content);

			if (!headerAndMaskListMatch.Success)
				yield break;



			var headerMatch = _reHeader.Match(headerAndMaskListMatch.Groups["header"].Value);

			if (!headerMatch.Success)
				yield break;


			var pnrCode = headerMatch.Groups["pnrCode"].Value;



			var passengersMatches = _reHeaderPassangers.Matches(headerMatch.Groups["passengersAndSegments"].Value);

			if (passengersMatches.No())
				yield break;


			var passengers = passengersMatches.ToArray(a => new
			{
				No = a.Groups["passengerNo1"].Value + a.Groups["passengerNo2"].Value,
				Name = a.Groups["passenger"].Value,
			});



			var headerSegmentsMatches = _reHeaderSegments.Matches(headerMatch.Groups["passengersAndSegments"].Value);

			if (headerSegmentsMatches.No())
				yield break;


			var headerSegments = headerSegmentsMatches.ToArray(a => new
			{

				No = a.Groups["segmentNo"].Value,//.As().Int,

				AirlineIataCode = a.Groups["airline"].Value,
				FlightNo = a.Groups["flightNo"].Value,
				ServiceClassCode = a.Groups["class"].Value,

				FromAirportCode = a.Groups["fromAirport"].Value,
				ToAirportCode = a.Groups["toAirport"].Value,

				DepartureDate = a.Groups["departureDate"].Value,
				DepartureTime = a.Groups["departureTime"].Value,
				ArrivalDate = a.Groups["arrivalDate"].Value.Clip(),
				ArrivalTime = a.Groups["arrivalTime"].Value,

			});



			var masksMatches = _reMasks.Matches(headerAndMaskListMatch.Groups["masks"].Value);

			if (masksMatches.No())
				yield break;



			var masks = masksMatches.ToArray(maskMatch =>
			{

				var body = maskMatch.Groups["body"].Value.Trim();

				var issueMatch = _reMaskIssue.Match(body);

				var faresMatch = _reMaskFares.Match(body);

				var feeListMatch = _reMaskFeeList.Match(body);

				var feeAmounts = feeListMatch.Groups["amount"].Captures.ToArray();

				var feeCodes = feeListMatch.Groups["code"].Captures.ToArray();

				var segmentsMatches = _reMaskSegments.Matches(body);


				return new
				{


					No = maskMatch.Groups["maskNo"].Value,


					IssueDate = issueMatch.Groups["issueDate"].Value.As().ToDateTime("ddMMMyy"),

					Office = issueMatch.Groups["office"].Value,

					Agent = issueMatch.Groups["agent"].Value,


					Fare = new Money(
						faresMatch.Groups["fareCurrency"].Value,
						faresMatch.Groups["fare"].Value.As().Decimal
					),

					EqualFare = new Money(
						faresMatch.Groups["equalFareCurrency"].Value,
						faresMatch.Groups["equalFare"].Value.As().Decimal
					),

					FeesTotal = new Money(
						faresMatch.Groups["equalFareCurrency"].Value,
						faresMatch.Groups["feesTotal"].Value.As().Decimal
					),

					Total = new Money(
						faresMatch.Groups["totalCurrency"].Value,
						faresMatch.Groups["total"].Value.As().Decimal
					),


					Fees = feeAmounts.ToArray((amount, i) => new
					{
						Code = feeCodes.By(i).Value,
						Amount = amount.Value.As().Decimal,
					}),


					Segments = segmentsMatches.ToArray(a => new
					{

						No2 = a.Groups["segmentNo"].Value,//.As().Int,

						Stopover = a.Groups["stopover"].Value == "X",

						FromAirportCode = a.Groups["airport"].Value,
						CarrierIataCode = a.Groups["airline"].Value,
						FlightNumber = a.Groups["flightNo"].Value,
						ServiceClassCode = a.Groups["class"].Value,

						DepartureTime = a.Groups["departureTime"].Value.PadLeft(4, '0'),
						DepartureDate = a.Groups["departureDate"].Value,
						//ArrivalDate = a.Groups["arrivalDate"].Value,

						FareBasis = a.Groups["fareBasis"].Value,
						Luggage = a.Groups["luggage"].Value,

					}),


				};


			});



			var maskListMatches = _reMaskList.Matches(headerAndMaskListMatch.Groups["maskList"].Value);

			if (maskListMatches.No())
				yield break;


			var maskList = maskListMatches.ToArray(a => new
			{
				PassengerNo = a.Groups["passengerNo"].Value,
				MaskNo = a.Groups["maskNo"].Value,
			});



			foreach (var passengerMask in maskList)
			{

				var passenger = passengers.By(a => a.No == passengerMask.PassengerNo);
				var mask = masks.By(a => a.No == passengerMask.MaskNo);


				var doc = new AviaTicket
				{

					IssueDate = mask.IssueDate,

					PnrCode = pnrCode,
					PassengerName = passenger.Name.Trim(),

					Origin = ProductOrigin.SabreTerminal,
					Originator = GdsOriginator.Sabre,

					BookerOffice = mask.Office,
					BookerCode = mask.Agent,

					Fare = mask.Fare.Clone(),
					EqualFare = mask.EqualFare.Clone(),
					//FeesTotal = mask.FeesTotal.Clone(),
					Total = mask.Total.Clone(),

				};


				foreach (var fee in mask.Fees)
				{
					doc.AddFee(fee.Code, new Money(doc.EqualFare.Currency, fee.Amount));
				}


				var position = 0;

				foreach (var mseg in mask.Segments)
				{

					var hseg = headerSegments.By(a =>
						a.FromAirportCode == mseg.FromAirportCode &&
						a.DepartureDate == mseg.DepartureDate &&
						a.DepartureTime == mseg.DepartureTime
					);


					var seg = new FlightSegment
					{

						Position = position++,

						Stopover = mseg.Stopover,

						CarrierIataCode = mseg.CarrierIataCode,
						FlightNumber = mseg.FlightNumber,
						ServiceClassCode = mseg.ServiceClassCode,

						FromAirportCode = mseg.FromAirportCode,
						ToAirportCode = hseg.ToAirportCode,
						DepartureTime = (mseg.DepartureDate + mseg.DepartureTime).As().ToDateTimen("ddMMMHHmm"),
						ArrivalTime = ((hseg.ArrivalDate ?? mseg.DepartureDate) + hseg.ArrivalTime).As().ToDateTimen("ddMMMHHmm"),

						FareBasis = mseg.FareBasis,
						Luggage = mseg.Luggage,

					};


					doc.AddSegment(seg);

				}


				yield return doc;

			}


		}



		//---g



		static readonly Regex _reHeaderAndMaskList = new Regex(
			@"(?<header>.+?)\*PQS«(?<maskList>.+?)(?=\*PQ\d«)(?<masks>.+)",
			RegexOptions.Singleline | RegexOptions.Compiled
		);


		static readonly Regex _reHeader = new Regex(
			@"\s*(?<pnrCode>\w+)\s*(?<passengersAndSegments>.+?)PHONES.+?REMARKS(?<remarks>.*)",
			RegexOptions.Singleline | RegexOptions.Compiled
		);

		static readonly Regex _reHeaderPassangers = new Regex(
			@"\s*(?<passengerNo1>\d\.)(?:\w\/)?(?<passengerNo2>\d)(?<passenger>.+?)(?=[\d*])",
			RegexOptions.Singleline | RegexOptions.Compiled
		);

		static readonly Regex _reHeaderSegments = new Regex(
			@"\s*(?<segmentNo>\d)\s+(?<airline>[\w\d]{2})\s*(?<flightNo>\d+)(?<class>\w)\s+(?<departureDate>\d\d\w\w\w)\s+\d+\s+(?<fromAirport>\w\w\w)(?<toAirport>\w\w\w).+?\s(?<departureTime>\d\d\d\d)\s+(?<arrivalTime>\d\d\d\d)\s+(?<arrivalDate>\d\d\w\w\w)?",
			RegexOptions.Multiline | RegexOptions.Compiled
		);


		static readonly Regex _reMaskList = new Regex(
			@"^\s+(?<passengerNo>\d\.\d)\s.+?\s(?<maskNo>\d+)\s",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reMasks = new Regex(
			@"\*PQ(?<maskNo>\d)«(?<body>.+?)(?=(?:(?:\*PQ\d«)|$))",
			RegexOptions.Singleline | RegexOptions.Compiled
		);


		static readonly Regex _reMaskFares = new Regex(
			@"^\s*(?<fareCurrency>\w\w\w)(?<fare>\d+(?:\.\d+)?)\s+(?<equalFareCurrency>\w\w\w)(?<equalFare>\d+(?:\.\d+)?)\s+(?<feesTotal>\d+(?:\.\d+)?)XT\s+(?<totalCurrency>\w\w\w)(?<total>\d+(?:\.\d+)?)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reMaskFeeList = new Regex(
			@"XT BREAKDOWN(?:\s+(?<amount>\d+(\.\d+)?)(?<code>\w+))+",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		//static readonly Regex _reFees = new Regex(
		//	@"\s+(?<amount>\d+(\.\d+)?)(?<code>\w+)",
		//	RegexOptions.Singleline | RegexOptions.Compiled
		//);


		static readonly Regex _reMaskSegments = new Regex(
			@"^\s*(?<segmentNo>\d\d)\s+(?<stopover>\w)\s+(?<airport>[\w\d]{3})\s+(?<airline>[\w\d]{2})\s*(?<flightNo>\d+)(?<class>\w)\s+(?<departureDate>\d\d\w\w\w)\s+(?<departureTime>\d+)\s+(?<fareBasis>[\w\d\/]+)\s+(?:\d\d\w\w\w\d\d)?(?:\d\d\w\w\w\d\d)\s+(?<luggage>[\w\d]+)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);


		static readonly Regex _reMaskIssue = new Regex(
			@"^\s*(?<office>\w+?)\s+\w+?\s\*A(?<agent>\w+)\s(?<issueTime>\d\d\d\d)\/(?<issueDate>\d\d\w\w\w\d\d)\s",
			RegexOptions.Multiline | RegexOptions.Compiled
		);



		//---g

	}






	//===g



}
