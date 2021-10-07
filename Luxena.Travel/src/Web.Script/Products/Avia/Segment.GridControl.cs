//using System;
//using System.Collections;
//using System.Runtime.CompilerServices;

//using Ext;
//using Ext.data;
//using Ext.grid;
//using Ext.util;

//using LxnBase;
//using LxnBase.Data;
//using LxnBase.Services;
//using LxnBase.UI;
//using LxnBase.UI.AutoControls;

//using Luxena.Travel.Controls;
//using Luxena.Travel.Services;

//using Record = Ext.data.Record;


//namespace Luxena.Travel
//{

//	public class SegmentGridControl
//	{

//		public SegmentGridControl(double height)
//		{
//			Initialize(height);
//		}

//		public bool IsModified()
//		{
//			return _isModified;
//		}

//		public void Initialize(double height)
//		{
//			_grid = new EditorGridPanel(new EditorGridPanelConfig()
//				.store(InitStore())
//				.cls("segments-grid")
//				.cm(InitColumnModel())
//				.tbar(InitToolbar())
//				.sm(InitSelectionModel())
//				.stripeRows(true)
//				.enableHdMenu(false)
//				.enableColumnResize(true)
//				.enableColumnMove(false)
//				.autoScroll(true)
//				.clicksToEdit(2)
//				.collapsible(false)
//				.frame(false)
//				.height(height)
//				.ToDictionary());
//			_grid.on("rowdblclick", new AnonymousDelegate(EditItem));
//			_grid.on("render", new AnonymousDelegate(
//				delegate
//				{
//					_grid.getEl().on("keydown", new Action<EventObject>(OnGridKeyDown));
//				}));
//		}

//		public FlightSegmentDto[] GetFlightSegments()
//		{
//			FlightSegmentDto[] segments = new FlightSegmentDto[_store.data.Length];
//			int index = 0;
//			for (int i = 0; i < _store.data.Length; i++)
//			{
//				Record record = _store.getAt(i);
//				segments[i] = new FlightSegmentDto();
//				object id = _store.getAt(i).get("Id");
//				segments[i].Id = id == null ? null : id.ToString();
//				segments[i].Position = index;
//				if ((string)record.get("StateInfo") == "D")
//				{
//					segments[i].StateInfo = "D";
//					continue;
//				}
//				index++;
//				segments[i].StateInfo = "U";
//			}

//			return segments;
//		}

//		private void OnGridKeyDown(EventObject e)
//		{
//			int key = e.getKey();

//			bool isAdditionalKey = e.ctrlKey || e.shiftKey || e.altKey;
//			bool isCtrlKey = e.ctrlKey && !e.shiftKey && !e.altKey;

//			if (key == EventObject.F2 && !isAdditionalKey)
//			{
//				if (_selectionModel.getSelections().Length != 1)
//					return;
//				e.stopEvent();
//				EditItem();
//			}
//			else if (key == EventObject.ESC)
//			{
//				e.stopEvent();
//				SetGridFocus();
//			}
//			else if (key == EventObject.INSERT && !isAdditionalKey)
//			{
//				e.stopEvent();
//				CreateItem();
//			}
//			else if (key == EventObject.DELETE && isCtrlKey)
//			{
//				e.stopEvent();
//				RemoveItem();
//			}
//			else if (key == EventObject.ENTER && !e.ctrlKey && e.shiftKey && !e.altKey)
//			{
//				e.stopEvent();
//				EditItem();
//			}
//			else if (key == EventObject.HOME && !isAdditionalKey)
//			{
//				e.stopEvent();
//				SetGridFocus(0);
//			}
//			else if (key == EventObject.END && !isAdditionalKey)
//			{
//				e.stopEvent();
//				SetGridFocus(_store.getCount() - 1);
//			}
//		}

//		[AlternateSignature]
//		private extern void SetGridFocus();

//		private void SetGridFocus(int selectRowIndex)
//		{
//			if (_store.getCount() == 0)
//				return;

//			_grid.getView().focusRow(0);

//			if (!Script.IsNullOrUndefined(selectRowIndex))
//			{
//				int count = _store.getCount();
//				int index = selectRowIndex < count ? selectRowIndex : count - 1;

//				_selectionModel.selectRow(index);
//			}
//		}

//		private Store InitStore()
//		{
//			_store = new JsonStore(new JsonStoreConfig()
//				.fields(new string[] { "Id", "FromAirport", "FromAirportName", "ToAirport", "ToAirportName", "CarrierCode", "Carrier", "FlightNumber", "ServiceClassCode", "ServiceClass", "DepartureTime", "ArrivalTime", "MealValue", "MealString", "NumberOfStops", "Luggage", "CheckInTerminal", "CheckInTime", "Duration", "ArrivalTerminal", "Seat", "FareBasis", "Stopover", "Position", "StateInfo" })
//				.listeners(new Dictionary("update", new StoreUpdateDelegate(
//					delegate (Store store, Record record, string operation)
//					{
//						if (operation == Record.EDIT)
//						{
//							record.commit();
//							_isModified = true;
//						}
//					})))
//				.ToDictionary());
//			return _store;
//		}

//		private AbstractSelectionModel InitSelectionModel()
//		{
//			_selectionModel = new CheckboxSelectionModel();
//			_selectionModel.singleSelect = false;

//			_selectionModel.on("selectionchange", new SelectionChangedDelegate(delegate
//			{
//				if (_selectionModel.getCount() == 0)
//				{
//					_deleteAction.disable();
//					_upAction.disable();
//					_downAction.disable();
//				}
//				else
//				{
//					_deleteAction.enable();
//					Record record = _selectionModel.getSelected();
//					_upAction.enable();
//					_downAction.enable();

//					int position = (int)((Dictionary)record.data)["Position"];
//					if (position == 0)
//						_upAction.disable();
//					if (position == _ticket.Segments.Length - 1)
//						_downAction.disable();
//				}

//				if (_selectionModel.getCount() != 1)
//					_editAction.disable();
//				else
//					_editAction.enable();
//			}));

//			return _selectionModel;
//		}

//		private static ColumnModel InitColumnModel()
//		{
//			ArrayList columnModelConfig = new ArrayList();

//			columnModelConfig.Add(new Dictionary(
//				"id", "FromAirport",
//				"header", DomainRes.FlightSegment_FromAirportName,
//				"sortable", false,
//				"dataIndex", "FromAirport",
//				"width", 80,
//				"renderer", new RenderDelegate(RenderObjectInfoToString)
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "ToAirport",
//				"header", DomainRes.FlightSegment_ToAirportName,
//				"sortable", false,
//				"dataIndex", "ToAirport",
//				"width", 68,
//				"renderer", new RenderDelegate(RenderObjectInfoToString)
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "Carrier",
//				"header", DomainRes.FlightSegment_Carrier,
//				"sortable", false,
//				"dataIndex", "Carrier",
//				"width", 72,
//				"renderer", new RenderDelegate(RenderObjectInfoToString)
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "FlightNumber",
//				"header", DomainRes.FlightSegment_FlightNumber,
//				"sortable", false,
//				"dataIndex", "FlightNumber",
//				"width", 33
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "ServiceClassCode",
//				"header", DomainRes.FlightSegment_ServiceClass,
//				"sortable", false,
//				"dataIndex", "ServiceClassCode",
//				"width", 40
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "Seat",
//				"header", DomainRes.FlightSegment_Seat,
//				"sortable", false,
//				"dataIndex", "Seat",
//				"width", 40
//				));

//			DateTimeColumnConfig dateTimeConfig = new DateTimeColumnConfig();
//			dateTimeConfig.Type = TypeEnum.Date;

//			columnModelConfig.Add(new Dictionary(
//				"id", "DepartureTime",
//				"header", DomainRes.FlightSegment_DepartureTime,
//				"sortable", false,
//				"dataIndex", "DepartureTime",
//				"width", 78,
//				"renderer", new RenderDelegate(RenderDateTime)
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "CheckInTime",
//				"header", Res.FlightSegment_Registration,
//				"sortable", false,
//				"dataIndex", "CheckInTime",
//				"width", 75
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "ArrivalTime",
//				"header", DomainRes.FlightSegment_ArrivalTime,
//				"sortable", false,
//				"dataIndex", "ArrivalTime",
//				"width", 78,
//				"renderer", new RenderDelegate(RenderDateTime)
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "Duration",
//				"header", DomainRes.FlightSegment_Duration,
//				"sortable", false,
//				"dataIndex", "Duration",
//				"width", 51
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "FareBasis",
//				"header", DomainRes.FlightSegment_FareBasis,
//				"sortable", false,
//				"dataIndex", "FareBasis",
//				"width", 51
//			));

//			columnModelConfig.Add(new Dictionary(
//				"id", "NumberOfStops",
//				"header", DomainRes.FlightSegment_NumberOfStops,
//				"sortable", false,
//				"dataIndex", "NumberOfStops",
//				"width", 65
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "Luggage",
//				"header", DomainRes.FlightSegment_Luggage,
//				"sortable", false,
//				"dataIndex", "Luggage",
//				"width", 40
//				));

//			columnModelConfig.Add(new Dictionary(
//				"id", "MealString",
//				"header", DomainRes.FlightSegment_Meal,
//				"sortable", false,
//				"dataIndex", "MealString",
//				"width", 55
//				));

//			return new ColumnModel(columnModelConfig);
//		}

//		private static object RenderDateTime(object value)
//		{
//			return Format.date(value, "d.m.y H:i");
//		}

//		private static object RenderObjectInfoToString(object value)
//		{
//			if (!Script.IsValue(value))
//				return null;

//			if (Script.IsValue(((Reference)value).Name))
//				return ((Reference)value).Name;

//			return ((object[])value)[1];
//		}

//		private Toolbar InitToolbar()
//		{
//			_upAction = new LinkButton(new ButtonConfig().cls("upbtn").height(16).disabled(true).handler(new AnonymousDelegate(Up)));
//			_downAction = new LinkButton(new ButtonConfig().cls("downbtn").height(16).disabled(true).handler(new AnonymousDelegate(Down)));
//			_createAction = new LinkButton(new ButtonConfig().text(BaseRes.CreateItem_Lower).handler(new AnonymousDelegate(CreateItem)));
//			_editAction = new LinkButton(new ButtonConfig().text(BaseRes.Edit_Lower).handler(new AnonymousDelegate(EditItem)).disabled(true));
//			_deleteAction = new LinkButton(new ButtonConfig().text(BaseRes.Remove_Lower).handler(new AnonymousDelegate(RemoveItem)).disabled(true));

//			_createAction.disable();
//			_deleteAction.disable();
//			_upAction.disable();
//			_downAction.disable();

//			return new Toolbar(new object[] { "->", _upAction, _downAction, _createAction, _editAction, _deleteAction });
//		}

//		private void Up()
//		{
//			if (_selectionModel.getCount() == 0)
//				return;

//			Record record = _selectionModel.getSelected();

//			Dictionary data = (Dictionary)record.data;

//			int position = (int)data["Position"];
//			int index = -1;
//			foreach (FlightSegmentDto dto in _ticket.Segments)
//			{
//				if (dto.Position == position)
//				{
//					index = _ticket.Segments.IndexOf(dto);
//					break;
//				}
//			}
//			if (index > 0)
//			{
//				_ticket.Segments[index].Position -= 1;
//				_ticket.Segments[index].StateInfo = "U";
//				_ticket.Segments[index - 1].Position += 1;
//				_ticket.Segments[index - 1].StateInfo = "U";
//				FlightSegmentDto[] param = new FlightSegmentDto[2];
//				param[0] = _ticket.Segments[index - 1];
//				param[1] = _ticket.Segments[index];
//				Dictionary.GetDictionary(_selectionModel)["lastActive"] = (int)Dictionary.GetDictionary(_selectionModel)["lastActive"] - 1;
//				AviaService.UpdateFlightSegments(_ticket.Id, param, UpdateOnPositionChanged, null);
//			}
//		}

//		private void Down()
//		{
//			if (_selectionModel.getCount() == 0)
//				return;

//			Record record = _selectionModel.getSelected();

//			Dictionary data = (Dictionary)record.data;

//			int position = (int)data["Position"];
//			int index = -1;
//			foreach (FlightSegmentDto dto in _ticket.Segments)
//			{
//				if (dto.Position == position)
//				{
//					index = _ticket.Segments.IndexOf(dto);
//					break;
//				}
//			}
//			if (index > -1 && index < _ticket.Segments.Length - 1)
//			{
//				_ticket.Segments[index].Position += 1;
//				_ticket.Segments[index].StateInfo = "U";
//				_ticket.Segments[index + 1].Position -= 1;
//				_ticket.Segments[index + 1].StateInfo = "U";
//				FlightSegmentDto[] param = new FlightSegmentDto[2];
//				param[0] = _ticket.Segments[index];
//				param[1] = _ticket.Segments[index + 1];
//				Dictionary.GetDictionary(_selectionModel)["lastActive"] = (int)Dictionary.GetDictionary(_selectionModel)["lastActive"] + 1;
//				AviaService.UpdateFlightSegments(_ticket.Id, param, UpdateOnPositionChanged, null);
//			}
//		}

//		private void UpdateOnPositionChanged(object results)
//		{
//			AviaTicketDto dto = (AviaTicketDto)results;
//			if (Script.IsNullOrUndefined(dto))
//				return;
//			int rowIndex = (int)Dictionary.GetDictionary(_selectionModel)["lastActive"];
//			_grid.getStore().removeAll();
//			LoadData(dto);
//			SetGridFocus(rowIndex);
//		}

//		private void CreateItem()
//		{
//			Dictionary data = new Dictionary();

//			Reference ticket = new Reference();
//			ticket.Id = _ticket.Id;
//			ticket.Name = _ticket.ToString();

//			data["Ticket"] = ticket;
//			data["Position"] = Script.IsValue(_store.data) ? _store.data.Length : 0;

//			FormsRegistry.EditObject(
//				"FlightSegment", null, data,
//				delegate (object result)
//				{
//					Record record = new Record();
//					//Log.Add(result);
//					record.data = ((ItemResponse)result).Item;
//					//record.data = result;
//					record.fields = _store.fields;

//					_store.add(new Record[] { record });
//					_isModified = true;
//				},
//				null, null
//			);
//		}

//		private void EditItem()
//		{
//			if (_selectionModel.getCount() == 0)
//				return;

//			Record record = _selectionModel.getSelected();

//			Dictionary data = (Dictionary)record.data;

//			Reference ticket = new Reference();
//			ticket.Id = _ticket.Id;
//			ticket.Name = _ticket.ToString();

//			data["Ticket"] = ticket;

//			FormsRegistry.EditObject("FlightSegment", data["Id"], data,
//				delegate (object result)
//				{
//					UpdateRecord(result, record);
//					_isModified = true;
//				}
//				, null);
//		}

//		private void RemoveItem()
//		{
//			if (_selectionModel.getCount() == 0)
//				return;
//			MessageBoxWrap.Confirm(BaseRes.Confirmation, StringUtility.GetNumberText(_selectionModel.getSelections().Length, BaseRes.AutoGrid_DeleteMsg1, BaseRes.AutoGrid_DeleteMsg2, BaseRes.AutoGrid_DeleteMsg3),
//					delegate (string button, string text)
//					{
//						if (button == "yes")
//						{
//							Array selected = _selectionModel.getSelections();
//							FlightSegmentDto[] segments = new FlightSegmentDto[_store.data.Length];
//							for (int i = 0; i < selected.Length; i++)
//							{
//								((Record)selected[i]).set("StateInfo", "D");
//							}
//							int index = 0;
//							for (int i = 0; i < _store.data.Length; i++)
//							{
//								Record record = _store.getAt(i);
//								segments[i] = new FlightSegmentDto();
//								segments[i].Id = _store.getAt(i).get("Id").ToString();
//								segments[i].Position = index;
//								if ((string)record.get("StateInfo") == "D")
//								{
//									segments[i].StateInfo = "D";
//									continue;
//								}
//								index++;
//								segments[i].StateInfo = "U";
//							}
//							AviaService.UpdateFlightSegments(_ticket.Id, segments, UpdateStore, null);
//							_isModified = true;
//						}
//						else
//							SetGridFocus();
//					});
//		}

//		private void UpdateStore(object results)
//		{
//			AviaTicketDto dto = (AviaTicketDto)results;
//			if (Script.IsNullOrUndefined(dto))
//				return;
//			int rowIndex = (int)Dictionary.GetDictionary(_selectionModel)["lastActive"];
//			_grid.getStore().removeAll();
//			LoadData(dto);
//			MessageRegister.Info(DomainRes.FlightSegment, BaseRes.Deleted);
//			SetGridFocus(rowIndex);
//		}

//		internal void LoadData(AviaTicketDto ticket)
//		{
//			_ticket = ticket;
//			if (_ticket == null || _ticket.Segments == null) return;

//			_ticket.Segments.Sort(SegmentComparer);

//			_grid.getStore().loadData(_ticket.Segments);
//			_grid.getStore().sort("Position", "ASC");

//			_createAction.enable();
//		}

//		private static int SegmentComparer(object x, object y)
//		{
//			FlightSegmentDto dx = (FlightSegmentDto)x;
//			FlightSegmentDto dy = (FlightSegmentDto)y;
//			return dx.Position - dy.Position;
//		}

//		private static void UpdateRecord(object obj, Record record)
//		{
//			Field[] recordFields = (Field[])record.fields.getRange();

//			for (int i = 0; i < recordFields.Length; i++)
//			{
//				string name = recordFields[i].name;

//				object value = Type.GetField(obj, name);

//				if (!Script.IsUndefined(value))
//					Type.SetField(record.data, name, value);
//			}

//			record.commit();
//		}

//		public Component Widget
//		{
//			get { return _grid; }
//		}

//		private AviaTicketDto _ticket;

//		private EditorGridPanel _grid;
//		private JsonStore _store;
//		private CheckboxSelectionModel _selectionModel;
//		private bool _isModified;

//		private LinkButton _upAction;
//		private LinkButton _downAction;
//		private LinkButton _createAction;
//		private LinkButton _editAction;
//		private LinkButton _deleteAction;
//	}

//}