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
				Ui.fieldRow(se, se.FromAirport, a => [a.FromAirportCode, a.FromAirport]),
				Ui.fieldRow(se, se.ToAirport, a => [a.ToAirportCode, a.ToAirport]),
				se.Carrier,
				Ui.fieldRow(se, "/", a => [a.FlightNumber, a.Seat]),
				Ui.fieldRow(se, se.ServiceClass, a => [a.ServiceClassCode, a.ServiceClass]),
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