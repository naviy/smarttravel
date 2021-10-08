namespace Luxena.Travel.Domain
{

	[RU("Паспорт в GDS")]
	public enum GdsPassportStatus
	{
		[RU("Неизвестно")]
		Unknown = 0,

		[RU("Есть")]
		Exist = 1,

		[RU("Нет")]
		NotExist = 2,

		[RU("Некорректен")]
		Incorrect = 3
	}

}