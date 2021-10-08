using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace Luxena.Travel.Web
{

	public class WebApiApplication : HttpApplication
	{

		protected void Application_Start()
		{
			AppConfig.WebApiRegister(GlobalConfiguration.Configuration);
			AppConfig.RegisterRoutes(RouteTable.Routes);
			AppConfig.RegisterBundles(BundleTable.Bundles);

			AppConfig.RegisterIoC(this);

			ViewEngines.Engines.Add(new AppConfig.AppRazorViewEngine());

			//GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			//GlobalConfiguration.Configuration.Formatters.JsonFormatter.Indent = true;
		}

		//		protected void Session_Start()
		//		{
		//			AppConfig.RegisterIoC(this);
		//		}

	}

}