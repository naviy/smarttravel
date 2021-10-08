/*
using System;
using System.Collections;


namespace Luxena.Travel
{

	public abstract class PaymentEditForm : EntityEditForm
	{

		protected override void Initialize()
		{
			base.Initialize();
			Form.labelWidth = 160;

			PaymentView se = (PaymentView)Entity;

			se.Payer.SetEditor(-3);
			se.Owner.SetEditor(-3);
			se.AssignedTo.SetEditor(-3);
			se.Note.SetEditor(-3);
		}

		protected override Dictionary GetInitData()
		{
			return new Dictionary("Date", Date.Today);
		}

	}


	public partial class CashInOrderPaymentEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Date,
				se.DocumentNumber,
				se.Invoice,
				se.Order,
				se.Payer,
				se.Amount,
				se.Vat,
				se.Owner,
				se.AssignedTo,
				se.Note,
				se.SavePosted,
			}));
		}

	}


	public partial class CashOutOrderPaymentEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Date,
				se.DocumentNumber,
				se.Invoice,
				se.Order,
				se.Payer,
				se.Amount,
				se.Vat,
				se.Owner,
				se.AssignedTo,
				se.Note,
				se.SavePosted,
			}));
		}

	}


	public partial class CheckPaymentEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Date,
				se.DocumentNumber,
				se.Invoice,
				se.Order,
				se.Payer,
				se.Amount,
				se.Vat,
				se.Owner,
				se.AssignedTo,
				se.Note,
			}));
		}

	}


	public partial class ElectronicPaymentEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Date,
				se.DocumentNumber,
				se.AuthorizationCode,
				se.PaymentSystem,
				se.Invoice,
				se.Order,
				se.Payer,
				se.Amount,
				se.Vat,
				se.Owner,
				se.AssignedTo,
				se.Note,
			}));
		}

	}


	public partial class WireTransferEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Date,
				se.DocumentNumber,
				se.Invoice,
				se.Order,
				se.Payer,
				se.Amount,
				se.Vat,
				se.ReceivedFrom,
				se.Owner.ToField(-3),
				se.AssignedTo,
				se.Note,
			}));
		}

	}

}
*/