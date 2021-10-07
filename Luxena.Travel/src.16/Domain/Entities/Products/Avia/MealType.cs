using System;


namespace Luxena.Travel.Domain
{

	[RU("Питание")]
	[Flags]
	public enum MealType
	{
		[RU("нет")]
		NoData = 0,

		[RU("Завтрак")]
		Breakfast = 0x0001,

		[RU("Континентальный завтрак")]
		ContinentalBreakfast = 0x0002,

		[RU("Ланч")]
		Lunch = 0x0004,

		[RU("Обед")]
		Dinner = 0x0008,

		[RU("Закуска")]
		Snack = 0x0010,

		[RU("Холодная еда")]
		ColdMeal = 0x0020,

		[RU("Горячая еда")]
		HotMeal = 0x0040,

		[RU("Еда")]
		Meal = 0x0080,

		[RU("Напитки")]
		Refreshment = 0x0100,

		[RU("Бесплатные алкогольные напитки")]
		AlcoholicComplimentaryBeverages = 0x0200,

		[RU("Еда")]
		Food = 0x0400,

		[RU("Платные алкогольные напитки")]
		AlcoholicBeveragesForPurchase = 0x0800,

		[RU("DutyFree")]
		DutyFree = 0x1000

	}

}