using System.Web.Mvc;


namespace Luxena.Travel.Web
{

	static partial class AppConfig
	{

		public class AppRazorViewEngine : RazorViewEngine
		{

			public AppRazorViewEngine()
			{

				ViewLocationFormats = new[]
				{
					"~/app/Home/{0}.cshtml",
				};

				MasterLocationFormats = new[]
				{
					"~/app/Layouts/{0}.cshtml",
				};

				PartialViewLocationFormats = new[]
				{
					"~/app/{0}.cshtml",
					"~/app/Home/{0}.cshtml",

//					"~/app/Accounting/{1}/{0}.cshtml",
//					"~/app/Avia/{1}/{0}.cshtml",
//					"~/app/Configuration/{1}/{0}.cshtml",
//					"~/app/Parties/{1}/{0}.cshtml",
				};

			}

		}

	}

}