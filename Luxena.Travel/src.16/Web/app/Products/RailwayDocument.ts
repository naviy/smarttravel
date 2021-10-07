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

				sd.row(se.Number, se.Provider),

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title(se.DeparturePlace),
				sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title(se.ArrivalPlace),

				sd.row(sd.col(se.ServiceClass, se.TrainNumber), sd.col(se.CarNumber, se.SeatNumber)),
				
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

				sd.row(se.Number, se.Provider),

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title(se.DeparturePlace),
				sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title(se.ArrivalPlace),

				sd.row(sd.col(se.ServiceClass, se.TrainNumber), sd.col(se.CarNumber, se.SeatNumber)),

				se.BookerRow,
				se.TicketerRow,
				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}