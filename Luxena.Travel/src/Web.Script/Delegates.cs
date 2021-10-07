using Ext;
using Ext.data;
using Ext.form;


namespace Luxena.Travel
{
	public delegate void ComboBoxChangeDelegate(Ext.form.Field objthis, object newValue, object oldValue);
	public delegate void ComboBoxSelectDelegate(ComboBox combo, Record record, double index);
	public delegate void CheckboxCheckDelegate(Checkbox objthis, bool chckd);
	public delegate void EditorGridPanelStaterestoreDelegate(Component objthis, object state);
}