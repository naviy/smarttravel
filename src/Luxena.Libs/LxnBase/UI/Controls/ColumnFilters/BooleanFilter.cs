using System.Collections;

using Ext;
using Ext.menu;

using LxnBase.Data;


namespace LxnBase.UI.Controls.ColumnFilters
{
	public class BooleanFilter : BaseFilter
	{
		public override BaseFilter Create()
		{
			return new BooleanFilter();
		}

		public override PropertyFilterCondition[] Conditions
		{
			get
			{
				PropertyFilterCondition value = new PropertyFilterCondition();
				value.Operator = FilterOperator.Equals;
				value.Value = _value;

				return new PropertyFilterCondition[]
				{
					value
				};
			}
			set
			{
				if (value != null && (bool) value[0].Value)
				{
					_value = true;

					_suppressEvent = true;
					_items[0].setChecked(true, true);
				}
				else
				{
					_value = false;

					_suppressEvent = true;
					_items[1].setChecked(true, true);
				}
			}
		}

		public override Menu GetFilterMenu()
		{
			string groupId = ExtClass.id();

			_items = new CheckItem[]
			{
				new CheckItem(new CheckItemConfig()
					.text(BaseRes.Filter_True)
					.group(groupId)
					.checked_(_value)
					.listeners(new Dictionary("checkchange", new System.Action(delegate { CheckItemClicked(true); })))
					.ToDictionary()),

				new CheckItem(new CheckItemConfig()
					.text(BaseRes.Filter_False)
					.group(groupId)
					.checked_(!_value)
					.listeners(new Dictionary("checkchange", new System.Action(delegate { CheckItemClicked(false); })))
					.ToDictionary())
			};

			return new Menu(new MenuConfig()
				.cls(_filterMenuClass)
				.items(_items)
				.ToDictionary());
		}

		private void CheckItemClicked(bool value)
		{
			if (_suppressEvent)
			{
				_suppressEvent = false;
				return;
			}

			if (value == _value)
				return;

			_value = value;

			InvokeChanged();
		}

		private bool _value;
		private CheckItem[] _items;
		private bool _suppressEvent;
	}
}