using Ext;

namespace Luxena.Travel.Controls
{
	public class PropertyListControlConfig : PanelConfig
	{
		public PropertyItem[] PropertyListItems
		{
			get { return _propertyListItems; }
			set { _propertyListItems = value; }
		}

		public PropertyListControlConfig SetListItems(PropertyItem[] items)
		{
			_propertyListItems = items;

			return this;
		}

		public PropertyListControlConfig SetCssClass(string className)
		{
			string css = (string) (o["cls"] ?? string.Empty);

			css += string.Format(" {0}", className);

			o["cls"] = css;

			return this;
		}

		private PropertyItem[] _propertyListItems;
	}
}