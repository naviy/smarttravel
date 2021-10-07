using System;
using System.Collections;
using System.Html;

using Ext;
using Ext.data;
using Ext.grid;
using Ext.util;

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

	public class FlightSegmentGridControl : IGridControl
	{

		public FlightSegmentGridControl()
		{
			Initialize();
		}

		public Component Widget { get { return _grid; } }

		public void LoadData(object[] data)
		{
			_items = (FlightSegmentDto[])data;

			if (_items != null && _items.Length > 0)
				_grid.getStore().loadData(_items);
		}

		public void Initialize()
		{
			_grid = new EditorGridPanel(new EditorGridPanelConfig()
				.store(InitStore())
				.cls("segments-grid")
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
			return GetFlightSegments();
		}

		public FlightSegmentDto[] GetFlightSegments()
		{
			int count = _store.getCount();

			if (count == 0)
				return new FlightSegmentDto[0];

			FlightSegmentDto[] dtos = new FlightSegmentDto[count];

			for (int i = 0; i < count; i++)
			{
				dtos[i] = (FlightSegmentDto)_store.getAt(i).data;
				dtos[i].Position = i;
			}

			return dtos;
		}

		protected virtual Store InitStore()
		{
			_store = new JsonStore(new JsonStoreConfig()
				.fields(new string[]
				{
					"Id", "Version",
					"Position",
					"FromAirport", "FromAirportName",
					"ToAirport", "ToAirportName",
					"CarrierCode", "Carrier", "Operator",
					"FlightNumber", "Seat", "Equipment",
					"ServiceClassCode", "ServiceClass",
					"DepartureTime",
					"CheckInTerminal", "CheckInTime",
					"ArrivalTime", "ArrivalTerminal",
					"NumberOfStops", "Luggage",
					"Duration",
					"FareBasis", "Stopover", "Type",
					"MealTypes",
				})
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
			_deleteAction = new LinkButton(new ButtonConfig().text(BaseRes.Remove_Lower).handler(new AnonymousDelegate(RemoveItem)).disabled(true));
			_upAction = new LinkButton(new ButtonConfig().text("вверх").disabled(true).handler(new AnonymousDelegate(Up)));
			_downAction = new LinkButton(new ButtonConfig().text("вниз").disabled(true).handler(new AnonymousDelegate(Down)));

			//return new Toolbar(new object[] { "->", _upAction, _downAction, _deleteAction });
			return new Toolbar(new object[] { "->", _upAction, _downAction, _createAction, _editAction, _deleteAction });
		}

		private AbstractSelectionModel InitSelectionModel()
		{
			_selectionModel = new CheckboxSelectionModel();
			_selectionModel.singleSelect = false;

			_selectionModel.on("selectionchange", new SelectionChangedDelegate(delegate
			{
				if (_selectionModel.getCount() == 0)
				{
					_deleteAction.disable();
					_upAction.disable();
					_downAction.disable();
				}
				else
				{
					_deleteAction.enable();
					Record record = _selectionModel.getSelected();
					_upAction.enable();
					_downAction.enable();

					int position = (int)((Dictionary)record.data)["Position"];
					if (position == 0)
						_upAction.disable();
					if (position == _items.Length - 1)
						_downAction.disable();
				}

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
				"id", "Position",
				"header", '#',
				"sortable", false,
				"dataIndex", "Position",
				"width", 40
			));

			columnModelConfig.Add(new Dictionary(
				"id", "FromAirport",
				"header", DomainRes.FlightSegment_FromAirportName,
				"sortable", false,
				"dataIndex", "FromAirport",
				"width", 80,
				"renderer", new RenderDelegate(RenderObjectInfoToString)
			));

			columnModelConfig.Add(new Dictionary(
				"id", "ToAirport",
				"header", DomainRes.FlightSegment_ToAirportName,
				"sortable", false,
				"dataIndex", "ToAirport",
				"width", 68,
				"renderer", new RenderDelegate(RenderObjectInfoToString)
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Carrier",
				"header", DomainRes.FlightSegment_Carrier,
				"sortable", false,
				"dataIndex", "Carrier",
				"width", 72,
				"renderer", new RenderDelegate(RenderObjectInfoToString)
			));

			columnModelConfig.Add(new Dictionary(
				"id", "FlightNumber",
				"header", DomainRes.FlightSegment_FlightNumber,
				"sortable", false,
				"dataIndex", "FlightNumber",
				"width", 33
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Seat",
				"header", DomainRes.FlightSegment_Seat,
				"sortable", false,
				"dataIndex", "Seat",
				"width", 40
			));

			columnModelConfig.Add(new Dictionary(
				"id", "ServiceClassCode",
				"header", DomainRes.FlightSegment_ServiceClass,
				"sortable", false,
				"dataIndex", "ServiceClassCode",
				"width", 40
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Equipment",
				"header", "Тип судна",
				"sortable", false,
				"dataIndex", "Equipment",
				"width", 72,
				"renderer", new RenderDelegate(RenderObjectInfoToString)
			));

			DateTimeColumnConfig dateTimeConfig = new DateTimeColumnConfig();
			dateTimeConfig.Type = TypeEnum.Date;

			columnModelConfig.Add(new Dictionary(
				"id", "DepartureTime",
				"header", DomainRes.FlightSegment_DepartureTime,
				"sortable", false,
				"dataIndex", "DepartureTime",
				"width", 78,
				"renderer", new RenderDelegate(RenderDateTime)
			));

			columnModelConfig.Add(new Dictionary(
				"id", "CheckInTime",
				"header", Res.FlightSegment_Registration,
				"sortable", false,
				"dataIndex", "CheckInTime",
				"width", 75
			));

			columnModelConfig.Add(new Dictionary(
				"id", "ArrivalTime",
				"header", DomainRes.FlightSegment_ArrivalTime,
				"sortable", false,
				"dataIndex", "ArrivalTime",
				"width", 78,
				"renderer", new RenderDelegate(RenderDateTime)
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Duration",
				"header", DomainRes.FlightSegment_Duration,
				"sortable", false,
				"dataIndex", "Duration",
				"width", 51
			));

			columnModelConfig.Add(new Dictionary(
				"id", "FareBasis",
				"header", DomainRes.FlightSegment_FareBasis,
				"sortable", false,
				"dataIndex", "FareBasis",
				"width", 51
			));

			columnModelConfig.Add(new Dictionary(
				"id", "NumberOfStops",
				"header", DomainRes.FlightSegment_NumberOfStops,
				"sortable", false,
				"dataIndex", "NumberOfStops",
				"width", 65
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Luggage",
				"header", DomainRes.FlightSegment_Luggage,
				"sortable", false,
				"dataIndex", "Luggage",
				"width", 40
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Type",
				"header", "Тип",
				"sortable", false,
				"dataIndex", "Type",
				"width", 40
			));

			//columnModelConfig.Add(new Dictionary(
			//	"id", "MealString",
			//	"header", DomainRes.FlightSegment_Meal,
			//	"sortable", false,
			//	"dataIndex", "MealTypes",
			//	"width", 55
			//));

			return new ColumnModel(columnModelConfig);
		}

		private void CreateItem()
		{
			FormsRegistry.EditObject("FlightSegment", null, null,
				delegate (object result)
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

			FormsRegistry.EditObject("FlightSegment", data["Id"], data,
				delegate (object result) { UpdateRecord(((ItemResponse)result).Item, record); }
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


		private void Up()
		{
			if (_selectionModel.getCount() == 0) return;

			Record record = _selectionModel.getSelected();

			Dictionary data = (Dictionary)record.data;

			int position = (int)data["Position"];
			int index = -1;
			for (int i = 0; i < _items.Length; i++)
			{
				if (_items[i].Position == position)
				{
					index = i;
					break;
				}
			}

			if (index > 0)
			{
				//_items[index].Position -= 1;
				//_items[index - 1].Position += 1;

				FlightSegmentDto item = _items[index];
				FlightSegmentDto item2 = _items[index - 1];

				item.Position -= 1;
				item2.Position = item.Position + 1;

				_items[index] = item2;
				_items[index - 1] = item;

				_grid.getStore().loadData(_items);

				_isModified = true;
			}
		}

		private void Down()
		{
			if (_selectionModel.getCount() == 0) return;

			Record record = _selectionModel.getSelected();

			Dictionary data = (Dictionary)record.data;

			int position = (int)data["Position"];
			int index = -1;
			for (int i = 0; i < _items.Length; i++)
			{
				if (_items[i].Position == position)
				{
					index = i;
					break;
				}
			}

			if (index > -1 && index < _items.Length - 1)
			{
				//Log.log("index:", index);
				FlightSegmentDto item = _items[index];
				FlightSegmentDto item2 = _items[index + 1];

				item.Position += 1;
				item2.Position = item.Position - 1;

				_items[index] = item2;
				_items[index + 1] = item;

				_grid.getStore().loadData(_items);

				_isModified = true;
			}
		}


		private static object RenderDateTime(object value)
		{
			return Format.date(value, "d.m.y H:i");
		}

		private static object RenderObjectInfoToString(object value)
		{
			if (!Script.IsValue(value))
				return null;

			if (Script.IsValue(((Reference)value).Name))
				return ((Reference)value).Name;

			return ((object[])value)[1];
		}


		private FlightSegmentDto[] _items;

		private EditorGridPanel _grid;
		private JsonStore _store;
		private CheckboxSelectionModel _selectionModel;

		private LinkButton _createAction;
		private LinkButton _editAction;
		private LinkButton _deleteAction;
		private LinkButton _upAction;
		private LinkButton _downAction;

		private bool _isModified;
	}
}