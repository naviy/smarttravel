using System;


namespace Luxena
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
	public class TypeResourcesAttribute : Attribute
	{
		public TypeResourcesAttribute(Type type)
		{
			Type = type;
		}

		public Type Type { get; private set; }
	}
}