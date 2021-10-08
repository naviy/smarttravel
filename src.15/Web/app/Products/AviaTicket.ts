module Luxena.Views
{

	registerEntityControllers(sd.AviaDocument, se => ({
		list: [
			se.IssueDate,
			se.Type,
			se.Name,
			se.PassengerName,
			se.Itinerary,
			se.Customer,
			se.Order,
			se.Total,
			se.ServiceFee,
			se.GrandTotal,
		],
	}));


	registerEntityControllers(sd.AviaTicket, se => ({

		list: sd.AviaDocument,

		formTitle: se.FullNumber,

		view: () => ({
			"fields1": [
				se.IssueDate,

				Ui.fieldRow(se, "/", () => [se.FullNumber, se.Producer, ]),

				se.ReissueFor,
				se.PassengerRow,
				se.Itinerary,
				se.CustomerAndOrder,
				se.Intermediary,

				se.GdsPassportStatus,

				se.PnrCode,
				se.TourCode,

				se.Booker,
				se.Ticketer,
				se.SellerAndOwner,
				se.OriginalDocument,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		}),

		edit: () => ({
			"fields1": [
				se.IssueDateAndReissueFor,
				se.NumberRow,

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				Ui.fieldSet(se, null,
					[se.PnrCode, se.TourCode, ],
					[se.GdsPassportStatus, se.Originator, ]
				),

				se.BookerRow,
				se.TicketerRow,
				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		}),

		viewScope: ctrl => ({
			tabs: [
				se.Segments.toTab(ctrl, a => [
					a.Position,
					a.FromAirport,
					a.ToAirport,
					a.Carrier,
					a.FlightNumber,
					a.Seat,
					a.ServiceClass,
					a.DepartureTime,
					a.ArrivalTime,
					a.Duration,
					a.FareBasis,
					a.Luggage,
					a.MealTypes,
					a.CouponAmount,
				])
			],
		}),

	}));

}