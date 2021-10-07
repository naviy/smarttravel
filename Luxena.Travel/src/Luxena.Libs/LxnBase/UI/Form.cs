using Ext;
using Ext.form;

using DomElement = System.Html.Element;

namespace LxnBase.UI
{
	public class Form : FormPanel
	{
		public Form(object config) : base(config)
		{
		}

		public void HideItem(Field field)
		{
			if (field.rendered)
			{
				DomElement formItem = field.getEl().findParent(".x-form-item");
				Element element = new Element(formItem);
				element.addClass("x-hide-display");
			}
			else
				field.itemCls = "x-hide-display";

			field.setVisible(false);
		}

		public void ShowItem(Field field)
		{
			if (field.rendered)
			{
				DomElement formItem = field.getEl().findParent(".x-form-item");
				Element element = new Element(formItem);
				element.removeClass("x-hide-display");
			}

			field.setVisible(true);
		}
	}
}