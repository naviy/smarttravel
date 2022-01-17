using System;

using Ext;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI.Controls;

using Element = System.Html.Element;


namespace Luxena.Travel.Controls
{
	public delegate void PropertyItemRenderDelegate(PropertyItem propertyItem, object value, jQueryObject container);

	public class PropertyListControl : Panel
	{
		public PropertyListControl(PropertyListControlConfig config) 
			: base( config.SetCssClass("property-list-control").border(false).ToDictionary())
		{
			_propertyListItems = config.PropertyListItems;

		}

		public void LoadInstance(object instance)
		{
			_instance = instance;

			for (int i = body.dom.Children.Length - 1; i >= 0; i--)
				body.dom.RemoveChild(body.dom.Children[i]);

			body.dom.AppendChild(GetContent());
		}

		private Element GetContent()
		{
			jQueryObject table = jQuery.FromHtml("<table></table>");

			for (int index = 0; index < _propertyListItems.Length; index++)
			{
				PropertyItem item = _propertyListItems[index];
				object value = string.IsNullOrEmpty(item.FieldName) ? null : Type.GetField(_instance, item.FieldName);

				if (!string.IsNullOrEmpty(item.FieldName) && ((Script.IsNullOrUndefined(value) || value is Array && ((Array)value).Length == 0) && item.HideIsEmpty))
					continue;

				jQueryObject row = jQuery.FromHtml("<tr></tr>");
				string css = item.GetRowCssClass(value);

				if (!string.IsNullOrEmpty(css))
					row.AddClass(css);

				row.Append(string.Format("<td class='fieldLabel'>{0}:</td>", item.FieldLabel));

				jQueryObject itemCell = jQuery.FromHtml("<td class='fieldValue'></td>");
				RenderValue(item, value, itemCell);

				row.Append(itemCell);

				table.Append(row);
			}

			jQueryObject cont = jQuery.FromHtml("<div class='item-list'></div>").Append(table);

			return cont.GetElement(0);
		}



		private static void RenderValue(PropertyItem propertyItem, object value, jQueryObject container)
		{

			string text = string.Empty;

			if (!Script.IsNullOrUndefined(propertyItem.Renderer))
			{
				PropertyItemRenderDelegate renderDelegate = (PropertyItemRenderDelegate)propertyItem.Renderer;

				renderDelegate(propertyItem, value, container);
			}
			else
			{
				if (Script.IsNullOrUndefined(propertyItem.Type))
					propertyItem.Type = Script.IsNullOrUndefined(propertyItem.EnumType) ? PropertyType.String : PropertyType.EnumType;

				switch (propertyItem.Type)
				{
					case PropertyType.String:
						text = Script.IsNullOrUndefined(((Reference) value).Name) ? value.ToString() : ((Reference) value).Name;
						break;

					case PropertyType.Bool:
						text = (bool) value ? @"<div class='checkBoxDisabled checked'></div>" : @"<div class='checkBoxDisabled'></div>";
						break;

					case PropertyType.Date:
						text = ((Date) value).Format(propertyItem.Format ?? "d.m.Y");
						break;

					case PropertyType.Number:
						text = ((decimal) value).Format(propertyItem.Format ?? "N2");
						break;

					case PropertyType.Money:
						text = MoneyDto.ToMoneyFullString((MoneyDto)value);
						break;

					case PropertyType.ObjectInfo:
						text = Link((Reference) value);
						break;

					case PropertyType.EnumType:
						text = EnumUtility.Localize(propertyItem.EnumType, (Enum) value, typeof (DomainRes));
						break;
				}

				if (!string.IsNullOrEmpty(text))
					container.Html(text);
			}
		}

		private static string Link(Reference obj)
		{
			if (Script.IsNullOrUndefined(obj))
				return string.Empty;

			if (Script.IsNullOrUndefined(obj.Id))
				return obj.Name;

			return ObjectLink.RenderInfo(obj);
		}

		private readonly PropertyItem[] _propertyListItems;
		private object _instance;
	}
}