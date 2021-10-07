namespace Luxena.Travel.Domain
{

	public enum CheckItemTaxMode
	{

		[RU("По умолчанию")]
		Default,

		[RU("Ставка Д и без позиции для сервисного сбора")]
		WithoutVAT,

		[RU("Ставка Д и ставка А для сервисного сбора")]
		WithoutVATAndServiceFeeVAT,

	}

}
