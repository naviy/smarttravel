using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

using NLog;

using Npgsql;


namespace Luxena.Travel.Web
{

	static partial class AppConfig
	{

		public static void RegisterWebApi(HttpConfiguration config)
		{
			config.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger());
			config.Services.Replace(typeof(IExceptionHandler), new GeneralExceptionHandler());

			//config.SuppressDefaultHostAuthentication();
			//config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

			var allowedCors = new EnableCorsAttribute("*", "*", "*");
			config.EnableCors(allowedCors);

			config.MapHttpAttributeRoutes();

			RegisterOData(config);

			config.EnsureInitialized();
		}

	}


	class WebApiExceptionLogger : ExceptionLogger
	{
		public static readonly Logger Logger = LogManager.GetLogger("WebApi");

		public override void Log(ExceptionLoggerContext context)
		{
			var ex = context.ExceptionContext.Exception;
			Logger.Error(ex);

			ex.OfType<NpgsqlException>().Do(a =>
				Logger.Error(a.ErrorSql)
			);
		}
	}


	class GeneralExceptionHandler : ExceptionHandler
	{

		public override void Handle(ExceptionHandlerContext context)
		{
			var result = context.Result as ExceptionResult;
			if (result == null) return;

			HandleException<NpgsqlException>(context, result,
				ex => ex.Message + ex.ErrorSql.As(a => a + "\r\n")
			);
		}

		private bool HandleException<TException>(
			ExceptionHandlerContext context, ExceptionResult result,
			Func<TException, string> messageGetter
		)
			where TException: Exception
		{
			var ex = context.Exception;

			var ex2 = ex.OfType<TException>();
			if (ex2 == null) return false;

			var ex3 = new Exception(messageGetter(ex2), ex);

			context.Result = new ExceptionResult(
				ex3, result.IncludeErrorDetail, result.ContentNegotiator, 
				result.Request, result.Formatters
			);

			return true;
		}

	}

}
