using System;


namespace Luxena.Base.Serialization
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public class ContractMemberAttribute : Attribute
	{
		public ContractMemberAttribute()
		{
		}

		public ContractMemberAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public bool Ignore { get; set; }
	}
}