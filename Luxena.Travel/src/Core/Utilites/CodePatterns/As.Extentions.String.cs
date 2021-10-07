using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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


		#region Lower/Upper

		public static string AsLower(this string me)
		{
			return me == null ? null : me.ToLower();
		}

		public static string AsLower(this string me, CultureInfo culture)
		{
			return me == null ? null : me.ToLower(culture);
		}

		public static string AsLowerInvariant(this string me)
		{
			return me == null ? null : me.ToLowerInvariant();
		}

		public static string AsUpper(this string me)
		{
			return me == null ? null : me.ToUpper();
		}

		public static string AsUpper(this string me, CultureInfo culture)
		{
			return me == null ? null : me.ToUpper(culture);
		}

		public static string AsUpperInvariant(this string me)
		{
			return me == null ? null : me.ToUpperInvariant();
		}

		#endregion


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

		public static string AsTrim(this string me)
		{
			return me == null ? null : me.Trim();
		}

		public static string AsTrim(this string me, params char[] trimChars)
		{
			return me == null ? null : me.Trim(trimChars);
		}

		public static string AsTrimStart(this string me)
		{
			return me == null ? null : me.TrimStart();
		}

		public static string AsTrimStart(this string me, params char[] trimChars)
		{
			return me == null ? null : me.TrimStart(trimChars);
		}

//		public static string AsTrimStart(this string me, params string[] trimStrings)
//		{
//			return me.TrimStart(trimStrings);
//		}

		public static string AsTrimEnd(this string me)
		{
			return me == null ? null : me.TrimEnd();
		}

		public static string AsTrimEnd(this string me, params char[] trimChars)
		{
			return me == null ? null : me.TrimEnd(trimChars);
		}

		#endregion

	}

}
