using System;
using System.Text;


namespace Luxena
{

	public static class EnumExtensions
	{

		public static string ToDisplayString(this Enum value)
		{
			if (value == null) return null;

			var type = value.GetType();

			if (type.Has<FlagsAttribute>())
			{
				var builder = new StringBuilder();

				var separator = string.Empty;

				foreach (Enum enumValue in Enum.GetValues(type))
				{
					if ((Convert.ToInt32(value) & Convert.ToInt32(enumValue)) == Convert.ToInt32(enumValue) &&
						Convert.ToInt32(enumValue) != 0)
					{
						builder.Append(separator).Append(type.GetCaption(enumValue.ToString()));

						separator = ", ";
					}
				}

				return builder.ToString();
			}

			var valueText = value.ToString();

			return type.GetCaption(valueText) ?? valueText;
		}

	}

}