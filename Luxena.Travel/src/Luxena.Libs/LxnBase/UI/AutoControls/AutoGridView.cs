using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext.grid;
using Ext.menu;


namespace LxnBase.UI.AutoControls
{
	public class AutoGridView : GridView
	{
		[AlternateSignature]
		public extern AutoGridView();

		public AutoGridView(object config) : base(config)
		{
		}

		public void BeforeColMenuShow()
		{
			ColumnModel colModel = (ColumnModel)Type.GetField(this, "cm");
			Menu colMenu = (Menu)Type.GetField(this, "colMenu");

			if (((Array)colMenu.items).Length > 0)
				return;

			ArrayList main = new ArrayList();
			ArrayList system = new ArrayList();

			for (int i = 0; i < _columns.Length; i++)
			{
				Column col = _columns[i];
				double index = colModel.findColumnIndex(col.dataIndex);
				
				CheckItem checkItem = new CheckItem(new CheckItemConfig()
					.text(col.header)
					.itemId("col-" + col.id)
					.hideOnClick(false)
					.ToDictionary());

				checkItem.setChecked(!colModel.isHidden(index));

				if (_s.Contains(col.id))
					system.Insert(_s.IndexOf(col.id), checkItem);
				else
					main.Add(checkItem);
			}

			main.Sort(delegate (object x, object y) { return ((CheckItem) x).text.CompareTo(((CheckItem) y).text); });

			colMenu.add(main);

			if (system.Count > 0)
			{
				colMenu.add("-");
				colMenu.add(system);
			}
		}

		public void SetColumns(Column[] columns)
		{
			_columns = columns;
		}

		public string GetRowClass(Ext.data.Record record, double index, object rowParams, Ext.data.Store store)
		{
			string cls = Script.IsValue(((RecordMeta) record.data).__deleted) ? "deleted" : string.Empty;

			string custom = GetCustomRowClass(record);

			return (cls + " " + custom).Trim();
		}

		public virtual string GetCustomRowClass(Ext.data.Record record)
		{
			return null;
		}

		private Column[] _columns;

		private readonly string[] _s = new string[] { "CreatedOn", "CreatedBy", "ModifiedOn", "ModifiedBy", "Id" };
	}
}