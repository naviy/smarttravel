namespace Luxena.Travel.Domain
{

	public enum TaskStatus
	{
		[RU("Открыта")]
		Open = 0,

		[RU("Выполняется")]
		InProgress = 1,

		[RU("В ожидании ответа")]
		WaitForResponse = 2,

		[RU("Закрыта")]
		Closed = 3,
	}

}