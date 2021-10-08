using System;
using System.Collections;

using Ext.form;
using Ext.menu;

using LxnBase.Data;


namespace LxnBase.UI.Controls.ColumnFilters
{
	public abstract class EditorFilter : BaseFilter
	{
		protected EditorFilter()
		{
			_operator = _defaultOperator;
		}

		protected bool Not
		{
			get { return _not; }
			set { _not = value; }
		}

		protected FilterOperator Operator
		{
			get { return _operator; }
			set { _operator = value; }
		}

		protected object Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public override PropertyFilterCondition[] Conditions
		{
			get
			{
				if (Operator != FilterOperator.IsNull && _value == null)
					return null;

				PropertyFilterCondition filterValue = new PropertyFilterCondition();

				filterValue.Not = _not;
				filterValue.Operator = Operator;
				filterValue.Value = _value;

				return new PropertyFilterCondition[] { filterValue };
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					_not = false;
					_value = null;

					Operator = _defaultOperator;
				}
				else
				{
					_not = value[0].Not;
					_value = value[0].Value;

					Operator = value[0].Operator;

					NotItem.setChecked(_not, true);

					for (int i = 0; i < CheckItems.Length; i++)
						if ((FilterOperator) Type.GetField(CheckItems[i].initialConfig, "value") == Operator)
						{
							CheckItems[i].setChecked(true, true);

							break;
						}

					Editor.setValue(Value);
				}
			}
		}

		protected abstract Field Editor { get; }

		protected abstract CheckItem[] CheckItems { get; }

		protected abstract CheckItem NotItem { get; }

		protected CheckItem CreateCheckItem(string groupId, FilterOperator filterOperator, Dictionary listeners)
		{
			CheckItem item = new CheckItem(new Dictionary(
				"text", EnumUtility.Localize(typeof (FilterOperator), filterOperator, typeof (BaseRes)),
				"value", filterOperator,
				"group", groupId,
				"checked", Operator == filterOperator,
				"hideOnClick", false,
				"listeners", listeners));

			return item;
		}

		protected void CheckItemClicked(CheckItem checkItem, bool @checked)
		{
			if (!@checked)
				return;

			Operator = (FilterOperator) Type.GetField(checkItem.initialConfig, "value");

			CheckEditor();

			InvokeChanged();
		}

		protected void CheckEditor()
		{
			if (Operator == FilterOperator.IsNull)
				Editor.disable();
			else
				Editor.enable();
		}

		protected const string _editorClassName = "filterEditor";
		protected const FilterOperator _defaultOperator = FilterOperator.Equals;

		private bool _not;
		private FilterOperator _operator;
		private object _value;
	}
}