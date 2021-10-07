using System.ComponentModel;
using System.Runtime.InteropServices;


namespace Luxena
{
	public class StopWatch
	{
		static StopWatch()
		{
			if (!QueryPerformanceFrequency(out _frequency))
				throw new Win32Exception();
		}

		public static long GetTimeMark()
		{
			long result;

			QueryPerformanceCounter(out result);

			return result;
		}

		public static double GetSeconds(long start)
		{
			return GetSeconds(start, GetTimeMark());
		}

		public static double GetSeconds(long start, long finish)
		{
			double ticks = finish - start;

			return ticks/_frequency;
		}

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		private static readonly long _frequency;
	}
}