using System.Web;
using System.Web.Mvc;


namespace Luxena.Travel.Web
{

	static partial class AppConfig
	{

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

	}

}