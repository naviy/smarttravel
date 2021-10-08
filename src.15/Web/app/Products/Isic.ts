module Luxena.Views
{

	registerEntityControllers(sd.Isic, se => ({

		members: [
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

				se.CardType,
				se.Number1,
				se.Number2,

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

				se.CardType,
				se.Number1,
				se.Number2,

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

	}));

}