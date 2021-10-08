namespace Luxena.Travel.Domain
{

	public enum PeriodState
	{
		[RU("Открытый")]
		Open = 0,

		[RU("Ограниченный")]
		Restricted = 1,

		[RU("Закрытый")]
		Closed = 2,
	}

}