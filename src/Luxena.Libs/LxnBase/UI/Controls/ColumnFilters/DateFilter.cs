using System;
using System.Collections;

using Ext;
using Ext.menu;

using LxnBase.Data;


namespace LxnBase.UI.Controls.ColumnFilters
{
	public class DateFilter : BaseFilter
	{
		public override BaseFilter Create()
		{
			return new DateFilter();
		}

		public override PropertyFilterCondition[] Conditions
		{
			get
			{
				if (_on != null)
				{
					PropertyFilterCondition onValue = new PropertyFilterCondition();
					onValue.Value = _on;
					onValue.Operator = FilterOperator.Equals;

					return new PropertyFilterCondition[] { onValue };
				}

				if (_after == null && _before == null)
					return null;

				ArrayList result = new ArrayList();
				if (_before != null)
				{
					PropertyFilterCondition beforeValue = new PropertyFilterCondition();
					beforeValue.Value = _before;
					beforeValue.Operator = FilterOperator.LessOrEquals;

					result.Add(beforeValue);
				}

				if (_after != null)
				{
					PropertyFilterCondition afterValue = new PropertyFilterCondition();
					afterValue.Value = _after;
					afterValue.Operator = FilterOperator.GreaterOrEquals;

					result.Add(afterValue);
				}

				return (PropertyFilterCondition[]) result;
			}
			set
			{
				if (value == null || value.Length == 0)
					return;

				for (int i = 0; i < value.Length; i++)
				{
					if (value[i].Operator == FilterOperator.GreaterOrEquals || value[i].Operator == FilterOperator.Greater)
					{
						_after = value[i].Value;

						_afterItem.setChecked(true, true);
						_afterDateMenu.picker.setValue((Date) _after);
					}
					else if (value[i].Operator == FilterOperator.LessOrEquals || value[i].Operator == FilterOperator.Less)
					{
						_before = value[i].Value;

						_beforeItem.setChecked(true, true);
						_beforeDateMenu.picker.setValue((Date) _before);
					}
					else if (value[i].Operator == FilterOperator.Equals)
					{
						_on = value[i].Value;

						_onItem.setChecked(true, true);
						_onDateMenu.picker.setValue((Date) _on);
					}
					else
						throw new Exception("Argument error");
				}
			}
		}

		public override Menu GetFilterMenu()
		{
			_beforeDateMenu = new DateMenu();
			_beforeDateMenu.on("select", new DateMenuSelectDelegate(BeforeDateSelect));
			SetDate(_beforeDateMenu, (Date) _before);
			_beforeItem = new CheckItem(new Dictionary("text", BaseRes.Filter_BeforeDate, "hideOnClick", false, "checked", _before != null, "menu", _beforeDateMenu));
			_beforeItem.on("checkchange", new CheckItemBeforecheckchangeDelegate(BeforeItemChecked));

			_afterDateMenu = new DateMenu();
			_afterDateMenu.on("select", new DateMenuSelectDelegate(AfterDateSelect));
			SetDate(_afterDateMenu, (Date) _after);
			_afterItem = new CheckItem(new Dictionary("text", BaseRes.Filter_AfterDate, "hideOnClick", false, "checked", _after != null, "menu", _afterDateMenu));
			_afterItem.on("checkchange", new CheckItemBeforecheckchangeDelegate(AfterItemChecked));

			_onDateMenu = new DateMenu();
			_onDateMenu.on("select", new DateMenuSelectDelegate(OnDateSelect));
			SetDate(_onDateMenu, (Date) _on);
			_onItem = new CheckItem(new Dictionary("text", BaseRes.Filter_OnDate, "hideOnClick", false, "checked", _on != null, "menu", _onDateMenu));
			_onItem.on("checkchange", new CheckItemBeforecheckchangeDelegate(OnItemChecked));

			_menu = new Menu(new Dictionary(
				"items", new object[]
				{
					_afterItem,
					_beforeItem,
					"-",
					_onItem
				}
				));

			return _menu;
		}

		private static void SetDate(DateMenu dateMenu, Date date)
		{
			if (!Script.IsNull(date))
				dateMenu.picker.setValue(date);
		}

		private void BeforeDateSelect(DatePicker picker, Date date)
		{
			_before = date;
			_beforeItem.setChecked(true, true);

			ClearOnDate();

			InvokeChanged();
		}

		private void AfterDateSelect(DatePicker picker, Date date)
		{
			_after = date;
			_afterItem.setChecked(true, true);

			ClearOnDate();

			InvokeChanged();
		}

		private void OnDateSelect(DatePicker picker, Date date)
		{
			_on = date;
			_onItem.setChecked(true, true);

			_after = null;
			_afterItem.setChecked(false, true);

			_before = null;
			_beforeItem.setChecked(false, true);

			InvokeChanged();
		}

		private void BeforeItemChecked(CheckItem checkItem, bool chckd)
		{
			if (chckd)
				checkItem.setChecked(false, true);
			else
			{
				_beforeItem = null;
				InvokeChanged();
			}
		}

		private void AfterItemChecked(CheckItem checkItem, bool chckd)
		{
			if (chckd)
				checkItem.setChecked(false, true);
			else
			{
				_after = null;
				InvokeChanged();
			}
		}

		private void OnItemChecked(CheckItem checkItem, bool chckd)
		{
			if (chckd)
				checkItem.setChecked(false, true);
			else
			{
				_on = null;
				InvokeChanged();
			}
		}

		private void ClearOnDate()
		{
			_on = null;
			_onItem.setChecked(false, true);
		}

		private Menu _menu;

		private CheckItem _afterItem;
		private CheckItem _beforeItem;
		private CheckItem _onItem;

		private DateMenu _afterDateMenu;
		private DateMenu _beforeDateMenu;
		private DateMenu _onDateMenu;

		private object _after;
		private object _before;
		private object _on;
	}
}