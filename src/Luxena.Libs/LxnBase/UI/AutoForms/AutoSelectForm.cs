using System.Collections;

using Ext;
using Ext.grid;

using jQueryApi;

using LxnBase.Services;
using LxnBase.UI.AutoControls;

using WindowClass = Ext.Window;


namespace LxnBase.UI.AutoForms
{
	public class AutoSelectForm : IKeyHandler
	{
		public AutoSelectForm(SelectArgs args, ListConfig config)
		{
			_args = args;

			_listConfig = config;
			_listConfig.SingleSelect = _args.SingleSelect;
		}

		public int Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public int Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public ListConfig ListConfig
		{
			get { return _listConfig; }
		}

		public void Open()
		{
			InitGrid();

			_window = new WindowClass(new WindowConfig()
				.title(_listConfig.Caption ?? _args.Type)
				.items(_grid)
				.layout("fit")
				.width(_width)
				.height(_height)
				.listeners(new Dictionary("close", new AnonymousDelegate(OnWindowClose)))
				.modal(true)
				.ToDictionary());

			_grid.OnSelect +=
				delegate(object arg1)
				{
					if (_args.OnSelect != null)
						_args.OnSelect(arg1);

					_isSelect = true;

					_window.close();
				};

			_grid.OnCancelSelect += _window.close;

			EventsManager.RegisterKeyDownHandler(this, false);

			_window.show();
		}

		private void InitGrid()
		{
			AutoGridArgs args = new AutoGridArgs();

			args.Type = _args.Type;
			args.ListConfig = _listConfig;

			args.BaseRequest = _args.BaseRequest;

			args.Mode = ListMode.Select;

			EditorGridPanelConfig config = new EditorGridPanelConfig();

			OnInitGrid(args, config);

			_grid = new AutoGrid(args, config);
		}

		public virtual void OnInitGrid(AutoGridArgs config, EditorGridPanelConfig editorGridPanelConfig)
		{
		}

		private void OnWindowClose()
		{
			if (!_isSelect && _args.OnCancel != null)
				_args.OnCancel();

			EventsManager.UnregisterKeyDownHandler(this);
		}

		public bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			return _grid.HandleKeyEvent(keyEvent);
		}

		public void RestoreFocus()
		{
		}

		private readonly SelectArgs _args;
		private readonly ListConfig _listConfig;

		private WindowClass _window;
		private AutoGrid _grid;

		private int _width = 700;
		private int _height = 400;
		private bool _isSelect;
	}
}