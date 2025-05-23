using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;




namespace Luxena.Travel.Tests.Parsers
{



	//===g






	[TestFixture]
	public class AdamAiJsonParserTests
	{

		//---g



		[Test]
		public void Money_01()
		{
			AssertMoney("€378.08", "EUR", 378.08m);
			AssertMoney("79,00 zł", "PLN", 79m);
			AssertMoney("$138.08", "USD", 138.08m);
			AssertMoney("1260.00 UAH", "UAH", 1260);
			AssertMoney("279.00", "UAH", 279);
			AssertMoney("2548,00", "UAH",2548);
			AssertMoney("116 EUR", "EUR", 116);
			AssertMoney("CHF 20.40", "CHF", 20.40m);
			AssertMoney("3236.80ГРН", "UAH", 3236.80m);
			AssertMoney("UAH 1500", "UAH", 1500);
			AssertMoney("203.38 EUR", "EUR", 203.38m);
			AssertMoney("UAH 9845", "UAH", 9845);
			AssertMoney("EUR 785.00", "EUR", 785.00m);
		}

		[Test]
		public void Money_02()
		{
			AssertMoney("279.00", "UAH", 279);
		}


		public void AssertMoney(string money, string currency, decimal amount)
		{
			var m = AdamAiJsonParser.ParseMoney(money, "UAH");
			Assert.NotNull(m);
			Assert.AreEqual(currency, m.Currency.Code);
			Assert.AreEqual(amount, m.Amount);
		}




		//		[Test]
		//		public void Product_00_EMD()
		//		{
		//			var r = ParseAviaMco(
		//@"{
		//    'date_of_issue': '05.08.2024',

		//	'booking_number': 'K7GG7LD',
		//    'agent_ID': 'Volodymyr Hokh',
		//    'passenger_title': 'Mrs',
		//    'passenger_name': 'ANTONINA',
		//    'passenger_surname': 'PATRAMANSKA',
		//    'route': 'Lisbon (T1) to Nice (Terminal 2)',
		//    'baggage': '1 x under seat cabin bag (max. 45 x 36 x 20 cm), 1 x large cabin bag (max. 56 x 45 x 25 cm)',
		//    'seat': 'Auto allocated',
		//    'flight_number': 'EJU7611',
		//    'departure_date': '18.06.2024 13:45',
		//    'arrival_date': '18.06.2024 17:10',
		//    'grand_total': '€378.08',
		//    'booking_date': '',
		//    'ticketing_date': '',
		//    'type_of_issue': 'EMD'
		//}");


		//			Assert.AreEqual(DateTime.Parse("2024-08-05"), r.IssueDate);

		//			Assert.AreEqual("K7GG7LD", r.PnrCode);
		//			Assert.AreEqual("MAKSYMOVSKYI/RUSLAN MR", r.PassengerName);
		//			Assert.AreEqual("EUR", r.Fare.Currency);
		//			Assert.AreEqual(203.38d, r.Fare.Amount);

		//			Assert.AreEqual(1, r.Segments.Count);
		//			Assert.AreEqual("OTP", r.Segments[0].FromAirportCode);
		//			Assert.AreEqual("TIA", r.Segments[0].ToAirportCode);
		//			Assert.AreEqual("FR8415", r.Segments[0].FlightNumber);
		//			Assert.AreEqual("20C", r.Segments[0].Seat);
		//			Assert.AreEqual(DateTime.Parse("2025-05-05T11:00"), r.Segments[0].DepartureTime);
		//			Assert.AreEqual(DateTime.Parse("2025-05-05T11:30"), r.Segments[0].ArrivalTime);
		//		}



		[Test]
		public void Product_01_Ticket()
		{
			var r = ParseAviaTicket(
@"{
    'date_of_issue': '14.05.2025',

	'booking_number': 'A38DPB',
    'agent_ID': 'max_sadovski',
    'passenger_title': 'Mr',
    'passenger_name': 'RUSLAN',
    'passenger_surname': 'MAKSYMOVSKYI',
    'route': 'from OTP to TIA',
    'baggage': 'Checked Bag (20kg)',
    'seat': '20C',
    'flight_number': 'FR8415',
    'departure_date': '05.05.2025 11:00',
    'arrival_date': '05.05.2025 11:30',
    'grand_total': '203.38 EUR',
    'booking_date': '',
    'ticketing_date': '',
    'type_of_issue': 'Ticket'
}");


			Assert.AreEqual(DateTime.Parse("2025-05-14"), r.IssueDate);

			Assert.AreEqual("A38DPB", r.PnrCode);
			Assert.AreEqual("MAKSYMOVSKYI/RUSLAN MR", r.PassengerName);
			Assert.AreEqual("EUR", r.Fare.Currency.Code);
			Assert.AreEqual(203.38m, r.Fare.Amount);

			Assert.AreEqual(1, r.Segments.Count);
			Assert.AreEqual("OTP", r.Segments[0].FromAirportCode);
			Assert.AreEqual("TIA", r.Segments[0].ToAirportCode);
			Assert.AreEqual("FR8415", r.Segments[0].FlightNumber);
			Assert.AreEqual("20C", r.Segments[0].Seat);
			Assert.AreEqual(DateTime.Parse("2025-05-05T11:00"), r.Segments[0].DepartureTime);
			Assert.AreEqual(DateTime.Parse("2025-05-05T11:30"), r.Segments[0].ArrivalTime);

		}



		[Test]
		public void Product_02_Ticket()
		{
			var r = ParseAviaTicket(
@"{
    'date_of_issue': '12.05.2025',

	'booking_number': 'aipars000068',
    'agent_ID': 'max_sadovski',
    'passenger_title': 'Adult',
    'passenger_name': 'Olga',
    'passenger_surname': 'Danko',
    'route': 'VLC-MAD-ATH includes \'VLC\' which is the IATA code for Valencia Airport, \'MAD\' for Madrid-Barajas Airport, and \'ATH\' for Athens International Airport. The route with IATA codes is:  \n\nValencia to Madrid to Athens',
    'baggage': '',
    'seat': '',
    'flight_number': 'IB1084, IB0837',
    'departure_date': '04.05.2025 06:15',
    'arrival_date': '04.05.2025 07:20',
    'grand_total': '',
    'booking_date': '',
    'ticketing_date': '',
    'type_of_issue': 'Ticket'
}");


			Assert.AreEqual(DateTime.Parse("2025-05-12"), r.IssueDate);

			Assert.AreEqual("aipars000068", r.PnrCode);
			Assert.AreEqual("DANKO/OLGA ADULT", r.PassengerName);

			Assert.AreEqual(2, r.Segments.Count);

			Assert.AreEqual("VLC", r.Segments[0].FromAirportCode);
			Assert.AreEqual("MAD", r.Segments[0].ToAirportCode);
			Assert.AreEqual("IB1084", r.Segments[0].FlightNumber);
			Assert.AreEqual(DateTime.Parse("2025-05-04T06:15"), r.Segments[0].DepartureTime);
			Assert.AreEqual(DateTime.Parse("2025-05-04T07:20"), r.Segments[0].ArrivalTime);

			Assert.AreEqual("MAD", r.Segments[1].FromAirportCode);
			Assert.AreEqual("ATH", r.Segments[1].ToAirportCode);
			Assert.AreEqual("IB0837", r.Segments[1].FlightNumber);

		}



		[Test]
		public void Product_03_Ticket()
		{
			var r = ParseAviaTicket(
@"{
    'date_of_issue': '13.05.2025',

	'booking_number': 'aipars000069',
    'agent_ID': 'O_Yanchuk',
    'passenger_title': '',
    'passenger_name': 'OLGA',
    'passenger_surname': 'DANKO',
    'route': 'from VLC to MAD to ATH',
    'baggage': '',
    'seat': '',
    'flight_number': 'IB1084, IB0837',
    'departure_date': '04.05.2025 06:15',
    'arrival_date': '04.05.2025 07:20',
    'grand_total': '',
    'booking_date': '',
    'ticketing_date': '',
    'type_of_issue': 'Ticket'
}");


			Assert.AreEqual(DateTime.Parse("2025-05-13"), r.IssueDate);

			Assert.AreEqual("aipars000069", r.PnrCode);
			Assert.AreEqual("DANKO/OLGA", r.PassengerName);

			Assert.AreEqual(2, r.Segments.Count);

			Assert.AreEqual("VLC", r.Segments[0].FromAirportCode);
			Assert.AreEqual("MAD", r.Segments[0].ToAirportCode);
			Assert.AreEqual("IB1084", r.Segments[0].FlightNumber);
			Assert.AreEqual(DateTime.Parse("2025-05-04T06:15"), r.Segments[0].DepartureTime);
			Assert.AreEqual(DateTime.Parse("2025-05-04T07:20"), r.Segments[0].ArrivalTime);

			Assert.AreEqual("MAD", r.Segments[1].FromAirportCode);
			Assert.AreEqual("ATH", r.Segments[1].ToAirportCode);
			Assert.AreEqual("IB0837", r.Segments[1].FlightNumber);

		}



		[Test]
		public void Product_04_Ticket()
		{
			var r = ParseAviaTicket(
@"{
    'date_of_issue': '08.05.2025',

	'booking_number': 'H4/ACHXZV',
    'agent_ID': 'O_Yanchuk',
    'passenger_title': '',
    'passenger_name': 'Vitalina',
    'passenger_surname': 'Marchenko',
    'route': 'from RMO to STN',
    'baggage': '',
    'seat': '',
    'flight_number': 'H70411',
    'departure_date': '11.05.2025 20:00',
    'arrival_date': '11.05.2025 21:15',
    'grand_total': 'UAH 9845',
    'booking_date': '',
    'ticketing_date': '',
    'type_of_issue': 'Ticket'
}");


			Assert.AreEqual(DateTime.Parse("2025-05-08"), r.IssueDate);

			Assert.AreEqual("H4/ACHXZV", r.PnrCode);
			Assert.AreEqual("MARCHENKO/VITALINA", r.PassengerName);
			Assert.AreEqual("UAH", r.Fare.Currency.Code);
			Assert.AreEqual(9845m, r.Fare.Amount);

			Assert.AreEqual(1, r.Segments.Count);

			Assert.AreEqual("RMO", r.Segments[0].FromAirportCode);
			Assert.AreEqual("STN", r.Segments[0].ToAirportCode);
			Assert.AreEqual("H70411", r.Segments[0].FlightNumber);
			Assert.AreEqual(DateTime.Parse("2025-05-11T20:00"), r.Segments[0].DepartureTime);
			Assert.AreEqual(DateTime.Parse("2025-05-11T21:15"), r.Segments[0].ArrivalTime);

		}



		[Test]
		public void Product_05_Ticket()
		{
			var r = ParseAviaTicket(
@"{
    'date_of_issue': '13.05.2025',

	'booking_number': 'K7GG7LD',
    'agent_ID': 'O_Yanchuk',
    'passenger_title': 'Mrs',
    'passenger_name': 'Antonina',
    'passenger_surname': 'Patramanska',
    'route': 'from LIS to NCE',
    'baggage': '1 x under seat cabin bag (max. 45 x 36 x 20 cm), 1 x large cabin bag (max. 56 x 45 x 25 cm)',
    'seat': 'Auto allocated',
    'flight_number': 'EJU7611',
    'departure_date': '18.06.2024 13:45',
    'arrival_date': '18.06.2024 17:10',
    'grand_total': '€378.08',
    'booking_date': '',
    'ticketing_date': '19.05.2024',
    'type_of_issue': 'Ticket'
}");


			Assert.AreEqual(DateTime.Parse("2025-05-13"), r.IssueDate);

			Assert.AreEqual("K7GG7LD", r.PnrCode);
			Assert.AreEqual("PATRAMANSKA/ANTONINA MRS", r.PassengerName);
			Assert.AreEqual("EUR", r.Fare.Currency.Code);
			Assert.AreEqual(378.08m, r.Fare.Amount);

			Assert.AreEqual(1, r.Segments.Count);

			Assert.AreEqual("LIS", r.Segments[0].FromAirportCode);
			Assert.AreEqual("NCE", r.Segments[0].ToAirportCode);
			Assert.AreEqual("EJU7611", r.Segments[0].FlightNumber);
			Assert.AreEqual(DateTime.Parse("2024-06-18T13:45"), r.Segments[0].DepartureTime);
			Assert.AreEqual(DateTime.Parse("2024-06-18T17:10"), r.Segments[0].ArrivalTime);

		}



		[Test]
		public void Product_06_Ticket()
		{
			var r = ParseAviaTicket(
@"{
    'date_of_issue': '13.05.2025',

	'booking_number': 'WN19655340',
    'agent_ID': 'O_Yanchuk',
    'passenger_title': '',
    'passenger_name': 'Evheniya',
    'passenger_surname': 'Molchanova',
    'route': 'from WAW to IEG',
    'baggage': '',
    'seat': '51 W',
    'flight_number': '',
    'departure_date': '24.03.2025 15:30',
    'arrival_date': '24.03.2025 20:35',
    'grand_total': '79,00 zł',
    'booking_date': '',
    'ticketing_date': '',
    'type_of_issue': 'Ticket'
}");


			Assert.AreEqual(DateTime.Parse("2025-05-13"), r.IssueDate);

			Assert.AreEqual("WN19655340", r.PnrCode);
			Assert.AreEqual("MOLCHANOVA/EVHENIYA", r.PassengerName);
			Assert.AreEqual("PLN", r.Fare.Currency.Code);
			Assert.AreEqual(79m, r.Fare.Amount);

			Assert.AreEqual(1, r.Segments.Count);

			Assert.AreEqual("WAW", r.Segments[0].FromAirportCode);
			Assert.AreEqual("IEG", r.Segments[0].ToAirportCode);
			Assert.Null(r.Segments[0].FlightNumber);
			Assert.AreEqual("51 W", r.Segments[0].Seat);
			Assert.AreEqual(DateTime.Parse("2025-03-24T15:30"), r.Segments[0].DepartureTime);
			Assert.AreEqual(DateTime.Parse("2025-03-24T20:35"), r.Segments[0].ArrivalTime);

		}


		//---g



		#region Utils

		private Product ParseProduct(string documentContent)
		{
			var docs = AdamAiJsonParser.Parse(documentContent, new Currency("UAH"));

			Assert.NotNull(docs);
			Assert.GreaterOrEqual(docs.Count(), 1);
			var r = (AviaTicket)docs.First();
			Assert.NotNull(r);

			return r;
		}


		private AviaTicket ParseAviaTicket(string documentContent)
		{
			var r = ParseProduct(documentContent);

			Assert.IsInstanceOf<AviaTicket>(r);

			return (AviaTicket)r;
		}


		private AviaMco ParseAviaMco(string documentContent)
		{
			var r = ParseProduct(documentContent);

			Assert.IsInstanceOf<AviaMco>(r);

			return (AviaMco)r;
		}


		#endregion

	}
}