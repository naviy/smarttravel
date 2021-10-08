using System;
using System.Reflection;


namespace Luxena.Base.Metamodel
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DataPathAttribute : Attribute
	{
		public DataPathAttribute(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }

		public static string Get(MemberInfo memberInfo)
		{
			var attribute = memberInfo.GetAttribute<DataPathAttribute>();

			return attribute == null ? null : attribute.Value;
		}
	}
}