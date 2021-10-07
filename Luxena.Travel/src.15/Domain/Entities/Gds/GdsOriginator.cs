namespace Luxena.Travel.Domain
{

	public enum GdsOriginator
	{
		Unknown = 0,
		Amadeus = 1,
		Galileo = 2,
		Sirena = 3,
		Airline = 4,
		Gabriel = 5,

		[RU("Wizz Air")]
		WizzAir = 6,

		IATI = 7,

		[RU("E-Travels")]
		ETravels = 8,

		[RU("Ticket Consolidator")]
		TicketConsolidator = 9,

		[RU("Delta TRAVEL")]
		DeltaTravel = 10,

		[RU("Tickets.UA")]
		TicketsUA = 11,

		[RU("Fly Dubai")]
		FlyDubai = 12,

		[RU("Air Arabia")]
		AirArabia = 13,

		Pegasus = 14,

		[RU("ВіаКиїв")]
		ViaKiev = 15,

		AirLife = 16,

		Sabre = 17,

		[RU("Amadeus Altea")]
		AmadeusAltea = 18,

		SPRK = 19,

		[EN("Travel & Marketing")]
		TravelAndMarketing = 20,
	}

}