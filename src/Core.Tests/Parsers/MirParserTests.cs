using System;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Parsers
{
	[TestFixture]
	public class MirParserTests
	{
		[Test]
		public void TestParseTicket1()
		{
			var docs = MirParser.Parse(Res.MirTicket1, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
			Assert.IsInstanceOf<AviaTicket>(docs[1]);

			var ticket = (AviaTicket) docs[0];

			//Assert.AreEqual(2, ticket.Fares.Count);
			//Assert.AreEqual(160.00m, ticket.Fares[0].Amount.Amount);
			//Assert.AreEqual(290.01m, ticket.Fares[1].Amount.Amount);
			//Assert.AreEqual(1, ticket.Fares[0].Segments.Count);
			//AssertFareSegment(ticket.Fares[0].Segments[0], "IEV", "S7", "MOW");
			//Assert.AreEqual(1, ticket.Fares[1].Segments.Count);
			//AssertFareSegment(ticket.Fares[1].Segments[0], "MOW", "S7", "CEK");


			Assert.AreEqual(GdsOriginator.Galileo, ticket.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, ticket.Origin);

			Assert.AreEqual(new DateTime(2009, 7, 1, 15, 36, 0), ticket.IssueDate);

			Assert.AreEqual("VF88JE", ticket.PnrCode);
			Assert.AreEqual("S7 V4XBT", ticket.AirlinePnrCode);

			Assert.AreEqual("7Y5X", ticket.BookerOffice);
			Assert.AreEqual("NN", ticket.BookerCode);
			Assert.AreEqual("7Y5X", ticket.TicketerOffice);
			Assert.AreEqual("72320441", ticket.TicketingIataOffice);
			Assert.AreEqual("NN", ticket.TicketerCode);

			Assert.AreEqual("S7", ticket.AirlineIataCode);
			Assert.AreEqual("421", ticket.AirlinePrefixCode);
			Assert.AreEqual("SIBERIA AIRLINES", ticket.AirlineName);
			Assert.AreEqual("S7", ticket.Producer.AirlineIataCode);
			Assert.AreEqual("421", ticket.Producer.AirlinePrefixCode);
			Assert.AreEqual("SIBERIA AIRLINES", ticket.Producer.Name);

			Assert.AreEqual(9664933244, ticket.Number);
			Assert.AreEqual("CHAIKOVSKA/ANZHELIKAMRS", ticket.PassengerName);

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreSame(ticket, ticket.Segments[0].Ticket);
			Assert.AreEqual(1, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("KBP", ticket.Segments[0].FromAirportCode);
			Assert.AreEqual("KIEV/BORISPOL", ticket.Segments[0].FromAirportName);
			Assert.AreEqual("KBP", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("KIEV", ticket.Segments[0].FromAirport.Settlement);
			Assert.AreEqual("BORISPOL", ticket.Segments[0].FromAirport.Name);
			Assert.AreEqual("DME", ticket.Segments[0].ToAirportCode);
			Assert.AreEqual("MOSCOW/DOMODE", ticket.Segments[0].ToAirportName);
			Assert.AreEqual("DME", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("MOSCOW", ticket.Segments[0].ToAirport.Settlement);
			Assert.AreEqual("DOMODE", ticket.Segments[0].ToAirport.Name);
			Assert.AreEqual("S7", ticket.Segments[0].CarrierIataCode);
			Assert.AreEqual("421", ticket.Segments[0].CarrierPrefixCode);
			Assert.AreEqual("SIBERIA AIRL", ticket.Segments[0].CarrierName);
			Assert.AreEqual("S7", ticket.Segments[0].Carrier.AirlineIataCode);
			Assert.AreEqual("421", ticket.Segments[0].Carrier.AirlinePrefixCode);
			Assert.AreEqual("SIBERIA AIRL", ticket.Segments[0].Carrier.Name);
			Assert.AreEqual("160", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("T", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 7, 3, 16, 40, 0), ticket.Segments[0].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 7, 3, 19, 5, 0), ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("M", ticket.Segments[0].MealCodes);
			Assert.AreEqual(0, ticket.Segments[0].NumberOfStops);
			//Assert.AreEqual("20K", ticket.Segments[0].Luggage);
			Assert.AreEqual("B", ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.AreEqual("01:25", ticket.Segments[0].Duration);
			Assert.IsNull(ticket.Segments[0].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("TLEOW", ticket.Segments[0].FareBasis);

			Assert.AreSame(ticket, ticket.Segments[1].Ticket);
			Assert.AreEqual(2, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("DME", ticket.Segments[1].FromAirportCode);
			Assert.AreEqual("MOSCOW/DOMODE", ticket.Segments[1].FromAirportName);
			Assert.AreEqual("DME", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("MOSCOW", ticket.Segments[1].FromAirport.Settlement);
			Assert.AreEqual("DOMODE", ticket.Segments[1].FromAirport.Name);
			Assert.AreEqual("CEK", ticket.Segments[1].ToAirportCode);
			Assert.AreEqual("CHELYABINSK", ticket.Segments[1].ToAirportName);
			Assert.AreEqual("CEK", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("CHELYABINSK", ticket.Segments[1].ToAirport.Settlement);
			Assert.AreEqual("CHELYABINSK", ticket.Segments[1].ToAirport.Name);
			Assert.AreEqual("S7", ticket.Segments[1].CarrierIataCode);
			Assert.AreEqual("421", ticket.Segments[1].CarrierPrefixCode);
			Assert.AreEqual("SIBERIA AIRL", ticket.Segments[1].CarrierName);
			Assert.AreEqual("S7", ticket.Segments[1].Carrier.AirlineIataCode);
			Assert.AreEqual("421", ticket.Segments[1].Carrier.AirlinePrefixCode);
			Assert.AreEqual("SIBERIA AIRL", ticket.Segments[1].Carrier.Name);
			Assert.AreEqual("8", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("B", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 7, 3, 23, 45, 0), ticket.Segments[1].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 7, 4, 3, 50, 0), ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("M", ticket.Segments[1].MealCodes);
			Assert.AreEqual(0, ticket.Segments[1].NumberOfStops);
			//Assert.AreEqual("20K", ticket.Segments[1].Luggage);
			Assert.IsNull(ticket.Segments[1].CheckInTerminal);
			Assert.IsNull(ticket.Segments[1].CheckInTime);
			Assert.AreEqual("02:05", ticket.Segments[1].Duration);
			Assert.IsNull(ticket.Segments[1].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[1].Seat);
			Assert.AreEqual("BLIOW", ticket.Segments[1].FareBasis);

			Assert.AreEqual(new Money("USD", 450.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 3439.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 707.10m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 4146.10m), ticket.Total);

			Assert.AreEqual(6, ticket.Fees.Count);
			Assert.AreEqual(new Money("UAH", 30.60m), ticket.Fees[0].Amount);
			Assert.AreEqual("UA", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 15.30m), ticket.Fees[1].Amount);
			Assert.AreEqual("UD", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 129.90m), ticket.Fees[2].Amount);
			Assert.AreEqual("YK", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 10.80m), ticket.Fees[3].Amount);
			Assert.AreEqual("RU", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 151.20m), ticket.Fees[4].Amount);
			Assert.AreEqual("YQ", ticket.Fees[4].Code);
			Assert.AreEqual(new Money("UAH", 369.30m), ticket.Fees[5].Amount);
			Assert.AreEqual("YR", ticket.Fees[5].Code);

			Assert.AreEqual(new Money("UAH", 171.95m), ticket.Commission);
			Assert.AreEqual(5, ticket.CommissionPercent);
			Assert.AreEqual("CASH", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);

			Assert.IsNull(ticket.TourCode);

			Assert.AreEqual("HK1/P/UA/EA796634/UA/05JUN52/F/24MAR18/YAKUBOVA/LYUDMYLA", ((AviaTicket)docs[0]).GdsPassport);
			Assert.AreEqual("HK1/P/UA/EA924568/UA/05MAY48/M/22APR18/KATSYN/IGOR", ((AviaTicket)docs[1]).GdsPassport);

		}

		[Test]
		public void TestParseTicket2()
		{
			var docs = MirParser.Parse(Res.MirTicket2, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
			Assert.IsInstanceOf<AviaTicket>(docs[1]);

			var ticket = (AviaTicket) docs[1];


			//Assert.AreEqual(2, ticket.Fares.Count);
			//Assert.AreEqual(234.50m, ticket.Fares[0].Amount.Amount);
			//Assert.AreEqual(209.50m, ticket.Fares[1].Amount.Amount);
			//Assert.AreEqual(1, ticket.Fares[0].Segments.Count);
			//AssertFareSegment(ticket.Fares[0].Segments[0], "IEV", "PS", "PAR");
			//Assert.AreEqual(1, ticket.Fares[1].Segments.Count);
			//AssertFareSegment(ticket.Fares[1].Segments[0], "PAR", "PS", "IEV");

			Assert.AreEqual(new DateTime(2009, 3, 19, 12, 35, 0), ticket.IssueDate);

			Assert.AreEqual("M0X46G", ticket.PnrCode);
			Assert.AreEqual("PS RNPD3", ticket.AirlinePnrCode);

			Assert.AreEqual("7Y5X", ticket.BookerOffice);
			Assert.AreEqual("NV", ticket.BookerCode);
			Assert.AreEqual("3L0B", ticket.TicketerOffice);
			Assert.AreEqual("72321874", ticket.TicketingIataOffice);
			Assert.AreEqual("YY", ticket.TicketerCode);

			Assert.AreEqual("PS", ticket.AirlineIataCode);
			Assert.AreEqual("566", ticket.AirlinePrefixCode);
			Assert.AreEqual("UKRAINE INTERNATIONAL AI", ticket.AirlineName);
			Assert.AreEqual("PS", ticket.Producer.AirlineIataCode);
			Assert.AreEqual("566", ticket.Producer.AirlinePrefixCode);
			Assert.AreEqual("UKRAINE INTERNATIONAL AI", ticket.Producer.Name);

			Assert.AreEqual(3323397915, ticket.Number);
			Assert.AreEqual("IATSENKO/VOLODYMYRMR", ticket.PassengerName);

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreSame(ticket, ticket.Segments[0].Ticket);
			Assert.AreEqual(1, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("KBP", ticket.Segments[0].FromAirportCode);
			Assert.AreEqual("KIEV/BORISPOL", ticket.Segments[0].FromAirportName);
			Assert.AreEqual("KBP", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("KIEV", ticket.Segments[0].FromAirport.Settlement);
			Assert.AreEqual("BORISPOL", ticket.Segments[0].FromAirport.Name);
			Assert.AreEqual("CDG", ticket.Segments[0].ToAirportCode);
			Assert.AreEqual("PARIS/CHARLES", ticket.Segments[0].ToAirportName);
			Assert.AreEqual("CDG", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("PARIS", ticket.Segments[0].ToAirport.Settlement);
			Assert.AreEqual("CHARLES", ticket.Segments[0].ToAirport.Name);
			Assert.AreEqual("PS", ticket.Segments[0].CarrierIataCode);
			Assert.AreEqual("566", ticket.Segments[0].CarrierPrefixCode);
			Assert.AreEqual("UKRAINE INTE", ticket.Segments[0].CarrierName);
			Assert.AreEqual("PS", ticket.Segments[0].Carrier.AirlineIataCode);
			Assert.AreEqual("566", ticket.Segments[0].Carrier.AirlinePrefixCode);
			Assert.AreEqual("UKRAINE INTE", ticket.Segments[0].Carrier.Name);
			Assert.AreEqual("701", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("M", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 3, 21, 6, 30, 0), ticket.Segments[0].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 3, 21, 9, 0, 0), ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("B", ticket.Segments[0].MealCodes);
			Assert.AreEqual(0, ticket.Segments[0].NumberOfStops);
			//Assert.AreEqual("20K", ticket.Segments[0].Luggage);
			Assert.AreEqual("B", ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.AreEqual("03:30", ticket.Segments[0].Duration);
			Assert.IsNull(ticket.Segments[0].ArrivalTerminal);
			Assert.AreEqual("6D", ticket.Segments[0].Seat);
			Assert.AreEqual("MPX2UA", ticket.Segments[0].FareBasis);

			Assert.AreSame(ticket, ticket.Segments[1].Ticket);
			Assert.AreEqual(2, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("CDG", ticket.Segments[1].FromAirportCode);
			Assert.AreEqual("PARIS/CHARLES", ticket.Segments[1].FromAirportName);
			Assert.AreEqual("CDG", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("PARIS", ticket.Segments[1].FromAirport.Settlement);
			Assert.AreEqual("CHARLES", ticket.Segments[1].FromAirport.Name);
			Assert.AreEqual("KBP", ticket.Segments[1].ToAirportCode);
			Assert.AreEqual("KIEV/BORISPOL", ticket.Segments[1].ToAirportName);
			Assert.AreEqual("KBP", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("KIEV", ticket.Segments[1].ToAirport.Settlement);
			Assert.AreEqual("BORISPOL", ticket.Segments[1].ToAirport.Name);
			Assert.AreEqual("PS", ticket.Segments[1].CarrierIataCode);
			Assert.AreEqual("566", ticket.Segments[1].CarrierPrefixCode);
			Assert.AreEqual("UKRAINE INTE", ticket.Segments[1].CarrierName);
			Assert.AreEqual("PS", ticket.Segments[1].Carrier.AirlineIataCode);
			Assert.AreEqual("566", ticket.Segments[1].Carrier.AirlinePrefixCode);
			Assert.AreEqual("UKRAINE INTE", ticket.Segments[1].Carrier.Name);
			Assert.AreEqual("702", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("E", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 3, 24, 10, 05, 0), ticket.Segments[1].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 3, 24, 14, 20, 0), ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("L", ticket.Segments[1].MealCodes);
			Assert.AreEqual(0, ticket.Segments[1].NumberOfStops);
			//Assert.AreEqual("20K", ticket.Segments[1].Luggage);
			Assert.AreEqual("T2B", ticket.Segments[1].CheckInTerminal);
			Assert.IsNull(ticket.Segments[1].CheckInTime);
			Assert.AreEqual("03:15", ticket.Segments[1].Duration);
			Assert.IsNull(ticket.Segments[1].ArrivalTerminal);
			Assert.AreEqual("6E", ticket.Segments[1].Seat);
			Assert.AreEqual("EPX1UA", ticket.Segments[1].FareBasis);

			Assert.AreEqual(new Money("USD", 444.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 3673.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 1188.50m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 4861.50m), ticket.Total);

			Assert.AreEqual(7, ticket.Fees.Count);
			Assert.AreEqual(new Money("UAH", 33.10m), ticket.Fees[0].Amount);
			Assert.AreEqual("UA", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 16.60m), ticket.Fees[1].Amount);
			Assert.AreEqual("UD", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 140.70m), ticket.Fees[2].Amount);
			Assert.AreEqual("YK", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 188.30m), ticket.Fees[3].Amount);
			Assert.AreEqual("FR", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 43.30m), ticket.Fees[4].Amount);
			Assert.AreEqual("IZ", ticket.Fees[4].Code);
			Assert.AreEqual(new Money("UAH", 187.50m), ticket.Fees[5].Amount);
			Assert.AreEqual("QX", ticket.Fees[5].Code);
			Assert.AreEqual(new Money("UAH", 579.00m), ticket.Fees[6].Amount);
			Assert.AreEqual("YQ", ticket.Fees[6].Code);

			//Assert.AreEqual(new Money("UAH", 1.00m), ticket.Commission);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.AreEqual("CASH", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);

			Assert.IsNull(ticket.TourCode);
		}

		[Test]
		public void TestVoid1()
		{
			var docs = MirParser.Parse(Res.MirVoid1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaDocumentVoiding>(docs[0]);

			var voiding = (AviaDocumentVoiding) docs[0];

			Assert.IsInstanceOf<AviaTicket>(voiding.Document);
			Assert.AreEqual("181", voiding.Document.AirlinePrefixCode);
			Assert.AreEqual(9664954782, voiding.Document.Number);
			Assert.AreEqual(true, voiding.IsVoid);
			Assert.AreEqual(new DateTime(2009, 7, 7, 15, 48, 00), voiding.TimeStamp);
			Assert.AreEqual(GdsOriginator.Galileo, voiding.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, voiding.Origin);
			Assert.AreEqual("7Y5X", voiding.AgentOffice);
			Assert.AreEqual("72320441", voiding.IataOffice);
			Assert.AreEqual("LX", voiding.AgentCode);
		}

		[Test]
		public void TestVoid2()
		{
			var docs = MirParser.Parse(Res.MirVoid2, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaDocumentVoiding>(docs[0]);

			var voiding = (AviaDocumentVoiding) docs[0];

			Assert.IsInstanceOf<AviaTicket>(voiding.Document);
			Assert.AreEqual("181", voiding.Document.AirlinePrefixCode);
			Assert.AreEqual(9664928229, voiding.Document.Number);
			Assert.AreEqual(true, voiding.IsVoid);
			Assert.AreEqual(new DateTime(2009, 6, 21, 14, 22, 00), voiding.TimeStamp);
			Assert.AreEqual(GdsOriginator.Galileo, voiding.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, voiding.Origin);
			Assert.AreEqual("3L0B", voiding.AgentOffice);
			Assert.AreEqual("72321874", voiding.IataOffice);
			Assert.AreEqual("NA", voiding.AgentCode);
		}

		[Test]
		public void TestRefund1()
		{
			var docs = MirParser.Parse(Res.MirRefund1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaRefund>(docs[0]);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(new DateTime(2009, 7, 10, 12, 3, 0), refund.IssueDate);

			Assert.AreEqual("ZZZZZZ", refund.PnrCode);
			Assert.IsNull(refund.AirlinePnrCode);

			Assert.AreEqual("7Y5X", refund.BookerOffice);
			Assert.IsNull(refund.BookerCode);
			Assert.AreEqual("7Y5X", refund.TicketerOffice);
			Assert.AreEqual("LX", refund.TicketerCode);

			Assert.AreEqual("181", refund.AirlinePrefixCode);
			Assert.AreEqual(9664954770, refund.Number);

			Assert.AreEqual("TYMCHENKO/MYKYTAMSTR", refund.PassengerName);

			Assert.AreEqual(new Money("UAH", 359.00m), refund.Fare);
			Assert.AreEqual(new Money("UAH", 123.00m), refund.FeesTotal);
			Assert.IsNull(refund.Vat);

			Assert.AreEqual(1, refund.CommissionPercent);
			Assert.AreEqual(new Money("UAH", 3.59m), refund.Commission);

			Assert.AreEqual(new Money("UAH", 206.00m), refund.CancelFee);

			Assert.IsNull(refund.CancelCommissionPercent);
			Assert.AreEqual(new Money("UAH", 0.00m), refund.CancelCommission);

			Assert.AreEqual(new Money("UAH", 276.00m), refund.Total);

			Assert.AreEqual(4, refund.Fees.Count);

			Assert.AreEqual("UA", refund.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 12.30m), refund.Fees[0].Amount);
			Assert.AreEqual("UD", refund.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 3.90m), refund.Fees[1].Amount);
			Assert.AreEqual("YK", refund.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 26.80m), refund.Fees[2].Amount);
			Assert.AreEqual("HF", refund.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 80.00m), refund.Fees[3].Amount);

			Assert.IsNull(refund.PaymentForm);
			Assert.AreEqual(PaymentType.Unknown, refund.PaymentType);
		}

		[Test]
		public void TestRefund2()
		{
			var docs = MirParser.Parse(Res.MirRefund2, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaRefund>(docs[0]);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(GdsOriginator.Galileo, refund.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, refund.Origin);

			Assert.AreEqual(new DateTime(2009, 5, 29, 15, 59, 0), refund.IssueDate);

			Assert.AreEqual("ZZZZZZ", refund.PnrCode);
			Assert.IsNull(refund.AirlinePnrCode);

			Assert.AreEqual("3L0B", refund.BookerOffice);
			Assert.IsNull(refund.BookerCode);
			Assert.AreEqual("3L0B", refund.TicketerOffice);
			Assert.AreEqual("IN", refund.TicketerCode);

			Assert.AreEqual("566", refund.AirlinePrefixCode);
			Assert.AreEqual(5782436996, refund.Number);

			Assert.AreEqual("LUTSENKO/IGORMR", refund.PassengerName);

			Assert.AreEqual(new Money("UAH", 584.00m), refund.Fare);
			Assert.AreEqual(new Money("UAH", 175.30m), refund.FeesTotal);
			Assert.IsNull(refund.Vat);

			Assert.IsNull(refund.CommissionPercent);
			Assert.AreEqual(new Money("UAH", 1.00m), refund.Commission);

			Assert.AreEqual(new Money("UAH", 77.00m), refund.CancelFee);

			Assert.IsNull(refund.CancelCommissionPercent);
			Assert.AreEqual(new Money("UAH", 0.00m), refund.CancelCommission);

			Assert.AreEqual(new Money("UAH", 682.30m), refund.Total);

			Assert.AreEqual(5, refund.Fees.Count);

			Assert.AreEqual("UA", refund.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 11.50m), refund.Fees[0].Amount);
			Assert.AreEqual("UD", refund.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 3.90m), refund.Fees[1].Amount);
			Assert.AreEqual("YK", refund.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 26.90m), refund.Fees[2].Amount);
			Assert.AreEqual("HF", refund.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 125.30m), refund.Fees[3].Amount);
			Assert.AreEqual("YQ", refund.Fees[4].Code);
			Assert.AreEqual(new Money("UAH", 7.70m), refund.Fees[4].Amount);

			Assert.IsNull(refund.PaymentForm);
			Assert.AreEqual(PaymentType.Unknown, refund.PaymentType);
		}

		/*
		[Test]
		public void TestTicketIssue1()
		{
			var docs = MirParser.Parse(Res.MirTicketIssue1, new Currency("UAH"));

			Assert.AreEqual(12, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(GdsOriginator.Galileo, ticket.Originator);
			Assert.AreEqual(AviaDocumentOrigin.GalileoMir, ticket.Origin);
		}
		*/

		[Test]
		public void TestRefundIssue1()
		{
			var docs = MirParser.Parse(Res.MirRefundIssue1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaRefund>(docs[0]);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(GdsOriginator.Galileo, refund.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, refund.Origin);
		}

		[Test]
		public void TestRefundIssue2()
		{
			var docs = MirParser.Parse(Res.MirRefundIssue2, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaRefund>(docs[0]);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(GdsOriginator.Galileo, refund.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, refund.Origin);
		}

		[Test]
		public void TestRefundIssue3()
		{
			var docs = MirParser.Parse(Res.MirRefundIssue3, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaRefund>(docs[0]);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(GdsOriginator.Galileo, refund.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, refund.Origin);
			Assert.IsNotNull(refund.FeesTotal);
			Assert.IsNotNull(refund.Total);
		}

		[Test]
		public void TestRefundIssue4()
		{
			var docs = MirParser.Parse(Res.MirRefundIssue4, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaRefund>(docs[0]);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(GdsOriginator.Galileo, refund.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, refund.Origin);
			Assert.IsNotNull(refund.FeesTotal);
			Assert.IsNotNull(refund.Total);
		}

		[Test]
		public void TestMco1()
		{
			var docs = MirParser.Parse(Res.MirMco1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaMco>(docs[0]);

			var mco = (AviaMco) docs[0];

			Assert.AreEqual(GdsOriginator.Galileo, mco.Originator);
			Assert.AreEqual(ProductOrigin.GalileoMir, mco.Origin);

			Assert.AreEqual(new DateTime(2009, 7, 12, 9, 10, 0), mco.IssueDate);

			Assert.AreEqual("ZT7SG0", mco.PnrCode);
			Assert.AreEqual("PS R8CL0", mco.AirlinePnrCode);

			Assert.AreEqual("3L0B", mco.BookerOffice);
			Assert.AreEqual("IN", mco.BookerCode);
			Assert.AreEqual("3L0B", mco.TicketerOffice);
			Assert.AreEqual("YY", mco.TicketerCode);

			Assert.AreEqual("566", mco.AirlinePrefixCode);
			Assert.AreEqual(1918985843, mco.Number);

			Assert.AreEqual("FEIRACHIOS/ALESSANDROMR", mco.PassengerName);

			Assert.AreEqual(new Money("UAH", 534.00m), mco.Fare);
			Assert.AreEqual(new Money("UAH", 0.00m), mco.FeesTotal);
			Assert.IsNull(mco.Vat);

			Assert.AreEqual(0, mco.CommissionPercent);
			Assert.AreEqual(new Money("UAH", 0.00m), mco.Commission);

			Assert.AreEqual(new Money("UAH", 534.00m), mco.Total);

			Assert.AreEqual(0, mco.Fees.Count);

			Assert.AreEqual("CASH", mco.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, mco.PaymentType);
		}

		/*
		[Test]
		public void TestParseAgencyData1()
		{
			var docs = MirParser.Parse(Res.MirTicket3, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);
		}

		[Test]
		public void TestParseAgencyData2()
		{
			var docs = MirParser.Parse(Res.MirTicket4, new Currency("UAH"));

			Assert.AreEqual(3, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[1];

			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[2];

			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);
		}

		[Test]
		public void TestParseAgencyData3()
		{
			var docs = MirParser.Parse(Res.MirTicket5, new Currency("UAH"));

			Assert.AreEqual(3, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual("ISMAILOV/ELDARMR", ticket.PassengerName);
			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[1];

			Assert.AreEqual("ALKAYA/ZAFER", ticket.PassengerName);
			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 50), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[2];

			Assert.AreEqual("TOPCU/SENER", ticket.PassengerName);
			Assert.AreEqual(new Money("UAH", 80), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 0), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);
		}

		[Test]
		public void TestParseAgencyData4()
		{
			var docs = MirParser.Parse(Res.MirTicket6, new Currency("UAH"));

			Assert.AreEqual(3, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual("ISMAILOV/ELDARMR", ticket.PassengerName);
			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[1];

			Assert.AreEqual("ALKAYA/ZAFER", ticket.PassengerName);
			Assert.IsNull(ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[2];

			Assert.AreEqual("TOPCU/SENER", ticket.PassengerName);
			Assert.IsNull(ticket.ServiceFee);
			Assert.IsNull(ticket.Discount);
			Assert.IsNull(ticket.Customer);
		}

		[Test]
		public void TestParseAgencyData5()
		{
			var docs = MirParser.Parse(Res.MirTicket7, new Currency("UAH"));

			Assert.AreEqual(3, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual("ISMAILOV/ELDARMR", ticket.PassengerName);
			Assert.AreEqual(new Money("UAH", 200), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 100), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[1];

			Assert.AreEqual("ALKAYA/ZAFER", ticket.PassengerName);
			Assert.AreEqual(new Money("UAH", 300), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 50), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);

			ticket = (AviaTicket) docs[2];

			Assert.AreEqual("TOPCU/SENER", ticket.PassengerName);
			Assert.AreEqual(new Money("UAH", 50), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 0), ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);
		}

		[Test]
		public void TestParseAgencyData6()
		{
			var docs = MirParser.Parse(Res.MirTicket8, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(null, ticket.ServiceFee);
			Assert.AreEqual(null, ticket.Discount);
			Assert.IsNotNull(ticket.Customer);
			Assert.AreEqual("ARGO", ticket.Customer.LegalName);
		}
		*/

		[Test]
		public void Test12HoursDateTimeParse()
		{
			var docs = MirParser.Parse(Res.MirTicket9, new Currency("UAH"));
			var document = (AviaTicket) docs[0];

			Assert.AreEqual(new DateTime(2010, 4, 8, 8, 46, 0), document.IssueDate);

			Assert.AreEqual(4, document.Segments.Count);

			Assert.AreEqual(new DateTime(2010, 4, 16, 5, 35, 0), document.Segments[0].DepartureTime);
			Assert.AreEqual(new DateTime(2010, 4, 16, 7, 15, 0), document.Segments[0].ArrivalTime);

			Assert.AreEqual(new DateTime(2010, 4, 16, 9, 55, 0), document.Segments[1].DepartureTime);
			Assert.AreEqual(new DateTime(2010, 4, 16, 12, 5, 0), document.Segments[1].ArrivalTime);

			Assert.AreEqual(new DateTime(2010, 6, 29, 21, 00, 0), document.Segments[2].DepartureTime);
			Assert.AreEqual(new DateTime(2010, 6, 30, 17, 15, 0), document.Segments[2].ArrivalTime);

			Assert.AreEqual(new DateTime(2010, 6, 30, 19, 25, 0), document.Segments[3].DepartureTime);
			Assert.AreEqual(new DateTime(2010, 6, 30, 22, 40, 0), document.Segments[3].ArrivalTime);
		}


		[Test]
		public void Test13()
		{

			var docs = MirParser.Parse(@"
T51G773392028070226824FEB210925 QR157QATAR AIRWAYS           14MAR21A34D54FE3267
68EA59ZU72324350 ISP3TE         041459N59AG21JAN2103424FEB21060
UAH0000000136640UAH00000000  00000000  00000000  00000000  00000000  000000000001               
NYNYN5YNYAYH NNNX   UA                             
000000001000004000000001001001001000001000000000

A02SHASH/DANIIL MR                  055517802151362366425901000001668ADT   0101Y
SI:BEN:SHASH/DANIIL MR                                        C35:NTD:24FEB21

A0401QR157QATAR AIRWAY 296N HK14MAR1730 2330 2KBPKYIV/KYIV BORDOHDOHA         INM   X0   788  D 02055F TK:YJT:05.00ANL:QATAR AIRWAYS           DDL:14MAR21
A0402QR157QATAR AIRWAY 672N HK15MAR0210 0900 2DOHDOHA         MLEMALE         INM   O0   77W    02061F TK:YJT:04.50ANL:QATAR AIRWAYS           DDL:15MAR21
A0403QR157QATAR AIRWAY 675N HK25MAR1955 2255 2MLEMALE         DOHDOHA         INM   X0   77W    02061F TK:YJT:05.00ANL:QATAR AIRWAYS           DDL:25MAR21
A0404QR157QATAR AIRWAY 295N HK26MAR0240 0700 2DOHDOHA         KBPKYIV/KYIV BORINM   X0   788    02055F TK:YJT:05.20ANL:QATAR AIRWAYS           DDL:26MAR21

A0701USD      490.00UAH       16732UAH       13664UAHT1:    3068XP

A080101NLR2R1RE0000000014MAR2114MAR21      F:NLR2R1RE       E:TAX XP AS DATE CHNG FEE-NON END-CHNG PENALTIES AS PER RULE  B:25K
EF:TAX XP AS DATE CHNG FEE-NON E/ND-CHNG PENALTIES AS PER RULE
A080102NLR2R1RE0000000015MAR2115MAR21      F:NLR2R1RE       E:TAX XP AS DATE CHNG FEE-NON END-CHNG PENALTIES AS PER RULE  B:25K
EF:TAX XP AS DATE CHNG FEE-NON E/ND-CHNG PENALTIES AS PER RULE
A080103NLR2R1RE0000000025MAR2125MAR21      F:NLR2R1RE       E:TAX XP AS DATE CHNG FEE-NON END-CHNG PENALTIES AS PER RULE  B:25K
EF:TAX XP AS DATE CHNG FEE-NON E/ND-CHNG PENALTIES AS PER RULE
A080104NLR2R1RE0000000026MAR2126MAR21      F:NLR2R1RE       E:TAX XP AS DATE CHNG FEE-NON END-CHNG PENALTIES AS PER RULE  B:25K
EF:TAX XP AS DATE CHNG FEE-NON E/ND-CHNG PENALTIES AS PER RULE

A09010IEV QR X/DOH QR MLE 245.00NLR2R1RE QR X/DOH QR IEV 245.00NLR2
R1RE NUC490.00END ROE1.0 XT PD57UD PD368YK PD544G4 PD30PZ PD7
07BQ PD707H9 PD8470YQ PD456YR

A100122JAN217232435 IEV                   S                                    A
TI:157362366425561234
UAH000000006748   T1:     114UAT2:      57UDT3:   11282XTT4:          T5:          000000018201000000000000
IT:     368YK     544G4      30PZ     707BQ     707H9    8470YQ     456YR                                                                                                              

A11S         9984N                                     P:02

A12IEVT *38 044 22 33 989 KYIV EXPERT

A14VL-075024FEBMUCRM1AOAHGLR

A24010IEV QR X/DOH QR MLE 245.00NLR2R1RE QR X/DOH QR IEV 245.00NLR2
R1RE NUC490.00END ROE1.0 XT PD57UD PD368YK PD544G4 PD30PZ PD7
07BQ PD707H9 PD8470YQ PD456YR
", new Currency("UAH"));

			var doc = (AviaTicket)docs[0];



		}


	}
}