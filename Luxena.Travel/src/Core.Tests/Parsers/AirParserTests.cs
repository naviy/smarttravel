// ReSharper disable StringLiteralTypo
using System;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Parsers
{


	[TestFixture]
	public class AirParserTests
	{

		#region Parse Ticket

		[Test]
		public void TestParseTicket1()
		{
			var docs = AirParser.Parse(Res.AirTicket1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];


			Assert.AreEqual(GdsOriginator.Amadeus, ticket.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, ticket.Origin);

			Assert.AreEqual("2JSYHH", ticket.PnrCode);
			Assert.AreEqual("VV 2JSYHH", ticket.AirlinePnrCode);

			Assert.AreEqual("IEVU23561", ticket.BookerOffice);
			Assert.AreEqual("1984LX", ticket.BookerCode);
			Assert.AreEqual("IEVU23561", ticket.TicketerOffice);
			Assert.AreEqual("72320441", ticket.TicketingIataOffice);
			Assert.AreEqual("1962VZ", ticket.TicketerCode);

			Assert.AreEqual("AEROSVIT AIRLINES", ticket.Producer.Name);
			Assert.AreEqual("VV", ticket.Producer.AirlineIataCode);
			Assert.AreEqual("870", ticket.Producer.AirlinePrefixCode);

			Assert.AreEqual("870", ticket.AirlinePrefixCode);
			Assert.AreEqual("3324622112", ticket.Number);
			Assert.AreEqual("VLADIMIROV/VITALII MR", ticket.PassengerName);

			Assert.AreEqual(new DateTime(2009, 3, 26), ticket.IssueDate);

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreSame(ticket, ticket.Segments[0].Ticket);
			Assert.AreEqual(0, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("KBP", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("KIEV BORISPOL", ticket.Segments[0].FromAirport.Name);
			Assert.AreEqual("JFK", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("NEW YORK JFK", ticket.Segments[0].ToAirport.Name);
			Assert.AreEqual("VV", ticket.Segments[0].CarrierIataCode);
			Assert.AreEqual("0131", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("V", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 24, 13, 0, 0), ticket.Segments[0].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 24, 16, 25, 0), ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("L", ticket.Segments[0].MealCodes);
			Assert.AreEqual(0, ticket.Segments[0].NumberOfStops);
			Assert.AreEqual("PC", ticket.Segments[0].Luggage);
			Assert.AreEqual("B", ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.AreEqual("10:25", ticket.Segments[0].Duration);
			Assert.AreEqual("4", ticket.Segments[0].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("VLPX3M", ticket.Segments[0].FareBasis);

			Assert.AreEqual(1, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("JFK", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("NEW YORK JFK", ticket.Segments[1].FromAirport.Name);
			Assert.AreEqual("KBP", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("KIEV BORISPOL", ticket.Segments[1].ToAirport.Name);
			Assert.AreEqual("VV", ticket.Segments[1].CarrierIataCode);
			Assert.AreEqual("0132", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("X", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 5, 2, 18, 25, 0), ticket.Segments[1].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 5, 3, 11, 10, 0), ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("L", ticket.Segments[1].MealCodes);
			Assert.AreEqual(0, ticket.Segments[1].NumberOfStops);
			Assert.AreEqual("PC", ticket.Segments[1].Luggage);
			Assert.AreEqual("4", ticket.Segments[1].CheckInTerminal);
			Assert.IsNull(ticket.Segments[1].CheckInTime);
			Assert.AreEqual("09:45", ticket.Segments[1].Duration);
			Assert.AreEqual("B", ticket.Segments[1].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[1].Seat);
			Assert.AreEqual("XLPX3M1", ticket.Segments[1].FareBasis);

			Assert.AreEqual(new Money("USD", 418.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 3499.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 2173.90m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 5672.90m), ticket.Total);

			Assert.AreEqual(11, ticket.Fees.Count);
			Assert.AreSame(ticket, ticket.Fees[0].Document);
			Assert.AreEqual(new Money("UAH", 142.30m), ticket.Fees[0].Amount);
			Assert.AreEqual("YK", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 16.80m), ticket.Fees[1].Amount);
			Assert.AreEqual("UD", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 1506.40m), ticket.Fees[2].Amount);
			Assert.AreEqual("YQ", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 33.50m), ticket.Fees[3].Amount);
			Assert.AreEqual("UA", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 46.10m), ticket.Fees[4].Amount);
			Assert.AreEqual("YC", ticket.Fees[4].Code);
			Assert.AreEqual(new Money("UAH", 134.80m), ticket.Fees[5].Amount);
			Assert.AreEqual("US", ticket.Fees[5].Code);
			Assert.AreEqual(new Money("UAH", 134.80m), ticket.Fees[6].Amount);
			Assert.AreEqual("US", ticket.Fees[6].Code);
			Assert.AreEqual(new Money("UAH", 41.90m), ticket.Fees[7].Amount);
			Assert.AreEqual("XA", ticket.Fees[7].Code);
			Assert.AreEqual(new Money("UAH", 58.60m), ticket.Fees[8].Amount);
			Assert.AreEqual("XY", ticket.Fees[8].Code);
			Assert.AreEqual(new Money("UAH", 21.00m), ticket.Fees[9].Amount);
			Assert.AreEqual("AY", ticket.Fees[9].Code);
			Assert.AreEqual(new Money("UAH", 37.70m), ticket.Fees[10].Amount);
			Assert.AreEqual("XF", ticket.Fees[10].Code);

			Assert.AreEqual(new Money("UAH", 34.99m), ticket.Commission);
			Assert.AreEqual(1, ticket.CommissionPercent);
			Assert.AreEqual("CASH", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);
			Assert.AreEqual("NON ENDO REFUND REBOOK RESTR", ticket.Endorsement);
		}


		[Test]
		public void TestParseTicket2()
		{
			var docs = AirParser.Parse(Res.AirTicket2, new Currency("UAH"));

			Assert.AreEqual(5, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
			Assert.IsInstanceOf<AviaTicket>(docs[1]);
			Assert.IsInstanceOf<AviaTicket>(docs[2]);
			Assert.IsInstanceOf<AviaTicket>(docs[3]);
			Assert.IsInstanceOf<AviaTicket>(docs[4]);

			var ticket = (AviaTicket)docs[4];


			Assert.AreEqual("ZYT5D9", ticket.PnrCode);
			Assert.AreEqual("LH ZYT5D9", ticket.AirlinePnrCode);

			Assert.AreEqual("QLHLH0564", ticket.BookerOffice);
			Assert.AreEqual("1000AP", ticket.BookerCode);
			Assert.AreEqual("DOKC32530", ticket.TicketerOffice);
			Assert.AreEqual("72320942", ticket.TicketingIataOffice);
			Assert.AreEqual("0001AA", ticket.TicketerCode);

			Assert.AreEqual("220", ticket.AirlinePrefixCode);

			Assert.AreEqual("3324650332", ticket.Number);
			Assert.AreEqual("PERTSEVA/OLENA MRS", ticket.PassengerName);

			Assert.AreEqual(new DateTime(2009, 4, 1), ticket.IssueDate);

			Assert.AreEqual(4, ticket.Segments.Count);

			Assert.AreEqual(0, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("DOK", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("DONETSK", ticket.Segments[0].FromAirport.Name);
			Assert.AreEqual("MUC", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("MUNICH", ticket.Segments[0].ToAirport.Name);
			Assert.AreEqual("LH", ticket.Segments[0].CarrierIataCode);
			Assert.AreEqual("3249", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("W", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 14, 16, 0, 0), ticket.Segments[0].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 14, 18, 5, 0), ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("M", ticket.Segments[0].MealCodes);
			Assert.AreEqual(0, ticket.Segments[0].NumberOfStops);
			Assert.AreEqual("20K", ticket.Segments[0].Luggage);
			Assert.IsNull(ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.AreEqual("03:05", ticket.Segments[0].Duration);
			Assert.AreEqual("2", ticket.Segments[0].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("QAXGR", ticket.Segments[0].FareBasis);

			Assert.AreEqual(1, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("MUC", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("MUNICH", ticket.Segments[1].FromAirport.Name);
			Assert.AreEqual("MRS", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("MARSEILLE", ticket.Segments[1].ToAirport.Name);
			Assert.AreEqual("LH", ticket.Segments[1].CarrierIataCode);
			Assert.AreEqual("4368", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("W", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 14, 19, 25, 0), ticket.Segments[1].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 14, 21, 0, 0), ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("S", ticket.Segments[1].MealCodes);
			Assert.AreEqual(0, ticket.Segments[1].NumberOfStops);
			Assert.AreEqual("20K", ticket.Segments[1].Luggage);
			Assert.AreEqual("2", ticket.Segments[1].CheckInTerminal);
			Assert.AreEqual("18:45", ticket.Segments[1].CheckInTime);
			Assert.AreEqual("01:35", ticket.Segments[1].Duration);
			Assert.AreEqual("1", ticket.Segments[1].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[1].Seat);
			Assert.AreEqual("QAXGR", ticket.Segments[1].FareBasis);


			Assert.AreEqual(new Money("USD", 455.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 3771.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 2047.60m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 5818.60m), ticket.Total);

			//Assert.AreEqual(10, ticket.Fees.Count);

			Assert.AreEqual(new Money("UAH", 124.40m), ticket.Fees[0].Amount);
			Assert.AreEqual("YK", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 16.60m), ticket.Fees[1].Amount);
			Assert.AreEqual("UD", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 1270.40m), ticket.Fees[2].Amount);
			Assert.AreEqual("YQ", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 43.10m), ticket.Fees[3].Amount);
			Assert.AreEqual("UA", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 313.00m), ticket.Fees[4].Amount);
			Assert.AreEqual("RA", ticket.Fees[4].Code);
			Assert.AreEqual(new Money("UAH", 50.80m), ticket.Fees[5].Amount);
			Assert.AreEqual("DE", ticket.Fees[5].Code);
			Assert.AreEqual(new Money("UAH", 52.30m), ticket.Fees[6].Amount);
			Assert.AreEqual("QX", ticket.Fees[6].Code);
			Assert.AreEqual(new Money("UAH", 43.80m), ticket.Fees[7].Amount);
			Assert.AreEqual("IZ", ticket.Fees[7].Code);
			Assert.AreEqual(new Money("UAH", 43.00m), ticket.Fees[8].Amount);
			Assert.AreEqual("FR", ticket.Fees[8].Code);
			Assert.AreEqual(new Money("UAH", 90.20m), ticket.Fees[9].Amount);
			Assert.AreEqual("FR", ticket.Fees[9].Code);

			Assert.AreEqual(new Money("UAH", 1.00m), ticket.Commission);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.AreEqual("CASH", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);
			Assert.AreEqual("FT*IT9LH2IEV", ticket.TourCode);
			Assert.IsNull(ticket.Endorsement);


			//Assert.Fail("TODO: FareSegments");

		}


		[Test]
		public void TestParseTicket3()
		{
			var docs = AirParser.Parse(Res.AirTicket3, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
			Assert.IsInstanceOf<AviaTicket>(docs[1]);

			var ticket = (AviaTicket)docs[1];


			Assert.AreEqual("39CGHB", ticket.PnrCode);
			Assert.AreEqual("KL ZK2M5D", ticket.AirlinePnrCode);

			Assert.AreEqual("IEVU23561", ticket.BookerOffice);
			Assert.AreEqual("1984LX", ticket.BookerCode);
			Assert.AreEqual("IEVU23561", ticket.TicketerOffice);
			Assert.AreEqual("72320441", ticket.TicketingIataOffice);
			Assert.AreEqual("1984LX", ticket.TicketerCode);

			Assert.AreEqual("074", ticket.AirlinePrefixCode);

			Assert.AreEqual("3324622172", ticket.Number);
			Assert.AreEqual("73", ticket.ConjunctionNumbers);
			Assert.AreEqual("SAMOLEVSKA/OLENAMRS", ticket.PassengerName);

			Assert.AreEqual(new DateTime(2009, 4, 1), ticket.IssueDate);

			Assert.AreEqual(6, ticket.Segments.Count);

			Assert.AreEqual(0, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("KBP", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("KIEV BORISPOL", ticket.Segments[0].FromAirport.Name);
			Assert.AreEqual("AMS", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("AMSTERDAM", ticket.Segments[0].ToAirport.Name);
			Assert.AreEqual("KL", ticket.Segments[0].CarrierIataCode);
			Assert.AreEqual("1386", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("Q", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 7, 17, 30, 0), ticket.Segments[0].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 7, 19, 35, 0), ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("M", ticket.Segments[0].MealCodes);
			Assert.AreEqual(0, ticket.Segments[0].NumberOfStops);
			Assert.AreEqual("PC", ticket.Segments[0].Luggage);
			Assert.AreEqual("B", ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.AreEqual("03:05", ticket.Segments[0].Duration);
			Assert.IsNull(ticket.Segments[0].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("QLSXUA", ticket.Segments[0].FareBasis);

			Assert.AreEqual(1, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("AMS", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("AMSTERDAM", ticket.Segments[1].FromAirport.Name);
			Assert.AreEqual("DTW", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("DETROIT METRO", ticket.Segments[1].ToAirport.Name);
			Assert.AreEqual("KL", ticket.Segments[1].CarrierIataCode);
			Assert.AreEqual("6039", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("Q", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 10, 8, 5, 0), ticket.Segments[1].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 10, 10, 35, 0), ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("BS", ticket.Segments[1].MealCodes);
			Assert.AreEqual(0, ticket.Segments[1].NumberOfStops);
			Assert.AreEqual("PC", ticket.Segments[1].Luggage);
			Assert.IsNull(ticket.Segments[1].CheckInTerminal);
			Assert.IsNull(ticket.Segments[1].CheckInTime);
			Assert.AreEqual("08:30", ticket.Segments[1].Duration);
			Assert.AreEqual("EM", ticket.Segments[1].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[1].Seat);
			Assert.AreEqual("QLSXUA", ticket.Segments[1].FareBasis);

			Assert.AreEqual(2, ticket.Segments[2].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[2].Type);
			Assert.AreEqual("DTW", ticket.Segments[2].FromAirport.Code);
			Assert.AreEqual("DETROIT METRO", ticket.Segments[2].FromAirport.Name);
			Assert.AreEqual("FLL", ticket.Segments[2].ToAirport.Code);
			Assert.AreEqual("FT LAUDERDALE", ticket.Segments[2].ToAirport.Name);
			Assert.AreEqual("KL", ticket.Segments[2].CarrierIataCode);
			Assert.AreEqual("6242", ticket.Segments[2].FlightNumber);
			Assert.AreEqual("Q", ticket.Segments[2].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 10, 13, 41, 0), ticket.Segments[2].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 10, 16, 35, 0), ticket.Segments[2].ArrivalTime);
			Assert.AreEqual("L", ticket.Segments[2].MealCodes);
			Assert.AreEqual(0, ticket.Segments[2].NumberOfStops);
			Assert.AreEqual("PC", ticket.Segments[2].Luggage);
			Assert.AreEqual("EM", ticket.Segments[2].CheckInTerminal);
			Assert.IsNull(ticket.Segments[2].CheckInTime);
			Assert.AreEqual("02:54", ticket.Segments[2].Duration);
			Assert.AreEqual("1", ticket.Segments[2].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[2].Seat);
			Assert.AreEqual("QLSXUA", ticket.Segments[2].FareBasis);

			/*Assert.AreEqual(3, ticket.Segments[3].Position);
			Assert.AreEqual(FlightSegmentType.Voided, ticket.Segments[3].Type);
			Assert.AreEqual("FLL", ticket.Segments[3].FromAirport.Code);
			Assert.AreEqual("FT LAUDERDALE", ticket.Segments[3].FromAirport.Name);
			Assert.AreEqual("JFK", ticket.Segments[3].ToAirport.Code);
			Assert.AreEqual("NEW YORK JFK", ticket.Segments[3].ToAirport.Name);
*/
			Assert.AreEqual(3, ticket.Segments[3].Position);
			Assert.AreEqual(FlightSegmentType.Unticketed, ticket.Segments[3].Type);
			Assert.AreEqual("FLL", ticket.Segments[3].FromAirport.Code);
			Assert.AreEqual("FT LAUDERDALE", ticket.Segments[3].FromAirport.Name);
			Assert.AreEqual("JFK", ticket.Segments[3].ToAirport.Code);
			Assert.AreEqual("NEW YORK JFK", ticket.Segments[3].ToAirport.Name);
			Assert.AreEqual("DL", ticket.Segments[3].CarrierIataCode);
			Assert.AreEqual("1800", ticket.Segments[3].FlightNumber);
			Assert.AreEqual("M", ticket.Segments[3].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 18, 9, 0, 0), ticket.Segments[3].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 18, 11, 52, 0), ticket.Segments[3].ArrivalTime);
			Assert.AreEqual("F", ticket.Segments[3].MealCodes);
			Assert.AreEqual(0, ticket.Segments[3].NumberOfStops);
			Assert.IsNull(ticket.Segments[3].Luggage);
			Assert.AreEqual("2", ticket.Segments[3].CheckInTerminal);
			Assert.IsNull(ticket.Segments[3].CheckInTime);
			Assert.AreEqual("02:52", ticket.Segments[3].Duration);
			Assert.AreEqual("3", ticket.Segments[3].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[3].Seat);
			Assert.IsNull(ticket.Segments[3].FareBasis);

			Assert.AreEqual(4, ticket.Segments[4].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[4].Type);
			Assert.AreEqual("JFK", ticket.Segments[4].FromAirport.Code);
			Assert.AreEqual("NEW YORK JFK", ticket.Segments[4].FromAirport.Name);
			Assert.AreEqual("AMS", ticket.Segments[4].ToAirport.Code);
			Assert.AreEqual("AMSTERDAM", ticket.Segments[4].ToAirport.Name);
			Assert.AreEqual("KL", ticket.Segments[4].CarrierIataCode);
			Assert.AreEqual("0642", ticket.Segments[4].FlightNumber);
			Assert.AreEqual("Q", ticket.Segments[4].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 21, 17, 40, 0), ticket.Segments[4].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 22, 7, 15, 0), ticket.Segments[4].ArrivalTime);
			Assert.AreEqual("M", ticket.Segments[4].MealCodes);
			Assert.AreEqual(0, ticket.Segments[4].NumberOfStops);
			Assert.AreEqual("PC", ticket.Segments[4].Luggage);
			Assert.AreEqual("4", ticket.Segments[4].CheckInTerminal);
			Assert.IsNull(ticket.Segments[4].CheckInTime);
			Assert.AreEqual("07:35", ticket.Segments[4].Duration);
			Assert.IsNull(ticket.Segments[4].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[4].Seat);
			Assert.AreEqual("QLSXUA", ticket.Segments[4].FareBasis);

			Assert.AreEqual(5, ticket.Segments[5].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[5].Type);
			Assert.AreEqual("AMS", ticket.Segments[5].FromAirport.Code);
			Assert.AreEqual("AMSTERDAM", ticket.Segments[5].FromAirport.Name);
			Assert.AreEqual("KBP", ticket.Segments[5].ToAirport.Code);
			Assert.AreEqual("KIEV BORISPOL", ticket.Segments[5].ToAirport.Name);
			Assert.AreEqual("KL", ticket.Segments[5].CarrierIataCode);
			Assert.AreEqual("3096", ticket.Segments[5].FlightNumber);
			Assert.AreEqual("Q", ticket.Segments[5].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 22, 9, 15, 0), ticket.Segments[5].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 22, 13, 10, 0), ticket.Segments[5].ArrivalTime);
			Assert.AreEqual("L", ticket.Segments[5].MealCodes);
			Assert.AreEqual(0, ticket.Segments[5].NumberOfStops);
			Assert.AreEqual("PC", ticket.Segments[5].Luggage);
			Assert.IsNull(ticket.Segments[5].CheckInTerminal);
			Assert.IsNull(ticket.Segments[5].CheckInTime);
			Assert.AreEqual("02:55", ticket.Segments[5].Duration);
			Assert.AreEqual("B", ticket.Segments[5].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[5].Seat);
			Assert.AreEqual("QLSXUA", ticket.Segments[5].FareBasis);

			Assert.AreEqual("USD", ticket.Fare.Currency.Code);
			Assert.AreEqual(695.00m, ticket.Fare.Amount);
			Assert.AreEqual(new Money("UAH", 5761.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 3640.40m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 9401.40m), ticket.Total);

			Assert.AreEqual(15, ticket.Fees.Count);

			Assert.AreEqual(new Money("UAH", 140.90m), ticket.Fees[0].Amount);
			Assert.AreEqual("YK", ticket.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 16.60m), ticket.Fees[1].Amount);
			Assert.AreEqual("UD", ticket.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 1971.00m), ticket.Fees[2].Amount);
			Assert.AreEqual("YR", ticket.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 33.20m), ticket.Fees[3].Amount);
			Assert.AreEqual("UA", ticket.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 492.80m), ticket.Fees[4].Amount);
			Assert.AreEqual("KV", ticket.Fees[4].Code);
			Assert.AreEqual(new Money("UAH", 229.80m), ticket.Fees[5].Amount);
			Assert.AreEqual("RN", ticket.Fees[5].Code);
			Assert.AreEqual(new Money("UAH", 43.80m), ticket.Fees[6].Amount);
			Assert.AreEqual("VV", ticket.Fees[6].Code);
			Assert.AreEqual(new Money("UAH", 221.20m), ticket.Fees[7].Amount);
			Assert.AreEqual("CJ", ticket.Fees[7].Code);
			Assert.AreEqual(new Money("UAH", 45.60m), ticket.Fees[8].Amount);
			Assert.AreEqual("YC", ticket.Fees[8].Code);
			Assert.AreEqual(new Money("UAH", 133.50m), ticket.Fees[9].Amount);
			Assert.AreEqual("US", ticket.Fees[9].Code);
			Assert.AreEqual(new Money("UAH", 133.50m), ticket.Fees[10].Amount);
			Assert.AreEqual("US", ticket.Fees[10].Code);
			Assert.AreEqual(new Money("UAH", 41.50m), ticket.Fees[11].Amount);
			Assert.AreEqual("XA", ticket.Fees[11].Code);
			Assert.AreEqual(new Money("UAH", 58.10m), ticket.Fees[12].Amount);
			Assert.AreEqual("XY", ticket.Fees[12].Code);
			Assert.AreEqual(new Money("UAH", 41.60m), ticket.Fees[13].Amount);
			Assert.AreEqual("AY", ticket.Fees[13].Code);
			Assert.AreEqual(new Money("UAH", 37.30m), ticket.Fees[14].Amount);
			Assert.AreEqual("XF", ticket.Fees[14].Code);

			Assert.AreEqual(new Money("UAH", 1.00m), ticket.Commission);
			Assert.IsNull(ticket.CommissionPercent);
			Assert.AreEqual("CASH", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);
			Assert.IsNull(ticket.TourCode);
			Assert.AreEqual("NONENDO/NONREF", ticket.Endorsement);
		}

		[Test]
		public void TestParseTicket8()
		{
			var docs = AirParser.Parse(Res.AirTicket8, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];


			Assert.AreEqual("5U2CGW", ticket.PnrCode);
			Assert.AreEqual("TK RSVVNR", ticket.AirlinePnrCode);

			Assert.AreEqual("IEVU23561", ticket.BookerOffice);
			Assert.AreEqual("7337EV", ticket.BookerCode);
			Assert.AreEqual("IEVU23561", ticket.TicketerOffice);
			Assert.AreEqual("72320441", ticket.TicketingIataOffice);
			Assert.AreEqual("7337EV", ticket.TicketerCode);

			Assert.AreEqual("235", ticket.AirlinePrefixCode);
			Assert.AreEqual("TK", ticket.Producer.AirlineIataCode);
			Assert.AreEqual("235", ticket.Producer.AirlinePrefixCode);
			Assert.AreEqual("TURKISH AIRLINES", ticket.Producer.Name);

			Assert.AreEqual("3324678964", ticket.Number);
			Assert.AreEqual("AVRAMENKO/OLEH MR", ticket.PassengerName);

			Assert.AreEqual(new DateTime(2009, 4, 22), ticket.IssueDate);

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreEqual(0, ticket.Segments[0].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[0].Type);
			Assert.AreEqual("KRT", ticket.Segments[0].FromAirport.Code);
			Assert.AreEqual("KHARTOUM", ticket.Segments[0].FromAirport.Name);
			Assert.AreEqual("IST", ticket.Segments[0].ToAirport.Code);
			Assert.AreEqual("ISTANBUL", ticket.Segments[0].ToAirport.Name);
			Assert.AreEqual("TK", ticket.Segments[0].CarrierIataCode);
			Assert.AreSame(ticket.Producer, ticket.Segments[0].Carrier);
			Assert.AreEqual("1151", ticket.Segments[0].FlightNumber);
			Assert.AreEqual("Y", ticket.Segments[0].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 23, 2, 20, 0), ticket.Segments[0].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 23, 6, 35, 0), ticket.Segments[0].ArrivalTime);
			Assert.AreEqual("M", ticket.Segments[0].MealCodes);
			Assert.AreEqual(0, ticket.Segments[0].NumberOfStops);
			Assert.AreEqual("20K", ticket.Segments[0].Luggage);
			Assert.IsNull(ticket.Segments[0].CheckInTerminal);
			Assert.IsNull(ticket.Segments[0].CheckInTime);
			Assert.AreEqual("04:15", ticket.Segments[0].Duration);
			Assert.AreEqual("I", ticket.Segments[0].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("YIF", ticket.Segments[0].FareBasis);

			Assert.AreEqual(1, ticket.Segments[1].Position);
			Assert.AreEqual(FlightSegmentType.Ticketed, ticket.Segments[1].Type);
			Assert.AreEqual("IST", ticket.Segments[1].FromAirport.Code);
			Assert.AreEqual("ISTANBUL", ticket.Segments[1].FromAirport.Name);
			Assert.AreEqual("KBP", ticket.Segments[1].ToAirport.Code);
			Assert.AreEqual("KIEV BORISPOL", ticket.Segments[1].ToAirport.Name);
			Assert.AreEqual("TK", ticket.Segments[1].CarrierIataCode);
			Assert.AreSame(ticket.Producer, ticket.Segments[1].Carrier);
			Assert.AreEqual("1457", ticket.Segments[1].FlightNumber);
			Assert.AreEqual("Y", ticket.Segments[1].ServiceClassCode);
			Assert.AreEqual(new DateTime(2009, 4, 23, 8, 45, 0), ticket.Segments[1].DepartureTime);
			Assert.AreEqual(new DateTime(2009, 4, 23, 10, 45, 0), ticket.Segments[1].ArrivalTime);
			Assert.AreEqual("M", ticket.Segments[1].MealCodes);
			Assert.AreEqual(0, ticket.Segments[1].NumberOfStops);
			Assert.AreEqual("20K", ticket.Segments[1].Luggage);
			Assert.AreEqual("I", ticket.Segments[1].CheckInTerminal);
			Assert.IsNull(ticket.Segments[1].CheckInTime);
			Assert.AreEqual("02:00", ticket.Segments[1].Duration);
			Assert.AreEqual("B", ticket.Segments[1].ArrivalTerminal);
			Assert.IsNull(ticket.Segments[1].Seat);
			Assert.AreEqual("YIF", ticket.Segments[1].FareBasis);

			Assert.AreEqual("USD", ticket.Fare.Currency.Code);
			Assert.AreEqual(1085.00m, ticket.Fare.Amount);
			Assert.IsNull(ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 0.00m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 0.00m), ticket.Total);

			Assert.AreEqual(0, ticket.Commission.Amount);
			Assert.AreEqual("UAH", ticket.Commission.Currency.Code);
			Assert.AreEqual(1, ticket.CommissionPercent);
			Assert.AreEqual("O/INVOICE+/INVOICE", ticket.PaymentForm);
			Assert.AreEqual(PaymentType.Invoice, ticket.PaymentType);
			Assert.IsNull(ticket.TourCode);
			Assert.IsNull(ticket.Endorsement);
		}

		[Test]
		public void TestParseTicket11()
		{
			var docs = AirParser.Parse(Res.AirTicket11, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			Assert.AreEqual(PaymentType.Invoice, ((AviaTicket)docs[0]).PaymentType);
		}

		[Test]
		public void TestParseTicket12()
		{
			var docs = AirParser.Parse(Res.AirTicket12, new Currency("UAH"));

			Assert.AreEqual(4, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
		}

		[Test]
		public void TestParseTicket13()
		{
			var docs = AirParser.Parse(Res.AirTicket13, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
		}

		[Test]
		public void TestParseTicket14()
		{
			var docs = AirParser.Parse(Res.AirTicket14, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			AssertSegmentCoupons((AviaTicket)docs[0], 6303.71m, null, 2083.00m);
		}

		[Test]
		public void TestParseTicket15()
		{
			var docs = AirParser.Parse(Res.AirTicket15, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			AssertSegmentCoupons((AviaTicket)docs[0], 390.00m, 390.00m);
		}

		[Test]
		public void TestParseTicket16()
		{
			var docs = AirParser.Parse(Res.AirTicket16, new Currency("UAH"));

			Assert.AreEqual(3, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			AssertSegmentCoupons((AviaTicket)docs[0], null, null);
		}

		[Test]
		public void TestParserTicket17()
		{
			var docs = AirParser.Parse(Res.AirTicket17, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);

			Assert.IsInstanceOf<AviaTicket>(docs[0]);
			Assert.IsInstanceOf<AviaTicket>(docs[1]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(4, ticket.Segments.Count);
			Assert.AreEqual("01D", ticket.Segments[0].Seat);
			Assert.AreEqual("03F", ticket.Segments[1].Seat);
			Assert.AreEqual("03F", ticket.Segments[2].Seat);
			Assert.AreEqual("01A", ticket.Segments[3].Seat);

			ticket = (AviaTicket)docs[1];

			Assert.AreEqual(4, ticket.Segments.Count);
			Assert.AreEqual("01F", ticket.Segments[0].Seat);
			Assert.AreEqual("03E", ticket.Segments[1].Seat);
			Assert.AreEqual("03E", ticket.Segments[2].Seat);
			Assert.AreEqual("01C", ticket.Segments[3].Seat);

			AssertSegmentCoupons(ticket, null, 945.00m,null, 945.00m);
		}

		[Test]
		public void TestParserTicket18()
		{
			var docs = AirParser.Parse(Res.AirTicket18, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(4, ticket.Segments.Count);
			Assert.IsNull(ticket.Segments[0].Seat);
			Assert.AreEqual("07K", ticket.Segments[1].Seat);
			Assert.AreEqual("11B", ticket.Segments[2].Seat);
			Assert.IsNull(ticket.Segments[3].Seat);

			AssertSegmentCoupons(ticket, null, 274.50m, null, 274.50m);
		}

		[Test]
		public void TestParserTicket27()
		{
			var docs = AirParser.Parse(Res.AirTicket27, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];
		}

		[Test]
		public void TestParserTicket28()
		{
			var docs = AirParser.Parse(Res.AirTicket28, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];
		}

		[Test]
		public void TestParserTicket29()
		{
			var docs = AirParser.Parse(Res.AirTicket29, new Currency("UAH"));
		}


		[Test]
		public void TestParserTicket30()
		{
			var docs = AirParser.Parse(Res.AirTicket30, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];
		}


		[Test]
		public void TestParserTicket31()
		{
			var docs = AirParser.Parse(
@"AIR-BLK207;7A;;244;0000000000;1A1326991;001001
AMDR2013238813;1/1;              
1A1226242;1A1326991;IEV1A0985;AIR
MUC1A NITMT2007;0101;IEVUW2109;72325013;IEVUW2109;72325013;IEVUW2109;72325013;IEVUW2109;72325013;;;;;;;;;;;;;;;;;;;;;;L6 MHQWH
A-HAHN AIR;HR 1691
B-TTP/RT
C-7906/ 1501TGSU-1501TGSU-I-0--
D-200120;200120;200120
G-X  ;;NDBLPA;
H-002;002ONDB;NOUADHIBOU       ;LPA;GRAN CANARIA     ;L6    0110 O O 02FEB1000 1120 02FEB;OK01;HK01;B ;0;E75;;;2PC;;;ET;0120 ;N;493;MR;ES;
K-FMRU1636.00    ;UAH1070       ;;;;;;;;;;;UAH4234       ;0.653747   ;;
KFTF; UAH1348     YQ AC; UAH399      HO TR; UAH393      MR AE; UAH197      MR LO; UAH827      ZI DP;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
TAX-UAH1348     YQ ;UAH399      HO ;UAH1417     XT ;
L-
M-OAOWMR         
N-NUC43.16
O-XXXX;LD02FEB202359
Q-NDB L6 LPA43.16NUC43.16END ROE37.901260XT393MR197MR827ZI;FXB
I-001;01RUD/ANDREY MR;;APIEV 38044 2067578 - UFSA - A//E-ANDREYRUDD@GMAIL.COM;;
SSR OTHS 1A  /AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY L6 BY 0809/21JAN/NKC LT
SSR OTHS 1A  /MOBILE AND EMAIL CONTACT USING SSR REQUIRED X PLS PROVIDE
SSR DOCS L6  HK1/P/RUS/530589790/RUS/05FEB70/M/28APR24/RUD/ANDREY;P1
SSR CTCE L6  HK1/ANDREYRUDD//GMAIL.COM
SSR CTCM L6  HK1/380442067575
T-K169-9487880809
FM*M*1A
FPCASH
FVHR;S2;P1
TKOK20JAN/IEVUW2109//ETHR
RIIUSD205
ENDX"
				, new Currency("UAH")
			);

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);
			var ticket = (AviaTicket)docs[0];
		}


		#endregion


		[Test]
		public void TestParseVatAndServiceFee()
		{
			var docs = AirParser.Parse(Res.AirTicket9, new Currency("UAH"));

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(new Money("UAH", 375.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 348.70m), ticket.FeesTotal);
			Assert.IsNull(ticket.Vat);
			Assert.AreEqual(new Money("UAH", 723.70m), ticket.Total);
			Assert.AreEqual(new Money("UAH", 100.00m), ticket.ServiceFee);
			Assert.AreEqual(new Money("UAH", 823.70m), ticket.GrandTotal);
		}


		#region Parse Void

		[Test]
		public void TestParseVoid1()
		{
			var docs = AirParser.Parse(Res.AirVoid1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaDocumentVoiding>(docs[0]);

			var voiding = (AviaDocumentVoiding)docs[0];

			Assert.IsInstanceOf<AviaTicket>(voiding.Document);
			Assert.AreEqual("870", voiding.Document.AirlinePrefixCode);
			Assert.AreEqual("3324622080", voiding.Document.Number);
			Assert.AreEqual(true, voiding.IsVoid);
			Assert.AreEqual(new DateTime(DateTime.Today.Year, 3, 24), voiding.TimeStamp);
			Assert.AreEqual(GdsOriginator.Amadeus, voiding.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, voiding.Origin);
			Assert.AreEqual("IEVU23561", voiding.AgentOffice);
			Assert.AreEqual("72320441", voiding.IataOffice);
			Assert.AreEqual("LX", voiding.AgentCode);
		}

		[Test]
		public void TestParseVoid2()
		{
			var docs = AirParser.Parse(Res.AirVoid2, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaDocumentVoiding>(docs[0]);

			var voiding = (AviaDocumentVoiding)docs[0];

			Assert.IsInstanceOf<AviaRefund>(voiding.Document);
			Assert.AreEqual("870", voiding.Document.AirlinePrefixCode);
			Assert.AreEqual("3103872242", voiding.Document.Number);
			Assert.AreEqual(true, voiding.IsVoid);
			Assert.AreEqual(new DateTime(DateTime.Today.Year, 8, 6), voiding.TimeStamp);
			Assert.AreEqual(GdsOriginator.Amadeus, voiding.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, voiding.Origin);
			Assert.AreEqual("IEVU22118", voiding.AgentOffice);
			Assert.AreEqual("72320135", voiding.IataOffice);
			Assert.AreEqual("AA", voiding.AgentCode);
		}

		[Test]
		public void TestParseVoid3()
		{
			var docs = AirParser.Parse(Res.AirVoid3, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaDocumentVoiding>(docs[0]);

			var voiding = (AviaDocumentVoiding)docs[0];

			Assert.IsInstanceOf<AviaTicket>(voiding.Document);
			Assert.AreEqual("220", voiding.Document.AirlinePrefixCode);
			Assert.AreEqual("1791448244", voiding.Document.Number);
			Assert.AreEqual(false, voiding.IsVoid);
			Assert.AreEqual(new DateTime(DateTime.Today.Year, 2, 22), voiding.TimeStamp);
			Assert.AreEqual(GdsOriginator.Amadeus, voiding.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, voiding.Origin);
		}

		[Test]
		public void TestParseVoid4()
		{
			var docs = AirParser.Parse(Res.AirVoid4, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaDocumentVoiding>(docs[0]);

			var voiding = (AviaDocumentVoiding)docs[0];

			Assert.IsInstanceOf<AviaMco>(voiding.Document);

			Assert.AreEqual("220", voiding.Document.AirlinePrefixCode);
			Assert.AreEqual("1927917694", voiding.Document.Number);
			Assert.AreEqual(false, voiding.IsVoid);
			Assert.AreEqual(new DateTime(DateTime.Today.Year, 8, 17), voiding.TimeStamp);
		}

		[Test]
		public void TestParseVoid5()
		{
			var docs = AirParser.Parse(Res.AirVoid5, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaDocumentVoiding>(docs[0]);

			var voiding = (AviaDocumentVoiding)docs[0];

			Assert.IsInstanceOf<AviaRefund>(voiding.Document);
			Assert.AreEqual("566", voiding.Document.AirlinePrefixCode);
			Assert.AreEqual("6972253583", voiding.Document.Number);
			Assert.AreEqual(true, voiding.IsVoid);
			Assert.AreEqual(new DateTime(DateTime.Today.Year, 3, 30), voiding.TimeStamp);
		}

		#endregion


		#region Parse Mco

		[Test]
		public void TestParseMco1()
		{
			var docs = AirParser.Parse(Res.AirMco1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var mco1 = (AviaMco)docs[0];

			Assert.AreEqual(GdsOriginator.Amadeus, mco1.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, mco1.Origin);

			Assert.AreEqual("YQV5WF", mco1.PnrCode);
			Assert.AreEqual("VV YQV5WF", mco1.AirlinePnrCode);

			Assert.AreEqual("IEVU23561", mco1.BookerOffice);
			Assert.AreEqual("1962VZ", mco1.BookerCode);
			Assert.AreEqual("IEVU23561", mco1.TicketerOffice);
			Assert.AreEqual("1717LS", mco1.TicketerCode);

			Assert.AreEqual("870", mco1.AirlinePrefixCode);
			Assert.AreEqual("AEROSVIT AIRLINES", mco1.Producer.Name);
			Assert.AreEqual("VV", mco1.Producer.AirlineIataCode);
			Assert.AreEqual("870", mco1.Producer.AirlinePrefixCode);

			Assert.AreEqual("1927920633", mco1.Number);
			Assert.AreEqual("KHONIN/DMITRYMR", mco1.PassengerName);

			Assert.AreEqual(new DateTime(2008, 2, 7), mco1.IssueDate);

			Assert.AreEqual(new Money("UAH", 101.00m), mco1.Fare);

			Assert.IsNull(mco1.EqualFare);

			Assert.AreEqual(new Money("UAH", 0.00m), mco1.FeesTotal);

			Assert.AreEqual(new Money("UAH", 101.00m), mco1.Total);

			Assert.AreEqual(0, mco1.Fees.Count);

			Assert.AreEqual(new Money("UAH", 0.00m), mco1.Commission);

			Assert.AreEqual(0, mco1.CommissionPercent);

			Assert.AreEqual("CASH", mco1.PaymentForm);
		}

		[Test]
		public void TestParseMco2()
		{
			var docs = AirParser.Parse(Res.AirMco2, new Currency("UAH"));

			Assert.AreEqual(3, docs.Count);

			var mco = (AviaMco)docs[0];

			Assert.AreEqual("335LNS", mco.PnrCode);
			Assert.AreEqual("SU GUIAQC", mco.AirlinePnrCode);

			Assert.AreEqual("IEVU22118", mco.BookerOffice);
			Assert.AreEqual("0007OP", mco.BookerCode);
			Assert.AreEqual("IEVU22118", mco.TicketerOffice);
			Assert.AreEqual("0004AD", mco.TicketerCode);

			Assert.AreEqual("555", mco.AirlinePrefixCode);
			Assert.AreEqual("AEROFLOT", mco.Producer.Name);
			Assert.AreEqual("SU", mco.Producer.AirlineIataCode);
			Assert.AreEqual("555", mco.Producer.AirlinePrefixCode);

			Assert.AreEqual("1927917690", mco.Number);
			Assert.AreEqual("CHEREDNYK/OLEKSIY MR", mco.PassengerName);

			Assert.AreEqual(new DateTime(2007, 8, 15), mco.IssueDate);

			Assert.AreEqual(new Money("UAH", 143.55m), mco.Fare);

			Assert.IsNull(mco.EqualFare);

			Assert.AreEqual(new Money("UAH", 0.00m), mco.FeesTotal);

			Assert.AreEqual(new Money("UAH", 143.55m), mco.Total);

			Assert.AreEqual(0, mco.Fees.Count);

			Assert.AreEqual(new Money("UAH", 0.00m), mco.Commission);

			Assert.AreEqual(0, mco.CommissionPercent);

			Assert.AreEqual("CASH", mco.PaymentForm);

			mco = (AviaMco)docs[2];

			Assert.AreEqual("335LNS", mco.PnrCode);
			Assert.AreEqual("SU GUIAQC", mco.AirlinePnrCode);

			Assert.AreEqual("IEVU22118", mco.BookerOffice);
			Assert.AreEqual("0007OP", mco.BookerCode);
			Assert.AreEqual("IEVU22118", mco.TicketerOffice);
			Assert.AreEqual("0004AD", mco.TicketerCode);

			Assert.AreEqual("555", mco.AirlinePrefixCode);
			Assert.AreEqual("AEROFLOT", mco.Producer.Name);
			Assert.AreEqual("SU", mco.Producer.AirlineIataCode);
			Assert.AreEqual("555", mco.Producer.AirlinePrefixCode);

			Assert.AreEqual("1927917692", mco.Number);
			Assert.AreEqual("YAREMYCH/DMYTRO MR", mco.PassengerName);

			Assert.AreEqual(new DateTime(2007, 8, 15), mco.IssueDate);

			Assert.AreEqual(new Money("UAH", 143.55m), mco.Fare);

			Assert.IsNull(mco.EqualFare);

			Assert.AreEqual(new Money("UAH", 0.00m), mco.FeesTotal);

			Assert.AreEqual(new Money("UAH", 143.55m), mco.Total);

			Assert.AreEqual(0, mco.Fees.Count);

			Assert.AreEqual(new Money("UAH", 0.00m), mco.Commission);

			Assert.AreEqual(0, mco.CommissionPercent);

			Assert.AreEqual("CASH", mco.PaymentForm);
		}

		[Test]
		public void TestParseMco3()
		{
			var docs = AirParser.Parse(Res.AirMco3, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var mco1 = (AviaMco)docs[0];

			Assert.AreEqual("29DCTX", mco1.PnrCode);
			Assert.AreEqual("SU MASFHO", mco1.AirlinePnrCode);

			Assert.AreEqual("IEVU22118", mco1.BookerOffice);
			Assert.AreEqual("0002AB", mco1.BookerCode);
			Assert.AreEqual("IEVU22118", mco1.TicketerOffice);
			Assert.AreEqual("0002AB", mco1.TicketerCode);

			Assert.AreEqual("555", mco1.AirlinePrefixCode);
			Assert.AreEqual("AEROFLOT", mco1.Producer.Name);
			Assert.AreEqual("SU", mco1.Producer.AirlineIataCode);
			Assert.AreEqual("555", mco1.Producer.AirlinePrefixCode);

			Assert.AreEqual("1927917629", mco1.Number);
			Assert.AreEqual("GANZHENKO/ALEXEY MR", mco1.PassengerName);

			Assert.AreEqual(new DateTime(2007, 4, 24), mco1.IssueDate);

			Assert.AreEqual("EUR", mco1.Fare.Currency.Code);
			Assert.AreEqual(365.00m, mco1.Fare.Amount);

			Assert.AreEqual(new Money("UAH", 2499.00m), mco1.EqualFare);

			Assert.AreEqual(new Money("UAH", 396.70m), mco1.FeesTotal);

			Assert.AreEqual(new Money("UAH", 3072.70m), mco1.Total);

			Assert.AreEqual(5, mco1.Fees.Count);

			Assert.AreEqual(new Money("UAH", 124.95m), mco1.Commission);

			Assert.AreEqual(5, mco1.CommissionPercent);

			Assert.AreEqual("CASH", mco1.PaymentForm);
		}

		[Test]
		public void TestParseMco4()
		{
			var docs = AirParser.Parse(Res.AirMco4, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaMco>(docs[0]);
		}

		[Test]
		public void TestParseMco5()
		{
			var docs = AirParser.Parse(Res.AirMco5, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			Assert.IsInstanceOf<AviaMco>(docs[0]);
		}

		[Test]
		public void TestParseMco6()
		{
			var docs = AirParser.Parse(Res.AirMco6, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);

			Assert.IsInstanceOf<AviaMco>(docs[0]);
		}

		[Test]
		public void TestParseEmd1()
		{
			var docs = AirParser.Parse(Res.AirEmd1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var mco1 = (AviaMco)docs[0];

			Assert.AreEqual(GdsOriginator.Amadeus, mco1.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, mco1.Origin);

			Assert.AreEqual("75I2K4", mco1.PnrCode);
			Assert.AreEqual("BA NOSYNC", mco1.AirlinePnrCode);

			Assert.AreEqual("IEVUW2100", mco1.BookerOffice);
			Assert.AreEqual("2710YD", mco1.BookerCode);
			Assert.AreEqual("IEVUW2100", mco1.TicketerOffice);
			Assert.AreEqual("2710YD", mco1.TicketerCode);

			Assert.AreEqual("125", mco1.AirlinePrefixCode);
			Assert.AreEqual("BRITISH AIRWAYS", mco1.Producer.Name);
			Assert.AreEqual("BA", mco1.Producer.AirlineIataCode);
			Assert.AreEqual("125", mco1.Producer.AirlinePrefixCode);

			Assert.AreEqual("1814958750", mco1.Number);
			Assert.AreEqual("ZACHARIEVICH/NATALIA MRS", mco1.PassengerName);

			Assert.IsNotNull(mco1.InConnectionWith);
			Assert.AreEqual("125", mco1.InConnectionWith.AirlinePrefixCode);
			Assert.AreEqual("3232006652", mco1.InConnectionWith.Number);

			Assert.AreEqual(new DateTime(2013, 1, 18), mco1.IssueDate);

			Assert.AreEqual(new Money("USD", 210.00m), mco1.Fare);

			Assert.AreEqual(new Money("UAH", 1679), mco1.EqualFare);

			Assert.AreEqual(new Money("UAH", 0.00m), mco1.FeesTotal);

			Assert.AreEqual(new Money("UAH", 1679), mco1.Total);

			Assert.AreEqual(0, mco1.Fees.Count);

			Assert.IsNull(mco1.Commission);

			Assert.IsNull(mco1.CommissionPercent);

			Assert.AreEqual("CASH", mco1.PaymentForm);
		}


		#endregion


		#region Test Parse Refund

		[Test]
		public void TestParseRefund1()
		{
			var docs = AirParser.Parse(Res.AirRefund1, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var refund = (AviaRefund)docs[0];

			Assert.IsNull(refund.PnrCode);
			Assert.IsNull(refund.AirlinePnrCode);

			Assert.AreEqual(GdsOriginator.Amadeus, refund.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, refund.Origin);

			Assert.AreEqual("IEVU23561", refund.BookerOffice);
			Assert.AreEqual("1234ZX", refund.BookerCode);
			Assert.AreEqual("IEVU23561", refund.TicketerOffice);
			Assert.AreEqual("1234ZX", refund.TicketerCode);

			Assert.AreEqual("555", refund.AirlinePrefixCode);
			Assert.AreEqual("3324622050", refund.Number);

			Assert.AreEqual(new Money("UAH", 4002.00m), refund.Fare);
			Assert.AreEqual(new Money("UAH", 11.50m), refund.FeesTotal);
			Assert.AreEqual(new Money("UAH", 1000.50m), refund.CancelFee);
			Assert.AreEqual(new Money("UAH", 3013.00m), refund.Total);

			Assert.AreEqual(1, refund.Fees.Count);

			Assert.AreEqual("RU", refund.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 11.50m), refund.Fees[0].Amount);

			Assert.AreEqual("INVOICE/UAH3013.00", refund.PaymentForm);
			Assert.AreEqual(PaymentType.Invoice, refund.PaymentType);
		}

		[Test]
		public void TestParseRefund2()
		{
			var docs = AirParser.Parse(Res.AirRefund2, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var refund = (AviaRefund)docs[0];

			Assert.IsNull(refund.PnrCode);
			Assert.IsNull(refund.AirlinePnrCode);

			Assert.AreEqual("IEVU22221", refund.BookerOffice);
			Assert.AreEqual("5555LE", refund.BookerCode);
			Assert.AreEqual("IEVU22221", refund.TicketerOffice);
			Assert.IsNull(refund.TicketerCode);

			Assert.AreEqual("074", refund.AirlinePrefixCode);
			Assert.AreEqual("6972205888", refund.Number);

			Assert.AreEqual(new Money("UAH", 2828.00m), refund.Fare);
			Assert.AreEqual(new Money("UAH", 397.10m), refund.FeesTotal);
			Assert.AreEqual(new Money("UAH", 0.00m), refund.CancelFee);
			Assert.AreEqual(new Money("UAH", 3225.10m), refund.Total);

			Assert.AreEqual(4, refund.Fees.Count);

			Assert.AreEqual("YQ", refund.Fees[0].Code);
			Assert.AreEqual(new Money("UAH", 213.60m), refund.Fees[0].Amount);
			Assert.AreEqual("CH", refund.Fees[1].Code);
			Assert.AreEqual(new Money("UAH", 78.10m), refund.Fees[1].Amount);
			Assert.AreEqual("FR", refund.Fees[2].Code);
			Assert.AreEqual(new Money("UAH", 47.00m), refund.Fees[2].Amount);
			Assert.AreEqual("FR", refund.Fees[3].Code);
			Assert.AreEqual(new Money("UAH", 58.40m), refund.Fees[3].Amount);
		}

		[Test]
		public void TestParseRefund3()
		{
			var docs = AirParser.Parse(Res.AirRefund3, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
		}

		[Test]
		public void TestParseRefund6()
		{
			var docs = AirParser.Parse(Res.AirRefund6, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);

			var refund = (AviaRefund)docs[0];

			Assert.IsNull(refund.PnrCode);
			Assert.IsNull(refund.AirlinePnrCode);


			Assert.AreEqual(new Money("UAH", 1.00m), refund.Commission);
		}


		#endregion


		#region Test Parse Reservation

		[Test]
		public void TestParseReservation1()
		{
			var docs = AirParser.Parse(Res.AirReservation1, AmadeusRizUsingMode.ServiceFeeOnly, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(GdsOriginator.Amadeus, ticket.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, ticket.Origin);

			Assert.AreEqual("YCWIAP", ticket.PnrCode);
			Assert.AreEqual("VV YCWIAP", ticket.AirlinePnrCode);

			Assert.AreEqual("IEVUW2100", ticket.BookerOffice);
			Assert.AreEqual("0101SN", ticket.BookerCode);
			Assert.AreEqual("IEVUW2100", ticket.TicketerOffice);
			Assert.AreEqual("1701GT", ticket.TicketerCode);

			Assert.IsNull(ticket.Producer);

			Assert.AreEqual("VV", ticket.AirlineIataCode);
			Assert.IsNull(ticket.AirlinePrefixCode);
			Assert.IsNull(ticket.Number);

			Assert.AreEqual("BLOKHA/OKSANA MRS", ticket.PassengerName);
			Assert.AreEqual(new DateTime(2010, 6, 14), ticket.IssueDate);

			Assert.AreEqual(1, ticket.Segments.Count);
			Assert.AreEqual(FlightSegmentType.Unticketed, ticket.Segments[0].Type);

			Assert.AreEqual(new Money("USD", 121.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 958.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 229.70m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 1187.70m), ticket.Total);

			Assert.AreEqual(new Money("UAH", 200.00m), ticket.ServiceFee);

			Assert.AreEqual(4, ticket.Fees.Count);

			Assert.AreEqual(PaymentType.Cash, ticket.PaymentType);
		}

		[Test]
		public void TestParseReservation2()
		{
			var docs = AirParser.Parse(Res.AirReservation2, new Currency("UAH"));

			Assert.AreEqual(12, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(GdsOriginator.Amadeus, ticket.Originator);
			Assert.AreEqual(ProductOrigin.AmadeusAir, ticket.Origin);

			Assert.AreEqual("34N4TF", ticket.PnrCode);
			Assert.AreEqual("7W HQNMK", ticket.AirlinePnrCode);

			Assert.IsNull(ticket.Producer);

			Assert.AreEqual("7W", ticket.AirlineIataCode);
			Assert.IsNull(ticket.AirlinePrefixCode);

			Assert.IsNull(ticket.Number);

			Assert.AreEqual("BOCHAROVA/OLENA MRS", ticket.PassengerName);
			Assert.AreEqual(2, ticket.Segments.Count);
			Assert.AreEqual(new Money("USD", 268.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 2121.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 410.20m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 2531.20m), ticket.Total);

			ticket = (AviaTicket)docs[1];

			Assert.AreEqual("GORSHYKHINA/MARIIA MRS", ticket.PassengerName);
			Assert.AreEqual("7W", ticket.AirlineIataCode);
			Assert.AreEqual(2, ticket.Segments.Count);
			Assert.AreEqual(new Money("USD", 268.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 2121.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 410.20m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 2531.20m), ticket.Total);

			ticket = (AviaTicket)docs[2];

			Assert.AreEqual("KOVALENKO/OLEKSII MR", ticket.PassengerName);
			Assert.AreEqual("7W", ticket.AirlineIataCode);
			Assert.AreEqual(2, ticket.Segments.Count);
			Assert.AreEqual(new Money("USD", 268.00m), ticket.Fare);
			Assert.AreEqual(new Money("UAH", 2121.00m), ticket.EqualFare);
			Assert.AreEqual(new Money("UAH", 410.20m), ticket.FeesTotal);
			Assert.AreEqual(new Money("UAH", 2531.20m), ticket.Total);
		}

		[Test]
		public void TestParseReservation3()
		{
			var docs = AirParser.Parse(Res.AirReservation3, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual("Z6", ticket.AirlineIataCode);
		}

		[Test]
		public void TestParseReservation4()
		{
			var docs = AirParser.Parse(Res.AirReservation4, AmadeusRizUsingMode.ServiceFeeOnly, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(null, ticket.ServiceFee);
		}

		[Test]
		public void TestParseReservation5()
		{
			var docs = AirParser.Parse(Res.AirReservation6, AmadeusRizUsingMode.ServiceFeeOnly, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(new Money("USD", 251), ticket.Fare);
		}

		#endregion


		[Test]
		public void TestParseRemarks1()
		{
			var docs = AirParser.Parse(Res.AirTicket19, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(@"ON-LINE BOOKING UFSA
ID1/ON-LINE BOOKING UFSA SIP
DELIVER E-TKT
FOP/ CASH", ticket.Remarks);
		}

		[Test]
		public void TestParseHandTicketNumber()
		{
			var docs = AirParser.Parse(Res.AirReservation5, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(false, ticket.IsReservation);
			//Assert.AreEqual(AviaDocumentOrigin.AmadeusPrint, ticket.Origin);
			Assert.AreEqual("250", ticket.AirlinePrefixCode);
			Assert.AreEqual("4403858754", ticket.Number);

			Assert.AreEqual(4, ticket.GetTicketedSegments().Count);
		}

		[Test]
		public void TestParse1()
		{
			var docs = AirParser.Parse(Res.AirTicket21, new Currency("UAH"));

			Assert.AreEqual(1, docs.Count);
			Assert.IsInstanceOf<AviaTicket>(docs[0]);

			var ticket = (AviaTicket)docs[0];

			Assert.AreEqual(2, ticket.Segments.Count);

			Assert.AreEqual(new DateTime(2010, 11, 15, 0, 0, 0), ticket.Segments[0].ArrivalTime);
		}

		[Test]
		public void TestAirReservation7()
		{
			var docs = AirParser.Parse(Res.AirReservation7, new Currency("UAH"));

			Assert.AreEqual(2, docs.Count);

			Assert.AreEqual("DVORKIN/YEHUDIT(INF/CHAYAMUSHKA/10APR10)", ((AviaTicket)docs[0]).PassengerName);
			Assert.AreEqual("DVORKIN/CHAYAMUSHKA(INF)", ((AviaTicket)docs[1]).PassengerName);
		}


		#region Tools

		public void AssertSegmentCoupons(AviaTicket ticket, params decimal?[] amounts)
		{
			var i = 0;
			foreach (var amount in amounts)
			{
				var  couponAmount = ticket.Segments[i++].CouponAmount;
				if (amount == null)
					Assert.IsNull(couponAmount);
				else
				{
					Assert.IsNotNull(couponAmount);
					Assert.AreEqual(amount, couponAmount.Amount);
				}
			}
		}

		#endregion
	}


}