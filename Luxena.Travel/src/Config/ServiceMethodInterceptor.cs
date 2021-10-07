using System;
using System.Security;
using System.Web;

using Castle.DynamicProxy;
using Common.Logging;

using Luxena.Base.Text;


namespace Luxena.Travel.Config
{
	public class ServiceMethodInterceptor : IInterceptor
	{
		public IErrorTranslator MainTranslator { get; set; }

		public IErrorTranslator DefaultTranslator { get; set; }

		public bool AllowUnknownExceptions { get; set; }

		public void Intercept(IInvocation invocation)
		{
			var httpContext = HttpContext.Current;
			if (httpContext != null && httpContext.Request.Url.Host.ToLower() == "travel")
			{
				invocation.Proceed();
				return;
			}

			var log = LogManager.GetLogger(invocation.TargetType);

			string logString = null;

			if (log.IsTraceEnabled)
			{
				logString = CreateInvocationLogString(invocation);
				log.Trace(logString);
			}

			try
			{
				invocation.Proceed();
			}
			catch (Exception ex)
			{
				Exception translation = null;

				if (MainTranslator != null)
					translation = MainTranslator.Translate(ex);

				if (translation == null)
				{
					translation = DefaultTranslator != null ? DefaultTranslator.Translate(ex) : ex;

					if (log.IsErrorEnabled)
					{
						if (logString == null)
							logString = CreateInvocationLogString(invocation);

						log.Error(logString, ex);
					}

					if (AllowUnknownExceptions)
						throw;
				}
				else
					log.Warn(translation);

				if (ex is SecurityException)
					throw new SecurityException(translation.Message);

				throw translation;
			}
		}

		public static string CreateInvocationLogString(IInvocation invocation)
		{
			return string.Format("Calling {0}", invocation.Method.Name);
		}
	}
}