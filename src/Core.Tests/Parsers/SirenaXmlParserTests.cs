using System;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Parsers
{
	[TestFixture]
	public class SirenaXmlParserTests
	{
		// TODO: test passport
		[Test]
		public void TestTicketParse1()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaTicket1);

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(GdsOriginator.Sirena, ticket.Originator);
			Assert.AreEqual(ProductOrigin.SirenaXml, ticket.Origin);

			Assert.AreEqual(new DateTime(2011, 6, 30, 13, 0, 0), ticket.IssueDate);

			Assert.AreEqual(6166223231, ticket.Number);
			Assert.IsNull(ticket.ConjunctionNumbers);

			Assert.AreEqual("TISHIN/IGOR MR", ticket.PassengerName);

			Assert.AreEqual("555", ticket.AirlinePrefixCode);

			Assert.IsNull(ticket.TicketerOffice);
			Assert.IsNull(ticket.TicketingIataOffice);
			Assert.AreEqual("“ œ36ÃŒ—0465", ticket.TicketerCode);

			Assert.AreEqual("“ œ36ÃŒ—0465", ticket.BookerCode);
			Assert.AreEqual("36ÃŒ—", ticket.BookerOffice);

			Assert.IsNull(ticket.TourCode);
			Assert.AreEqual("V2CS7Z", ticket.PnrCode);

			Assert.AreEqual(2, ticket.Segments.Count);

			var segment = ticket.Segments[0];

			Assert.AreEqual(true, segment.Stopover);
			Assert.AreEqual("SVO", segment.FromAirportCode);

			Assert.AreEqual("SVO", segment.FromAirport.Code);
			Assert.AreEqual("SVO", segment.FromAirport.Name);
			Assert.AreEqual("MOW", segment.FromAirport.Settlement);

			Assert.AreEqual("BCN", segment.ToAirportCode);

			Assert.AreEqual("BCN", segment.ToAirport.Code);
			Assert.AreEqual("BCN", segment.ToAirport.Name);
			Assert.AreEqual("BCN", segment.ToAirport.Settlement);

			Assert.AreEqual("E", segment.CheckInTerminal);
			Assert.AreEqual("1", segment.ArrivalTerminal);

			Assert.AreEqual("SU", segment.CarrierIataCode);

			Assert.AreEqual("Q", segment.ServiceClassCode);
			Assert.AreEqual("287", segment.FlightNumber);
			Assert.AreEqual(new DateTime(2011, 9, 10, 10, 5, 0), segment.DepartureTime);
			Assert.AreEqual(new DateTime(2011, 9, 10, 12, 35, 0), segment.ArrivalTime);

			Assert.AreEqual("QPX1", segment.FareBasis);

			segment = ticket.Segments[1];

			Assert.AreEqual(true, segment.Stopover);
			Assert.AreEqual("BCN", segment.FromAirportCode);

			Assert.AreEqual("BCN", segment.FromAirport.Code);
			Assert.AreEqual("BCN", segment.FromAirport.Name);
			Assert.AreEqual("BCN", segment.FromAirport.Settlement);

			Assert.AreEqual("SVO", segment.ToAirportCode);

			Assert.AreEqual("SVO", segment.ToAirport.Code);
			Assert.AreEqual("SVO", segment.ToAirport.Name);
			Assert.AreEqual("MOW", segment.ToAirport.Settlement);

			Assert.AreEqual("1", segment.CheckInTerminal);
			Assert.AreEqual("E", segment.ArrivalTerminal);

			Assert.AreEqual("SU", segment.CarrierIataCode);

			Assert.AreEqual("V", segment.ServiceClassCode);
			Assert.AreEqual("288", segment.FlightNumber);
			Assert.AreEqual(new DateTime(2011, 9, 25, 13, 25, 0), segment.DepartureTime);
			Assert.AreEqual(new DateTime(2011, 9, 25, 19, 40, 0), segment.ArrivalTime);

			Assert.AreEqual("VEX1", segment.FareBasis);

			Assert.AreEqual(9, ticket.Fees.Count);

			var fees = ticket.Fees;

			Assert.AreEqual("RU", fees[0].Code);
			Assert.AreEqual(new Money("RUB", 41), fees[0].Amount);

			Assert.AreEqual("ZZ", fees[1].Code);
			Assert.AreEqual(new Money("RUB", 125), fees[1].Amount);

			Assert.AreEqual("YQ", fees[2].Code);
			Assert.AreEqual(new Money("RUB", 1418), fees[2].Amount);

			Assert.AreEqual("YR", fees[3].Code);
			Assert.AreEqual(new Money("RUB", 134), fees[3].Amount);

			Assert.AreEqual("ZZ", fees[4].Code);
			Assert.AreEqual(new Money("RUB", 125), fees[4].Amount);

			Assert.AreEqual("YQ", fees[5].Code);
			Assert.AreEqual(new Money("RUB", 1418), fees[5].Amount);

			Assert.AreEqual("YR", fees[6].Code);
			Assert.AreEqual(new Money("RUB", 134), fees[6].Amount);

			Assert.AreEqual("QV", fees[7].Code);
			Assert.AreEqual(new Money("RUB", 106), fees[7].Amount);

			Assert.AreEqual("JD", fees[8].Code);
			Assert.AreEqual(new Money("RUB", 395), fees[8].Amount);

			Assert.AreEqual("CA", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);

			Assert.AreEqual(new Money("NUC", 701.97m), ticket.Fare);
			Assert.AreEqual(new Money("RUB", 20415), ticket.EqualFare);
			Assert.AreEqual(new Money("RUB", 3896), ticket.FeesTotal);
			Assert.AreEqual(new Money("RUB", 24311), ticket.Total);
			Assert.IsNull(ticket.Vat);
		}

		[Test]
		public void TestTicketParse2()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaTicket2);

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(GdsOriginator.Sirena, ticket.Originator);
			Assert.AreEqual(ProductOrigin.SirenaXml, ticket.Origin);

			Assert.AreEqual(new DateTime(2011, 6, 25, 12, 58, 0), ticket.IssueDate);

			Assert.AreEqual(6166047298, ticket.Number);
			Assert.IsNull(ticket.ConjunctionNumbers);

			Assert.AreEqual("FASSAKHOVA/GULYANISA MS", ticket.PassengerName);

			Assert.AreEqual("362", ticket.AirlinePrefixCode);

			Assert.AreEqual("“ œ36ÃŒ—0465", ticket.TicketerCode);

			Assert.AreEqual("“ œ36ÃŒ—0465", ticket.BookerCode);
			Assert.AreEqual("36ÃŒ—", ticket.BookerOffice);

			Assert.IsNull(ticket.TourCode);
			Assert.AreEqual("V25TGP", ticket.PnrCode);

			Assert.AreEqual(1, ticket.Segments.Count);

			var segment = ticket.Segments[0];

			Assert.AreEqual(true, segment.Stopover);
			Assert.AreEqual("SVX", segment.FromAirportCode);

			Assert.AreEqual("SVX", segment.FromAirport.Code);
			Assert.AreEqual("SVX", segment.FromAirport.Name);
			Assert.AreEqual("SVX", segment.FromAirport.Settlement);

			Assert.AreEqual("ALA", segment.ToAirportCode);

			Assert.AreEqual("ALA", segment.ToAirport.Code);
			Assert.AreEqual("ALA", segment.ToAirport.Name);
			Assert.AreEqual("ALA", segment.ToAirport.Settlement);

			Assert.IsNull(segment.CheckInTerminal);
			Assert.IsNull(segment.ArrivalTerminal);

			Assert.AreEqual("7R", segment.CarrierIataCode);

			Assert.AreEqual("O", segment.ServiceClassCode);
			Assert.AreEqual("821", segment.FlightNumber);
			Assert.AreEqual(new DateTime(2011, 9, 10, 12, 30, 0), segment.DepartureTime);
			Assert.AreEqual(new DateTime(2011, 9, 10, 15, 25, 0), segment.ArrivalTime);

			Assert.AreEqual("O45DAY", segment.FareBasis);

			Assert.AreEqual(1, ticket.Fees.Count);

			var fees = ticket.Fees;

			Assert.AreEqual("ZZ", fees[0].Code);
			Assert.AreEqual(new Money("RUB", 125), fees[0].Amount);

			Assert.AreEqual("CA", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);

			Assert.AreEqual(new Money("NUC", 100), ticket.Fare);
			Assert.AreEqual(new Money("RUB", 3025), ticket.EqualFare);
			Assert.AreEqual(new Money("RUB", 125), ticket.FeesTotal);
			Assert.AreEqual(new Money("RUB", 3150), ticket.Total);
			Assert.IsNull(ticket.Vat);
		}

		[Test]
		public void TestTicketParse3()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaTicket3);

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(GdsOriginator.Sirena, ticket.Originator);
			Assert.AreEqual(ProductOrigin.SirenaXml, ticket.Origin);

			Assert.AreEqual(new DateTime(2011, 2, 14, 13, 31, 0), ticket.IssueDate);

			Assert.AreEqual(6162114281, ticket.Number);
			Assert.IsNull(ticket.ConjunctionNumbers);

			Assert.AreEqual("DUBROV/VIACHESLAV MR", ticket.PassengerName);

			Assert.AreEqual("298", ticket.AirlinePrefixCode);

			Assert.AreEqual("“ œ36ÃŒ—0113", ticket.TicketerCode);

			Assert.IsNull(ticket.BookerCode);
			Assert.IsNull(ticket.BookerOffice);

			Assert.IsNull(ticket.TourCode);
			Assert.AreEqual("BDKSSD", ticket.PnrCode);

			Assert.AreEqual(2, ticket.Segments.Count);

			var segment = ticket.Segments[0];

			Assert.AreEqual(true, segment.Stopover);
			Assert.AreEqual("VKO", segment.FromAirportCode);

			Assert.AreEqual("VKO", segment.FromAirport.Code);
			Assert.AreEqual("VKO", segment.FromAirport.Name);
			Assert.AreEqual("MOW", segment.FromAirport.Settlement);

			Assert.AreEqual("HAJ", segment.ToAirportCode);

			Assert.AreEqual("HAJ", segment.ToAirport.Code);
			Assert.AreEqual("HAJ", segment.ToAirport.Name);
			Assert.AreEqual("HAJ", segment.ToAirport.Settlement);

			Assert.IsNull(segment.CheckInTerminal);
			Assert.IsNull(segment.ArrivalTerminal);

			Assert.AreEqual("UT", segment.CarrierIataCode);

			Assert.AreEqual("K", segment.ServiceClassCode);
			Assert.AreEqual("721", segment.FlightNumber);
			Assert.AreEqual(new DateTime(2011, 5, 2, 11, 10, 0), segment.DepartureTime);
			Assert.AreEqual(new DateTime(2011, 5, 2, 12, 10, 0), segment.ArrivalTime);

			Assert.AreEqual("KSALERT", segment.FareBasis);

			segment = ticket.Segments[1];

			Assert.AreEqual(true, segment.Stopover);
			Assert.AreEqual("HAJ", segment.FromAirportCode);

			Assert.AreEqual("HAJ", segment.FromAirport.Code);
			Assert.AreEqual("HAJ", segment.FromAirport.Name);
			Assert.AreEqual("HAJ", segment.FromAirport.Settlement);

			Assert.AreEqual("VKO", segment.ToAirportCode);

			Assert.AreEqual("VKO", segment.ToAirport.Code);
			Assert.AreEqual("VKO", segment.ToAirport.Name);
			Assert.AreEqual("MOW", segment.ToAirport.Settlement);

			Assert.IsNull(segment.CheckInTerminal);
			Assert.IsNull(segment.ArrivalTerminal);

			Assert.AreEqual("UT", segment.CarrierIataCode);

			Assert.AreEqual("K", segment.ServiceClassCode);
			Assert.AreEqual("722", segment.FlightNumber);
			Assert.AreEqual(new DateTime(2011, 5, 30, 13, 5, 0), segment.DepartureTime);
			Assert.AreEqual(new DateTime(2011, 5, 30, 18, 5, 0), segment.ArrivalTime);

			Assert.AreEqual("KSALERT", segment.FareBasis);

			Assert.AreEqual(7, ticket.Fees.Count);

			var fees = ticket.Fees;

			Assert.AreEqual("ZZ", fees[0].Code);
			Assert.AreEqual(new Money("RUB", 125), fees[0].Amount);

			Assert.AreEqual("YQ", fees[1].Code);
			Assert.AreEqual(new Money("RUB", 360), fees[1].Amount);

			Assert.AreEqual("DE", fees[2].Code);
			Assert.AreEqual(new Money("RUB", 243), fees[2].Amount);

			Assert.AreEqual("RA", fees[3].Code);
			Assert.AreEqual(new Money("RUB", 576), fees[3].Amount);

			Assert.AreEqual("ZZ", fees[4].Code);
			Assert.AreEqual(new Money("RUB", 125), fees[4].Amount);

			Assert.AreEqual("YQ", fees[5].Code);
			Assert.AreEqual(new Money("RUB", 360), fees[5].Amount);

			Assert.AreEqual("OY", fees[6].Code);
			Assert.AreEqual(new Money("RUB", 320), fees[6].Amount);

			Assert.AreEqual("CA", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);

			Assert.AreEqual(new Money("NUC", 226.8m), ticket.Fare);
			Assert.AreEqual(new Money("RUB", 6840), ticket.EqualFare);
			Assert.AreEqual(new Money("RUB", 2109), ticket.FeesTotal);
			Assert.AreEqual(new Money("RUB", 8949), ticket.Total);
			Assert.IsNull(ticket.Vat);
		}

		[Test]
		public void TestTicketParse4()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaTicket4);

			Assert.AreEqual(1, docs.Count);
		}

		[Test]
		public void TestTicketParse5()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaTicket5);

			Assert.AreEqual(1, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(GdsOriginator.Sirena, ticket.Originator);
			Assert.AreEqual(ProductOrigin.SirenaXml, ticket.Origin);

			Assert.AreEqual(new DateTime(2011, 5, 10, 18, 37, 0), ticket.IssueDate);

			Assert.AreEqual(6164310677, ticket.Number);
			Assert.AreEqual("394", ticket.AirlinePrefixCode);

			Assert.AreEqual(1, ticket.Segments.Count);

			var segment = ticket.Segments[0];

			Assert.AreEqual(true, segment.Stopover);
			Assert.AreEqual("DME", segment.FromAirportCode);

			Assert.AreEqual("DME", segment.FromAirport.Code);
			Assert.AreEqual("DME", segment.FromAirport.Name);
			Assert.AreEqual("ÃŒ¬", segment.FromAirport.Settlement);

			Assert.IsNull(segment.ToAirportCode);
			Assert.IsNull(segment.ToAirport);

			Assert.AreEqual(1, ticket.Fees.Count);

			var fees = ticket.Fees;

			Assert.AreEqual("ZZ", fees[0].Code);
			Assert.AreEqual(new Money("–”¡", 125), fees[0].Amount);

			Assert.AreEqual("Õ¿", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);

			Assert.AreEqual(new Money("–”¡", 5600), ticket.Fare);
			Assert.AreEqual(new Money("–”¡", 5600), ticket.EqualFare);
			Assert.AreEqual(new Money("–”¡", 125), ticket.FeesTotal);
			Assert.AreEqual(new Money("–”¡", 5725), ticket.Total);
			Assert.AreEqual(new Money("–”¡", 873.31m), ticket.Vat);
		}

		[Test]
		public void TestTicketParse6()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaTicket6);

			Assert.AreEqual(1, docs.Count);

			var ticket = (AviaTicket) docs[0];

			Assert.AreEqual(6164537722, ticket.Number);
			Assert.AreEqual("823", ticket.AirlinePrefixCode);

			Assert.AreEqual(2, ticket.Segments.Count);
			Assert.AreEqual(4, ticket.Fees.Count);

			Assert.AreEqual(new Money("–”¡", 7200), ticket.Fare);
			Assert.AreEqual(new Money("–”¡", 7200), ticket.EqualFare);
			Assert.AreEqual(new Money("–”¡", 850), ticket.FeesTotal);
			Assert.AreEqual(new Money("–”¡", 8050), ticket.Total);
			Assert.AreEqual(new Money("–”¡", 1227.98m), ticket.Vat);
		}

		[Test]
		public void TestRefundParser1()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaRefund1);

			Assert.AreEqual(1, docs.Count);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(GdsOriginator.Sirena, refund.Originator);
			Assert.AreEqual(ProductOrigin.SirenaXml, refund.Origin);

			Assert.AreEqual(new DateTime(2011, 6, 26, 15, 23, 0), refund.IssueDate);

			Assert.AreEqual(6166047298, refund.Number);
			Assert.IsNull(refund.ConjunctionNumbers);

			Assert.AreEqual("FASSAKHOVA/GULYANISA MS", refund.PassengerName);

			Assert.AreEqual("362", refund.AirlinePrefixCode);

			Assert.AreEqual("36ÃŒ—", refund.TicketerOffice);
			Assert.AreEqual("92390476", refund.TicketingIataOffice);
			Assert.AreEqual("“ œ36ÃŒ—0720", refund.TicketerCode);

			Assert.AreEqual("36ÃŒ—", refund.BookerOffice);
			Assert.AreEqual("“ œ36ÃŒ—0465", refund.BookerCode);

			Assert.IsNull(refund.TourCode);
			Assert.AreEqual("V25TGP", refund.PnrCode);

			Assert.AreEqual(0, refund.Fees.Count);

			Assert.AreEqual("CA", refund.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, refund.PaymentType);

			Assert.AreEqual(new Money("NUC", 100), refund.Fare);
			Assert.AreEqual(new Money("RUB", 3025), refund.EqualFare);
			Assert.AreEqual(new Money("RUB", 0), refund.FeesTotal);
			Assert.AreEqual(new Money("RUB", 3025), refund.Total);
			Assert.IsNull(refund.Vat);
		}

		[Test]
		public void TestRefundParser2()
		{
			var docs = SirenaXmlParser.Parse(Res.SirenaRefund2);

			Assert.AreEqual(1, docs.Count);

			var refund = (AviaRefund) docs[0];

			Assert.AreEqual(GdsOriginator.Sirena, refund.Originator);
			Assert.AreEqual(ProductOrigin.SirenaXml, refund.Origin);

			Assert.AreEqual(new DateTime(2011, 5, 17, 17, 6, 0), refund.IssueDate);

			Assert.AreEqual(6164537722, refund.Number);
			Assert.IsNull(refund.ConjunctionNumbers);

			Assert.AreEqual("823", refund.AirlinePrefixCode);

			Assert.AreEqual(2, refund.Fees.Count);

			Assert.AreEqual(PaymentType.Cash, refund.PaymentType);

			Assert.AreEqual(new Money("–”¡", 7200), refund.Fare);
			Assert.AreEqual(new Money("–”¡", 7200), refund.EqualFare);
			Assert.AreEqual(new Money("–”¡", 600), refund.FeesTotal);
			Assert.AreEqual(new Money("–”¡", 5800), refund.Total);
			Assert.AreEqual(new Money("–”¡", 2000), refund.CancelFee);
			Assert.AreEqual(new Money("–”¡", 1189.84m), refund.Vat);
		}
	}
}