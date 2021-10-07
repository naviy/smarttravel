using System;

using Ext;
using Ext.grid;

using jQueryApi;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI.AutoControls;


namespace LxnBase.UI.AutoForms
{
	public class AutoListTab : Tab
	{
		public AutoListTab(string tabId, ListArgs args) : base(
			new PanelConfig()
				.closable(true)
				.autoScroll(true)
				.layout("fit")
				.title(BaseRes.Loading)
				.ToDictionary(),
			tabId)
		{
			if (args == null)
				throw new Exception("Args cannot be null");

			_args = args;

			ConfigManager.GetListConfig(_args.Type, Load);
		}

		protected ListArgs ListArgs
		{
			get { return _args; }
		}

		protected AutoGrid AutoGrid
		{
			get { return _autoGrid; }
		}

		protected Ext.data.Record[] SelectedRecords
		{
			get { return (Ext.data.Record[])_autoGrid.SelectionModel.getSelections(); }
		}


		public override void BeforeActivate(object @params)
		{
			_updateOnActivation = Script.IsNullOrUndefined(@params);

			if (_updateOnActivation)
				return;

			_autoGrid.ReloadGrid((RangeRequest) @params);
		}

		protected override void OnActivate(bool isFirst)
		{
			if (!isFirst && _updateOnActivation)
				_autoGrid.Refresh();
		}

		public override bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			return _autoGrid.HandleKeyEvent(keyEvent);
		}

		private void Load(ListConfig listConfig)
		{
			setTitle(string.IsNullOrEmpty(listConfig.Caption) ? _args.Type : listConfig.Caption);

			add(InitGrid(listConfig));

			OnLoad();

			doLayout();
		}

		private Component InitGrid(ListConfig listConfig)
		{
			AutoGridArgs args = new AutoGridArgs();

			args.Type = _args.Type;
			args.ListConfig = listConfig;
			args.BaseRequest = _args.BaseRequest;

			EditorGridPanelConfig config = new EditorGridPanelConfig();

			OnInitGrid(args, config);

			_autoGrid = new AutoGrid(args, config);

			return _autoGrid;
		}

		protected virtual void OnInitGrid(AutoGridArgs config, EditorGridPanelConfig editorGridPanelConfig)
		{
		}

		protected virtual void OnLoad()
		{
		}

		private readonly ListArgs _args;
		private AutoGrid _autoGrid;
		private bool _updateOnActivation = true;
	}
}