using System;

using Castle.DynamicProxy;

using Common.Logging;


namespace Luxena.Castle
{
	public class LoggingInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			var log = LogManager.GetLogger(invocation.TargetType);

			long id = 0;
			long start = 0;

			if (log.IsTraceEnabled)
			{
				lock (_sync)
					id = _id == long.MaxValue ? 1 : ++_id;

				log.Trace(CreateInvocationLogString(invocation, id));

				start = StopWatch.GetTimeMark();
			}

			try
			{
				invocation.Proceed();
			}
			catch (Exception ex)
			{
				if (log.IsErrorEnabled) 
				{
					log.Error($"Failed ({id}):", ex);
					log.Error(ex.StackTrace);
				}
				throw;
			}

			if (log.IsTraceEnabled)
				log.Trace($"Completed ({id}) in {StopWatch.GetSeconds(start)}s");
		}

		public static string CreateInvocationLogString(IInvocation invocation, long id)
		{
			return $"Calling ({id}): {invocation.Method.Name}";
		}

		private readonly object _sync = new object();

		private long _id;
	}
}