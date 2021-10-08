using System;
using System.Collections;

using Ext.data;
using Ext.form;

using LxnBase.Data;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace LxnBase.UI.Controls
{

	public delegate Action[] GetSelectorCustomActionsDelegate(Store store, Record[] records, string  query);

	public delegate void ObjectSelectorValueChangeEvent(object sender, object[] data);

	public class ObjectSelectorConfig : ComboBoxConfig
	{
		public ObjectSelectorConfig()
		{
			minChars(0);
			typeAhead(false);
			forceSelection(true);
			resizable(true);
			enableKeyEvents(true);
			queryDelay(50);
			selectOnFocus(true);
			valueField(ObjectPropertyNames.Id);
			displayField(ObjectPropertyNames.Name);
		}

		public string Clazz
		{
			get { return _class; }
		}

		public string ValueField
		{
			get { return (string) o["valueField"]; }
		}

		public string DisplayField
		{
			get { return (string) o["displayField"]; }
		}

		public string[] ValueProperties
		{
			get { return _valueProperties; }
		}

		public Action[] GetActions(Store store, Record[] records, string query)
		{
			ArrayList actions = new ArrayList();

			if (_getCustomActions != null)
			{
				Action[] list = _getCustomActions.Invoke(store, records, query);

				if (list != null && list.Length > 0)
					actions.AddRange(list);
			}
			if (_customActions != null && _customActions.Length > 0)
				actions.AddRange(_customActions);

			return (Action[]) actions;
		}

		public DataProxy DataProxy
		{
			get { return _dataProxy; }
		}

		public Dictionary ReaderMetadata
		{
			get { return _readerMetadata; }
		}

		public bool AllowCreate
		{
			get { return _allowCreate; }
		}

		public bool AllowEdit
		{
			get { return _allowEdit; }
		}

		public object Store
		{
			get { return o["store"]; }
		}

		public ObjectSelectorConfig setClass(string clazz)
		{
			_class = clazz;
			return this;
		}

		public ObjectSelectorConfig valueProperties(string[] valueProperties)
		{
			_valueProperties = valueProperties;
			return this;
		}

		public ObjectSelectorConfig customActions(Action[] customActions)
		{
			_customActions = customActions;
			return this;
		}

		public ObjectSelectorConfig customActionsDelegate(GetSelectorCustomActionsDelegate getCustomActions)
		{
			_getCustomActions = getCustomActions;
			return this;
		}

		public ObjectSelectorConfig setValue(Reference val)
		{
			if (!Script.IsNullOrUndefined(val))
				value(new object[] { val.Id, val.Name });

			return this;
		}

		public ObjectSelectorConfig setDataProxy(DataProxy proxy)
		{
			_dataProxy = proxy;
			return this;
		}

		public ObjectSelectorConfig setReaderMetadata(Dictionary readerMetadata)
		{
			_readerMetadata = readerMetadata;
			return this;
		}

		public ObjectSelectorConfig allowCreate(bool allowCreate)
		{
			_allowCreate = allowCreate;
			return this;
		}

		public ObjectSelectorConfig allowEdit(bool allowEdit)
		{
			_allowEdit = allowEdit;
			return this;
		}

		private string _class;

		private string[] _valueProperties;
		private Action[] _customActions;
		private GetSelectorCustomActionsDelegate _getCustomActions;

		private bool _allowEdit;
		private bool _allowCreate;

		private DataProxy _dataProxy;
		private Dictionary _readerMetadata;
	}

}