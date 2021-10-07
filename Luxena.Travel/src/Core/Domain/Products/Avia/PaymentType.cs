using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[RU("Тип оплаты")]
	[DataContract]
	public enum PaymentType
	{
		Unknown = 0,
		Cash = 1,
		Invoice = 2,
		Check = 3,
		CreditCard = 4,
		Exchange = 5,

		[RU("Без оплаты")]
		WithoutPayment = 6,
	}

}