using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Optimization;


namespace Luxena.Travel.Web
{

	public static partial class AppConfig
	{

		public static void RegisterLocalizationBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/Scripts/Localize").Include(
				"~/Scripts/globalize/globalize.js",
				"~/Scripts/globalize/cultures/globalize.culture.ru.js",
				"~/Scripts/devextreme-localization/dx.webappjs.ru.js",
				"~/app/App.localize.culture.ru.js"
			));
		}

		public static void RegisterAppBundles(BundleCollection bundles)
		{
			appStyles = new StyleBundle("~/Styles/App");
			appScripts = new ScriptBundle("~/Scripts/App");

			//appScripts.Include("~/app/_app.js");

//			appScripts.Include(new[]
//			{
//				"~/app/Config.js",
//				"~/app/App.js",
//				"~/app/Domain.Entities.js", 
//				"~/app/Domain.js",
//				"~/app/Semantics.js", 
//
//			});
//			appStyles.Include("~/icons/_icons.css");

			appStyles.Include("~/app/_support/app.css");
			SearchAppViews();

			bundles.Add(appStyles);
			bundles.Add(appScripts);
		}



		public static string SitePath;

		private static string ToVirtualPath(string path)
		{
			return path.Replace(SitePath, "~/").Replace('\\', '/');
		}

		private static void SearchAppViews()
		{
			SitePath = System.Web.HttpContext.Current.Server.MapPath(@"~/");
			var viewsPath = Path.Combine(SitePath, @"app");

			foreach (var dirPath in Directory.GetDirectories(viewsPath))
			{
				SearchAppViews(dirPath);
			}
		}

		private static void SearchAppViews(string path)
		{

			//foreach (var filePath in Directory.GetFiles(path, "*.css"))
			//{
			//	appStyles.Include(ToVirtualPath(filePath));
			//}

			foreach (var filePath in Directory.GetFiles(path, "*.cshtml"))
			{
				var fileUrl = ToVirtualPath(filePath);
				if (fileUrl.Equals("~/app/home/index.cshtml", StringComparison.InvariantCultureIgnoreCase)) continue;
				if (fileUrl.Equals("~/app/home/login.cshtml", StringComparison.InvariantCultureIgnoreCase)) continue;

				DxViewUrls.Add(fileUrl);
			}

			foreach (var dirPath in Directory.GetDirectories(path))
			{
				SearchAppViews(dirPath);
			}
		}


		public static readonly List<string> DxViewUrls = new List<string>();

		private static StyleBundle appStyles;
		private static ScriptBundle appScripts;

	}

}