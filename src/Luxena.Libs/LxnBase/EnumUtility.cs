using System;
using System.Collections.Generic;


namespace LxnBase
{
	public static class EnumUtility
	{
		public static string Localize(Type type, Enum value, Type resources)
		{
			if (value == null)
				return null;

			string name = ToString(type, value);

			string typeName = type.Name.Substr(0, 1).ToLowerCase() + type.Name.Substr(1);

			string text = (string) Type.GetField(resources, typeName + "_" + name);

			return string.IsNullOrEmpty(text) ? name : text;
		}

		public static string ToString(Type type, Enum value)
		{
			Dictionary<string, Enum> d = (Dictionary<string, Enum>)(object)type;

			foreach (KeyValuePair<string, Enum> pair in d)
			{
				if (pair.Value == value)
					return StringUtility.Capitalize(pair.Key);
			}
	
			return null;
		}
	}
}