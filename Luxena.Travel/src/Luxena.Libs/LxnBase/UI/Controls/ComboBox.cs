using System;
using System.Html;
using System.Runtime.CompilerServices;

using Ext;
using Ext.data;
using Ext.ux.grid;

using LxnBase.Data;

using Record = Ext.data.Record;


namespace LxnBase.UI.Controls
{

	public class ComboBox : Ext.form.ComboBox
	{

		[AlternateSignature]
		public extern ComboBox();

		public ComboBox(object config) : base(config)
		{
		}

		public bool BackgroundLoading
		{
			get { return _backgroundLoading; }
		}

#pragma warning disable 108,114
		public void setValue(object val)
#pragma warning restore 108,114
		{
			object oldValues = _values;

			if (Script.IsNullOrUndefined(val))
			{
				base.setValue(val);

				_values = null;

				lastQuery = null;

				if (oldValues != val)
					fireEvent("changeValue", this, val, oldValues);

				return;
			}

			Array values;

			if (val is Array)
				values = (Array) val;
			else if (!Script.IsNullOrUndefined(((Reference) val).Id) && !Script.IsNullOrUndefined(((Reference) val).Name))
				values = NormalizeValue(val);
			else
			{
				base.setValue(val);

				return;
			}

			base.setValue(values[Reference.NamePos]);

			_values = values;

			fireEvent("changeValue", this, _values, oldValues);
		}


#pragma warning disable 108,114
		public Array getValue()
#pragma warning restore 108,114
		{
			object text = getRawValue();

			if ((text == null || (string) text == string.Empty) && !Script.IsUndefined(value))
				setValue(null);

			if (!Script.IsNullOrUndefined(_values))
				return _values;

			return null;
		}

		public Reference GetObjectInfo()
		{
			Array array = getValue();

			if (array == null)
				return null;

			Reference info = new Reference();
			info.Id = array[Reference.IdPos];
			info.Name = (string) array[Reference.NamePos];
			info.Type = (string) array[Reference.TypePos];

			return info;
		}

		public object GetSelectedId()
		{
			return _values == null ? null : _values[Reference.IdPos];
		}

		public Ext.Element GetTrigger()
		{
			if (!rendered)
				return null;

			CompositeElement trigger = (CompositeElement) ((Ext.Element)getEl().parent()).select(".selector-trigger", true);

			if (trigger != null)
				return (Ext.Element) trigger.elements[0];

			return null;
		}

		protected void InitComponent()
		{
			base.initComponent();

			addEvents("changeValue");
		}

		protected void OnLoad()
		{
			if (_backgroundLoading)
			{
				_backgroundLoading = false;

				Store st = (Store) store;

				if (st.getCount() == 1)
					setValue(st.getAt(0).data);
				else
				{
					string text = (string) Type.GetField(this, "lastSelectionText");

					((InputElement) el.dom).Value = text ?? string.Empty;
					Type.InvokeMethod(this, "applyEmptyText");
				}
			}
			else
				base.onLoad();
		}

		protected void OnSelect(Record record, object index)
		{
			if (fireEvent("beforeselect", this, record.data))
			{
				setValue(record.data);

				collapse();

				fireEvent("select", this, record.data);
			}
		}

		protected void OnKeyUp(EventObject e)
		{
			if (e.getKey() == (double) Type.GetField(e, "F2"))
				return;

			if (editable && string.IsNullOrEmpty((string)getRawValue()))
				setValue(null);

			base.onKeyUp(e);
		}

		protected void BeforeBlur()
		{
			Reference info = GetObjectInfo();

			string val = (string) getRawValue();

			if (info != null && val == info.Name)
				return;

			Record rec = (Record) Type.InvokeMethod(this, "findRecord", displayField, val);

			if (Script.IsNullOrUndefined(rec) && forceSelection)
			{
				if (!string.IsNullOrEmpty(val) && val != emptyText && val != lastQuery)
				{
					_backgroundLoading = true;

					doQuery(val, true);
				}
				else
					clearValue();
			}
			else
			{
				if (!Script.IsNullOrUndefined(rec))
					val = (string) rec.get(valueField ?? displayField);

				setValue(val);
			}
		}

		private static Array NormalizeValue(object val)
		{
			if (val != null && !(val is Array))
			{
				object[] values = new object[3];

				values[Reference.IdPos] = ((Reference) val).Id;
				values[Reference.NamePos] = ((Reference) val).Name;
				values[Reference.TypePos] = ((Reference) val).Type;

				return values;
			}

			return (Array) val;
		}

		private Array _values;
		private bool _backgroundLoading;
	}
}