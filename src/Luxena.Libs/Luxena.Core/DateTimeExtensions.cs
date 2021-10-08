using System;


namespace Luxena
{
	public static class DateTimeExtensions
	{
		public static DateTime AsUtc(this DateTime dateTime)
		{
			return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
		}
	}
}