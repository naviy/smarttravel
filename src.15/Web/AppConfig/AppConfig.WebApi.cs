using System;
using System.Data.Entity.Core;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

using Luxena.Domain.Web;

using NLog;


namespace Luxena.Travel.Web
{

	static partial class AppConfig
	{

		public static void RegisterWebApi(HttpConfiguration config)
		{
			config.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger());

			var allowedCors = new EnableCorsAttribute("*", "*", "*");
			config.EnableCors(allowedCors);

			config.MapHttpAttributeRoutes();

			RegisterOData(config);

			config.EnsureInitialized();
		}

	}


	public class WebApiExceptionLogger : ExceptionLogger
	{
		public static readonly Logger Logger = LogManager.GetLogger("WebApi");

		public override void Log(ExceptionLoggerContext context)
		{
			var ex = context.ExceptionContext.Exception;
			Logger.Error(ex);

			ex.OfType<Npgsql.NpgsqlException>().Do(a => 
				Logger.Error(a.ErrorSql)
			);
		}
	}

}
