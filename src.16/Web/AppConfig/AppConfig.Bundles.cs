using System.Web.Optimization;


namespace Luxena.Travel.Web
{

	static partial class AppConfig
	{

		public static void RegisterBundles(BundleCollection bundles)
		{
			RegisterBundles(
				bundles, "Libs",
				jQuery: true,
				Knockout: true,
				DevExtreme : true,
				FontAwesome: true
			);
		}


		public static void RegisterBundles(
			BundleCollection bundles,
			string bundleName,
			bool jQuery = false,
			bool AngularJS = false,
			bool CustomBootstrap = false,
			bool Bootstrap = false,
			bool Knockout = false,
			bool DevExtreme = false,
			bool FontAwesome = false
		)
		{
			styles = new StyleBundle("~/Styles/" + bundleName);
			scripts = new ScriptBundle("~/Scripts/" + bundleName);

			if (jQuery)
				RegisterJQuery();

			if (AngularJS)
				RegisterAngularJS();

			if (Bootstrap)
				RegisterBootstrap();
			else if (CustomBootstrap)
				RegisterCustomBootstrap();

			if (Knockout)
				RegisterKnockout();

			if (DevExtreme)
				RegisterDevExtreme();

			if (FontAwesome)
				RegisterFontAwesome();


			bundles.Add(styles);
			bundles.Add(scripts);
		}


		#region MVC Bundles

		private static StyleBundle styles;
		private static ScriptBundle scripts;

		public static void RegisterJQuery()
		{
			scripts.Include("~/Scripts/jquery-{version}.js");
		}

		public static void RegisterAngularJS()
		{
			scripts.Include("~/Scripts/angular.js");
			scripts.Include("~/Scripts/angular-sanitize.js");
		}

		public static void RegisterBootstrap()
		{
			styles.Include("~/Content/bootstrap.css");
			scripts.Include("~/Scripts/bootstrap.js");
		}
		public static void RegisterCustomBootstrap()
		{
			styles.Include("~/Content/bootstrap.custom/bootstrap.css");
//			scripts.Include("~/Content/bootstrap.custom/bootstrap.js");
		}

		public static void RegisterKnockout()
		{
			scripts.Include("~/Scripts/knockout-{version}.js");
			scripts.Include("~/Scripts/knockout.mapping-latest.js");
		}

		public static void RegisterDevExtreme()
		{
			scripts.Include("~/Scripts/jszip.js");
			styles.Include("~/Content/dx.spa.css");
			styles.Include("~/Content/dx.common.css");
//			styles.Include("~/Content/dx.light.css");
			scripts.Include("~/Scripts/dx.webappjs.js");
			scripts.Include("~/Scripts/dx.chartjs.js");
		}

		public static void RegisterFontAwesome()
		{
			styles.Include("~/Content/font-awesome.css");
		}

		#endregion

	}


}
