using System.Web.Mvc;


namespace Luxena.Travel.Web
{

	public partial class AppConfig
	{

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

	}

}