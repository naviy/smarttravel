using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.AutoForms;

using ColumnConfig = LxnBase.Services.ColumnConfig;
using Record = Ext.data.Record;


namespace Luxena.Travel.Controls
{

	public delegate void GridColumnConfigAction(Ext.grid.ColumnConfig cfg);

	public class AutoListTabExt : AutoListTab
	{
		public static void ListObjects(ListArgs args, bool newTab)
		{
			Tabs.Open(newTab, args.Type, delegate(string tabId) { return new AutoListTabExt(tabId, args); }, args.BaseRequest);
		}

		public AutoListTabExt(string tabId, ListArgs args) : base(tabId, args)
		{
		}

		public ListConfig ListConfig
		{
			get { return _listConfig; }
		}

		public AutoGridArgs GridArgs
		{
			get { return _args; }
		}

		protected Record SelectedRecord
		{
			get { return ((RowSelectionModel) AutoGrid.getSelectionModel()).getSelected(); }
		}

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			_args = args;
			_listConfig = args.ListConfig;

			config
				.stateful(true)
				.stateId(string.Format("{0}_{1}_List", AppManager.CurrentUser.Name, ListArgs.Type))
				.listeners(new Dictionary("beforestaterestore", new EditorGridPanelStaterestoreDelegate(OnBeforeStateRestore)));

			args.CreateActionToolbar += OnCreateActionToolbar;
		}

		private void OnCreateActionToolbar(object arg1, object arg2)
		{
			ArrayList actions = (ArrayList) arg1;
			AutoGrid grid = (AutoGrid) arg2;

			OnAddToolbarButtons(actions, grid);
		}

		private void OnBeforeStateRestore(Component component, object state)
		{
			if (Script.IsNullOrUndefined(state))
				return;

			ArrayList columns = (ArrayList) Type.GetField(state, "columns");

			if (Script.IsValue(columns))
			{
				for (int i = columns.Count - 1; i >= 0; i--)
				{
					Column col = (Column) columns[i];

					if (col.id != "checker" && col.id != "Id" && Script.IsNullOrUndefined(TryGetColumnConfigByName(col.id)))
						columns.RemoveAt(i);
					else if (Script.IsNullOrUndefined(col.sortable))
						col.sortable = true;
				}
			}

			object sort = Type.GetField(state, "sort");

			if (Script.IsValue(sort))
			{
				string field = (string) Type.GetField(sort, "field");

				if (Script.IsValue(field) && Script.IsNullOrUndefined(TryGetColumnConfigByName(field)))
					Type.SetField(state, "sort", null);
			}
		}

		protected override void OnLoad()
		{
			AutoGrid.SelectionModel.on("selectionchange", new SelectionChangedDelegate(OnSelectionChange));
		}

		protected virtual void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
		}

		protected virtual void OnSelectionChange(AbstractSelectionModel selectionModel)
		{
		}

		public OperationStatus GetOperationStatus(string actionName)
		{
			Dictionary permissions = _listConfig.CustomActionPermissions;

			if (Script.IsValue(permissions) && permissions.ContainsKey(actionName))
				return (OperationStatus) permissions[actionName];

			OperationStatus publicStatus = new OperationStatus();
			publicStatus.Visible = true;
			publicStatus.IsDisabled = false;

			return publicStatus;
		}

		public ColumnConfig TryGetColumnConfigByName(string name)
		{
			ColumnConfig[] columnConfigs = _listConfig.Columns;

			foreach (ColumnConfig col in columnConfigs)
				if (col.Name == name)
					return col;

			return null;
		}

		public ColumnConfig GetColumnConfigByName(string name)
		{
			ColumnConfig cfg = TryGetColumnConfigByName(name);

			if (Script.IsNullOrUndefined(cfg))
			{
				throw new Exception("Unknown column \"" + name + "\"");
			}

			return cfg;
		}

		[AlternateSignature]
		public extern Dictionary ColumnCfg(string name);

		[AlternateSignature]
		public extern Dictionary ColumnCfg(string name, bool isHidden);

		[AlternateSignature]
		public extern Dictionary ColumnCfg(string name, bool isHidden, object elWidth);

		[AlternateSignature]
		public extern Dictionary ColumnCfg(string name, bool isHidden, object elWidth, object renderer);

		public Dictionary ColumnCfg(string name, bool isHidden, object elWidth, object renderer, GridColumnConfigAction initAction)
		{
			return ColumnCfg_(name, isHidden, elWidth, renderer, initAction).ToDictionary();
		}

		public Ext.grid.ColumnConfig ColumnCfg_(string name, bool isHidden, object elWidth, object renderer, GridColumnConfigAction initAction)
		{
			ColumnConfig config = GetColumnConfigByName(name);

			Ext.grid.ColumnConfig cfg = new Ext.grid.ColumnConfig()
				.id(config.Name)
				.header(!string.IsNullOrEmpty(config.Caption) ? config.Caption : config.Name)
				.sortable(config.IsPersistent)
				.dataIndex(config.Name)
				.hidden(isHidden);

			if (Script.IsNullOrUndefined(renderer))
				renderer = ControlFactory.CreateRenderer(config);

			if (renderer != null)
				cfg.renderer(renderer);

			if (Script.IsValue(elWidth))
				cfg.width((double)elWidth);

			if (Script.IsValue(initAction))
				initAction(cfg);

			return cfg;
		}


		public Dictionary CreateReferenceCfg(string name, bool isHidden, object elWidth)
		{
			return ColumnCfg(name, isHidden, elWidth, ControlFactory.CreateRefrenceRenderer(GridArgs.Type));
		}

		private ListConfig _listConfig;
		private AutoGridArgs _args;
	}
}