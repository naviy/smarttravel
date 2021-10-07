module Luxena
{

	export interface IPaymentSemantic
	{
		DateAndDocumentNumber?: SemanticMember;
		OrderAndPayer?: SemanticMember;
		AmountAndVat?: SemanticMember;
		AssignedToAndOwner?: SemanticMember;
	}


	$doForDerived(sd.Payment, se =>
	{
		se.DateAndDocumentNumber = sd.row(
			sd.col(se.Date), sd.col(se.DocumentNumber)
		);
		se.OrderAndPayer = sd.row(se.Order, se.Payer);
		se.AmountAndVat = sd.row(se.Amount, se.Vat);
		se.AssignedToAndOwner = sd.row(se.AssignedTo, se.Owner);
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
				sd.row(se.Void, se.Unvoid),
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
				sd.row(se.Void, se.Unvoid),
				se.IsVoid,
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