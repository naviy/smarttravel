using LxnBase.UI.Controls.ColumnFilters;


namespace LxnBase.UI.AutoControls
{
	public class GridFilterConfig
	{
		public GridFilterConfig setCaption(string caption)
		{
			_caption = caption;
			return this;
		}

		public GridFilterConfig setFilter(BaseFilter filter)
		{
			_filter = filter;
			return this;
		}

		public GridFilterConfig setDataPath(string dataPath)
		{
			_internalPath = dataPath;
			return this;
		}

		public string Caption
		{
			get { return _caption; }
			set { _caption = value; }
		}

		public BaseFilter Filter
		{
			get { return _filter; }
			set { _filter = value; }
		}

		public string InternalPath
		{
			get { return _internalPath; }
			set { _internalPath = value; }
		}

		private string _caption;
		private BaseFilter _filter;
		private string _internalPath;
	}
}