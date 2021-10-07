namespace Luxena.Travel.Domain
{

	public enum ModificationType
	{
		[RU("Добавлен")]
		Create,

		[RU("Изменён")]
		Update,

		[RU("Удалён")]
		Delete,
	}

}