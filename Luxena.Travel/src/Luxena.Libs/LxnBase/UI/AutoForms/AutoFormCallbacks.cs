using LxnBase.Data;
using LxnBase.Services;


namespace LxnBase.UI.AutoForms
{
	public static class AutoFormCallbacks
	{
		public static void RegisterAsDefaults()
		{
			FormsRegistry.RegisterDefaultList(ListObjects);
			FormsRegistry.RegisterDefaultView(ViewObject);
			FormsRegistry.RegisterDefaultEdit(EditObject);
			FormsRegistry.RegisterDefaultSelect(SelectObjects);
		}

		public static void ListObjects(ListArgs args, bool newTab)
		{
			Tabs.Open(newTab, args.Type, delegate(string tabId) { return new AutoListTab(tabId, args); }, args.BaseRequest);
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new AutoViewForm(tabId, id, type); });
		}

		public static void EditObject(EditFormArgs args)
		{
			ConfigManager.GetEditConfig(args.Type,
				delegate(ItemConfig config)
				{
					AutoEditForm form = new AutoEditForm(args, config);
					form.Open();
				});
		}

		public static void SelectObjects(SelectArgs args)
		{
			ConfigManager.GetListConfig(args.Type,
				delegate(ListConfig config)
				{
					AutoSelectForm form = new AutoSelectForm(args, config);

					form.Open();
				});
		}
	}
}