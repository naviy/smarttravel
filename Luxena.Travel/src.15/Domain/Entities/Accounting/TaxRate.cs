namespace Luxena.Travel.Domain
{

	public enum TaxRate
	{

		[RU("По умолчанию")]
		Default = 0,

		[RU("не печатать")]
		None = -1,

		[RU("А (с НДС)")]
		A = 1,

		[RU("Б (без НДС)")]
		B = 2,

		[RU("Д (без НДС)")]
		D = 5,

	}

}
