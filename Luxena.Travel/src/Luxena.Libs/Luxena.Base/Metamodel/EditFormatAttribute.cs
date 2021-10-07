using System;
using System.Reflection;


namespace Luxena.Base.Metamodel
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EditFormatAttribute : Attribute
	{
		public EditFormatAttribute(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }

		public static string Get(MemberInfo memberInfo)
		{
			var attribute = memberInfo.GetAttribute<EditFormatAttribute>();

			return attribute == null ? null : attribute.Value;
		}
	}
}