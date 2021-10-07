using System;
using System.Collections;

using Ext.menu;

using LxnBase.Data;
using LxnBase.Services;


namespace LxnBase.UI.Controls.ColumnFilters
{


	public class ListFilter : BaseFilter
	{

		public override BaseFilter Create()
		{
			return new ListFilter();
		}


		public override PropertyFilterCondition[] Conditions
		{
			get
			{
				if (_value == null || _value.Count == 0)
					return null;

				PropertyFilterCondition filterValue = new PropertyFilterCondition();

				ArrayList valueList = new ArrayList();
				ArrayList tempList = _value;

				for (int i = 0; i < tempList.Count; i++)
					valueList[i] = tempList[i];

				filterValue.Not = _not;
				filterValue.Value = valueList;
				filterValue.Operator = FilterOperator.IsIn;

				return new PropertyFilterCondition[] { filterValue };
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					_not = false;
					_value = new ArrayList();
				}
				else
				{
					_not = value[0].Not;
					_value = new ArrayList();

					ArrayList tempList = (ArrayList) value[0].Value;

					for (int i = 0; i < tempList.Count; i++)
						_value[i] = tempList[i];

					_notItem.setChecked(_not, true);

					for (int i = 0; i < _checkItems.Count; i++)
					{
						CheckItem checkItem = (CheckItem) _checkItems[i];

						for (int j = 0; j < _value.Count; j++)

							if (Type.GetField(checkItem.initialConfig, "checkItemId") == _value[j])
							{
								checkItem.setChecked(true, true);
								break;
							}
					}
				}
			}
		}

		public override Menu GetFilterMenu()
		{
			_notItem = new CheckItem(new CheckItemConfig()
				.text(BaseRes.PropertyFilterCondition_Not)
				.checked_(_not)
				.listeners(new Dictionary("checkchange", new CheckItemCheckchangeDelegate(delegate(CheckItem sender, bool @checked)
				{
					_not = @checked;

					InvokeChanged();
				})))
				.hideOnClick(false)
				.ToDictionary());

			_checkItems = new ArrayList();

			ListColumnConfig config = (ListColumnConfig) ColumnConfig;

			for (int i = 0; i < config.Items.Length; i++)
			{
				object[] item = (object[]) config.Items[i];

				_checkItems.Add(new CheckItem(new CheckItemConfig()
					.text((string) item[Reference.NamePos])
					.checked_(IsChecked(item[Reference.IdPos]))
					.listeners(new Dictionary("checkchange", new CheckItemCheckchangeDelegate(CheckChanged)))
					.hideOnClick(false)
					.custom("checkItemId", item[Reference.IdPos])
					.ToDictionary()));
			}

			ArrayList items = new ArrayList();
			items.Add(_notItem);
			items.Add(new Separator());
			items.AddRange((object[]) _checkItems);

			return new Menu(new Dictionary("items", items));
		}

		private void CheckChanged(CheckItem checkItem, bool isChecked)
		{
			object id = Type.GetField(checkItem.initialConfig, "checkItemId");

			if (!isChecked)
			{
				for (int i = 0; i < _value.Count; i++)
					if (_value[i] == id)
					{
						_value.RemoveAt(i);
						break;
					}
			}
			else
				_value.Add(id);

			InvokeChanged();
		}

		private bool IsChecked(object id)
		{
			for (int i = 0; i < _value.Count; i++)
			{
				if (_value[i] == id)
					return true;
			}

			return false;
		}

		private bool _not;
		private ArrayList _value = new ArrayList();
		private CheckItem _notItem;
		private ArrayList _checkItems;
	}
}