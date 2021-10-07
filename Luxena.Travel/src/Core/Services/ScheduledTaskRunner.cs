using System;

namespace Luxena.Travel.Services
{
	public class ScheduledTaskRunner : BaseTaskRunner
	{
		public int Days { get; set; }

		public int Hours { get; set; }

		public int Minutes { get; set; }

		protected override void SetTimer()
		{
			var interval = DateTime.Today.AddDays(Days).AddHours(Hours).AddMinutes(Minutes) - DateTime.Now;

			Timer.Change(interval, TimeSpan.FromMilliseconds(-1));
		}
	}
}