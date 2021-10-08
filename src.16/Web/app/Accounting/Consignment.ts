module Luxena.Views
{
	registerEntityControllers(sd.Consignment, se => ({

		list: [
			se.IssueDate,
			se.Number,
			se.Supplier,
			se.Acquirer,
			se.TotalSupplied,
			se.GrandTotal,
		],

		view: {
			"fields1": [
				se.IssueDate,
				se.Number,
				se.Supplier,
				se.Acquirer,
				se.TotalSupplied,
			],
			"fields2": [
				se.Discount,
				se.Total,
				se.Vat,
				se.GrandTotal,
			],
		},

		viewScope: ctrl => ({
			tabs: [
				se.OrderItems.toTab(ctrl, a => [a.Order, a.Position, a.Product, a.Text, a.GrandTotal, ]),
				se.IssuedConsignments.toTab(ctrl, a => [a.TimeStamp, a.Number, a.IssuedBy, ]),
			]
		}),


		edit: {
			"fields": [
				se.Number,
				se.IssueDate,
				se.Supplier,
				se.Acquirer,
				se.TotalSupplied,
			],
		},

	}));


	registerEntityControllers(sd.IssuedConsignment, se => [
		se.Consignment, se.TimeStamp, se.Number, se.IssuedBy,
	]);
}