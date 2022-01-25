using Luxena.Base.Metamodel;
using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("Форма оплаты")]
	public enum PaymentForm
	{

		[Localization(typeof(CashInOrderPayment))]
		CashInOrder = 0,
	
		[Localization(typeof(WireTransfer))]
		WireTransfer = 1,

		[Localization(typeof(CheckPayment))]
		Check = 2,

		[Localization(typeof(ElectronicPayment))]
		Electronic = 3,

		[Localization(typeof(CashOutOrderPayment))]
		CashOutOrder = 4,

	}






	partial class Payment
	{

		[Utility, Hidden]
		public virtual bool IsCashOrder => PaymentForm == PaymentForm.CashInOrder || PaymentForm == PaymentForm.CashOutOrder;

		[Utility, Hidden]
		public virtual bool IsCashInOrder => PaymentForm == PaymentForm.CashInOrder;

		[Utility, Hidden]
		public virtual bool IsCashOutOrder => PaymentForm == PaymentForm.CashOutOrder;

		[Utility, Hidden]
		public virtual bool IsCheck => PaymentForm == PaymentForm.Check;

		[Utility, Hidden]
		public virtual bool IsElectronic => PaymentForm == PaymentForm.Electronic;

		[Utility, Hidden]
		public virtual bool IsWireTransfer => PaymentForm == PaymentForm.WireTransfer;

	}






	//===g



}