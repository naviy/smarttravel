using System;


namespace Luxena.Travel.Domain
{

	public static class SystemExtentions
	{
		private const string DateFormat = "dd.MM.yyyy";

		public static string ToDateString(this DateTime me)
		{
			return me.ToString(DateFormat);
		}
		public static string ToDateString(this DateTime? me)
		{
			return me?.ToString(DateFormat);
		}

		public static string ToDateString(this DateTimeOffset me)
		{
			return me.ToString(DateFormat);
		}
		public static string ToDateString(this DateTimeOffset? me)
		{
			return me?.ToString(DateFormat);
		}


		public static T AsDateString<T>(this DateTimeOffset? me, Func<string, T> evaluator)
		{
			return evaluator(me?.ToString(DateFormat));
		}
	}

}
