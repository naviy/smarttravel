using System;
using System.IO;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Parsers
{
	[TestFixture]
	public class TktParserTests
	{
		[Test]
		public void TestParseTicket1()
		{
			var doc = TktParser.Parse(Res.TktTicket1);

			Assert.IsInstanceOf<AviaTicket>(doc);

			var ticket = (AviaTicket) doc;

			Assert.AreEqual(new DateTime(2009, 7, 20), ticket.IssueDate);

			Assert.AreEqual("YH5FQR", ticket.PnrCode);
			Assert.IsNull(ticket.AirlinePnrCode);

			Assert.IsNull(ticket.BookerOffice);
			Assert.AreEqual("10", ticket.BookerCode);
			Assert.IsNull(ticket.TicketerOffice);
			Assert.AreEqual("10", ticket.TicketerCode);

			Assert.IsNull(ticket.AirlineIataCode);
			Assert.AreEqual("870", ticket.AirlinePrefixCode);

			Assert.AreEqual(4203245920, ticket.Number);
			Assert.AreEqual("TROINIKOVA/KATERYNA MISS*CHDDOB2", ticket.PassengerName);

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreSame(ticket, ticket.Segments[0].Ticket);
			Assert.AreEqual(1, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("LCA", ticket.Segments[0].FromAirportCode);
			Assert.IsNull(ticket.Segments[0].FromAirportName);
			Assert.AreEqual("LCA", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("BEY", ticket.Segments[0].ToAirportCode);
			Assert.IsNull(ticket.Segments[0].ToAirportName);
			Assert.AreEqual("BEY", ticket.Segments[0].ToAirport.Code);
			Assert.IsNull(ticket.Segments[0].CarrierIataCode);
			Assert.IsNull(ticket.Segments[0].CarrierPrefixCode);
			Assert.IsNull(ticket.Segments[0].CarrierName);
			Assert.IsNull(ticket.Segments[0].Carrier);
			Assert.AreEqual("262", ticket.Segments[0].FlightNumber);
			Assert.IsNull(ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 8, 8), ticket.Segments[0].DepartureTime);
			Assert.IsNull(ticket.Segments[0].ArrivalTime);
			Assert.IsNull(ticket.Segments[0].MealCodes);
			Assert.IsNull(ticket.Segments[0].NumberOfStops);
			Assert.IsNull(ticket.Segments[0].Luggage);
			Assert.IsNull(ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.IsNull(ticket.Segments[0].Duration);
			Assert.IsNull(ticket.Segments[0].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("HEE1M1CH", ticket.Segments[0].FareBasis);

			Assert.AreSame(ticket, ticket.Segments[1].Ticket);
			Assert.AreEqual(2, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("BEY", ticket.Segments[1].FromAirportCode);
			Assert.IsNull(ticket.Segments[1].FromAirportName);
			Assert.AreEqual("BEY", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("LCA", ticket.Segments[1].ToAirportCode);
			Assert.IsNull(ticket.Segments[1].ToAirportName);
			Assert.AreEqual("LCA", ticket.Segments[1].ToAirport.Code);
			Assert.IsNull(ticket.Segments[1].CarrierIataCode);
			Assert.IsNull(ticket.Segments[1].CarrierPrefixCode);
			Assert.IsNull(ticket.Segments[1].CarrierName);
			Assert.IsNull(ticket.Segments[1].Carrier);
			Assert.IsNull(ticket.Segments[1].FlightNumber);
			Assert.IsNull(ticket.Segments[1].ServiceClassCode);
			Assert.IsNull(ticket.Segments[1].DepartureTime);
			Assert.IsNull(ticket.Segments[1].ArrivalTime);
			Assert.IsNull(ticket.Segments[1].MealCodes);
			Assert.IsNull(ticket.Segments[1].NumberOfStops);
			Assert.IsNull(ticket.Segments[1].Luggage);
			Assert.IsNull(ticket.Segments[1].CheckInTerminal);
			Assert.IsNull(ticket.Segments[1].CheckInTime);
			Assert.IsNull(ticket.Segments[1].Duration);
			Assert.IsNull(ticket.Segments[1].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[1].Seat);
			Assert.IsNull(ticket.Segments[1].FareBasis);

			Assert.AreEqual(new Money("UAH", 1122.00m), ticket.Fare);
			Assert.IsNull(ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 644.80m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 1766.80m), ticket.Total);

			Assert.AreEqual(6, ticket.Fees.Count);
			Assert.AreEqual(new Money("UAH", 133.90m), ticket.Fees[0].Amount);
			Assert.AreEqual("CY", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 4.20m), ticket.Fees[1].Amount);
			Assert.AreEqual("JW", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 9.40m), ticket.Fees[2].Amount);
			Assert.AreEqual("JX", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 126.90m), ticket.Fees[3].Amount);
			Assert.AreEqual("LB", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 25.40m), ticket.Fees[4].Amount);
			Assert.AreEqual("VL", ticket.Fees[4].Code);
			Assert.AreEqual(new Money("UAH", 345.00m), ticket.Fees[5].Amount);
			Assert.AreEqual("YQ", ticket.Fees[5].Code);

			Assert.AreEqual(new Money("UAH", 100.98m), ticket.Commission);
			Assert.AreEqual(9, ticket.CommissionPercent);
			Assert.AreEqual("Ca", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);

			Assert.IsNull(ticket.TourCode);
		}

		[Test]
		public void TestBadTicket1()
		{
			var doc = TktParser.Parse(Res.TktBadTicket1);

			Assert.IsInstanceOf<AviaTicket>(doc);


			Assert.IsInstanceOf<AviaTicket>(doc);

			var ticket = (AviaTicket) doc;

			Assert.AreEqual(new DateTime(2009, 4, 21), ticket.IssueDate);

			Assert.AreEqual("2UG3S4", ticket.PnrCode);
			Assert.IsNull(ticket.AirlinePnrCode);

			Assert.IsNull(ticket.BookerOffice);
			Assert.AreEqual("17", ticket.BookerCode);
			Assert.IsNull(ticket.TicketerOffice);
			Assert.AreEqual("17", ticket.TicketerCode);

			Assert.IsNull(ticket.AirlineIataCode);
			Assert.AreEqual("897", ticket.AirlinePrefixCode);

			Assert.AreEqual(4400338913, ticket.Number);
			Assert.AreEqual("SHABLIIENKO/IURII MR", ticket.PassengerName);

			Assert.AreEqual(1, ticket.Segments.Count);

			Assert.AreSame(ticket, ticket.Segments[0].Ticket);
			Assert.AreEqual(1, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("EVN", ticket.Segments[0].FromAirportCode);
			Assert.IsNull(ticket.Segments[0].FromAirportName);
			Assert.AreEqual("EVN", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("IEV", ticket.Segments[0].ToAirportCode);
			Assert.IsNull(ticket.Segments[0].ToAirportName);
			Assert.AreEqual("IEV", ticket.Segments[0].ToAirport.Code);
			Assert.IsNull(ticket.Segments[0].CarrierIataCode);
			Assert.IsNull(ticket.Segments[0].CarrierPrefixCode);
			Assert.IsNull(ticket.Segments[0].CarrierName);
			Assert.IsNull(ticket.Segments[0].Carrier);
			Assert.AreEqual("855", ticket.Segments[0].FlightNumber);
			Assert.IsNull(ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 30), ticket.Segments[0].DepartureTime);
			Assert.IsNull(ticket.Segments[0].ArrivalTime);
			Assert.IsNull(ticket.Segments[0].MealCodes);
			Assert.IsNull(ticket.Segments[0].NumberOfStops);
			Assert.IsNull(ticket.Segments[0].Luggage);
			Assert.IsNull(ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.IsNull(ticket.Segments[0].Duration);
			Assert.IsNull(ticket.Segments[0].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("HEEOW", ticket.Segments[0].FareBasis);

			Assert.AreEqual(new Money("UAH", 1589.00m), ticket.Fare);
			Assert.IsNull(ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 533.10m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 2122.10m), ticket.Total);

			Assert.AreEqual(3, ticket.Fees.Count);
			Assert.AreEqual(new Money("UAH", 210.10m), ticket.Fees[0].Amount);
			Assert.AreEqual("AM", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 190.60m), ticket.Fees[1].Amount);
			Assert.AreEqual("KC", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 132.40m), ticket.Fees[2].Amount);
			Assert.AreEqual("YR", ticket.Fees[2].Code);

			Assert.AreEqual(new Money("UAH", 15.89m), ticket.Commission);
			Assert.AreEqual(1, ticket.CommissionPercent);
			Assert.AreEqual("In", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Invoice, ticket.PaymentType);

			Assert.IsNull(ticket.TourCode);
		}

		public void TestAllTkts()
		{
			foreach (var file in Directory.GetFiles(@"D:\temp\import\bsv\tkts"))
				using (var stream = new StreamReader(file))
					TktParser.Parse(stream.ReadToEnd());
		}
	}
}