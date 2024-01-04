using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.data;
using Ext.grid;
using Ext.menu;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel.Controls
{

	public abstract class AutoListTabExt2 : AutoListTabExt, IGridForm
	{

		protected static void RegisterList(string className, Type tabType)
		{
			FormsRegistry.RegisterList(className, delegate(ListArgs args, bool newTab)
			{
				ListObjectsOfType(tabType, args, newTab);
			});
		}

		protected static void ListObjectsOfType(Type type, ListArgs args, bool newTab)
		{
			Tabs.Open(
				newTab, args.Type,
				delegate(string tabId) { return (AutoListTabExt2)Type.CreateInstance(type, tabId, args); },
				args.BaseRequest
			);
		}

		protected static void ListObjectsOfType2(Type type, string tabId_, ListArgs args, bool newTab)
		{
			Tabs.Open(
				newTab, tabId_,
				delegate (string tabId) { return (AutoListTabExt2)Type.CreateInstance(type, tabId, args); },
				args.BaseRequest
			);
		}

		protected AutoListTabExt2(string tabId, ListArgs args) : base(tabId, args)
		{
		}


		protected override void OnLoad()
		{
			AutoGrid.on("beforerender", new ComponentRenderDelegate(GridBeforeRender));

			_baseParams = (RangeRequest)AutoGrid.store.baseParams;

			base.OnLoad();
		}

		private void GridBeforeRender(Component objthis)
		{
			AutoGrid.store.on("load", new StoreLoadDelegate(OnStoreLoad));
		}

		protected virtual void OnStoreLoad(Store sender, Record[] records, object options)
		{
		}


		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			if (args.BaseRequest == null)
				args.BaseRequest = new RangeRequest();

			PreInitialize();
			Initialize();
			PostInitialize();

			CreateColumnConfigs();

			args.ColumnsConfig = ColumnConfigs;
		}

		protected virtual void PreInitialize()
		{
		}

		protected virtual void Initialize()
		{
		}

		protected virtual void PostInitialize()
		{
		}


		protected abstract void CreateColumnConfigs();

		protected ArrayList ColumnConfigs = new ArrayList();

		protected void AddColumns(object[] views)
		{
			foreach (object col in views)
			{
				SemanticMember semantic = col as SemanticMember;
				ColumnConfigs.Add(semantic != null ? semantic.ToColumn() : col);
			}
		}

		protected virtual void Create(string type)
		{
			AutoGrid.Create(type);
		}

		public override bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			int key = keyEvent.Which;

			bool isAdditionalKey = keyEvent.CtrlKey || keyEvent.ShiftKey || keyEvent.AltKey;

			Dictionary eventDictionary = Dictionary.GetDictionary(typeof(EventObject));

			if (key == (int)eventDictionary["ENTER"] && !keyEvent.CtrlKey && !keyEvent.ShiftKey && keyEvent.AltKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				HandleAltEnterPress();

				return true;
			}

			if (key == (int)eventDictionary["INSERT"] && !isAdditionalKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				HandleInsertPress();

				return true;
			}

			return base.HandleKeyEvent(keyEvent);
		}

		protected virtual void HandleAltEnterPress()
		{
		}

		protected virtual void HandleInsertPress()
		{
		}


		#region Toolbar & Actions

		private Action AddToToolbar(Action action)
		{
			toolbarActions.Add(action);

			return action;
		}

		[AlternateSignature]
		protected extern Action Action(string title_, AnonymousDelegate handler);

		[AlternateSignature]
		protected extern Action Action(string title_, AnonymousDelegate handler, bool disabled_);

		protected Action Action(string title_, AnonymousDelegate handler, bool disabled_, string tooltip)
		{
			ActionConfig config = new ActionConfig()
				.text(title_)
				.handler(handler);

			if (Script.IsValue(disabled_))
				config = config.disabled(disabled_);

			if (Script.IsValue(tooltip))
				config = (ActionConfig)config.custom("tooltip", tooltip);

			return AddToToolbar(new Action(config.ToDictionary()));
		}

		[AlternateSignature]
		protected extern Action MenuAction(string title_, object[] items_);

		protected Action MenuAction(string title_, object[] items_, bool disabled_)
		{
			ActionConfig config = (ActionConfig)new ActionConfig()
				.text(title_)
				.custom("menu", new Menu(new MenuConfig().items(items_).ToDictionary()));

			if (!Script.IsNullOrUndefined(disabled_))
				config = config.disabled(disabled_);

			return AddToToolbar(new Action(config.ToDictionary()));
		}

		protected Item MenuItem(string title_, AnonymousDelegate handler)
		{
			return new Item(new ItemConfig()
				.text(title_)
				.handler(handler)
				.ToDictionary());
		}

		protected Item MenuCreateItem(string title_, string type)
		{
			return new Item(new ItemConfig()
				.text(title_)
				.handler(new AnonymousDelegate(delegate { Create(type); }))
				.ToDictionary()
			);
		}

		protected Item MenuCreateItem2(string title_, Action<string> create, string type)
		{
			return new Item(new ItemConfig()
				.text(title_)
				.handler(new AnonymousDelegate(delegate { create(type); }))
				.ToDictionary()
			);
		}


		protected void ToolbarSeparator()
		{
			toolbarActions.Add(new ToolbarSeparator());
		}

		protected ArrayList toolbarActions = new ArrayList();

		#endregion


		#region Utils

		public static string[] GetDictionaryKeys(Dictionary dictionary)
		{
			if (dictionary == null)
				return null;

			ArrayList keys = new ArrayList();

			foreach (DictionaryEntry entry in dictionary)
				keys.Add(entry.Key);

			return (string[])keys;
		}

		public static object[] GetDictionaryValues(Dictionary dictionary)
		{
			if (dictionary == null)
				return null;

			ArrayList values = new ArrayList();

			foreach (DictionaryEntry entry in dictionary)
				values.Add(entry.Value);

			return (object[])values;
		}

		#endregion


		protected RangeRequest _baseParams;

	}

}