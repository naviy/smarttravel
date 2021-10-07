using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public enum AirlinePassportRequirement
	{
		[RU("По умолчанию")]
		SystemDefault = 0,

		[RU("Требуется")]
		Required = 1,

		[RU("Не требуется")]
		NotRequired = 2,
	}

}