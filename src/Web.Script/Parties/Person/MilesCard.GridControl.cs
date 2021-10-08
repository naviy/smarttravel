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

using Field = Ext.data.Field;
using Record = Ext.data.Record;


namespace Luxena.Travel
{
	public class MilesCardGridControl : IGridControl
	{
		public MilesCardGridControl()
		{
			Initialize();
		}

		public Component Widget
		{
			get { return _grid; }
		}

		public void LoadData(object[] data)
		{
			_milesCards = (MilesCardDto[])data;

			if (_milesCards != null && _milesCards.Length > 0)
				_grid.getStore().loadData(_milesCards);
		}

		public void Initialize()
		{
			_grid = new EditorGridPanel(new EditorGridPanelConfig()
				.store(InitStore())
				.cls("miles-card-grid")
				.sm(InitSelectionModel())
				.cm(InitColumnModel())
				.tbar(InitToolbar())
				.stripeRows(true)
				.enableHdMenu(false)
				.enableColumnResize(false)
				.enableColumnMove(false)
				.autoScroll(true)
				.clicksToEdit(2)
				.collapsible(false)
				.frame(false)
				.height(115)
				.ToDictionary());
		}

		public bool IsModified()
		{
			return _isModified;
		}

		public object[] GetData()
		{
			return GetCards();
		}

		public MilesCardDto[] GetCards()
		{
			int count = _store.getCount();

			if (count == 0)
				return null;

			MilesCardDto[] dtos = new MilesCardDto[count];

			for (int i = 0; i < count; i++)
				dtos[i] = GetMilesCard(_store.getAt(i));

			return dtos;
		}

		protected virtual Store InitStore()
		{
			_store = new JsonStore(new JsonStoreConfig()
				.fields(new string[] { "Id", "Version", "Number", "Organization" })
				.listeners(new Dictionary("update", new StoreUpdateDelegate(
					delegate(Store store, Record record, string operation)
					{
						if (operation == "edit")
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
			_deleteAction = new LinkButton(new ButtonConfig().text(BaseRes.Remove_Lower).handler(new AnonymousDelegate(RemoveItem)).disabled(true));

			return new Toolbar(new object[] {"->", _createAction, _editAction, _deleteAction });
		}

		private AbstractSelectionModel InitSelectionModel()
		{
			_selectionModel = new CheckboxSelectionModel();
			_selectionModel.singleSelect = false;
			_selectionModel.on("selectionchange", new SelectionChangedDelegate(
				delegate
				{
					if (_selectionModel.getCount() == 0)
						_deleteAction.disable();
					else
						_deleteAction.enable();

					if (_selectionModel.getCount() != 1)
						_editAction.disable();
					else
						_editAction.enable();
				}));

			return _selectionModel;
		}

		private static ColumnModel InitColumnModel()
		{
			ArrayList columnModelConfig = new ArrayList();

			columnModelConfig.Add(new Dictionary(
				"id", "Number",
				"header", DomainRes.MilesCard,
				"dataIndex", "Number",
				"width", 110,
				"editor", new TextField(new TextFieldConfig().ToDictionary())
				));

			LxnBase.Services.ColumnConfig config = new ClassColumnConfig();
			config.Type = TypeEnum.Object;
			config.IsRequired = true;

			((ClassColumnConfig)config).Clazz = "Airline";

			columnModelConfig.Add(new Dictionary(
				"id", "Organization",
				"header", DomainRes.MilesCard_Organization,
				"dataIndex", "Organization", 
				"width", 200,
				"editor", ControlFactory.CreateEditor(config),
				"renderer", ControlFactory.CreateRenderer(config)
				));

			return new ColumnModel(columnModelConfig);
		}

		private void CreateItem()
		{
			FormsRegistry.EditObject("MilesCard", null, null,
				delegate(object result)
				{
					Record record = new Record();
					record.data = ((ItemResponse)result).Item;
					record.fields = _store.fields;

					_store.add(new Record[] { record });

					_isModified = true;

				}, null, null, LoadMode.Local);
		}

		private void EditItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Record record = _selectionModel.getSelected();

			Dictionary data = (Dictionary)record.data;

			FormsRegistry.EditObject("MilesCard", data["Id"], data,
				delegate(object result)
				{
					UpdateRecord(((ItemResponse)result).Item, record);
				}
				, null, null, LoadMode.Local);
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

			foreach (Field recordField in recordFields) 
			{
				string name = recordField.name;

				object value = Type.GetField(obj, name);

				if (!Script.IsNullOrUndefined(value))
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

		private static MilesCardDto GetMilesCard(Record record)
		{
			MilesCardDto dto = new MilesCardDto();
			dto.Id = record.get("Id");
			dto.Version = record.get("Version");
			dto.Number = (string) record.get("Number");

			object organization = record.get("Organization");

			if (organization != null && organization is Array)
			{
				Array arr = (Array) organization;

				dto.Organization = new Reference();

				dto.Organization.Id = arr[Reference.IdPos];
				dto.Organization.Name = (string)arr[Reference.NamePos];
				dto.Organization.Type = (string)arr[Reference.TypePos];
			}
			else
				dto.Organization = (Reference)organization;

			return dto;
		}

		private MilesCardDto[] _milesCards;

		private EditorGridPanel _grid;
		private JsonStore _store;
		private CheckboxSelectionModel _selectionModel;

		private LinkButton _createAction;
		private LinkButton _editAction;
		private LinkButton _deleteAction;
		private bool _isModified;
	}
}