using System;


namespace Luxena.Base.Serialization
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class IgnoreSerializationAttribute : Attribute
	{
	}
}