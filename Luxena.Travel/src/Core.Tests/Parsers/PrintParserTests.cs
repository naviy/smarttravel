using System;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Parsers
{
	[TestFixture]
	public class PrintParserTests
	{
		private const string PrintFilesPath = @"D:\data\luxena-ua\Projects\Luxena.Travel\Implementation\dotNet\src\Core.Tests\PrintFiles\";

		[SetUp]
		public void SetUp()
		{
			_parser = new PrintParser
			{
				NeutralAirlineCode = "100"
			};
		}

		[Test]
		public void TestPrintParser1()
		{
			var docs = _parser.Parse(PrintFilesPath + "1");

			Assert.AreEqual(3, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
			Assert.IsInstanceOf<AviaTicket>(docs[1]);
			Assert.IsInstanceOf<AviaTicket>(docs[2]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(ProductType.AviaTicket, ticket.Type);
			Assert.AreEqual(new DateTime(2010, 03, 31), ticket.IssueDate);
			Assert.IsNull(ticket.AirlineIataCode);
			Assert.AreEqual("897", ticket.AirlinePrefixCode);
			Assert.IsNull(ticket.Producer);
			Assert.AreEqual(4400375702, ticket.Number);
			Assert.IsNull(ticket.ConjunctionNumbers);
			Assert.AreEqual("ILNYTSKA/MARYNA MRS", ticket.PassengerName);

			Assert.AreEqual(new Money("USD", 112), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 888), ticket.EqualFare);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.IsNull(ticket.Commission);
			Assert.AreEqual(new Money("UAH", 309), ticket.FeesTotal);
			Assert.IsNull(ticket.Vat);
			Assert.AreEqual(new Money("UAH", 1197m), ticket.Total);

			Assert.IsNull(ticket.TicketerOffice);
			Assert.IsNull(ticket.TicketerCode);
			Assert.AreEqual(GdsOriginator.Amadeus, ticket.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusPrint, ticket.Origin);
			Assert.AreEqual("22JAUJ", ticket.PnrCode);
			Assert.IsNull(ticket.AirlinePnrCode);
			Assert.IsNull(ticket.TourCode);

			Assert.AreEqual(5, ticket.Fees.Count);

			Assert.AreEqual(new Money("UAH", 16), ticket.Fees[0].Amount);
			Assert.AreEqual("YQ", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 55.6m), ticket.Fees[1].Amount);
			Assert.AreEqual("YK", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 8), ticket.Fees[2].Amount);
			Assert.AreEqual("UD", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 196.9m), ticket.Fees[3].Amount);
			Assert.AreEqual("HF", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 32.5m), ticket.Fees[4].Amount);
			Assert.AreEqual("UA", ticket.Fees[4].Code);

			Assert.AreEqual("DOK-KBP-DOK", ticket.Itinerary);

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreEqual(0, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("DOK", ticket.Segments[0].FromAirportCode);
			Assert.AreEqual("DOK", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("KBP", ticket.Segments[0].ToAirportCode);
			Assert.AreEqual("KBP", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("7D", ticket.Segments[0].CarrierIataCode);
			Assert.IsNull(ticket.Segments[0].Carrier);
			Assert.AreEqual("363", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("V", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2010, 4, 13, 9, 30, 0), ticket.Segments[0].DepartureTime);
			Assert.IsNull(ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("VSALE1M", ticket.Segments[0].FareBasis);
			Assert.AreEqual(true, ticket.Segments[0].Stopover);

			Assert.AreEqual(1, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("KBP", ticket.Segments[1].FromAirportCode);
			Assert.AreEqual("KBP", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("DOK", ticket.Segments[1].ToAirportCode);
			Assert.AreEqual("DOK", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("7D", ticket.Segments[1].CarrierIataCode);
			Assert.IsNull(ticket.Segments[1].Carrier);
			Assert.AreEqual("354", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("V", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2010, 4, 23, 13, 45, 0), ticket.Segments[1].DepartureTime);
			Assert.IsNull(ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("VSALE1M", ticket.Segments[1].FareBasis);
			Assert.AreEqual(true, ticket.Segments[1].Stopover);

			ticket = (AviaTicket) docs[1];

			Assert.AreEqual(ProductType.AviaTicket, ticket.Type);
			Assert.AreEqual(new DateTime(2010, 03, 31), ticket.IssueDate);
			Assert.IsNull(ticket.AirlineIataCode);
			Assert.AreEqual("897", ticket.AirlinePrefixCode);
			Assert.IsNull(ticket.Producer);
			Assert.AreEqual(4400375703, ticket.Number);
			Assert.IsNull(ticket.ConjunctionNumbers);
			Assert.AreEqual("ILNYTSKYY/SERGIY MR", ticket.PassengerName);

			Assert.AreEqual(new Money("USD", 112), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 888), ticket.EqualFare);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.IsNull(ticket.Commission);
			Assert.AreEqual(new Money("UAH", 309), ticket.FeesTotal);
			Assert.IsNull(ticket.Vat);
			Assert.AreEqual(new Money("UAH", 1197m), ticket.Total);

			Assert.IsNull(ticket.TicketerOffice);
			Assert.IsNull(ticket.TicketerCode);
			Assert.AreEqual(GdsOriginator.Amadeus, ticket.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusPrint, ticket.Origin);
			Assert.AreEqual("22JAUJ", ticket.PnrCode);
			Assert.IsNull(ticket.AirlinePnrCode);
			Assert.IsNull(ticket.TourCode);

			Assert.AreEqual(5, ticket.Fees.Count);

			Assert.AreEqual(new Money("UAH", 16), ticket.Fees[0].Amount);
			Assert.AreEqual("YQ", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 55.6m), ticket.Fees[1].Amount);
			Assert.AreEqual("YK", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 8), ticket.Fees[2].Amount);
			Assert.AreEqual("UD", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 196.9m), ticket.Fees[3].Amount);
			Assert.AreEqual("HF", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 32.5m), ticket.Fees[4].Amount);
			Assert.AreEqual("UA", ticket.Fees[4].Code);

			Assert.AreEqual("DOK-KBP-DOK", ticket.Itinerary);

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreEqual(0, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("DOK", ticket.Segments[0].FromAirportCode);
			Assert.AreEqual("DOK", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("KBP", ticket.Segments[0].ToAirportCode);
			Assert.AreEqual("KBP", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("7D", ticket.Segments[0].CarrierIataCode);
			Assert.IsNull(ticket.Segments[0].Carrier);
			Assert.AreEqual("363", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("V", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2010, 4, 13, 9, 30, 0), ticket.Segments[0].DepartureTime);
			Assert.IsNull(ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("VSALE1M", ticket.Segments[0].FareBasis);
			Assert.AreEqual(true, ticket.Segments[0].Stopover);

			Assert.AreEqual(1, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("KBP", ticket.Segments[1].FromAirportCode);
			Assert.AreEqual("KBP", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("DOK", ticket.Segments[1].ToAirportCode);
			Assert.AreEqual("DOK", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("7D", ticket.Segments[1].CarrierIataCode);
			Assert.IsNull(ticket.Segments[1].Carrier);
			Assert.AreEqual("354", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("V", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2010, 4, 23, 13, 45, 0), ticket.Segments[1].DepartureTime);
			Assert.IsNull(ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("VSALE1M", ticket.Segments[1].FareBasis);
			Assert.AreEqual(true, ticket.Segments[1].Stopover);

			ticket = (AviaTicket) docs[2];

			Assert.AreEqual(4400375704, ticket.Number);
			Assert.AreEqual("ILNYTSKYY/VYACHESLAV MR", ticket.PassengerName);
		}

		[Test]
		public void TestPrintParser2()
		{
			var docs = _parser.Parse(PrintFilesPath + "2");

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(ProductType.AviaTicket, ticket.Type);
			Assert.AreEqual(new DateTime(2010, 04, 02), ticket.IssueDate);
			Assert.IsNull(ticket.AirlineIataCode);
			Assert.AreEqual("897", ticket.AirlinePrefixCode);
			Assert.IsNull(ticket.Producer);
			Assert.AreEqual(4400375718, ticket.Number);
			Assert.IsNull(ticket.ConjunctionNumbers);
			Assert.AreEqual("ASLANYAN/VANIK MR", ticket.PassengerName);

			Assert.AreEqual(new Money("USD", 298), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 2362), ticket.EqualFare);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.IsNull(ticket.Commission);
			Assert.AreEqual(new Money("UAH", 1174.4m), ticket.FeesTotal);
			Assert.IsNull(ticket.Vat);
			Assert.AreEqual(new Money("UAH", 3536.4m), ticket.Total);

			Assert.IsNull(ticket.TicketerOffice);
			Assert.IsNull(ticket.TicketerCode);
			Assert.AreEqual(GdsOriginator.Amadeus, ticket.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusPrint, ticket.Origin);
			Assert.AreEqual("3KYL95", ticket.PnrCode);
			Assert.IsNull(ticket.AirlinePnrCode);
			Assert.IsNull(ticket.TourCode);

			Assert.AreEqual(6, ticket.Fees.Count);

			Assert.AreEqual("DOK-EVN-DOK", ticket.Itinerary);

			Assert.AreEqual(2, ticket.Segments.Count);
		}

		[Test]
		public void TestPrintParser3()
		{
			var docs = _parser.Parse(PrintFilesPath + "3");

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(ProductType.AviaTicket, ticket.Type);
			Assert.AreEqual(new DateTime(2009, 08, 11), ticket.IssueDate);
			Assert.IsNull(ticket.AirlineIataCode);
			Assert.AreEqual("298", ticket.AirlinePrefixCode);
			Assert.IsNull(ticket.Producer);
			Assert.AreEqual(2202447245, ticket.Number);
			Assert.AreEqual("46", ticket.ConjunctionNumbers);
			Assert.AreEqual("BEYGELZIMER/YAKIV MR", ticket.PassengerName);

			Assert.AreEqual(new Money("USD", 219), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 1858), ticket.EqualFare);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.IsNull(ticket.Commission);
			Assert.AreEqual(new Money("UAH", 314.80m), ticket.FeesTotal);
			Assert.IsNull(ticket.Vat);
			Assert.AreEqual(new Money("UAH", 2172.8m), ticket.Total);

			Assert.AreEqual(4, ticket.Fees.Count);

			Assert.AreEqual("DOK-VKO-UFA-VKO-DOK", ticket.Itinerary);

			Assert.AreEqual(4, ticket.Segments.Count);
		}

		[Test]
		public void TestPrintParser4()
		{
			var docs = _parser.Parse(PrintFilesPath + "4");

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(ProductType.AviaTicket, ticket.Type);
			Assert.AreEqual(new DateTime(2010, 04, 02), ticket.IssueDate);
			Assert.IsNull(ticket.AirlineIataCode);
			Assert.AreEqual("897", ticket.AirlinePrefixCode);
			Assert.IsNull(ticket.Producer);
			Assert.AreEqual(4400375716, ticket.Number);
			Assert.IsNull(ticket.ConjunctionNumbers);
			Assert.AreEqual("YEVTUSHYK/DMYTRO MR", ticket.PassengerName);

			Assert.AreEqual(new Money("USD", 62), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 492), ticket.EqualFare);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.IsNull(ticket.Commission);
			Assert.AreEqual(new Money("UAH", 178.7m), ticket.FeesTotal);
			Assert.IsNull(ticket.Vat);
			Assert.AreEqual(new Money("UAH", 670.7m), ticket.Total);

			Assert.AreEqual(5, ticket.Fees.Count);

			Assert.AreEqual("DOK-KBP", ticket.Itinerary);

			Assert.AreEqual(1, ticket.Segments.Count);
		}

		[Test]
		public void TestPrintParser5()
		{
			var docs = _parser.Parse(PrintFilesPath + "5");

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaMco>(docs[0]);

			var mco = (AviaMco) docs[0];

			Assert.AreEqual(ProductType.AviaMco, mco.Type);
			Assert.AreEqual(new DateTime(2010, 03, 24), mco.IssueDate);
			Assert.IsNull(mco.AirlineIataCode);
			Assert.AreEqual("897", mco.AirlinePrefixCode);
			Assert.IsNull(mco.Producer);
			Assert.AreEqual(4010093581, mco.Number);
			Assert.IsNull(mco.ConjunctionNumbers);
			Assert.AreEqual("ZYAKUN/VOLODYMYR MR", mco.PassengerName);

			Assert.AreEqual(new Money("USD", 24), mco.Fare);
			Assert.AreEqual(new Money("UAH", 190), mco.EqualFare);
			Assert.IsNull(mco.CommissionPercent);
			Assert.IsNull(mco.Commission);
			Assert.AreEqual(new Money("UAH", 0), mco.FeesTotal);
			Assert.IsNull(mco.Vat);
			Assert.AreEqual(new Money("UAH", 190), mco.Total);

			Assert.AreEqual(0, mco.Fees.Count);
		}

		private PrintParser _parser;
	}
}