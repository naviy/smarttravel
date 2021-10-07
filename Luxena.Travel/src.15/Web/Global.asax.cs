using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace Luxena.Travel.Web
{

	public class MvcApplication : HttpApplication
	{

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			AppConfig.Register();
		}


//		protected void Application_BeginRequest(object sender, EventArgs e)
//		{
//			var config = GlobalConfiguration.Configuration;
//		}

	}

}