using System;
using System.Collections;
using System.Html;

using Ext;
using Ext.data;
using Ext.form;
using Ext.grid;



using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Controls;

using ColumnConfig = LxnBase.Services.ColumnConfig;
using Field = Ext.data.Field;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public class PassportGridControl : IGridControl
	{

		public PassportGridControl()
		{
			Initialize();
		}

		public Component Widget { get { return _grid; } }

		public void LoadData(object[] data)
		{
			_passports = (PassportDto[])data;

			if (_passports != null && _passports.Length > 0)
				_grid.getStore().loadData(_passports);
		}

		public void Initialize()
		{
			_grid = new EditorGridPanel(new EditorGridPanelConfig()
				.store(InitStore())
				.cls("passport-grid")
				.sm(InitSelectionModel())
				.cm(InitColumnModel())
				.tbar(InitToolbar())
				.stripeRows(true)
				.enableHdMenu(false)
				.enableColumnResize(true)
				.enableColumnMove(false)
				.autoScroll(true)
				.height(115)
				.clicksToEdit(2)
				.collapsible(false)
				.frame(false)
				.ToDictionary());
		}

		public bool IsModified()
		{
			return _isModified;
		}

		public object[] GetData()
		{
			return GetPassports();
		}

		public PassportDto[] GetPassports()
		{
			int count = _store.getCount();

			if (count == 0)
				return new PassportDto[0];

			PassportDto[] dtos = new PassportDto[count];

			for (int i = 0; i < count; i++)
				dtos[i] = GetPassport(_store.getAt(i));

			return dtos;
		}

		protected virtual Store InitStore()
		{
			_store = new JsonStore(new JsonStoreConfig()
				.fields(new string[] { "Id", "Version", "Number", "FirstName", "LastName", "MiddleName", "Citizenship", "Birthday", "Gender", "IssuedBy", "ExpiredOn", "Note" })
				.listeners(new Dictionary("update", new StoreUpdateDelegate(
					delegate (Store store, Record record, string operation)
					{
						if (operation == Record.EDIT)
						{
							record.commit();

							_isModified = true;
						}
					})))
				.ToDictionary());
			return _store;
		}

		private Toolbar InitToolbar()
		{
			_createAction = new LinkButton(new ButtonConfig().text(BaseRes.CreateItem_Lower).handler(new AnonymousDelegate(CreateItem)));
			_editAction = new LinkButton(new ButtonConfig().text(BaseRes.Edit_Lower).handler(new AnonymousDelegate(EditItem)).disabled(true));
			_copyAction = new LinkButton(new ButtonConfig().text(BaseRes.Copy_Lower).handler(new AnonymousDelegate(CopyItem)).disabled(true));
			_deleteAction = new LinkButton(new ButtonConfig().text(BaseRes.Remove_Lower).handler(new AnonymousDelegate(RemoveItem)).disabled(true));

			return new Toolbar(new object[] { "->", _createAction, _editAction, _copyAction, _deleteAction });
		}

		private AbstractSelectionModel InitSelectionModel()
		{
			_selectionModel = new CheckboxSelectionModel();
			_selectionModel.singleSelect = false;
			_selectionModel.on("selectionchange", new SelectionChangedDelegate(delegate
			{
				if (_selectionModel.getCount() == 0)
					_deleteAction.disable();
				else
					_deleteAction.enable();

				if (_selectionModel.getCount() != 1)
				{
					_editAction.disable();
					_copyAction.disable();
				}
				else
				{
					_editAction.enable();
					_copyAction.enable();
				}
			}));

			return _selectionModel;
		}

		private static ColumnModel InitColumnModel()
		{
			ArrayList columnModelConfig = new ArrayList();

			columnModelConfig.Add(new Dictionary(
				"id", "Number",
				"header", DomainRes.Passport,
				"sortable", false,
				"dataIndex", "Number",
				"width", 70,
				"editor", new TextField(new TextFieldConfig().allowBlank(false).ToDictionary())
				));

			columnModelConfig.Add(new Dictionary(
				"id", "LastName",
				"header", DomainRes.Passport_LastName,
				"sortable", false,
				"dataIndex", "LastName",
				"width", 90,
				"editor", new TextField(new TextFieldConfig().ToDictionary())
				));

			columnModelConfig.Add(new Dictionary(
				"id", "FirstName",
				"header", DomainRes.Passport_FirstName,
				"sortable", false,
				"dataIndex", "FirstName",
				"width", 75,
				"editor", new TextField(new TextFieldConfig().ToDictionary())
				));

			columnModelConfig.Add(new Dictionary(
				"id", "MiddleName",
				"header", DomainRes.Passport_MiddleName,
				"sortable", false,
				"dataIndex", "MiddleName",
				"width", 80,
				"editor", new TextField(new TextFieldConfig().ToDictionary())
				));

			ColumnConfig countryConfig = new ClassColumnConfig();
			countryConfig.Type = TypeEnum.Object;
			countryConfig.IsRequired = false;

			((ClassColumnConfig)countryConfig).Clazz = "Country";

			columnModelConfig.Add(new Dictionary(
				"id", "Citizenship",
				"header", DomainRes.Passport_Citizenship,
				"sortable", false,
				"dataIndex", "Citizenship",
				"width", 82,
				"editor", ControlFactory.CreateEditor(countryConfig),
				"renderer", ControlFactory.CreateRenderer(countryConfig)
				));

			DateTimeColumnConfig dateTimeConfig = new DateTimeColumnConfig();
			dateTimeConfig.Type = TypeEnum.Date;

			columnModelConfig.Add(new Dictionary(
				"id", "Birthday",
				"header", DomainRes.Passport_Birthday,
				"sortable", false,
				"dataIndex", "Birthday",
				"width", 90,
				"editor", ControlFactory.CreateEditor(dateTimeConfig),
				"renderer", ControlFactory.CreateRenderer(dateTimeConfig)
				));

			ListColumnConfig listColumnConfig = new ListColumnConfig();
			listColumnConfig.Type = TypeEnum.List;
			listColumnConfig.IsRequired = true;

			listColumnConfig.Items = new object[]
			{
				new object[] { 0, Res.PersonEditForm_Male },
				new object[] { 1, Res.PersonEditForm_Female },
			};

			columnModelConfig.Add(new Dictionary(
				"id", "Gender",
				"header", DomainRes.Passport_Gender,
				"sortable", false,
				"dataIndex", "Gender",
				"width", 40,
				"editor", ControlFactory.CreateEditor(listColumnConfig),
				"renderer", new RenderDelegate(
					delegate (object value)
					{
						object[] items = listColumnConfig.Items;

						if (value is Number)
							foreach (object[] item in items)
							{
								if (item[0] == value)
									return item[1];
							}

						return value;
					})
				));

			columnModelConfig.Add(new Dictionary(
				"id", "IssuedBy",
				"header", DomainRes.Passport_IssuedBy,
				"sortable", false,
				"dataIndex", "IssuedBy",
				"width", 105,
				"editor", ControlFactory.CreateEditor(countryConfig),
				"renderer", ControlFactory.CreateRenderer(countryConfig)
				));

			columnModelConfig.Add(new Dictionary(
				"id", "ExpiredOn",
				"header", DomainRes.Passport_ExpiredOn,
				"sortable", false,
				"dataIndex", "ExpiredOn",
				"width", 100,
				"editor", ControlFactory.CreateEditor(dateTimeConfig),
				"renderer", ControlFactory.CreateRenderer(dateTimeConfig)
				));

			columnModelConfig.Add(new Dictionary(
				"id", "Note",
				"header", DomainRes.Common_Note,
				"sortable", false,
				"dataIndex", "Note",
				"width", 80,
				"editor", new TextField(new TextFieldConfig().ToDictionary())
				));

			return new ColumnModel(columnModelConfig);
		}

		private void CreateItem()
		{
			FormsRegistry.EditObject("Passport", null, null,
				delegate (object result)
				{
					Record record = new Record();
					record.data = ((ItemResponse)result).Item;
					record.fields = _store.fields;

					_store.add(new Record[] { record });

					_isModified = true;
				}, null, null, LoadMode.Local
			);
		}

		private void EditItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Record record = _selectionModel.getSelected();
			Dictionary data = (Dictionary)record.data;

			FormsRegistry.EditObject("Passport", data["Id"], data,
				delegate (object result) { UpdateRecord(((ItemResponse)result).Item, record); }
				, null, null, LoadMode.Local
			);
		}

		private void CopyItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Record srcRecord = _selectionModel.getSelected();
			Dictionary data = (Dictionary)srcRecord.data;

			FormsRegistry.EditObject("Passport", null, data,
				delegate (object result)
				{
					Record record = new Record();
					record.data = ((ItemResponse)result).Item;
					record.fields = _store.fields;
					_store.add(new Record[] { record });
					_isModified = true;
				}, null, null, LoadMode.Local
			);
		}

		private void RemoveItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Array selected = _selectionModel.getSelections();

			foreach (object a in selected)
				_store.remove(a);

			_isModified = true;
		}

		private void UpdateRecord(object obj, Record record)
		{
			Field[] recordFields = (Field[])record.fields.getRange();

			foreach (Field field in recordFields)
			{
				string name = field.name;

				object value = Type.GetField(obj, name);

				if (!Script.IsUndefined(value))
					Type.SetField(record.data, name, value);
			}

			record.commit();

			_isModified = true;
		}

		public bool HandleKeyEvent(ElementEvent keyEvent)
		{
			Dictionary eventDictionary = Dictionary.GetDictionary(typeof(EventObject));

			int key = keyEvent.KeyCode;
			bool isAdditionalKey = keyEvent.CtrlKey || keyEvent.ShiftKey || keyEvent.AltKey;
			bool isCtrlKey = keyEvent.CtrlKey && !keyEvent.ShiftKey && !keyEvent.AltKey;

			if (key == (int)eventDictionary["UP"])
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				SetFocus();

				return true;
			}

			if (key == (int)eventDictionary["DOWN"] && !isAdditionalKey)
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				SetFocus();

				return true;
			}

			if (key == (int)eventDictionary["F2"] && !isAdditionalKey)
			{
				if (_selectionModel.getSelections().Length != 1)
					return true;

				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				_grid.startEditing(_store.indexOf(_selectionModel.getSelected()), 1);

				return true;
			}

			if (key == (int)eventDictionary["ESC"])
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				SetFocus();

				return true;
			}

			if (key == (int)eventDictionary["INSERT"] && !isAdditionalKey)
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				CreateItem();

				return true;
			}

			if (key == (int)eventDictionary["ENTER"] && !keyEvent.CtrlKey && keyEvent.ShiftKey && !keyEvent.AltKey)
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				if (_selectionModel.getSelections().Length == 0)
					return true;

				EditItem();

				return true;
			}

			if (key == (int)eventDictionary["DELETE"] && isCtrlKey)
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				RemoveItem();

				return true;
			}

			if (key == (int)eventDictionary["HOME"] && !isAdditionalKey)
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				_selectionModel.selectRow(0);

				return true;
			}

			if (key == (int)eventDictionary["END"] && !isAdditionalKey)
			{
				keyEvent.CancelBubble = true;
				keyEvent.ReturnValue = false;

				_selectionModel.selectRow(_store.getCount() - 1);

				return true;
			}

			return false;
		}

		private void SetFocus()
		{
			_grid.getView().focusRow(0);
		}

		private static PassportDto GetPassport(Record record)
		{
			PassportDto dto = new PassportDto();

			dto.Id = record.get("Id");
			dto.Version = record.get("Version");

			dto.Number = (string)record.get("Number");
			dto.FirstName = (string)record.get("FirstName");
			dto.MiddleName = (string)record.get("MiddleName");
			dto.LastName = (string)record.get("LastName");

			object citizenship = record.get("Citizenship");

			if (citizenship != null && citizenship is Array)
				dto.Citizenship = CreateObjectInfo((Array)citizenship);
			else
				dto.Citizenship = (Reference)citizenship;

			dto.Birthday = (Date)record.get("Birthday");
			dto.Gender = (int)record.get("Gender");

			object issuedBy = record.get("IssuedBy");

			if (issuedBy != null && issuedBy is Array)
				dto.IssuedBy = CreateObjectInfo((Array)issuedBy);
			else
				dto.IssuedBy = (Reference)issuedBy;

			dto.ExpiredOn = (Date)record.get("ExpiredOn");

			dto.Note = (string)record.get("Note");

			return dto;
		}

		private static Reference CreateObjectInfo(Array data)
		{
			Reference obj = new Reference();

			obj.Id = data[Reference.IdPos];
			obj.Name = (string)data[Reference.NamePos];
			obj.Type = (string)data[Reference.TypePos];

			return obj;
		}

		private PassportDto[] _passports;

		private EditorGridPanel _grid;
		private JsonStore _store;
		private CheckboxSelectionModel _selectionModel;

		private LinkButton _createAction;
		private LinkButton _editAction;
		private LinkButton _copyAction;
		private LinkButton _deleteAction;

		private bool _isModified;
	}
}