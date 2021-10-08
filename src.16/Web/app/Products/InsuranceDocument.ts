module Luxena.Views
{

	registerEntityControllers(sd.InsuranceDocument, se => ({
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

	registerEntityControllers([sd.Insurance, sd.InsuranceRefund], se => ({

		list: sd.InsuranceDocument,

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
				se.Country,
				se.PnrAndTourCode,

				se.SellerAndOwner,
			],

			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}