using System;


namespace Luxena.Base.Metamodel
{
	[AttributeUsage(AttributeTargets.Class)]
	public class OperationAttribute : Attribute
	{
		public OperationAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}