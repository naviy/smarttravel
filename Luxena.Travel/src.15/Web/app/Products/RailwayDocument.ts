module Luxena.Views
{

	registerEntityControllers(sd.RailwayDocument, se => ({
		list: [
			se.IssueDate,
			se.Type,
			se.Name,
			se.PassengerName,
			se.Provider,
			se.Customer,
			se.Order,
			se.Total,
			se.ServiceFee,
			se.GrandTotal,
		],
	}));


	registerEntityControllers([sd.Pasteboard, sd.PasteboardRefund], se => ({

		list: sd.RailwayDocument,

		view: {
			"fields1": [
				se.IssueDate,
				se.ReissueFor,

				Ui.fieldRow(se, "/", [se.Number, se.Provider, ]),

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				Ui.fieldRow(se, se.DeparturePlace, [se.DeparturePlace, se.DepartureDate, se.DepartureTime, ]),
				Ui.fieldRow(se, se.ArrivalPlace, [se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime, ]),

				Ui.fieldSet(se, null, [se.ServiceClass, se.TrainNumber], [se.CarNumber, se.SeatNumber]),
				
				se.BookerRow,
				se.TicketerRow,
				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

		edit: {
			"fields1": [
				se.IssueDateAndReissueFor,

				Ui.fieldRow(se, "/", [se.Number, se.Provider, ]),

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				Ui.fieldRow(se, se.DeparturePlace, [se.DeparturePlace, se.DepartureDate, se.DepartureTime, ]),
				Ui.fieldRow(se, se.ArrivalPlace, [se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime, ]),

				Ui.fieldSet(se, null, [se.ServiceClass, se.TrainNumber], [se.CarNumber, se.SeatNumber]),

				se.BookerRow,
				se.TicketerRow,
				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}