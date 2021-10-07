module Luxena
{

	export interface IPaymentSemantic
	{
		DateAndDocumentNumber?: SemanticFieldSet;
		OrderAndPayer?: SemanticFieldRow;
		AmountAndVat?: SemanticFieldSet;
		AssignedToAndOwner?: SemanticFieldRow;
	}


	Ui.fieldSet2(sd.Payment, {
		name: "DateAndDocumentNumber",
		members: se => [se.Date, ],
		members2: se => [se.DocumentNumber, ],
	});

	Ui.fieldRow2(sd.Payment, {
		name: "OrderAndPayer",
		members: se => [se.Order, se.Payer, ],
	});

	Ui.fieldSet2(sd.Payment, {
		name: "AmountAndVat",
		members: se => [se.Amount, se.Vat,],
	});

	Ui.fieldRow2(sd.Payment, {
		name: "AssignedToAndOwner",
		members: se => [se.AssignedTo, se.Owner, ],
	});

}


module Luxena.Views
{
	registerEntityControllers(sd.Payment, se => ({
		list: [
			se.DateAndDocumentNumber,
			se.Date,
			se.PaymentForm,
			se.Number,
			se.DocumentNumber,
			se.Invoice,
			se.Order,
			//se.Payer,
			se.Amount,
			//se.Vat,
			se.PostedOn,
			se.AssignedTo,
			//se.Owner,
		],

	}));

	
	registerEntityControllers(sd.CashInOrderPayment, se => ({
		list: sd.Payment,

		view: {
			"fields1": [
				se.Date,
				se.DocumentNumber,
				se.Invoice,
				se.Order,
				se.Payer,
				se.RegisteredBy,
				se.ReceivedFrom,
				se.AssignedTo,
				se.Owner,
			],
			"fields2": [
				se.Amount,
				se.Vat,
				se.SavePosted,
				se.PostedOn,
				Ui.fieldRow(se, null, [se.Void, se.Unvoid, ]),
				se.IsVoid,
			],
			"fields3": se.Note,
		},
		edit: {
			"fields1": [
				se.Date,
				se.DocumentNumber,
				se.Invoice,
				se.OrderAndPayer,
				se.ReceivedFrom,
				se.AssignedToAndOwner,
			],
			"fields2": [
				se.Amount,
				se.Vat,
				se.SavePosted,
				se.PostedOn,
				Ui.fieldRow(se, null, [se.Void, se.Unvoid, ]),
			],
			"fields3": se.Note,
		}
	}));


	registerEntityControllers(sd.CashOutOrderPayment, se => ({
		list: sd.Payment,

		view: [
			se.Date,
			se.DocumentNumber,
			se.Invoice,
			se.Order,
			se.Payer,
			se.RegisteredBy,
			se.Amount,
			se.Vat,
			se.ReceivedFrom,
			se.AssignedTo,
			se.Owner,
			se.Note,
			se.SavePosted,
			se.PostedOn,
		],
		edit: [
			se.DateAndDocumentNumber,
			se.Invoice,
			se.OrderAndPayer,
			se.RegisteredBy,
			se.AmountAndVat,
			se.ReceivedFrom,
			se.AssignedToAndOwner,
			se.Note,
			se.SavePosted,
			se.PostedOn,
		],
	}));


	registerEntityControllers(sd.CheckPayment, se => ({
		list: sd.Payment,

		form: [
			se.DateAndDocumentNumber,
			se.Invoice,
			se.OrderAndPayer,
			se.RegisteredBy,
			se.AmountAndVat,
			se.AssignedToAndOwner,
			se.Note,
		],
	}));


	registerEntityControllers(sd.ElectronicPayment, se => ({
		list: sd.Payment,

		form: [
			se.DateAndDocumentNumber,
			se.AuthorizationCode,
			se.PaymentSystem,
			se.Invoice,
			se.OrderAndPayer,
			se.RegisteredBy,
			se.AmountAndVat,
			se.AssignedToAndOwner,
			se.Note,
		],
	}));


	registerEntityControllers(sd.WireTransfer, se => ({
		list: sd.Payment,

		form: [
			se.DateAndDocumentNumber,
			se.Invoice,
			se.OrderAndPayer,
			se.RegisteredBy,
			se.AmountAndVat,
			se.ReceivedFrom,
			se.AssignedToAndOwner,
			se.Note,
		],
	}));


}