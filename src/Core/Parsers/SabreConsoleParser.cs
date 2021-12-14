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


			var pnpCode = headerMatch.Groups["pnpCode"].Value;



			var passengersMatches = _rePassangers.Matches(headerMatch.Groups["passengersAndSegments"].Value);

			if (passengersMatches.No())
				yield break;


			var passengers = passengersMatches
				.Select(a => new
				{
					No = a.Groups["passengerNo1"].Value + a.Groups["passengerNo1"].Value,
					Name = a.Groups["passenger"].Value,
				})
				.ToArray()
			;



			var maskListMatches = _reMaskList.Matches(headerAndMaskListMatch.Groups["maskList"].Value);

			if (maskListMatches.No())
				yield break;


			var maskList = maskListMatches
				.Select(a => new
				{
					PassengerNo = a.Groups["passengerNo"].Value,
					MaskNo = a.Groups["maskNo"].Value,
				})
				.ToArray()
			;



			var masksMatches = _reMasks.Matches(headerAndMaskListMatch.Groups["masks"].Value);

			if (masksMatches.No())
				yield break;


			var masks = masksMatches
				.Select(a => new
				{
					No = a.Groups["maskNo"].Value,
					body = a.Groups["body"].Value,
				})
				.ToArray()
			;


			foreach (var passengerMask in maskList)
			{

				var passenger = passengers.By(a => a.No == passengerMask.PassengerNo);
				var mask = masks.By(a => a.No == passengerMask.MaskNo);


				var doc = new AviaTicket
				{
					PassengerName = passenger.Name,

				};


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

		static readonly Regex _rePassangers = new Regex(
			@"\s*(?<passengerNo1>\d\.)(?:\w\/)?(?<passengerNo2>\d)(?<passenger>.+?)(?=[\d*])",
			RegexOptions.Singleline | RegexOptions.Compiled
		);



		static readonly Regex _reMaskList = new Regex(
			@"^\s+(?<passengerNo>\d\.\d)\s.+?\s(?<maskNo>\d+)\s",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reMasks = new Regex(
			@"\*PQ(?<maskNo>\d)«(?<body>.+?)(?=(?:(?:\*PQ\d«)|$))",
			RegexOptions.Singleline | RegexOptions.Compiled
		);


		static readonly Regex _reAmounts = new Regex(
			@"^\s*(?<fareCurrency>\w\w\w)(?<fare>\d+(?:\.\d+)?)\s+(?<equalFareCurrency>\w\w\w)(?<equalFare>\d+(?:\.\d+)?)\s+(?<fees>\d+(?:\.\d+)?)XT\s+(?<totalCurrency>\w\w\w)(?<total>\d+(?:\.\d+)?)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reFeeList = new Regex(
			@"XT BREAKDOWN(?<fees>(?:\s+(?<amount>\d+(\.\d+)?)(?<code>\w+))+)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reFees = new Regex(
			@"\s+(?<amount>\d+(\.\d+)?)(?<code>\w+)",
			RegexOptions.Singleline | RegexOptions.Compiled
		);


		static readonly Regex _reSegments = new Regex(
			@"\s*(?<segmentNo>\d)\s+(?<airline>[\w\d]{2})\s*(?<flightNo>\d+)(?<class>\w)\s+(?<day>\d\d)(?<month>\w\w\w)\s+\d+\s+(?<fromAirport>\w\w\w)(?<toAirport>\w\w\w).+?\s(?<departureTime>\d\d\d\d)\s+(?<arrivalTime>\d\d\d\d)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);




		//---g

	}






	//===g



}
