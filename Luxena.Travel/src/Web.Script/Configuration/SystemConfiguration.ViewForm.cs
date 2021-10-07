using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoForms;


namespace Luxena.Travel.Configuration
{
	public class SystemConfigurationViewForm : AutoViewForm
	{
		static SystemConfigurationViewForm()
		{
			FormsRegistry.RegisterView(ClassNames.SystemConfiguration, ViewObject);
		}

		private static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new SystemConfigurationViewForm(tabId, id, type); });
		}

		public SystemConfigurationViewForm(string tabId, object id, string type) : base(tabId, id, type)
		{
		}

		protected override void OnLoadConfig()
		{
			foreach (ColumnConfig t in ItemConfig.Columns)
				if (t.Name == "CompanyName")
				{
					t.Hidden = true;
					break;
				}
		}
	}
}