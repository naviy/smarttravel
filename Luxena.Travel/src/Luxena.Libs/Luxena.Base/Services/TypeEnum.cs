using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public enum TypeEnum
	{
		Object = 0,
		Number = 1,
		List = 2,
		Bool = 3,
		String = 4,
		Date = 5,
		Custom = 6
	}
}