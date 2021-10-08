using System;


namespace LxnBase
{
	public class StringUtility
	{
		public static string GetNumberText(int number, string textFormat1, string textFormat2, string textFormat3)
		{
			int twoLastDigits = number%100;

			if (5 <= twoLastDigits && twoLastDigits <= 20)
				return string.Format(textFormat3, number);

			int lastDigit = number%10;

			if (lastDigit == 1)
				return string.Format(textFormat1, number);

			if (lastDigit == 2 || lastDigit == 3 || lastDigit == 4)
				return string.Format(textFormat2, number);

			return string.Format(textFormat3, number);
		}

		public static string Capitalize(string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			return value.Substr(0, 1).ToUpperCase() + value.Substr(1);
		}

		public static string ToString(object obj)
		{
			if (Script.IsNullOrUndefined(obj))
				return string.Empty;

			if (obj is Date)
				return ((Date) obj).Format("d.m.Y");

			if (Type.HasField(obj, "Name"))
				return (string)Type.GetField(obj, "Name");

			if (Type.HasField(obj, "Text"))
				return (string) Type.GetField(obj, "Text");

			return string.Empty;
		}
	}
}