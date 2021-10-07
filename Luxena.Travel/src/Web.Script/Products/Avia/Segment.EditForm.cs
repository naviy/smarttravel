//using System;
//using System.Collections;

//using Ext;
//using Ext.data;
//using Ext.form;
//using Ext.ux.form;

//using LxnBase;
//using LxnBase.Data;
//using LxnBase.Services;
//using LxnBase.UI;
//using LxnBase.UI.AutoControls;
//using LxnBase.UI.Controls;

//using Luxena.Travel.Services;

//using Field = Ext.form.Field;


//namespace Luxena.Travel
//{
//	public class SegmentEditForm : BaseEditForm
//	{
//		private SegmentEditForm(EditFormArgs args, ItemConfig config)
//		{
//			_args = args;
//			_itemConfig = config;

//			if (_args.OnSave != null)
//				Saved += _args.OnSave;

//			if (_args.OnCancel != null)
//				Canceled += _args.OnCancel; 
			
//			Window.setTitle(string.Format(Res.FlightSegment_Caption, _args.IsNew ? BaseRes.Creation : BaseRes.Editing));
//			Window.cls += " segment-edit-form";

//			Form.labelWidth = (int) (object) true;

//			_saveButton = Form.addButton(BaseRes.Save, new AnonymousDelegate(Save));
//			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

//			CreateFields();

//			Fields = new Field[]
//			{
//				_fromAirportCode.Widget,
//				_toAirportCode.Widget,
//				_carrier.Widget,
//				_flightNumber,
//				_seat,
//				_serviceClassCode,
//				_serviceClass,
//				_departureDate,
//				_departureHour,
//				_departureMinute,
//				_checkInTime,
//				_checkInTerminal,
//				_arrivalDate,
//				_arrivalHour,
//				_arrivalMinute,
//				_arrivalTerminal,
//				_numberOfStops,
//				_luggage,
//				_duration,
//				_fareBasis,
//				_stopover
//			};

//			ArrayList list = new ArrayList();

//			list.AddRange(Fields);
//			list.Add(_saveButton);
//			list.Add(_cancelButton);

//			ComponentSequence = (Component[])list;

//			Panel mainDataPanel = new Panel(new PanelConfig()
//				.items(new object[]
//				{
//					new Panel(new PanelConfig()
//						.items(new object[]
//						{
//							RowPanel( new Component[]{ _fromAirportCode.Widget, TextComponent("/"), _fromAirport}),
//							RowPanel( new Component[]{ _toAirportCode.Widget, TextComponent("/"), _toAirport}),
//							_carrier.Widget,
//							RowPanel( new Component[]{ _flightNumber, TextComponent("/"), _seat}),
//							RowPanel( new Component[]{ _serviceClassCode, TextComponent("/"), _serviceClass}),
//							RowPanel( new Component[]{_departureDate, TextComponent("/"), _departureHour, TextComponent(":"), _departureMinute}),
//							RowPanel( new Component[]{_checkInTime , TextComponent("/"), _checkInTerminal}),
//							RowPanel( new Component[]{ _arrivalDate, TextComponent("/"),  _arrivalHour, TextComponent(":"), _arrivalMinute}),
//							_arrivalTerminal
//						})
//						.cls("left").layout("form").ToDictionary()),
//					new Panel(new PanelConfig()
//						.items(new object[]
//						{
//							RowPanel( new Component[]{ _numberOfStops, TextComponent("/"), _luggage}),
//							RowPanel( new Component[]{ _duration, TextComponent("/"), _fareBasis}),
//							_stopover,
//							_meal
//						})
//						.cls("right").layout("form").ToDictionary())
//				}).layout("column")
//				.ToDictionary());

//			Form.layout = "form";
//			Form.add(mainDataPanel);
//			Form.doLayout();
//		}

//		public void SetFieldValue()
//		{
//			if (!Script.IsNullOrUndefined(_args.FieldValues["FromAirportName"]))
//			{
//				if (_fromAirport.rendered)
//					_fromAirport.getEl().update(ReadOnlyRender("FromAirportName"));
//			}
//			if (!Script.IsNullOrUndefined(_args.FieldValues["FromAirport"]))
//				_fromAirportCode.SetValue(_args.FieldValues["FromAirport"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["ToAirportName"]))
//			{
//				if (_toAirport.rendered)
//					_toAirport.getEl().update(ReadOnlyRender("ToAirportName"));
//			}
//			if (!Script.IsNullOrUndefined(_args.FieldValues["ToAirport"]))
//				_toAirportCode.SetValue(_args.FieldValues["ToAirport"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["Carrier"]))
//				_carrier.Widget.setValue(_args.FieldValues["Carrier"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["FlightNumber"]))
//				_flightNumber.setValue(_args.FieldValues["FlightNumber"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["ServiceClassCode"]))
//				_serviceClassCode.setValue(_args.FieldValues["ServiceClassCode"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["ServiceClass"]))
//				_serviceClass.setValue(_args.FieldValues["ServiceClass"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["DepartureTime"]))
//			{
//				_departureDate.setValue(_args.FieldValues["DepartureTime"]);
//				_departureHour.setValue(((Date) _args.FieldValues["DepartureTime"]).GetHours());
//				_departureMinute.setValue(((Date) _args.FieldValues["DepartureTime"]).GetMinutes());
//			}
//			if (!Script.IsNullOrUndefined(_args.FieldValues["CheckInTerminal"]))
//				_checkInTerminal.setValue(_args.FieldValues["CheckInTerminal"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["CheckInTime"]))
//				_checkInTime.setValue(_args.FieldValues["CheckInTime"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["ArrivalTime"]))
//			{
//				_arrivalDate.setValue(_args.FieldValues["ArrivalTime"]);
//				_arrivalHour.setValue(((Date)_args.FieldValues["ArrivalTime"]).GetHours());
//				_arrivalMinute.setValue(((Date)_args.FieldValues["ArrivalTime"]).GetMinutes());
//			}
//			if (!Script.IsNullOrUndefined(_args.FieldValues["ArrivalTerminal"]))
//				_arrivalTerminal.setValue(_args.FieldValues["ArrivalTerminal"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["MealValue"]))
//				_meal.setValue(_args.FieldValues["MealValue"].ToString());
//			if (!Script.IsNullOrUndefined(_args.FieldValues["NumberOfStops"]))
//				_numberOfStops.setValue(_args.FieldValues["NumberOfStops"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["Luggage"]))
//				_luggage.setValue(_args.FieldValues["Luggage"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["Duration"]))
//				_duration.setValue(_args.FieldValues["Duration"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["Seat"]))
//				_seat.setValue(_args.FieldValues["Seat"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["FareBasis"]))
//				_fareBasis.setValue(_args.FieldValues["FareBasis"]);
//			if (!Script.IsNullOrUndefined(_args.FieldValues["Stopover"]))
//				_stopover.setValue(_args.FieldValues["Stopover"]);
//		}

//		private static BoxComponent TextComponent(string html)
//		{
//			return new BoxComponent(new BoxComponentConfig()
//				.autoEl(new Dictionary("tag", "div", "html", html))
//				.cls("x-form-item float-left box-label")
//				.ToDictionary());
//		}

//		private static Panel RowPanel(Component[] items)
//		{
//			return new Panel(new PanelConfig()
//				.layout("form")
//				.itemCls("float-left")
//				.items(items)
//				.width(350)
//				.ToDictionary());
//		}

//		private string ReadOnlyRender(string field)
//		{
//			return _args.FieldValues[field].ToString();
//		}

//		private void CreateFields()
//		{
//			_fromAirport = new BoxComponent( new BoxComponentConfig()
//											.cls("x-form-item")
//											.autoEl(new Dictionary("tag", "div"))
//											.ToDictionary());

//			_fromAirportCode = new ObjectSelector(new ObjectSelectorConfig().setClass("Airport"));
//			_fromAirportCode.Widget.allowBlank = false;
//			_fromAirportCode.Widget.name = "FromAirport";
//			_fromAirportCode.Widget.width = 50;
//			_fromAirportCode.Widget.emptyText = DomainRes.FlightSegment_FromAirportCode;
//			_fromAirportCode.Widget.fieldLabel = DomainRes.FlightSegment_FromAirportName;
//			_fromAirportCode.Widget.addListener("change", new FieldChangeDelegate(
//				delegate(Field objthis, object value, object oldValue)
//				{
//					if (value == null)
//						return;

//					AviaService.FindAirportNameById(((Array)value)[0].ToString(),
//						delegate(object result)
//						{
//							result = result ?? string.Empty;
//							_args.FieldValues["FromAirportName"] = result;
//							if (_fromAirport.rendered)
//								_fromAirport.getEl().update(ReadOnlyRender("FromAirportName"));
//							else
//								_fromAirport.autoEl = new Dictionary("html", ReadOnlyRender("FromAirportName")); 
//						}, null);
//				}));

//			_toAirport = new BoxComponent(new BoxComponentConfig()
//											.cls("x-form-item")
//											.autoEl(new Dictionary("tag", "div"))
//											.ToDictionary()); 
//			_toAirportCode = new ObjectSelector(new ObjectSelectorConfig().setClass("Airport"));
//			_toAirportCode.Widget.allowBlank = false;
//			_toAirportCode.Widget.width = 50;
//			_toAirportCode.Widget.name = "ToAirport";
//			_toAirportCode.Widget.emptyText = DomainRes.FlightSegment_FromAirportCode;
//			_toAirportCode.Widget.fieldLabel = DomainRes.FlightSegment_ToAirportName;
//			_toAirportCode.Widget.addListener("change", new FieldChangeDelegate(
//				delegate(Field objthis, object value, object oldValue)
//				{
//					if (value == null)
//						return;

//					AviaService.FindAirportNameById(((Array)value)[0].ToString(),
//						delegate(object result)
//						{
//							result = result ?? string.Empty;
//							_args.FieldValues["ToAirportName"] = result;
//							if (_toAirport.rendered)
//								_toAirport.getEl().update(ReadOnlyRender("ToAirportName"));
//							else
//								_toAirport.autoEl = new Dictionary("html", ReadOnlyRender("ToAirportName")); 
//						}, null);
//				}));

//			_carrier = new ObjectSelector(new ObjectSelectorConfig().setClass("Airline"));
//			_carrier.Widget.fieldLabel = DomainRes.Airline;
//			_carrier.Widget.width = 205;
//			_carrier.Widget.name = "Carrier";
//			_carrier.Widget.emptyText = DomainRes.Airline;
			
//			_flightNumber = ControlFactory.CreateEditor(GetItemConfig("FlightNumber"));
//			_flightNumber.width = 50;
//			_flightNumber.fieldLabel = string.Format("{0} / {1}", DomainRes.FlightSegment_FlightNumber, DomainRes.FlightSegment_Seat);

//			_serviceClassCode = ControlFactory.CreateEditor(GetItemConfig("ServiceClassCode"));
//			_serviceClassCode.fieldLabel = string.Format("{0} / {1}", DomainRes.Common_Code, DomainRes.FlightSegment_ServiceClass);
//			_serviceClassCode.width = 50;
//			_serviceClass = ControlFactory.CreateEditor(GetItemConfig("ServiceClass"));
//			_serviceClass.width = 140;
//			_serviceClass.hideLabel = true;

//			_departureDate = ControlFactory.CreateEditor(GetItemConfig("DepartureTime"));

//			_departureHour = new TextField(GetItemConfig("DepartureTime"));
//			_departureHour.name = "DepartureTime_1";
//			_departureHour.width = 30;
//			_departureHour.hideLabel = true;
			
//			_departureMinute = new TextField(GetItemConfig("DepartureTime"));
//			_departureMinute.name = "DepartureTime_2";
//			_departureMinute.width = 30;
//			_departureMinute.hideLabel = true;

//			_checkInTerminal = ControlFactory.CreateEditor(GetItemConfig("CheckInTerminal"));
//			_checkInTerminal.hideLabel= true;
//			_checkInTerminal.width = 94;

//			_checkInTime = new TextField(GetItemConfig("CheckInTime"));
//			_checkInTime.fieldLabel = string.Format("{0} /<br/>{1}", DomainRes.FlightSegment_CheckInTime, DomainRes.FlightSegment_CheckInTerminal);
//			_checkInTime.width = 95;
//			_checkInTime.name = "CheckInTime";
//			_checkInTime.hideLabel = false;

//			_arrivalDate = ControlFactory.CreateEditor(GetItemConfig("ArrivalTime"));

//			_arrivalHour = new TextField(GetItemConfig("ArrivalTime"));
//			_arrivalHour.name = "ArrivalTime_1";
//			_arrivalHour.width = 30;
//			_arrivalHour.hideLabel = true;

//			_arrivalMinute = new TextField(GetItemConfig("ArrivalTime"));
//			_arrivalMinute.name = "ArrivalTime_2";
//			_arrivalMinute.width = 30;
//			_arrivalMinute.hideLabel = true;

//			_arrivalTerminal = ControlFactory.CreateEditor(GetItemConfig("ArrivalTerminal"));
//			_arrivalTerminal.width = 95;

//			_meal = new MultiSelect();
//			_meal.store = new ArrayStore(new ArrayStoreConfig()
//				.fields(new string[] { "Id", "Name" })
//				.data(((ListColumnConfig)GetItemConfig("MealTypes")).Items)
//				.ToDictionary());
//			_meal.xtype = "multiselect";
//			_meal.width = 205;
//			_meal.name = "MealTypes";
//			_meal.height = 175;
//			_meal.displayField = "Name";
//			_meal.valueField = "Id";
//			_meal.fieldLabel = DomainRes.FlightSegment_Meal;
//			_meal.renderTo = _meal.id;
//			_meal.tbar = new ToolbarConfig().items(new object[] { new Ext.ActionConfig().text(Res.Clear).handler(new AnonymousDelegate(ClearData)).ToDictionary() }).ToDictionary();

//			_numberOfStops = ControlFactory.CreateEditor(GetItemConfig("NumberOfStops"));
//			_numberOfStops.width = 80;
//			_numberOfStops.fieldLabel = string.Format("{0} / {1}", DomainRes.FlightSegment_NumberOfStops, DomainRes.FlightSegment_Luggage);
//			_luggage = ControlFactory.CreateEditor(GetItemConfig("Luggage"));
//			_luggage.hideLabel = true;
//			_luggage.width = 80;

//			_duration = new TextField(GetItemConfig("Duration"));
//			_duration.fieldLabel = string.Format("{0} / {1}", DomainRes.FlightSegment_Duration, DomainRes.FlightSegment_FareBasis);
//			_duration.width = 80;
//			_duration.name = "Duration";

//			_seat = ControlFactory.CreateEditor(GetItemConfig("Seat"));
//			_seat.hideLabel = true;

//			_fareBasis = ControlFactory.CreateEditor(GetItemConfig("FareBasis"));
//			_fareBasis.width = 80;
//			_fareBasis.hideLabel = true;

//			_stopover = ControlFactory.CreateEditor(GetItemConfig("Stopover"));
//		}
		
//		private void ClearData()
//		{
//			_meal.reset();
//		}

//		//TODO: Create TimeControl for time values and instantiate it with this method.
//		//private TimeField CreateTimeField(string columnConfig)
//		//{
//		//    TimeField result = new TimeField(GetItemConfig(columnConfig));
//		//    result.width = 80;
//		//    result.format = "H:i";
//		//    result.increment = 1;
//		//    result.hideLabel = true;
//		//    return result;
//		//}

//		private ColumnConfig GetItemConfig(string name)
//		{
//			foreach (ColumnConfig col in _itemConfig.Columns)
//				if (col.Name == name)
//					return col;

//			return null;
//		}

//		public static void EditObject(EditFormArgs args)
//		{
//			ConfigManager.GetEditConfig(args.Type,
//				delegate(ItemConfig config)
//				{
//					SegmentEditForm form = new SegmentEditForm(args, config);
//					form.Open();
//					form.SetFieldValue();
//				});
//		}

//		protected override void OnSave()
//		{
//			Dictionary data = GetModifiedData();
			
//			if (Script.IsValue(((AviaTicketDto)_args.FieldValues["Ticket"]).Id))
//			{
//				//if (data.Count != 0)
//				//	AviaService.UpdateFlightSegment(
//				//		((AviaTicketDto)_args.FieldValues["Ticket"]).Id, 
//				//		data, 
//				//		delegate(object result) { CompleteSave(ItemResponse.Create(result)); }, 
//				//		null
//				//	);
//				//else
//				//	Cancel();
//			}
//			else if (Script.IsValue(_args.IsCopy))
//			{
//				CompleteSave(ItemResponse.Create(data));
//			}
//		}

//		protected override void OnSaved(object result)
//		{
//			if (Script.IsNullOrUndefined(_args.RangeRequest))
//				MessageRegister.Info(_itemConfig.ListCaption, (_args.IsNew ? BaseRes.Created : BaseRes.Updated) + " " + Type.GetField(result, ObjectPropertyNames.Reference));

//			base.OnSaved(result);
//		}

//		private Dictionary GetModifiedData()
//		{
//			Dictionary data = new Dictionary();
//			data["Id"] = _args.Id;
//			data["Position"] = _args.FieldValues["Position"];

//			Date departureDate = (Date) _departureDate.getValue();
//			if (!string.IsNullOrEmpty(_departureHour.getValue().ToString()))
//				departureDate.SetHours((int)_departureHour.getValue());
//			if (!string.IsNullOrEmpty(_departureMinute.getValue().ToString()))
//				departureDate.SetMinutes((int)_departureMinute.getValue());
//			data["DepartureTime"] = departureDate;

//			Date arrivalDate = (Date)_arrivalDate.getValue();
//			if (!string.IsNullOrEmpty(_arrivalHour.getValue().ToString()))
//				arrivalDate.SetHours((int)_arrivalHour.getValue());
//			if (!string.IsNullOrEmpty(_arrivalMinute.getValue().ToString()))
//				arrivalDate.SetMinutes((int)_arrivalMinute.getValue());
//			data["ArrivalTime"] = arrivalDate;
			
//			data["Carrier"] = _carrier.Widget.getValue();

//			data["MealValue"] = _meal.getValue()[0];
//			data["MealString"] = _meal.getValue()[0];

//			GetModifiedValue(_fromAirportCode.Widget, data);
//			GetModifiedValue(_toAirportCode.Widget, data);
//			GetModifiedValue(_carrier.Widget, data);
//			GetModifiedValue(_flightNumber, data);
//			GetModifiedValue(_serviceClassCode, data);
//			GetModifiedValue(_serviceClass, data);
//			GetModifiedValue(_arrivalTerminal, data);
//			GetModifiedValue(_numberOfStops, data);
//			GetModifiedValue(_luggage, data);
//			GetModifiedValue(_checkInTerminal, data);
//			GetModifiedValue(_checkInTime, data);
//			GetModifiedValue(_duration, data);
//			GetModifiedValue(_seat, data);
//			GetModifiedValue(_fareBasis, data);
//			GetModifiedValue(_stopover, data);
//			return data;
//		}

//		private void GetModifiedValue(Field field, Dictionary modified)
//		{
//			modified[field.name] = field.getValue();

//			/*object value = field.getValue();
			
//			if (value is string && string.IsNullOrEmpty((string)value))
//				value = null;

//			object initialValue = _args != null && _args.FieldValues != null && _args.FieldValues.ContainsKey(field.name) ? _args.FieldValues[field.name] : null;

//			if (_args == null || (bool)Type.InvokeMethod(field, "isDirty") && value != initialValue)
//				modified[field.name] = value;*/
//		}

//		private void Open()
//		{
//			Window.show();
//			Fields[0].focus(true, true);
//		}

//		private BoxComponent _fromAirport;
//		private ObjectSelector _fromAirportCode;
//		private BoxComponent _toAirport;
//		private ObjectSelector _toAirportCode;
//		private ObjectSelector _carrier;
//		private Field _flightNumber;
//		private Field _serviceClassCode;
//		private Field _serviceClass;
//		private Field _departureDate;
//		private Field _departureHour;
//		private Field _departureMinute;
//		private Field _arrivalDate;
//		private Field _arrivalHour;
//		private Field _arrivalMinute;
//		private MultiSelect _meal;
//		private Field _numberOfStops;
//		private Field _luggage;
//		private Field _checkInTerminal;
//		private Field _checkInTime;
//		private Field _duration;
//		private Field _arrivalTerminal;
//		private Field _seat;
//		private Field _fareBasis;
//		private Field _stopover;
//		private readonly Button _saveButton;
//		private readonly Button _cancelButton;
//		private readonly EditFormArgs _args;
//		private readonly ItemConfig _itemConfig;
//	}
//}