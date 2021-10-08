module Luxena.Views
{

	registerEntityControllers(sd.Excursion, se => ({

		list: [
			se.IssueDate,
			se.Name,
			se.PassengerName,
			se.Customer,
			se.Order,
			se.Total,
			se.ServiceFee,
			se.GrandTotal,
		],

		view: {
			"fields1": [
				se.IssueDate,
				se.ReissueFor,

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				se.StartDate,
				se.FinishDate,
				se.TourName,

				se.Country,
				se.PnrCode,
				se.TourCode,

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

				se.StartAndFinishDate,
				se.TourName,

				se.Country,
				se.PnrAndTourCode,

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}