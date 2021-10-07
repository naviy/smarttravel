using System.Web.Optimization;


namespace Luxena.Travel.Web
{

	public static partial class AppConfig
	{

		public static void RegisterAppBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/Scripts/App").Include(new[]
			{
				"~/app/App.js",
				"~/app/Domain.js",
			
				"~/app/Controls/GridCtrl.js",
				"~/app/Controls/ViewFormCtrl.js",
				"~/app/Controls/EditFormCtrl.js",
				"~/app/Controls/FormGridCtrl.js",

				"~/app/Parties/PartyServices.js",
				"~/app/Parties/Organization/OrganizationCtrls.js",
				"~/app/Parties/Person/PassportCtrls.js",
				"~/app/Parties/Person/PersonCtrls.js",

			}));
		}
	}

}