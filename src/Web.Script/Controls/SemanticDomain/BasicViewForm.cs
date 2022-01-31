using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.form;

using Luxena.Travel;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI.Controls;

using Action = Ext.Action;
using ActionConfig = Ext.ActionConfig;


namespace LxnBase.UI
{

	public abstract class BasicViewForm: Tab
	{
		protected BasicViewForm(string tabId, object id, string type, string tabCls)
			: base(new PanelConfig()
				.title(BaseRes.Loading)
				.autoScroll(true)
				.closable(true)
				.cls(tabCls)
				.ToDictionary(), tabId)
		{
			_id = id;
			_type = type;

			LoadInstance();
		}

		protected abstract void LoadInstance();

		protected abstract void Load(object item);
		protected abstract string GetTitle();


		protected override void OnActivate(bool isFirst)
		{
			if (!isFirst)
				LoadInstance();
		}

		internal void EditEntity()
		{
			FormsRegistry.EditObject(_type, _id, null,
				delegate(object result)
				{
					if (Script.IsValue(result))
						Load(((ItemResponse)result).Item);
					else
						LoadInstance();
				},
				null
			);
		}

		protected void RemoveEntity()
		{
			MessageBoxWrap.Confirm(BaseRes.Confirmation, BaseRes.Delete_Confirmation,
				delegate(string button, string text)
				{
					if (button != "yes")
						return;

					GenericService.Delete(_type, new object[] { _id }, null,
						delegate(object result)
						{
							DeleteOperationResponse response = (DeleteOperationResponse)result;

							if (response.Success)
							{
								Tabs.Close(this);

								MessageRegister.Info(DomainRes.AviaDocument_Caption_List, BaseRes.Deleted + " " + GetTitle());
							}
							else
								OnDeleteFailed();
						},
						delegate
						{
							OnDeleteFailed();
						}
					);
				});
		}


		protected void CopyEntity()
		{
			FormsRegistry.EditObject(_type, _id, null,
				delegate (object result)
				{
					ItemResponse response = (ItemResponse)result;

					Dictionary obj = (Dictionary)response.Item;
					//RangeResponse rangeResponse = response.RangeResponse;

					object reference = obj[ObjectPropertyNames.Reference];

					if (reference is Date)
						reference = ((Date)reference).Format("d.m.Y");

					string message = BaseRes.Created + " " + reference;

					//if (Script.IsNullOrUndefined(rangeResponse.SelectedRow))
					//{
					//	MessageRegister.Info(title, message, BaseRes.AutoGrid_NotDisplay_Msg);
					//	return;
					//}

					MessageRegister.Info(title, message);

					//ReloadWithData(rangeResponse);
				},
				null, null, LoadMode.Remote, true);
		}




		protected void OnDeleteFailed()
		{
			ReplaceForm.Exec(_type, _id, this);
		}



		protected override void initComponent()
		{
			tbar = CreateToolbarItems();

			base.initComponent();

			_contentPanel = (Panel) add(new Panel(new PanelConfig()
				.border(false)
				.cls("content")
				.ToDictionary()));

			add(CreateTitlePanel());
			add(_contentPanel);
		}

		protected abstract object[] CreateToolbarItems();

		protected Panel CreateTitlePanel()
		{
			_titleLabel = new Label(new LabelConfig().ToDictionary());

			Panel panel = new Panel(new PanelConfig()
				.border(false)
				.listeners(new Dictionary("render", new GenericOneArgDelegate(
					delegate(object arg)
					{
						Panel p = (Panel)arg;

						_titleLabel.render(p.body);
					})))
				.cls("title")
				.ToDictionary());

			return panel;
		}


		[AlternateSignature]
		protected extern Action MenuAction(string title_, object[] items_);
		protected Action MenuAction(string title_, object[] items_, bool disabled_)
		{
			ActionConfig config = (ActionConfig)new ActionConfig()
				.text(title_)
				.custom("menu", new Ext.menu.Menu(new Ext.menu.MenuConfig().items(items_).ToDictionary()))
			;

			if (!Script.IsNullOrUndefined(disabled_))
				config = config.disabled(disabled_);

			return new Action(config.ToDictionary());
		}

		protected Ext.menu.Item MenuItem(string title_, AnonymousDelegate handler)
		{
			return new Ext.menu.Item(new Ext.menu.ItemConfig()
				.text(title_)
				.handler(handler)
				.ToDictionary());
		}



		[AlternateSignature]
		public extern Action Action(string title_, AnonymousDelegate onclick);

		public Action Action(string title_, AnonymousDelegate onclick, bool disabled_)
		{
			return new Action(new ActionConfig()
				.text(title_)
				.disabled(!Script.IsNullOrUndefined(disabled_) && disabled_)
				.handler(onclick)
				.ToDictionary());
		}


		[AlternateSignature]
		public static extern string Link(Reference obj);

		public static string Link(Reference obj, string text)
		{

			if (Script.IsNullOrUndefined(obj) && Script.IsNullOrUndefined(text))
				return string.Empty;


			if (Script.IsNullOrUndefined(obj) && !Script.IsNullOrUndefined(text))
				return text;


			Reference info = new Reference();
			info.Id = obj.Id;
			info.Name = text ?? obj.Name;
			info.Type = obj.Type;


			return ObjectLink.RenderInfo(info);

		}


		protected static string NotEmpty(string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			return value;
		}

		[AlternateSignature]
		protected static extern string HasValue(object obj, StringDelegate fn);

		[AlternateSignature]
		protected static extern string HasValue(object[] objects, StringDelegate fn);

		protected static string HasValue(object obj, StringDelegate fn, string falseText)
		{
			string falseStr = Script.IsNullOrUndefined(falseText) ? string.Empty : falseText;

			if (Script.IsNullOrUndefined(obj) || (Number)obj == 0 || obj.ToString() == string.Empty)
				return falseStr;

			if (obj is Array)
			{
				object[] objects = (object[])obj;

				for (int i = 0; i < objects.Length; i++)
					if (objects[i] != null)
						return fn();

				return falseStr;
			}

			return fn();
		}

		protected static void AddValue(Dictionary dictionary, object obj, string name)
		{
			object value = Type.GetField(obj, name);

			if (Script.IsValue(value))
				dictionary[name] = value;
		}



		protected readonly object _id;
		protected readonly string _type;

		protected Panel _contentPanel;
		protected Label _titleLabel;
	}

}
