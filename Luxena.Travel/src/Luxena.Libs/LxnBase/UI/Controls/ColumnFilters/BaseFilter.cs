using System;

using Ext.menu;

using LxnBase.Data;
using LxnBase.Services;


namespace LxnBase.UI.Controls.ColumnFilters
{
	public abstract class BaseFilter
	{
		public event EventHandler Changed;

		public ColumnConfig ColumnConfig
		{
			get { return _columnConfig; }
			set { _columnConfig = value; }
		}

		public string InternalPath
		{
			get { return _internalPath; }
			set { _internalPath = value; }
		}

		public abstract BaseFilter Create();

		public abstract PropertyFilterCondition[] Conditions { get; set; }

		public abstract Menu GetFilterMenu();

		protected void InvokeChanged()
		{
			if (Changed != null)
				Changed(this, EventArgs.Empty);
		}

		protected const string _filterMenuClass = "filterMenu";

		private ColumnConfig _columnConfig;
		private string _internalPath;
	}
}