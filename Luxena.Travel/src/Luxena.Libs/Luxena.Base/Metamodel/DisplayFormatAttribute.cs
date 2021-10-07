using System;
using System.Reflection;


namespace Luxena.Base.Metamodel
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DisplayFormatAttribute : Attribute
	{
		public DisplayFormatAttribute(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }

		public static string Get(MemberInfo memberInfo)
		{
			var attribute = memberInfo.GetAttribute<DisplayFormatAttribute>();

			return attribute == null ? null : attribute.Value;
		}
	}
}