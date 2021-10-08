using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Avia
{
	[TestFixture]
	public class ItineraryTests
	{
		[Test]
		public void TestGenerateItinerary1()
		{
			var entities = MirParser.Parse(Res.MirTicket1, new Currency("UAH"));

			var ticket = ((AviaTicket) entities[0]);

			Assert.AreEqual("KBP-DME-CEK", ticket.Itinerary);
		}

		[Test]
		public void TestGenerateItinerary2()
		{
			var entities = MirParser.Parse(Res.MirTicket1, new Currency("UAH"));
			var ticket = ((AviaTicket) entities[0]);

			var segment = new FlightSegment
			{
				FromAirport = ticket.GetTicketedSegments()[1].ToAirport,
				ToAirport = new Airport
				{
					Code = "ACE",
					Country = new Country { Name = "Spain" },
					Settlement = "LANZAROTE",
					Name = "LANZAROTE"
				}
			};

			ticket.AddSegment(segment);

			Assert.AreEqual("KBP-DME-CEK-ACE", ticket.Itinerary);
		}

		[Test]
		public void TestGenerateItinerary3()
		{
			var entities = MirParser.Parse(Res.MirTicket1, new Currency("UAH"));
			var ticket = ((AviaTicket) entities[0]);

			var segment = new FlightSegment
			{
				FromAirport = new Airport
				{
					Code = "ACE",
					Country = new Country { Name = "Spain" },
					Settlement = "LANZAROTE",
					Name = "LANZAROTE"
				},
				ToAirport = ticket.GetTicketedSegments()[0].FromAirport
			};

			ticket.AddSegment(segment);

			Assert.AreEqual("KBP-DME-CEK;ACE-KBP", ticket.Itinerary);
		}
	}
}