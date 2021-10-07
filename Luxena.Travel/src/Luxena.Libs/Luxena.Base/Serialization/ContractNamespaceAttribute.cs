using System;


namespace Luxena.Base.Serialization
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
	public class ContractNamespaceAttribute : Attribute
	{
		public string ClrNamespace { get; set; }
		public string ContractNamespace { get; set; }
	}
}