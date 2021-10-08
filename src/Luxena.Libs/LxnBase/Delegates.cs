using System;

using Ext;
using Ext.data;
using Ext.form;
using Ext.grid;
using Ext.menu;

using Action = Ext.form.Action;
using Field = Ext.form.Field;
using Record = Ext.data.Record;


namespace LxnBase
{
	public delegate void AnonymousDelegate();

	public delegate bool AnonymousBoolDelegate();

	public delegate void GenericOneArgDelegate(object arg);

	public delegate void GenericTwoArgDelegate(object arg1, object arg2);

	public delegate void MessageBoxResponseDelegate(string button, string text);

	public delegate void MenuBeforeshowDelegate(Component objthis);

	public delegate void FormSubmitDelegate(FormPanel form, Action action);

	public delegate void CheckItemCheckchangeDelegate(CheckItem objthis, bool chckd);

	public delegate void PanelExpandDelegate(Panel p);

	public delegate void ComponentRenderDelegate(Component objthis);

	public delegate void TextFieldKeypressDelegate(TextField objthis, EventObject e);

	public delegate void DateMenuSelectDelegate(DatePicker picker, Date date);

	public delegate void CheckItemBeforecheckchangeDelegate(CheckItem objthis, bool chckd);

	public delegate void StoreBeforeloadDelegate(Store objthis, object options);

	public delegate void StoreLoadDelegate(Store objthis, Record[] records, object options);

	public delegate void ComboBoxBeforequeryDelegate(object queryEvent);

	public delegate void FieldChangeDelegate(Field objthis, object newValue, object oldValue);

	public delegate void FieldInvalidDelegate(Field objthis, String msg);

	public delegate void FieldValidDelegate(Field objthis);

	public delegate void DataProxyLoadDelegate(DataProxy objthis, object o, object options);

	public delegate void StoreUpdateDelegate(Store objthis, Record record, String operation);

	public delegate void ColumnModelHiddenchangeDelegate(ColumnModel objthis, double columnIndex, bool hidden);

	public delegate void PagingToolbarChangeDelegate(PagingToolbar objthis, object pageData);

	public delegate void GridViewRefreshDelegate(GridView view);

	public delegate void SelectionChangedDelegate(AbstractSelectionModel selectionModel);
}