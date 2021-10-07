using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;


namespace Luxena.Travel
{

	public abstract class BaseEditForm2 : BaseEditForm, IEditForm
	{

		protected static void RegisterEdit(string className, Type formType)
		{
			FormsRegistry.RegisterEdit(className, delegate (EditFormArgs args)
			{
				ConfigManager.GetEditConfig(args.Type,
					delegate (ItemConfig config)
					{
						BaseEditForm2 form = (BaseEditForm2)Type.CreateInstance(formType);
						form.Init(args, config);
						form.Open();
					});
			});
		}

		protected BaseEditForm2()
		{
		}

		public virtual void Init(EditFormArgs args, ItemConfig config)
		{
			_members = new FormMembers();
			_args = args;
			_itemConfig = config;

			if (_args.OnSave != null)
				Saved += _args.OnSave;

			if (_args.OnCancel != null)
				Canceled += _args.OnCancel;

			Window.setTitle(string.Format("{0} ({1})", _itemConfig.Caption, _args.IsNew ? BaseRes.Creation : BaseRes.Editing));

			_saveButton = Form.addButton(BaseRes.Save, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			FieldMaxWidth = 160;
			Form.labelWidth = 100;

			PreInitialize();
			Initialize();
			PostInitialize();
		}

		protected virtual void PreInitialize()
		{
		}

		protected virtual void Initialize()
		{
		}

		protected virtual void PostInitialize()
		{
			CreateControls();

			int ldelta = Form.labelWidth;

			if (Window.width == -1)
				Window.width = 260 + ldelta;
			else if (Window.width == -2)
				Window.width = 410 + 2 * ldelta;
			else if (Window.width == -3)
				Window.width = 420 + ldelta;
			else if (Window.width == -4)
				Window.width = 540 + 2 * ldelta;
			else if (Window.width == -6)
				Window.width = 770 + 2 * ldelta;

			ArrayList fields = new ArrayList();
			Members.AddToFieldList(fields);
			Fields = (Field[])fields;

			ArrayList list = new ArrayList();
			list.AddRange((Field[])fields);
			list.Add(_saveButton);
			list.Add(_cancelButton);

			ComponentSequence = (Component[])list;
		}


		public abstract void Open();

		//		protected void OpenProductObject1(string className)
		//		{
		//			ProductService.GetObject1(className, _args.IdToLoad, OnLoad, null);
		//		}
		//
		//		protected void SaveProductObject1(string className)
		//		{
		//			ProductService.UpdateObject1(className, GetData(), _args.RangeRequest, CompleteSave, FailSave);
		//		}

		protected abstract void CreateControls();

		protected virtual string GetNameBy(object data)
		{
			return data == null ? "<empty>" : (string)((Dictionary)data)["Name"];
		}


		protected virtual void OnLoad(object result)
		{
			Updating = true;

			if (_args.IsCopy)
			{
				_data = _args.FieldValues = Dictionary.GetDictionary(result);
				((Dictionary)result)["Id"] = null;

				//Log.log("FieldValues: ", _data);

				foreach (string name in _data.Keys)
				{
					object value = _data[name];
					if (!Script.IsValue(value)) continue;
					if (Script.IsValue(Type.GetField(value, "length")) && Type.HasMethod(value, "push"))
					{
						//Log.logb(name);
						ArrayList items = (ArrayList)value;
						//Log.log("Copy from: ", name, items);
						foreach (object item in items)
						{
							Type.SetField(item, "Id", null);
							Type.SetField(item, "Version", 0);
						}
						//Log.log("Copy to: ", items);
						//Log.loge();
					}

				}
				//Log.loge();
			}
			else
			{
				if (Script.IsValue(_args.IdToLoad))
					InitData = (Dictionary)result;

				if (!Script.IsValue(InitData))
					InitData = GetInitData();

				_data = InitData;
			}

			Members.LoadValues();

			Window.show();

			Updating = false;
		}

		protected virtual Dictionary GetInitData()
		{
			return null;
		}

		protected object GetData()
		{
			_data = new Dictionary();

			if (InitData != null)
			{
				_data["Id"] = InitData["Id"];
				_data["Version"] = InitData["Version"];
			}

			Members.SaveValues();

			return _data;
		}


		protected override bool IsModified()
		{
			return Members.IsModified();
		}



		protected override void Save()
		{
			if (!Validate()) return;

			_saveButton.setDisabled(true);

			if (InitData == null || IsModified())
				OnSave();
			else
				Cancel();
		}

		protected override void OnSaved(object result)
		{
			if (Script.IsUndefined(_args.RangeRequest))
			{
				if (Script.IsValue(result))
				{
					Dictionary dto = (Dictionary)((ItemResponse)result).Item;
					ShowSaveMessage(dto);
				}
			}

			base.OnSaved(result);
		}

		protected virtual void ShowSaveMessage(Dictionary r)
		{
			MessageRegister.Info(
				_itemConfig.ListCaption,
				string.Format("{0} {1}",
					_args.IsNew ? BaseRes.Created : BaseRes.Updated,
					GetNameBy(r)
				)
			);
		}


		public int FieldMaxWidth
		{
			get { return _fieldMaxWidth; }
			protected set { _fieldMaxWidth = value; }
		}
		private int _fieldMaxWidth;

		public bool Updating { get { return _updating; } set { _updating = value; } }
		private bool _updating;

		public object GetValue(string name)
		{
			if (_args.FieldValues != null && !Script.IsNullOrUndefined(_args.FieldValues[name]))
				return _args.FieldValues[name];

			if (Script.IsNullOrUndefined(_data)) return null;

			object value = Type.GetField(_data, name);

			return Script.IsValue(value) ? value : null;
		}

		public void SetValue(string name, object value)
		{
			Type.SetField(Data, name, value);
		}


		public object GetCurrentValue(string name)
		{
			foreach (FormMember member in Members.Items)
			{
				if (member.Semantic._name != name) continue;
				return member.GetFields()[0].getValue();
			}

			return null;
		}

		public void SetCurrentValue(string name, object value)
		{
			foreach (FormMember member in Members.Items)
			{
				if (member.Semantic._name != name) continue;

				member.GetFields()[0].setValue(value);
				return;
			}
		}

		protected override void CompleteSave(object result)
		{
			_saveButton.setDisabled(false);
			base.CompleteSave(result);
		}

		protected override void FailSave(object result)
		{
			_saveButton.setDisabled(false);
			base.FailSave(result);
		}


		protected ArrayList AddEditors(ArrayList fieldList, object[] editors)
		{
			if (fieldList == null)
				fieldList = new ArrayList();

			foreach (object editor in editors)
			{
				if (!Script.IsValue(editor)) continue;

				SemanticMember member = editor as SemanticMember;
				fieldList.Add(member != null ? member.ToEditor() : editor);
			}

			return fieldList;
		}


		#region Components


		public Panel TabPane(string title, object[] items)
		{
			return new Panel(new PanelConfig()
				.items((object[])AddEditors(null, items))
				.layout("form")
				.title(title)
				.ToDictionary()
			);
		}

		public Panel TabMainPane(string title, object[] items)
		{
			return new Panel(new PanelConfig()
				.items((object[])AddEditors(null, items))
				.layout("form")
				.cls("main-data")
				.title(title)
				.ToDictionary()
			);
		}

		public Panel TabColumnPane(string title, object[] items)
		{
			return new Panel(new PanelConfig()
				.items((object[])AddEditors(null, items))
				.layout("column")
				.title(title)
				.ToDictionary()
			);
		}

		public Panel TabFitPane(string title, object item)
		{
			return new Panel(new PanelConfig()
				.items((object[])AddEditors(null, new object[] { item }))
				.layout("fit")
				.title(title)
				.ToDictionary()
			);
		}

		public Panel MainDataPanel(object[] items)
		{
			return new Panel(new PanelConfig()
				.items((object[])AddEditors(null, items))
				.layout("form")
				.cls("main-data")
				.ToDictionary()
			);
		}

		public Panel ColumnPanel(object[] items)
		{
			return new Panel(new PanelConfig()
				.items((object[])AddEditors(null, items))
				.layout("column")
				.ToDictionary()
			);
		}

		public Panel FormPanel(object[] items)
		{
			return new Panel(new PanelConfig()
				.items((object[])AddEditors(null, items))
				.layout("form")
				.ToDictionary()
			);
		}

		#endregion


		private Dictionary _data;
		public Dictionary Data { get { return _data; } }
		protected Dictionary InitData;

		protected EditFormArgs _args;
		public EditFormArgs Args { get { return _args; } }

		protected ItemConfig _itemConfig;
		protected Button _saveButton;
		protected Button _cancelButton;

		public FormMembers Members { get { return _members; } }
		FormMembers _members;
	}

}