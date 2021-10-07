using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public enum Gender
	{
		[RU("Мужской")]
		Male = 0,

		[RU("Женский")]
		Female = 1,
	}

}