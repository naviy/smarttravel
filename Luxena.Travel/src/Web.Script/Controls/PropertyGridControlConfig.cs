using Ext;

namespace Luxena.Travel.Controls
{
	public delegate string GetRowCssClassDelegate(object value);

	public class PropertyGridControlConfig : PanelConfig
	{
		public PropertyItem[] PropertyListItems
		{
			get { return _propertyListItems; }
			set { _propertyListItems = value; }
		}

		public bool IsUseListCountColumn
		{
			get { return _isUseListCountColumn; }
			set { _isUseListCountColumn = value; }
		}

		public string ReferenceColumn
		{
			get { return _referenceColumn; }
			set { _referenceColumn = value; }
		}

		public string ReferenceType
		{
			get { return _referenceType; }
			set { _referenceType = value; }
		}

		public string GridTitle
		{
			get { return _gridTitle; }
			set { _gridTitle = value; }
		}

		public string VoidColumn
		{
			get { return _voidColumn; }
			set { _voidColumn = value; }
		}

		public string VoidClass
		{
			get { return _voidClass; }
			set { _voidClass = value; }
		}

		public string SavedColumn
		{
			get { return _savedColumn; }
			set { _savedColumn = value; }
		}

		public string SavedClass
		{
			get { return _savedClass; }
			set { _savedClass = value; }
		}

		public string PostColumn
		{
			get { return _postColumn; }
			set { _postColumn = value; }
		}

		public string PostClass
		{
			get { return _postClass; }
			set { _postClass = value; }
		}

		public bool NotShownIfVoid
		{
			get { return _notShownIfVoid; }
			set { _notShownIfVoid = value; }
		}
		
		public PropertyGridControlConfig SetListItems(PropertyItem[] items)
		{
			_propertyListItems = items;

			return this;
		}

		public PropertyGridControlConfig SetVoidClass(string voidClass)
		{
			_voidClass = voidClass;

			return this;
		}

		public PropertyGridControlConfig SetVoidColumn(string voidColumn)
		{
			_voidColumn = voidColumn;

			return this;
		}

		public PropertyGridControlConfig SetReferenceColumn(string referenceColumn)
		{
			_referenceColumn = referenceColumn;

			return this;
		}

		public PropertyGridControlConfig SetReferenceType(string referenceType)
		{
			_referenceType = referenceType;

			return this;
		} 

		public PropertyGridControlConfig SetGridTitle(string title)
		{
			_gridTitle = title;

			return this;
		}

		public PropertyGridControlConfig SetCssClass(string className)
		{
			string css = (string)(o["cls"] ?? string.Empty);

			css += string.Format(" {0}", className);

			o["cls"] = css;

			return this;
		}

		public PropertyGridControlConfig SetUseListCountColumn(bool isUseListCountColumn)
		{
			_isUseListCountColumn = isUseListCountColumn;

			return this;
		}

		public PropertyGridControlConfig SetRowCssClass(GetRowCssClassDelegate getCss)
		{
			_getCssClass = getCss;

			return this;
		}

		public string GetRowCssClass(object value)
		{
			string css = string.Empty;

			if (_getCssClass != null)
				css = _getCssClass(value);

			return css;
		}

		private string _referenceType;
		private string _referenceColumn;
		private bool _isUseListCountColumn;
		private PropertyItem[] _propertyListItems;
		private string _gridTitle;
		private string _voidColumn;
		private string _voidClass;
		private bool _notShownIfVoid;
		private string _savedColumn;
		private string _savedClass;
		private string _postColumn;
		private string _postClass;
		private GetRowCssClassDelegate _getCssClass;
	}
}