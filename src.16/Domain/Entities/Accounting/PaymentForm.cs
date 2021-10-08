using Luxena.Domain;


namespace Luxena.Travel.Domain
{

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
		[Utility, NotUiMapped]
		public bool IsCashOrder { get { return PaymentForm == PaymentForm.CashInOrder || PaymentForm == PaymentForm.CashOutOrder; } }
		[Utility, NotUiMapped]
		public bool IsCashInOrder { get { return PaymentForm == PaymentForm.CashInOrder; } }
		[Utility, NotUiMapped]
		public bool IsCashOutOrder { get { return PaymentForm == PaymentForm.CashOutOrder; } }

		[Utility, NotUiMapped]
		public bool IsCheck { get { return PaymentForm == PaymentForm.Check; } }
		[Utility, NotUiMapped]
		public bool IsElectronic { get { return PaymentForm == PaymentForm.Electronic; } }
		[Utility, NotUiMapped]
		public bool IsWireTransfer { get { return PaymentForm == PaymentForm.WireTransfer; } }
	}

}