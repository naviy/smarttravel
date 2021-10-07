using System.Collections;

using LxnBase.Services;


namespace LxnBase.Data
{

	public delegate void ListConfigLoaded(ListConfig listConfig);

	public delegate void ItemConfigLoaded(ItemConfig itemConfig);


	public static class ConfigManager
	{

		private static readonly Dictionary _listConfigs = new Dictionary();
		private static readonly Dictionary _viewConfigs = new Dictionary();
		private static readonly Dictionary _editConfigs = new Dictionary();


		public static void GetListConfig(string className, ListConfigLoaded listConfigLoaded)
		{
			if (_listConfigs.ContainsKey(className))
				listConfigLoaded((ListConfig) _listConfigs[className]);
			else
			{
				GenericService.GetRangeConfig(className,
					delegate(object result)
					{
						_listConfigs[className] = result;

						listConfigLoaded((ListConfig) result);
					},
					null);
			}
		}

		public static void GetViewConfig(string className, ItemConfigLoaded itemConfigLoaded)
		{
			if (_viewConfigs.ContainsKey(className))
				itemConfigLoaded((ItemConfig) _viewConfigs[className]);
			else
			{
				GenericService.GetItemConfig(className, true,
					delegate(object result)
					{
						_viewConfigs[className] = result;

						itemConfigLoaded((ItemConfig) result);
					}, null);
			}
		}

		public static void GetEditConfig(string className, ItemConfigLoaded itemConfigLoaded)
		{
			if (_editConfigs.ContainsKey(className))
				itemConfigLoaded((ItemConfig) _editConfigs[className]);
			else
			{
				GenericService.GetItemConfig(className, false,
					delegate(object result)
					{
						_editConfigs[className] = result;

						itemConfigLoaded((ItemConfig) result);
					}, null);
			}
		}

	}

}