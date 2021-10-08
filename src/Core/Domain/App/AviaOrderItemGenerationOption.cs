namespace Luxena.Travel.Domain
{

	public enum AviaOrderItemGenerationOption
	{

		[RU("Всегда одна позиция")]
		AlwaysOneOrderItem,

		[RU("Cервисный сбор отдельной позицией")]
		SeparateServiceFee,

		[RU("Настраивать вручную")]
		ManualSetting,

	}

}