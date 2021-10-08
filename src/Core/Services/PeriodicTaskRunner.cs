using System.Threading;

namespace Luxena.Travel.Services
{
	public class PeriodicTaskRunner : BaseTaskRunner
	{
		public int Interval { get; set; }

		protected override void SetTimer()
		{
			Timer.Change(Interval, Timeout.Infinite);
		}
	}
}