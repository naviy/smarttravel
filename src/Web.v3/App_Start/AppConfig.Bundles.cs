using System.Reflection;
using System.Web.Optimization;

using TwitterBootstrapMVC;


namespace Luxena.Travel.Web
{

	public static partial class AppConfig
	{

		public static void RegisterBundles(BundleCollection bundles)
		{
			RegisterBundles(
				bundles, "Libs",
				UIBootstrap: true,
				logger: true
			);

			RegisterAppBundles(bundles);
		}


		public static void RegisterBundles(BundleCollection bundles,
			string bundleName,
			bool AngularJS = true,
			bool Bootstrap = false,
			bool jQuery = false,
			bool UIBootstrap = false,
			bool ngGrid = false,
			bool Q = false,
			bool toastr = false,
			bool logger = false
		)
		{
			styles = new StyleBundle("~/Styles/" + bundleName);
			scripts = new ScriptBundle("~/Scripts/" + bundleName);

			if (jQuery || UIBootstrap || ngGrid)
				RegisterJQuery();

			if (AngularJS || UIBootstrap || ngGrid)
				RegisterAngularJS();

			if (Bootstrap || UIBootstrap || ngGrid)
				RegisterBootstrap();

			if (UIBootstrap)
				RegisterUIBootstrap();

			if (ngGrid)
				RegisterNgGrid();

			if (Q) // || Breeze)
				RegisterQ();

//			if (Breeze)
//				RegisterBreeze();

			if (toastr || logger)
				RegisterToastr();

			if (logger)
				RegisterLogger();

			bundles.Add(styles);
			bundles.Add(scripts);
		}


		#region MVC Bundles

		private static StyleBundle styles;
		private static ScriptBundle scripts;

		public static void RegisterAngularJS()
		{
			scripts.Include("~/Scripts/angular.js");
		}

		public static void RegisterBootstrap()
		{
			//styles.Include("~/Content/bootstrap/css/bootstrap.css");
			
			styles.Include("~/Content/bootstrap.css");//, "~/Content/bootstrap-responsive.css");
			//styles.Include("~/Content/bootstrap/bootstrap-theme.css");
			scripts.Include("~/Scripts/bootstrap.js");

			styles.Include("~/Content/font-awesome.css");

			Bootstrap.Configure();
			typeof(Bootstrap)
				.GetProperty("LicenseIsValid", BindingFlags.NonPublic | BindingFlags.Static)
				.SetValue(null, (bool?)true);
		}

		public static void RegisterJQuery()
		{
			scripts.Include("~/Scripts/jquery-{version}.js");
		}

		public static void RegisterUIBootstrap()
		{
			scripts.Include("~/Scripts/ui-bootstrap-tpls-{version}.js");
		}

		public static void RegisterNgGrid()
		{
			styles.Include("~/Content/ng-grid.css");
			scripts.Include("~/Scripts/ng-grid-{version}.js");
		}

		public static void RegisterQ()
		{
			scripts.Include("~/Scripts/q.js");
		}

		public static void RegisterToastr()
		{
			styles.Include("~/Content/toastr.css");
			scripts.Include("~/Scripts/toastr.js");
		}

		public static void RegisterLogger()
		{
			scripts.Include("~/Scripts/toastr-logger.js");
		}

		#endregion

	}


}
