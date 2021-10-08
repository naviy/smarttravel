using System;
using System.Collections.Generic;
using System.Text;


namespace Luxena
{
	public static class StringExtensions
	{
		public static string Fill(this string str, object arg0)
		{
			return string.Format(str, arg0);
		}

		public static string Fill(this string str, object arg0, object arg1)
		{
			return string.Format(str, arg0, arg1);
		}

		public static string Fill(this string str, object arg0, object arg1, object arg2)
		{
			return string.Format(str, arg0, arg1, arg2);
		}

		public static string Fill(this string str, params object[] args)
		{
			return string.Format(str, args);
		}

		public static string Fill(this string str, IFormatProvider provider, params object[] args)
		{
			return string.Format(provider, str, args);
		}

		public static string[] Split(this string str, params int[] lengthes)
		{
			return Split(str, true, lengthes);
		}

		public static string[] Split(this string str, bool trim, params int[] lengthes)
		{
			var result = new string[lengthes.Length];

			for (int i = 0, pos = 0; i < lengthes.Length; ++i)
			{
				bool eos = pos + lengthes[i] > str.Length;

				string part = eos ? str.Substring(pos) : str.Substring(pos, lengthes[i]);

				result[i] = trim ? part.Trim() : part;

				if (eos)
					break;

				pos += lengthes[i];
			}

			return result;
		}

		public static string TrimOrNull(this string str)
		{
			str = str.Trim();

			return str.Length == 0 ? null : str;
		}

		public static string TrimEndOrNull(this string str)
		{
			if (str == null) return null;

			str = str.TrimEnd();

			return str.Length == 0 ? null : str;
		}

		public static string ToLowerFirstLetter(this string str)
		{
			return string.IsNullOrEmpty(str) ? str : str.Substring(0, 1).ToLower() + str.Substring(1);
		}

		public static string ToUpperFirstLetter(this string str)
		{
			return string.IsNullOrEmpty(str) ? str : str.Substring(0, 1).ToUpper() + str.Substring(1);
		}

		public static string ResolvePath(this string str)
		{
			return str.Replace("~", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/'));
		}

		public static string Concat(this IList<string> strings, string separator)
		{
			var builder = new StringBuilder();

			string sep = string.Empty;

			foreach (string s in strings)
			{
				builder.Append(sep).Append(s);
				sep = separator;
			}

			return builder.ToString();
		}
	}
}