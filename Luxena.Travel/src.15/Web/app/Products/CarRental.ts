module Luxena.Views
{

	registerEntityControllers(sd.CarRental, se => ({

		list: [
			se.IssueDate,
			se.Name,
			se.PassengerName,
			se.Producer,
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

				se.Producer,
				se.StartDate,
				se.FinishDate,
				se.CarBrand,

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

				se.Producer,
				se.StartAndFinishDate,
				se.CarBrand,

				se.Country,
				se.PnrAndTourCode,

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}