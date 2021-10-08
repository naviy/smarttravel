using System;
using System.Collections;

using Ext;
using Ext.menu;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI.Controls.ColumnFilters;


namespace LxnBase.UI.AutoControls
{
	public class GridFilterPlugin
	{
		public GridFilterPlugin(AutoGrid grid)
		{
			_grid = grid;
		}

		public void Init(Component component)
		{
			component.on("render", new ComponentRenderDelegate(OnGridRender));
		}

		public void ResetFilter()
		{
			foreach (DictionaryEntry entry in _filterCheckItems)
			{
				Item[] items = ((Item[]) entry.Value);

				for (int i = 0; i < items.Length; i++)
					((CheckItem) items[i]).setChecked(false, true);
			}
		}

		public static void RegisterCustomFilters(string typeName, GridFilterConfig[] configs)
		{
			_customFilters[typeName] = configs;
		}

		private GridViewHack GridView
		{
			get { return (GridViewHack) _grid.GridView; }
		}

		private void OnGridRender(Component component)
		{
			InitFilterMenu();

			GridView.Hmenu.on("beforeshow", new AnonymousDelegate(
				delegate
				{
					EventsManager.RegisterKeyDownHandler(GridView.Hmenu, false);

					ShowFilterMenu();
				}));
			GridView.Hmenu.on("beforehide", new AnonymousDelegate(EventsManager.UnregisterKeyDownHandler));

			GridView.Cm.on("columnmoved", new AnonymousDelegate(
				delegate { RefreshGridColumnHeaders(); }));

			_grid.store.on("load", new AnonymousDelegate(
				delegate
				{
					ResetFilter();

					RefreshGridColumnHeaders();
				}));
		}

		private void InitFilterMenu()
		{
			GridView.Hmenu.addSeparator();

			for (int i = 0; i < _grid.ListConfig.Columns.Length; i++)
			{
				ColumnConfig columnConfig = _grid.ListConfig.Columns[i];

				Item[] items = CreateMenuItems(columnConfig);

				for (int j = 0; j < items.Length; j++)
					GridView.Hmenu.add(items[j]);

				_filterCheckItems[columnConfig.Name] = items;
			}
		}

		private Item[] CreateMenuItems(ColumnConfig config)
		{
			TypeEnum type = config.Type;

			if (type != TypeEnum.Custom)
			{
				BaseFilter filter = GetSimpleFilter(config);

				if (filter == null)
					return null;

				filter.ColumnConfig = config;
				filter.Changed += OnFilterChanged;

				return new Item[]
				{
					new CheckItem(new Dictionary(
						"text", BaseRes.Filter_Title,
						"hideOnClick", false,
						"menu", filter.GetFilterMenu(),
						"hidden", true,
						"filter", filter,
						"listeners", new Dictionary("checkchange", new CheckItemCheckchangeDelegate(UpdateGridWithFilter))))
				};
			}

			CustomTypeColumnConfig cfg = (CustomTypeColumnConfig) config;

			if (!Script.IsNullOrUndefined(cfg.TypeName))
			{
				ArrayList items = new ArrayList();

				GridFilterConfig[] filterConfigs = (GridFilterConfig[]) _customFilters[cfg.TypeName];

				if (filterConfigs==null)
					return (Item[])items;

				for (int i = 0; i < filterConfigs.Length; i++)
				{
					BaseFilter filter = filterConfigs[i].Filter.Create();
					filter.ColumnConfig = config;
					filter.InternalPath = filterConfigs[i].InternalPath;
					filter.Changed += OnFilterChanged;

					items.Add(new CheckItem(new Dictionary(
						"text", filterConfigs[i].Caption,
						"hideOnClick", false,
						"menu", filter.GetFilterMenu(),
						"filter", filter,
						"listeners", new Dictionary("checkchange", new CheckItemCheckchangeDelegate(UpdateGridWithFilter)))));
				}

				return (Item[]) items;
			}

			return null;
		}

		private static BaseFilter GetSimpleFilter(ColumnConfig columnConfig)
		{
			TypeEnum type = columnConfig.Type;

			if (type == TypeEnum.Bool)
				return new BooleanFilter();

			if (type == TypeEnum.String)
				return new StringFilter();

			if (type == TypeEnum.Number)
				return new NumberFilter();

			if (type == TypeEnum.Date)
				return new DateFilter();

			if (type == TypeEnum.Object)
			{
				ColumnConfig config = new ColumnConfig();

				config.Type = ((ClassColumnConfig) columnConfig).FilterType;

				return GetSimpleFilter(config);
			}

			if (type == TypeEnum.List)
				return new ListFilter();

			return null;
		}

		private void RefreshGridColumnHeaders()
		{
			RangeRequest rangeRequest = (RangeRequest) _grid.store.baseParams;

			for (int i = 0; i < _grid.ListConfig.Columns.Length; i++)
			{
				string name = _grid.ListConfig.Columns[i].Name;

				double colIndex = _grid.ColumnModel.findColumnIndex(name);

				if (colIndex >= 0)
				{
					Item[] menuItems = (Item[]) _filterCheckItems[name];

					bool isFiltered = false;

					for (int j = 0; j < menuItems.Length; j++)
					{
						BaseFilter filter = (BaseFilter) Type.GetField(menuItems[j], "filter");

						if (FindPropertyFilter(rangeRequest.Filters, name, filter.InternalPath) != null)
						{
							UpdateFilter(name, menuItems[j]);

							isFiltered = true;
							break;
						}
					}

					SetColumnFiltered((int) colIndex, isFiltered);
				}
			}
		}

		private void ShowFilterMenu()
		{
			ColumnConfig config = _grid.GetColumnConfig(GridView.HdCtxIndex);

			foreach (DictionaryEntry entry in _filterCheckItems)
			{
				for (int i = 0; i < ((Item[]) entry.Value).Length; i++)
					((Item[]) entry.Value)[i].hide();
			}

			if (Script.IsNullOrUndefined(_filterCheckItems[config.Name]))
				return;

			Item[] menuItems = (Item[]) _filterCheckItems[config.Name];

			for (int i = 0; i < menuItems.Length; i++)
				menuItems[i].show();
		}

		private void UpdateFilter(string name, Item menuItem)
		{
			BaseFilter filter = (BaseFilter) Type.GetField(menuItem, "filter");

			PropertyFilter propertyFilter = FindPropertyFilter(((RangeRequest) _grid.store.baseParams).Filters, name, filter.InternalPath);

			if (propertyFilter == null || propertyFilter.Conditions == null || propertyFilter.Conditions.Length == 0)
				return;

			filter.Conditions = propertyFilter.Conditions;

			((CheckItem) menuItem).setChecked(true, true);
		}

		private void OnFilterChanged(object sender, EventArgs e)
		{
			BaseFilter filter = (BaseFilter) sender;

			Item[] items = (Item[]) _filterCheckItems[filter.ColumnConfig.Name];

			for (int i = 0; i < items.Length; i++)
			{
				if (Type.GetField(items[i], "filter") == filter)
				{
					((CheckItem) items[i]).setChecked(filter.Conditions != null, true);

					UpdateGridWithFilter((CheckItem) items[i], filter.Conditions != null);

					break;
				}
			}
		}

		private void UpdateGridWithFilter(CheckItem menuItem, bool isChecked)
		{
			BaseFilter filter = (BaseFilter) Type.GetField(menuItem, "filter");

			if (isChecked && filter.Conditions != null)
			{
				ApplyFilter(filter);

				SetColumnFiltered(GridView.HdCtxIndex, true);

				_grid.Reload(true);
			}
			else
			{
				RangeRequest rangeRequest = (RangeRequest) _grid.store.baseParams;

				PropertyFilter propertyFilter = FindPropertyFilter(rangeRequest.Filters, filter.ColumnConfig.Name, filter.InternalPath);

				if (propertyFilter != null)
				{
					DeletePropertyFilter(rangeRequest, filter.ColumnConfig.Name, filter.InternalPath);

					SetColumnFiltered(GridView.HdCtxIndex, false);

					_grid.Reload(true);
				}
			}
		}

		private void ApplyFilter(BaseFilter filter)
		{
			RangeRequest rangeRequest = (RangeRequest) _grid.store.baseParams;

			PropertyFilter[] filters = rangeRequest.Filters;

			PropertyFilter propertyFilter = FindPropertyFilter(filters, filter.ColumnConfig.Name, filter.InternalPath);

			if (propertyFilter == null)
			{
				propertyFilter = new PropertyFilter();
				propertyFilter.Property = filter.ColumnConfig.Name;
				propertyFilter.InternalPath = filter.InternalPath;

				if (filters == null)
					rangeRequest.Filters = new PropertyFilter[] { propertyFilter };
				else
					filters[filters.Length] = propertyFilter;
			}

			propertyFilter.Conditions = filter.Conditions;
		}

		private static PropertyFilter FindPropertyFilter(PropertyFilter[] filters, string propertyName, string internalPath)
		{
			if (filters == null)
				return null;

			for (int i = 0; i < filters.Length; i++)
				if (filters[i].Property == propertyName && IsPathEquals(filters[i].InternalPath, internalPath))
					return filters[i];

			return null;
		}

		private static void DeletePropertyFilter(RangeRequest request, string propertyName, string internalPath)
		{
			if (request.Filters == null)
				return;

			PropertyFilter[] filters = new PropertyFilter[] { };

			for (int i = 0; i < request.Filters.Length; i++)
				if (request.Filters[i].Property != propertyName || !IsPathEquals(request.Filters[i].InternalPath, internalPath))
					filters[filters.Length] = request.Filters[i];

			request.Filters = filters;
		}

		private void SetColumnFiltered(int columnIndex, bool isFitered)
		{
			const string filterClassName = "filterCls";

			CompositeElement cellHeaders = (CompositeElement) GridView.MainHd.select("td");
			Element headerCell = cellHeaders.item(columnIndex);

			if (isFitered)
				headerCell.addClass(filterClassName);
			else
				headerCell.removeClass(filterClassName);
		}

		private static bool IsPathEquals(string path1, string path2)
		{
			if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2))
				return true;

			if (path1 == path2)
				return true;

			return false;
		}

		private readonly AutoGrid _grid;

		private static readonly Dictionary _customFilters = new Dictionary();

		private readonly Dictionary _filterCheckItems = new Dictionary();
	}
}