using System;

using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{

	[DataContract]
	[Flags]
	[RU("�������")]
	public enum MealType
	{
		NoData = 0,

		[RU("�������")]
		Breakfast = 0x0001,

		[RU("��������������� �������")]
		ContinentalBreakfast = 0x0002,

		[RU("����")]
		Lunch = 0x0004,

		[RU("����")]
		Dinner = 0x0008,

		[RU("�������")]
		Snack = 0x0010,

		[RU("�������� ���")]
		ColdMeal = 0x0020,

		[RU("������� ���")]
		HotMeal = 0x0040,

		[RU("���")]
		Meal = 0x0080,

		[RU("�������")]
		Refreshment = 0x0100,

		[RU("���������� ����������� �������")]
		AlcoholicComplimentaryBeverages = 0x0200,

		[RU("������� ���")]
		FoodForPurchase = 0x0400,

		[RU("������� ����������� �������")]
		AlcoholicBeveragesForPurchase = 0x0800,

		[RU("Duty Free")]
		DutyFree = 0x1000,

		[RU("no meal service")]
		NoMealService = 0x2000,
	}

}