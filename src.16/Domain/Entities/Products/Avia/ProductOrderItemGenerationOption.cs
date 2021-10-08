namespace Luxena.Travel.Domain
{

	public enum ProductOrderItemGenerationOption
	{

		[RU("Всегда одна позиция")]
		AlwaysOneOrderItem,

		[RU("Cервисный сбор отдельной позицией")]
		SeparateServiceFee,

		[RU("Настраивать вручную")]
		ManualSetting,

	}

}