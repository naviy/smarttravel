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
			return me == null ? null : me.ToString();
		}

		[DebuggerStepThrough]
		public static string AsString<T>(this T? me)
			where T : struct
		{
			return me == null ? null : me.Value.ToString();
		}

		[DebuggerStepThrough]
		public static string AsString(this DateTime? me, string format)
		{
			return me == null ? null : me.Value.ToString(format);
		}

		[DebuggerStepThrough]
		public static string AsString(this DateTime? me, string format, CultureInfo culture)
		{
			return me == null ? null : me.Value.ToString(format, culture);
		}

	}

}
