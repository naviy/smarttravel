using System.Web.Mvc;
using System.Web.Routing;


namespace Luxena.Travel.Web
{

	static partial class AppConfig
	{

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("LogIn", "login", new { controller = "Home", action = "LogIn" });
			routes.MapRoute("LogOut", "logout", new { controller = "Home", action = "LogOut" });


			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}

	}

}