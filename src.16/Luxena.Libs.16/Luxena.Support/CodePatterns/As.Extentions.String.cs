using System;
using System.Globalization;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		#region Format

		public static string AsFormat(this string me, object arg0)
		{
			return me == null ? null : string.Format(me, arg0);
		}

		public static string AsFormat(this string me, object arg0, object arg1)
		{
			return me == null ? null : string.Format(me, arg0, arg1);
		}

		public static string AsFormat(this string me, object arg0, object arg1, object arg2)
		{
			return me == null ? null : string.Format(me, arg0, arg1, arg2);
		}

		public static string AsFormat(this string me, params object[] args)
		{
			return me == null ? null : string.Format(me, args);
		}

		#endregion


		//#region Lower/Upper

		//public static string AsLower(this string me)
		//{
		//	return me?.ToLower();
		//}

		//public static string AsLower(this string me, CultureInfo culture)
		//{
		//	return me?.ToLower(culture);
		//}

		//public static string AsLowerInvariant(this string me)
		//{
		//	return me?.ToLowerInvariant();
		//}

		//public static string AsUpper(this string me)
		//{
		//	return me?.ToUpper();
		//}

		//public static string AsUpper(this string me, CultureInfo culture)
		//{
		//	return me?.ToUpper(culture);
		//}

		//public static string AsUpperInvariant(this string me)
		//{
		//	return me?.ToUpperInvariant();
		//}

		//#endregion


		#region Substring

		public static string AsSubstring(this string me, int startIndex)
		{
			return me != null && startIndex >= 0 && startIndex < me.Length
				? me.Substring(startIndex)
				: null;
		}

		public static string AsSubstring(this string me, int startIndex, int length)
		{
			return me != null && startIndex >= 0 && startIndex <= me.Length - length
				? me.Substring(startIndex, length)
				: null;
		}

		#endregion

		
		#region Trim

		//public static string AsTrim(this string me)
		//{
		//	return me == null ? null : me.Trim();
		//}

		//public static string AsTrim(this string me, params char[] trimChars)
		//{
		//	return me == null ? null : me.Trim(trimChars);
		//}

		//public static string AsTrimStart(this string me)
		//{
		//	return me == null ? null : me.TrimStart();
		//}

		//public static string AsTrimStart(this string me, params char[] trimChars)
		//{
		//	return me == null ? null : me.TrimStart(trimChars);
		//}

////		public static string AsTrimStart(this string me, params string[] trimStrings)
////		{
////			return me.TrimStart(trimStrings);
////		}

		//public static string AsTrimEnd(this string me)
		//{
		//	return me == null ? null : me.TrimEnd();
		//}

		//public static string AsTrimEnd(this string me, params char[] trimChars)
		//{
		//	return me == null ? null : me.TrimEnd(trimChars);
		//}

		#endregion



		#region Split

		//public static string[] AsSplit(this string me, params char[] separator)
		//{
		//	return me == null ? null : me.Split(separator);
		//}

		//public static string[] AsSplit(this string me, char[] separator, int count)
		//{
		//	return me == null ? null : me.Split(separator, count);
		//}

		//public static string[] AsSplit(this string me, char[] separator, StringSplitOptions options)
		//{
		//	return me == null ? null : me.Split(separator, options);
		//}

		//public static string[] AsSplit(this string me, char[] separator, int count, StringSplitOptions options)
		//{
		//	return me == null ? null : me.Split(separator, count, options);
		//}

		//public static string[] AsSplit(this string me, string[] separator, StringSplitOptions options) 
		//{
		//	return me == null ? null : me.Split(separator, options);
		//}

		//public static string[] AsSplit(this string me, string[] separator, int count, StringSplitOptions options)
		//{
		//	return me == null ? null : me.Split(separator, count, options);
		//}

		#endregion

	}

}
