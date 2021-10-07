module Luxena.Views
{

	registerEntityControllers(sd.FlightSegment, se => ({

		list: [
			se.CreatedOn.clone().entityDate(),
			se.Ticket,
			se.FromAirport,
			se.ToAirport,
			se.Carrier,
			se.FlightNumber,
			//se.Seat,
			se.ServiceClass,
			se.DepartureTime,
			se.ArrivalTime,
			se.Duration,
			se.FareBasis,
			se.Luggage,
			//se.MealTypes,
			se.CouponAmount,
		],

		form: {
			"fields1": [
				se.Ticket,
				sd.row(se.FromAirportCode, se.FromAirport).title(se.FromAirport),
				sd.row(se.ToAirportCode, se.ToAirport).title(se.ToAirport),
				se.Carrier,
				sd.row(se.FlightNumber, se.Seat),
				sd.row(se.ServiceClassCode, se.ServiceClass).title(se.ServiceClass),
			],
			"fields2": [
				se.DepartureTime,
				se.CheckInTime,
				se.CheckInTerminal,
				se.ArrivalTime,
				se.ArrivalTerminal,
				se.MealTypes,
				se.CouponAmount,
			],
		},

	}));

}