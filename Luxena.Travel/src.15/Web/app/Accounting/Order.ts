module Luxena.Views
{

	registerEntityControllers(sd.Order, se => ({

		list: [
			se.IssueDate,
			se.Number,
			se.Customer,
			se.Total,
			////se.Vat,
			se.Paid.hidden(),
			se.TotalDue,

			Ui.moneyProgress(se, se.Paid, se.Total),

			//se.DeliveryBalance,
			se.AssignedTo,
		],

		view: {
			"fields1": [
				se.Number,
				se.IssueDate,
				se.Customer,
				se.BillTo,
				se.ShipTo,
				se.AssignedTo,
				se.Owner,
				se.BankAccount,
				se.IsPublic,
				se.IsSubjectOfPaymentsControl,
				se.Note,
			],
			"fields2": [
				Ui.moneyProgress(se, se.Paid, se.Total),
				se.Total,
				se.ServiceFee,
				se.Discount,
				se.Vat,
				se.Paid,
				se.TotalDue,
				se.VatDue,
			],
		},

		smart: [
			se.IssueDate,
			se.Customer,
			se.BillTo,
			se.ShipTo,
			se.AssignedTo,
			se.Owner,
			se.Total,
			se.Paid,
			se.TotalDue,
			se.Note,
		],

		viewScope: ctrl => ({
			tabs: [
				se.Items.toTab(ctrl, a => [a.Position, a.Product, a.Text, a.GrandTotal, a.Consignment, ]),
				se.Payments.toTab(ctrl, a => [a.Date, a.Number, a.DocumentNumber, a.PostedOn, a.Payer, a.RegisteredBy, a.Amount, a.Note, ]),
				se.IncomingTransfers.toTab(ctrl, a => [a.Date, a.Number, a.FromOrder, a.FromParty, a.Amount]),
				se.OutgoingTransfers.toTab(ctrl, a => [a.Date, a.Number, a.ToOrder, a.ToParty, a.Amount]),
			]
		}),


		edit: {
			"fields": [
				se.IssueDate,
				se.Number,
				se.Customer,
				se.BillTo,
				se.ShipTo,
				Ui.fieldRow(se, "/",() => [se.AssignedTo, se.Owner, ]),
				se.BankAccount,
				se.IsPublic,
				se.IsSubjectOfPaymentsControl,
				se.SeparateServiceFee,
				se.Note,
			],
		},

	}));

}