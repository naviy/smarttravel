using System.Collections;

using Ext;
using Ext.form;
using Ext.menu;

using LxnBase.Data;


namespace LxnBase.UI.Controls.ColumnFilters
{
	public class NumberFilter : EditorFilter
	{
		protected override Field Editor
		{
			get { return _editor; }
		}

		protected override CheckItem[] CheckItems
		{
			get { return (CheckItem[]) _checkItems; }
		}

		protected override CheckItem NotItem
		{
			get { return _notItem; }
		}

		public override BaseFilter Create()
		{
			return new NumberFilter();
		}

		public override Menu GetFilterMenu()
		{
			string groupId = ExtClass.id();

			Dictionary listeners = new Dictionary(
				"checkchange", new CheckItemCheckchangeDelegate(CheckItemClicked));

			InitEditor();

			_notItem = new CheckItem(new Dictionary(
				"text", BaseRes.PropertyFilterCondition_Not,
				"checked", Not,
				"hideOnClick", false,
				"listeners", new Dictionary("checkchange", new CheckItemCheckchangeDelegate(delegate(CheckItem sender, bool @checked)
				{
					Not = @checked;

					InvokeChanged();
				}))
				));

			_checkItems = new ArrayList();

			_checkItems.Add(CreateCheckItem(groupId, FilterOperator.Equals, listeners));
			_checkItems.Add(CreateCheckItem(groupId, FilterOperator.Greater, listeners));
			_checkItems.Add(CreateCheckItem(groupId, FilterOperator.Less, listeners));
			_checkItems.Add(CreateCheckItem(groupId, FilterOperator.GreaterOrEquals, listeners));
			_checkItems.Add(CreateCheckItem(groupId, FilterOperator.LessOrEquals, listeners));
			_checkItems.Add(CreateCheckItem(groupId, FilterOperator.IsNull, listeners));

			ArrayList items = new ArrayList();
			items.Add(_notItem);
			items.Add(new Separator());
			items.AddRange((object[]) _checkItems);
			items.Add(new Separator());
			items.Add(new FilterMenuItem(new Dictionary("editor", _editor)));

			_menu = new Menu(new Dictionary(
				"cls", _filterMenuClass,
				"items", items
				));

			return _menu;
		}

		private void InitEditor()
		{
			_editor = new NumberField(new Dictionary(
				"cls", _editorClassName,
				"enableKeyEvents", true
				));

			_editor.setValue(Value);

			_editor.on("keypress", new TextFieldKeypressDelegate(delegate(TextField objthis, EventObject e)
			{
				object value = _editor.getValue();

				double key = e.getKey();
				if (key == EventObject.ENTER && value != Value)
				{
					Value = value;
					_menu.hide(true);

					InvokeChanged();
				}
				else if (key == EventObject.ESC)
				{
					_editor.setValue(Value);
					_menu.hide(true);
				}
			}));

			_editor.on("change", new AnonymousDelegate(delegate
			{
				object value = _editor.getValue();

				if (Value != value)
				{
					Value = value;
					InvokeChanged();
				}
			}));

			CheckEditor();
		}

		private Menu _menu;
		private NumberField _editor;

		private ArrayList _checkItems = new ArrayList();
		private CheckItem _notItem;
	}
}