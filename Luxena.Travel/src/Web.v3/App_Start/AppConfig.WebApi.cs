using System.Web.Http;


namespace Luxena.Travel.Web
{

	public static partial class AppConfig
	{

		public static void WebApiRegister(HttpConfiguration config)
		{

			config.Routes.MapHttpRoute(
				name: "ActionApi_Suggest",
				routeTemplate: "api/{controller}/suggest/{name}",
				defaults: new { action = "Suggest", }
			);

			config.Routes.MapHttpRoute(
				name: "ActionApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);


			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();
		}

	}

}
