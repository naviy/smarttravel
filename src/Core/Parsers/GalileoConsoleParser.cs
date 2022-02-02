using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Parsers
{



	//===g






	public class GalileoConsoleParser
	{

		//---g



		public static IEnumerable<AviaDocument> Parse(string content, Currency defaultCurrency)
		{

			if (content.No())
				throw new GdsImportException("Empty content");


			var parser = new GalileoConsoleParser
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

			//---g


			var headerAndMaskListMatchAndMasks = _reHeaderAndMaskListAndMasks.Match(Content);

			if (!headerAndMaskListMatchAndMasks.Success)
				yield break;


			var headerContent = headerAndMaskListMatchAndMasks.Groups["header"].Value;
			var maskListContent = headerAndMaskListMatchAndMasks.Groups["maskList"].Value;
			var masksContent = headerAndMaskListMatchAndMasks.Groups["masks"].Value;


			if (headerContent.No() || maskListContent.No())
				yield break;


			var headerCodesMatch = _reHeaderCodes.Match(headerContent);

			if (!headerCodesMatch.Success)
				yield break;


			var headerPassengersMatches = _reHeaderPassengers.Matches(headerContent);

			if (headerPassengersMatches.No())
				yield break;


			var headerSegmentsMatches = _reHeaderSegments.Matches(headerContent);

			if (!headerSegmentsMatches.No())
				yield break;



			var headerTicketsMatches = _reHeaderSegments.Matches(headerContent);

			if (!headerSegmentsMatches.No())
				yield break;



			var issueDate = headerCodesMatch.Groups["issueDate"].Value.As().ToDateTimen("ddMMMyyyy") ?? DateTime.Today;
			var pnrCode = headerCodesMatch.Groups["pnrCode"].Value;
			var tourCode = headerCodesMatch.Groups["tourCode"].Value;
			var officeCode = headerCodesMatch.Groups["officeCode"].Value;
			var agentCode = headerCodesMatch.Groups["agentCode"].Value;


			var headerPassengers = headerPassengersMatches.ToArray(m => new
			{
				No = m.Groups["no"].Value,
				Name = m.Groups["name"].Value,
			});


			if (headerPassengers.No())
				yield break;


			var headerSegments = headerSegmentsMatches.ToArray(a => new
			{

				AirlineIataCode = a.Groups["airline"].Value,
				FlightNo = a.Groups["flightNo"].Value,
				ServiceClassCode = a.Groups["serviceClass"].Value,

				FromAirportCode = a.Groups["fromAirport"].Value,
				ToAirportCode = a.Groups["toAirport"].Value,

				DepartureDate = a.Groups["departureDate"].Value,
				DepartureTime = a.Groups["departureTime"].Value,

				ArrivalDateOffset = a.Groups["arrivalDateOffset"].Value.As().Int,
				ArrivalTime = a.Groups["arrivalTime"].Value,

			});


			if (headerSegments.No())
				yield break;


			var headerTickets = _reHeaderTickets.Matches(headerContent).ToArray(a => new
			{
				AirlinePrefixCode = a.Groups["airlinePrefixCode"].Value,
				Number = a.Groups["number"].Value,
				PassengerNo = a.Groups["passengerNo"].Value,
			});


			//---g


			var maskList = _reMaskList.Matches(maskListContent).ToArray(a => new
			{
				MaskNo = a.Groups["maskNo"].Value,
				Passenger = a.Groups["passanger"].Value,
			});


			var masks0 = _reMasks.Matches(masksContent).ToArray(a => new
			{
				No = a.Groups["no"].Value,
				Body = a.Groups["body"].Value,
			});


			if (maskList.No() && masks0.No())
				yield break;


			if (masks0.No())
			{
				maskList = headerPassengers.ToArray(a => new { MaskNo = "1", Passenger = a.Name });
				masks0 = new[] { new { No = "1", Body = maskListContent } };
			}


			var masks = masks0.ToArray(m0 =>
			{

				var fareMatch = _reMaskFare.Match(m0.Body);
				var equalFareMatch = _reMaskEqualFare.Match(m0.Body);
				var feesMatches = _reMaskFee.Matches(m0.Body);
				var totalMatch = _reMaskTotal.Match(m0.Body);


				return new
				{

					No = m0.No,


					Fare = new Money(
						fareMatch.Groups["currency"].Value,
						fareMatch.Groups["amount"].Value.As().Decimal
					),

					EqualFare = new Money(
						equalFareMatch.Groups["currency"].Value,
						equalFareMatch.Groups["amount"].Value.As().Decimal
					),

					Fees = feesMatches.ToArray(fee => new
					{
						Code = fee.Groups["code"].Value,
						Currency = fee.Groups["currency"].Value,
						Amount = fee.Groups["amount"].Value.As().Decimal,
					}),

					Total = new Money(
						totalMatch.Groups["currency"].Value,
						totalMatch.Groups["amount"].Value.As().Decimal
					),


					Segments = _reMaskSegments.Matches(m0.Body).ToArray(seg => new
					{

						FromAirportCode = seg.Groups["fromAirport"].Value,
						ToAirportCode = seg.Groups["toAirport"].Value,
						CarrierIataCode = seg.Groups["carrier"].Value,
						FlightNumber = seg.Groups["flightNo"].Value,
						ServiceClassCode = seg.Groups["serviceClass"].Value,

						DepartureDate = seg.Groups["departureDate"].Value,
						DepartureTime = seg.Groups["departureTime"].Value.PadLeft(4, '0'),

						FareBasis = seg.Groups["fareBasis"].Value,
						Luggage = seg.Groups["luggage"].Value,

					}),

				};
			});



			foreach (var maskListItem in maskList)
			{

				var passenger = headerPassengers.By(a => Person.PassengerNamesIsEquals(a.Name, maskListItem.Passenger));

				if (passenger == null)
					continue;


				var mask = masks.By(a => a.No == maskListItem.MaskNo);

				if (mask == null)
					continue;


				var headerTicket = headerTickets.By(a => a.PassengerNo == passenger.No);



				var doc = new AviaTicket
				{

					IssueDate = issueDate,

					AirlinePrefixCode = headerTicket?.AirlinePrefixCode,
					Number = headerTicket?.Number,

					PnrCode = pnrCode,
					TourCode = tourCode,

					PassengerName = passenger.Name.Trim(),

					Origin = ProductOrigin.GalileoTerminal,
					Originator = GdsOriginator.Galileo,

					BookerOffice = officeCode,
					BookerCode = agentCode,

					Fare = mask.Fare.Clone(),
					EqualFare = mask.EqualFare.Clone(),
					Total = mask.Total.Clone(),

				};


				foreach (var fee in mask.Fees)
				{
					doc.AddFee(fee.Code, new Money(fee.Currency, fee.Amount));
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

						//Stopover = mseg.Stopover,

						CarrierIataCode = mseg.CarrierIataCode,
						FlightNumber = mseg.FlightNumber,
						ServiceClassCode = mseg.ServiceClassCode,

						FromAirportCode = mseg.FromAirportCode,
						ToAirportCode = hseg.ToAirportCode,

						DepartureTime = (mseg.DepartureDate + mseg.DepartureTime).As().ToDateTimen("ddMMMHHmm"),
						ArrivalTime = (mseg.DepartureDate + hseg.ArrivalTime).As().ToDateTimen("ddMMMHHmm")?.AddDays(hseg.ArrivalDateOffset),

						FareBasis = mseg.FareBasis,
						Luggage = mseg.Luggage,

					};


					doc.AddSegment(seg);

				}


				yield return doc;

			}


			//---g

		}



		//---g



		static readonly Regex _reHeaderAndMaskListAndMasks = new Regex(
			@"(?<header>--- TST ---.*?)(?<maskList>>[Tt][Qq][Tt].*?)(?=(?:>[Tt][Qq][Tt])|$)(?<masks>.*)",
			RegexOptions.Singleline | RegexOptions.Compiled
		);


		static readonly Regex _reHeaderCodes = new Regex(
			@"--- TST ---\n(?:.*?:(?<pnrCode>[\w\d]+).*?(?::(?<tourCode>[\w\d]+))?)\n(.*?\s+(?<officeCode>[\w\d]+)_(?<agentCode>[\w\d]+)\s+(?<issueDate>\d\d\w\w\w\d\d\d\d))?",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reHeaderPassengers = new Regex(
			@"(?<no>\d+)\.\s+(?<name>.+?)(?>$|(?:\s+\(CNN\)))",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reHeaderSegments = new Regex(
			@"^\s+\d+\s+(?<airline>[\w\d]{2})(?<flightNo>\d+)\s+(?<serviceClass>[\w\d]+)\s+(?<departureDate>\d\d\w\w\w)\s.+?\s(?<fromAirport>[\w\d]{3})(?<toAirport>[\w\d]{3})\s.+?(?<departureTime>\d{4})\s+(?<arrivalTime>\d{4})(?<arrivalDateOffset>\+\d+)?",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reHeaderTickets = new Regex(
			@"^\s+\d+\s+.*?(?<airlinePrefixCode>\d\d\d)(?<number>\d{10})\dS\d+\-\d+\.P(?<passengerNo>\d+)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);


		static readonly Regex _reMaskList = new Regex(
			@"^(?<maskNo>\d*)[ ]+(?<passanger>\w.*?)\s+\[",
			RegexOptions.Multiline | RegexOptions.Compiled
		);


		static readonly Regex _reMasks = new Regex(
			@">[Tt][Qq][Tt]\/t(?<no>\d+)(?<body>.*?)(?=(?:>[Tt][Qq][Tt])|\z)",
			RegexOptions.Singleline | RegexOptions.Compiled
		);


		static readonly Regex _reMaskSegments = new Regex(
			@"^\s+(?<fromAirport>[\w\d]{3})-(?<toAirport>[\w\d]{3})\s+(?<carrier>[\w\d]{2})\s+(?<flightNo>\d{1,4})\s*(?<serviceClass>\w+)\s+(?<departureDate>\d\d\w\w\w)\s+(?<departureTime>\d\d\d\d)\s+\w*\s+(?<fareBasis>[\w\d]+)\s+(\d\d\w\w\w)(\d\d\w\w\w)\s+(?<luggage>[\w\d]+)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reMaskFare = new Regex(
			@"^FARE\s+(?<currency>\w+)\s+(?<amount>\d+\.\d\d)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reMaskEqualFare = new Regex(
			@"^EQUIV\s+(?<currency>\w+)\s+(?<amount>\d+\.\d\d)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reMaskFee = new Regex(
			@"^TX\d+\s+(?<currency>\w+)\s+(?<amount>\d+\.\d\d)(?<code>[\w\d]+)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);

		static readonly Regex _reMaskTotal = new Regex(
			@"^TOTAL\s+(?<currency>\w+)\s+(?<amount>\d+\.\d\d)",
			RegexOptions.Multiline | RegexOptions.Compiled
		);



		//---g

	}






	//===g



}
