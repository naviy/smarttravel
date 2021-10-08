using System;

using Ext;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Element = System.Html.Element;


namespace Luxena.Travel.Controls
{
	public delegate void PropertyGridRenderDelegate(object instance, object value, int index, jQueryObject container);

	public class PropertyGridControl : Panel
	{
		public PropertyGridControl(PropertyGridControlConfig config)
			: base(config.SetCssClass("property-grid-control").border(false).ToDictionary())
		{
			_gridConfig = config;
		}

		public void LoadCollection(object[] list)
		{
			_list = list;

			for (int i = body.dom.Children.Length - 1; i >= 0; i--)
				body.dom.RemoveChild(body.dom.Children[i]);

			if (_list == null || _list.Length == 0)
				return;

			LoadContent(body.dom);
		}

		private void LoadContent(Element cont)
		{
			jQueryObject container = jQuery.FromElement(cont);

			if (_gridConfig.GridTitle != null)
				container.Append(string.Format("<div class='grid-title'>{0}</div>", _gridConfig.GridTitle));

			jQueryObject div = jQuery.FromHtml("<div class='item-list'></div>");
			jQueryObject table = jQuery.FromHtml("<table style='width: 100%'></table>");

			jQueryObject headerRow = jQuery.FromHtml("<tr class='alternate'></tr>");

			if (_gridConfig.IsUseListCountColumn)
				headerRow.Append(string.Format("<th {1}>{0}</th>", Res.Common_ListNumber, 20));

			for (int index = 0; index < _gridConfig.PropertyListItems.Length; index++)
			{
				PropertyItem item = _gridConfig.PropertyListItems[index];

				string colWidth = string.Empty;

				if (item.Width != 0)
					colWidth = string.Format("style='width:{0}px'", item.Width);

				headerRow.Append(string.Format("<th {1}>{0}</th>", item.FieldLabel, colWidth));
			}

			table.Append(headerRow);

			for (int index = 0; index < _list.Length; index++)
				RenderInstance(_list[index], table, index);

			container.Append(div.Append(table));
		}

		private void RenderInstance(object instance, jQueryObject table, int rowNumber)
		{
			jQueryObject row = jQuery.FromHtml(string.Format("<tr class='{0} {1}'></tr> ", (rowNumber % 2 == 1) ? "alternate" : string.Empty, _gridConfig.GetRowCssClass(instance)));

			if (_gridConfig.IsUseListCountColumn)
				row.Append(string.Format("<td align=\"center\">{0}</td>", (rowNumber + 1)));

			for (int index = 0; index < _gridConfig.PropertyListItems.Length; index++)
			{
				PropertyItem item = _gridConfig.PropertyListItems[index];

				jQueryObject cell = jQuery.FromHtml(string.Format("<td class='{0}'></td>", item.CssClass ?? string.Empty));

				object value = Type.GetField(instance, item.FieldName);

				if (!Script.IsNullOrUndefined(value))
				{
					if ((item.Type == PropertyType.ObjectInfo) && Script.IsNullOrUndefined(((Reference)value).Id))
						value = CreateObjectInfo(instance);

					RenderValue(item, instance, value, rowNumber, cell);
				}

				row.Append(cell);
			}

			table.Append(row);
		}
		
		private static void RenderValue(PropertyItem propertyItem, object instance, object value, int rowNumber, jQueryObject cell)
		{
			if (!Script.IsNullOrUndefined(propertyItem.Renderer))
			{
				PropertyGridRenderDelegate render = (PropertyGridRenderDelegate) propertyItem.Renderer;

				render(instance, value, rowNumber, cell);
			}
			else
			{
				if (Script.IsNullOrUndefined(propertyItem.Type))
					propertyItem.Type = !Script.IsNullOrUndefined(propertyItem.EnumType) ? PropertyType.EnumType : PropertyType.String;

				string text;

				switch (propertyItem.Type)
				{
					case PropertyType.String:
						if (!Script.IsNullOrUndefined(Type.GetField(value, "Name")))
							text = Type.GetField(value, "Name").ToString();
						else
							text = value.ToString();
						break;

					case PropertyType.Date:
						text = ((Date) value).Format(propertyItem.Format ?? "d.m.Y");
						break;

					case PropertyType.Number:
						text = ((decimal)value).Format(propertyItem.Format ?? "N2");
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

					case PropertyType.Bool:
						RenderDelegate renderer = (RenderDelegate) ControlFactory.CreateBooleanRenderer();
						text = (string) renderer(value);
						break;

					default:
						text = value.ToString();
						break;
				}

				cell.Append(text);
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

		private Reference CreateObjectInfo(object instance)
		{
			Reference reference = new Reference();
			reference.Id = Type.GetField(instance, "Id");
			reference.Name = Type.GetField(instance, _gridConfig.ReferenceColumn).ToString();
			reference.Type = _gridConfig.ReferenceType;

			return reference;
		}

		
		/*
	protected void OnRender(object container, object position)
	{
		if (!Script.IsNullOrUndefined(el))
		{
			Call.BaseMethod(typeof(PropertyGridControl), this, "onRender", container, Script.IsUndefined(position) ? null : position);
			return;
		}
			
		if (this.rendered)
			body.dom.InnerHTML = GetHtml();
	}*/

		private object[] _list;
		private readonly PropertyGridControlConfig _gridConfig;
	}
}