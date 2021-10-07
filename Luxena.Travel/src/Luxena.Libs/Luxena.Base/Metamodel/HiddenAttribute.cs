using System;
using System.Reflection;


namespace Luxena.Base.Metamodel
{
	[AttributeUsage(AttributeTargets.Property)]
	public class HiddenAttribute : Attribute
	{
		public HiddenAttribute()
		{
			_hiddenOptions = HiddenOptions.Hidden;
		}

		public HiddenAttribute(bool allowDisplay)
		{
			_hiddenOptions = allowDisplay ? HiddenOptions.AllowDisplay : HiddenOptions.Hidden;
		}

		private readonly HiddenOptions _hiddenOptions;

		public static HiddenOptions Get(PropertyInfo propertyInfo)
		{
			var attribute = propertyInfo.GetAttribute<HiddenAttribute>();

			return attribute == null ? HiddenOptions.Visible : attribute._hiddenOptions;
		}
	}

	public enum HiddenOptions
	{
		Visible,
		AllowDisplay,
		Hidden
	}
}