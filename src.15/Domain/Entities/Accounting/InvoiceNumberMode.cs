namespace Luxena.Travel.Domain
{

	public enum InvoiceNumberMode
	{

		[RU("По умолчанию")]
		Default,

		[RU("На основе номера заказа")]
		ByOrderNumber,
	}

}