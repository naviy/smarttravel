using System.Collections;

using LxnBase.Data;
using LxnBase.Services;


namespace LxnBase.UI.AutoControls
{
	public class AutoGridArgs
	{
		public event GenericTwoArgDelegate CreateActionToolbar;

		public string Type;

		public ListConfig ListConfig;

		public bool NonPaged;

		public RangeRequest BaseRequest;

		public string[] ForcedProperties;

		public bool AutoCommit = true;

		public ListMode Mode = ListMode.List;

		public ArrayList ColumnsConfig;

		/*public Store Store
		{
			get { return o.ContainsKey(StoreKey) ? (Store)o[StoreKey] : null; }
		}

		public AbstractSelectionModel SelectionModel
		{
			get { return o.ContainsKey(SelectionModelKey) ? (AbstractSelectionModel)o[SelectionModelKey] : null; }
		}

		public GridView GridView
		{
			get { return o.ContainsKey(GridViewKey) ? (GridView)o[GridViewKey] : null; }
		}

		public Dictionary GridViewConfig
		{
			get { return o.ContainsKey(GridViewConfigKey) ? (Dictionary)o[GridViewConfigKey] : null; }
		}
		*/
		public void OnCreateActionToolbar(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			if (CreateActionToolbar != null)
				CreateActionToolbar.Invoke(toolbarItems, autoGrid);
		}


		public void SetDefaultSort(string column, string dir)
		{
			if (BaseRequest.Sort == null)
			{
				BaseRequest.Sort = column;
				BaseRequest.Dir = dir;
			}
		}

		/*
		private const string StoreKey = "store";
		private const string SelectionModelKey = "selModel";
		private const string GridViewKey = "view";
		private const string GridViewConfigKey = "viewConfig";*/


	}

	public enum ListMode
	{
		List = 0,
		Select = 1
	}
}