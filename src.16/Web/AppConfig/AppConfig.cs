using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace Luxena.Travel.Web
{

	public static partial class AppConfig
	{

		public static void Register()
		{
			RegisterWebApi(GlobalConfiguration.Configuration);
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
			RegisterBundles(BundleTable.Bundles);
			RegisterLocalizationBundles(BundleTable.Bundles);
			RegisterAppBundles(BundleTable.Bundles);

			ViewEngines.Engines.Add(new AppRazorViewEngine());

			Domain.Domain.GetIdentityName = () => HttpContext.Current.User.Identity.Name;
		}

	}

}