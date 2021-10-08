namespace Luxena.Travel.Domain
{

	[RU("Сервис-класс")]
	public enum ServiceClass
	{
		[Patterns.Unknown]
		Unknown = 0,

		[RU("Эконом")]
		Economy = 1,

		[RU("Эконом-премиум")]
		PremiumEconomy = 2,

		[RU("Бизнес")]
		Business = 3,

		[RU("Первый")]
		First = 4,
	}

}