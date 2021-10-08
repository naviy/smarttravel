using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{
	[DataContract]
	public enum FilterOperator
	{
		None = 0,
		Equals = 1,
		IsNull = 2,
		StartsWith = 3,
		Contains = 4,
		EndsWith = 5,
		Less = 6,
		LessOrEquals = 7,
		GreaterOrEquals = 8,
		Greater = 9,
		IsIn = 10,
		IsIdIn = 11,
		IsIdInOrIsNull = 12,
	}
}