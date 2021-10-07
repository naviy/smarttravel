module Luxena.Views
{

	registerEntityControllers(sd.BusDocument, se => ({
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
		

	registerEntityControllers([sd.BusTicket, sd.BusTicketRefund], se => ({

		list: sd.BusDocument,

		view: {
			"fields1": [
				se.IssueDate,
				se.ReissueFor,

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,
				
				se.Provider,
				sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title("Отправление"),
				sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title("Прибытие"),
				se.SeatNumber,

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

		edit: {
			"fields1": [
				se.IssueDateAndReissueFor,

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				se.Provider,
				sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title("Отправление"),
				sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title("Прибытие"),
				se.SeatNumber,

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}