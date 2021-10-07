using System;
using System.Collections;

using Ext;
using Ext.data;

using jQueryApi;

using LxnBase.Data;
using LxnBase.Services;

using Action = Ext.Action;
using Element = Ext.Element;
using Field = Ext.form.Field;
using Record = Ext.data.Record;
using Window = System.Html.Window;


namespace LxnBase.UI.Controls
{
	public delegate bool ComboBoxBeforeSelectDelegate(ComboBox comboBox, Record record, Number index);

	public delegate void ComboBoxCustomActionDelegate(ObjectSelector selector, string text);

	public class ObjectSelector
	{
		public ObjectSelector(ObjectSelectorConfig objectSelectorConfig)
		{
			_config = objectSelectorConfig;
			_config.mode("remote");

			Render();
		}

		public ComboBox Widget
		{
			get { return _comboBox; }
		}

		public ObjectSelectorConfig Config
		{
			get { return _config; }
		}

		public string Text
		{
			get { return (string) _comboBox.getRawValue(); }
		}

		public Array GetValue()
		{
			return _comboBox.getValue();
		}

		public Reference GetObjectInfo()
		{
			return _comboBox.GetObjectInfo();
		}

		public object GetObjectId()
		{
			Reference info = _comboBox.GetObjectInfo();
			return info != null ? info.Id : null;
		}

		public object GetObjectName()
		{
			Reference info = _comboBox.GetObjectInfo();
			return info != null ? info.Name : null;
		}

		public void SetValue(object value)
		{
			_comboBox.setValue(value);
		}

		public void Focus()
		{
			_comboBox.focus();
		}

		private void Render()
		{
			ArrayList recordType = new ArrayList();

			recordType.Add(ObjectPropertyNames.Id);
			recordType.Add(ObjectPropertyNames.Name);
			recordType.Add(ObjectPropertyNames.Type);

			if (_config.ValueProperties != null)
			{
				foreach (string t in _config.ValueProperties)
					recordType.Add(t);
			}

			if (_config.Store == null)
			{
				DataProxy proxy = _config.DataProxy ?? GenericService.SuggestProxy(_config.Clazz);
				Dictionary metadata = _config.ReaderMetadata ?? new Dictionary(
					"id", 0,
					"root", "List",
					"totalProperty", "TotalCount"
				);

				Store store = new Store(new StoreConfig()
					.proxy(proxy)
					.reader(new RangeReader(metadata, recordType))
					.ToDictionary());

				store.on("beforeload", new StoreBeforeloadDelegate(OnBeforeLoadData));
				store.on("load", new StoreLoadDelegate(OnLoadData));

				_config.store(store);
			}

			ArrayList parts = new ArrayList();
			parts.Add("<tpl for=\".\">");
			parts.Add("<div class=\"{[values." + _actionPropertyName + " ? \"x-combo-list-item combo-list-action\" : values." + _isCaptionPropertyName + " ? \"x-combo-list-item combo-list-caption\" : \"x-combo-list-item\"]}\">{" +
				ObjectPropertyNames.Name + "}</div>");
			parts.Add("</tpl>");

			XTemplate template = new XTemplate(parts);
			_config.tpl(template);

			_config.triggerClass("selector-trigger");

			Dictionary dictionary = _config.ToDictionary();

			string css = (string) dictionary["cls"] ?? string.Empty;

			dictionary["cls"] ="selector " + css;

			_comboBox = new ComboBox(dictionary);

			if (_comboBox.minListWidth < _minListWidth)
				_comboBox.minListWidth = _minListWidth;

			_comboBox.on("beforeselect", new ComboBoxBeforeSelectDelegate(OnBeforeSelectItem));
			_comboBox.on("render", new AnonymousDelegate(OnComboBoxRender));
			_comboBox.on("keydown", new Action<object, EventObject>(OnKeyDown));
			_comboBox.on("beforequery", new ComboBoxBeforequeryDelegate(OnBeforeQuery));
			_comboBox.on("changeValue", new FieldChangeDelegate(OnChangeValue));

			//if (store != null)
			//{
			//	Type.SetField(store.baseParams, "query", "*");
			//	store.load();
			//}
		}

		private void CreateEditButton()
		{
			if (!_config.AllowEdit)
				return;

			Element wrap = (Element) Type.GetField(_comboBox, "wrap");
			_editButton = wrap.createChild(new Dictionary("tag", "div", "cls", "selectorEdit"));

//			_comboBox.width -= 20;
//			wrap.addClass("combobox-with-edit");

			_editButton.on("click", new AnonymousDelegate(OnEditClick));

			if (_comboBox.value == null)
				_editButton.addClass("selectorEditDisabled");
		}

		public void UpdateTriggerStatus()
		{
			Element trigger = _comboBox.GetTrigger();

			if (trigger == null)
				return;

			bool disabled = false;

			if (string.IsNullOrEmpty((string) _comboBox.value))
			{
				Action[] actions = _config.GetActions((Store) _comboBox.store, null, null);

				if (actions == null || actions.Length == 0)
					disabled = true;
			}

			if (disabled)
			{
				trigger.hide();

				jQueryObject disableTrigger = jQuery.FromHtml("<div class='tigger-disabled'></div>");
				disableTrigger.InsertBefore(jQuery.FromElement(trigger.dom));
			}
			else
			{
				jQuery.Select(".tigger-disabled", jQuery.FromElement(trigger.dom.ParentNode)).Remove();
				trigger.show();
			}
		}

		private void OnComboBoxRender()
		{
			CreateEditButton();

			UpdateTriggerStatus();
		}

		private void OnBeforeQuery(object e)
		{
			string query = (string) Type.GetField(e, "query");
			Action[] actions = _config.GetActions((Store) _comboBox.store, null, query);

			if (string.IsNullOrEmpty(query) && (actions == null || actions.Length == 0))
			{
				Type.SetField(e, "cancel", true);

				_comboBox.collapse();
			}
		}

		private static void OnBeforeLoadData(Store store, object options)
		{
			string query = (string) Type.GetField(store.baseParams, "query");

			if (!string.IsNullOrEmpty(query))
				return;

			RangeResponse response = new RangeResponse();
			response.List = new object[0];
			response.TotalCount = 0;

			WebServiceProxy proxy = (WebServiceProxy)store.proxy;
			proxy.SetResponse(response);
		}

		private void OnLoadData(Store store, Record[] records, object options)
		{
			if (_comboBox.BackgroundLoading)
				return;

			Action[] actions = _config.GetActions(store, records, (string) Type.GetField(store.baseParams, "query"));

			if (actions.Length > 0)
			{
				for (int i = 0; i < actions.Length; i++)
				{
					Action action = actions[i];

					Dictionary data = new Dictionary();

					data[ObjectPropertyNames.Name] = Type.InvokeMethod(action, "getText");
					data[ObjectPropertyNames.Id] = data[ObjectPropertyNames.Name];
					data[_actionPropertyName] = action;

					Record record = new Record();
					record.data = data;

					store.insert(records.Length + i, new Record[] { record });
				}
			}

			if (records == null || records.Length == 0 || string.IsNullOrEmpty((string) _comboBox.getRawValue()))
			{
				Dictionary data = new Dictionary();

				data[ObjectPropertyNames.Name] = BaseRes.NoData_Text;
				data[ObjectPropertyNames.Id] = -1;
				data[_isCaptionPropertyName] = true;

				Record caption = new Record();
				caption.data = data;

				store.insert(0, new Record[] { caption });
			}
		}

		private bool OnBeforeSelectItem(ComboBox comboBox, object record, Number index)
		{
			Dictionary data = Dictionary.GetDictionary(record);

			if ((bool) data[_isCaptionPropertyName])
			{
				comboBox.collapse();

				return false;
			}

			Action action = (Action) data[_actionPropertyName];

			if (Script.IsNullOrUndefined(action))
				return true;

			comboBox.collapse();

			action.execute(this, Text);

			return false;
		}

		private void OnChangeValue(Field comboBox, object newvalue, object oldvalue)
		{
			if (!_comboBox.rendered)
				return;

			UpdateTriggerStatus();

			if (_config.AllowEdit)
			{
				if (!_config.AllowCreate && (newvalue == null || (string)newvalue == string.Empty))
					_editButton.addClass("selectorEditDisabled");
				else
					_editButton.removeClass("selectorEditDisabled");
			}
		}

		private void OnKeyDown(object field, EventObject e)
		{
			if (_config.AllowEdit && e.keyCode == EventObject.F2 && !e.shiftKey && !e.ctrlKey && !e.altKey)
			{
				e.stopEvent();

				OnEditClick();
			}
		}

		private void OnEditClick()
		{
			Array value = GetValue();
			//Log.Add(value);
			//Log.log(_config.AllowCreate);
			//Log.Add(_config);
			if (value == null && !_config.AllowCreate)
				return;
			//Log.Add("OnEditClick");

			object id = value == null ? null : value[Reference.IdPos];
			string clazz = (string) (value == null || value[Reference.TypePos] == null ? _config.Clazz : value[Reference.TypePos]);

			FormsRegistry.EditObject(clazz, id, null,
				delegate(object result)
				{
					ItemResponse response = (ItemResponse) result;

//					Log.Add("EditObject.response", response);

					Reference info = new Reference();
					info.Id = Type.GetField(response.Item, ObjectPropertyNames.Id);
					info.Name = (string) Type.GetField(response.Item, ObjectPropertyNames.Reference);
					info.Type = (string) Type.GetField(response.Item, ObjectPropertyNames.ObjectClass);

//					Log.Add("EditObject.info", info);
					SetValue(info);

					Window.SetTimeout(Focus, 0);
				}, delegate { Focus(); });
		}

		private ComboBox _comboBox;
		private readonly ObjectSelectorConfig _config;
		private Element _editButton;

		private const int _minListWidth = 250;
		private const string _actionPropertyName = "Action";
		private const string _isCaptionPropertyName = "IsCaption";
	}
}