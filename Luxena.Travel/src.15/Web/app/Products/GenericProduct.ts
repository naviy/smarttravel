module Luxena.Views
{

	registerEntityControllers(sd.GenericProduct, se => ({

		list: [
			se.IssueDate,
			se.Name,
			se.PassengerName,
			se.Provider,
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

				se.Provider,

				se.GenericType,
				se.Number,
				se.StartDate,
				se.FinishDate,

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

				se.Provider,

				se.GenericType,
				se.Number,
				se.StartAndFinishDate,

				se.Country,
				se.PnrAndTourCode,

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}