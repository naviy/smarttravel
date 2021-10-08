using System;
using System.Diagnostics;
using System.Globalization;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		[DebuggerStepThrough]
		public static string AsString(this object me)
		{
			return me?.ToString();
		}

		[DebuggerStepThrough]
		public static string AsString<T>(this T? me)
			where T : struct
		{
			return me?.ToString();
		}

		[DebuggerStepThrough]
		public static string AsString(this DateTime? me, string format)
		{
			return me?.ToString(format);
		}

		[DebuggerStepThrough]
		public static string AsString(this DateTime? me, string format, CultureInfo culture)
		{
			return me?.ToString(format, culture);
		}

	}

}
