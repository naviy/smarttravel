using System.Web.Mvc;
using System.Web.Routing;


namespace Luxena.Travel.Web
{

	public partial class AppConfig
	{

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

//			routes.MapRoute(
//				name: "EmbeddedResource",
//				url: "scripts/domain/{file}",
//				defaults: new
//				{
//					controller = "Home",
//					action = "EmbeddedScript",
//					file = UrlParameter.Optional
//				}
//			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new
				{
					controller = "Home",
					action = "Index",
					id = UrlParameter.Optional
				}
			);

		}

	}

}