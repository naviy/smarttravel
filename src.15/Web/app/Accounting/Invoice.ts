module Luxena.Views
{
	registerEntityControllers([sd.Invoice, sd.Receipt], se => ({

		list: [
			se.IssueDate,
			se.Type,
			se.Number,
			se.Order,
			se.IssuedBy,
			se.Total,
			se.Vat,
			se.TimeStamp,

			//se.Customer,
			//se.BillTo,
			//se.ShipTo,
			//se.IsOrderVoid,
			//se.Owner,
		],

		view: {
			"fields1": [
				se.IssueDate,
				se.Number,
				se.Type,
				se.Order,
				se.IssuedBy,
			],
			"fields2": [
				se.TimeStamp,
				se.Total,
				se.Vat,
			],
		},

		viewScope: ctrl => ({
			tabs: [
				se.Payments.toTab(ctrl, a => [a.Date, a.PaymentForm, a.Number, a.Payer, a.Amount, a.Vat]),
			]
		}),
		
		edit: null,

	}));

}