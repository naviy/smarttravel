using System;
using System.Runtime.CompilerServices;


namespace Luxena.Travel.Controls
{
	public delegate string GetCssClassDelegate(PropertyItem item, object value);

	public class PropertyItem
	{
		[AlternateSignature]
		public extern PropertyItem(string fieldLabel);
		
		public PropertyItem(string fieldLabel, string fieldName)
		{
			FieldLabel = fieldLabel;

			if (!Script.IsNullOrUndefined(fieldName))
				FieldName = fieldName;
		}

		public PropertyItem SetFiledLabel(string fieldLabel)
		{
			FieldLabel = fieldLabel;

			return this;
		}

		public PropertyItem SetFieldName(string fieldName)
		{
			FieldName = fieldName;

			return this;
		}

		public PropertyItem SetPropertyType(PropertyType type)
		{
			Type = type;

			return this;
		}

		public PropertyItem SetHideIsEmpty(bool hideIsEmpty)
		{
			HideIsEmpty = hideIsEmpty;

			return this;
		}

		public PropertyItem SetFormat(string format)
		{
			Format = format;

			return this;
		}

		public PropertyItem SetEnumType(Type enumType)
		{
			EnumType = enumType;

			return this;
		}

		public PropertyItem SetCssClass(string cssClass)
		{
			CssClass = cssClass;

			return this;
		}

		public PropertyItem SetRenderer(Delegate renderer)
		{
			Renderer = renderer;

			return this;
		}

		public PropertyItem SetRowCssClass(GetCssClassDelegate getCss)
		{
			GetCssClass = getCss;

			return this;
		}

		public PropertyItem SetWidth(int width)
		{
			Width = width;

			return this;
		}

		public string GetRowCssClass(object value)
		{
			string css = CssClass;

			if (GetCssClass != null)
				css = GetCssClass(this, value);

			return css;
		}

		public string FieldLabel;

		public string FieldName;

		public PropertyType Type;

		public bool HideIsEmpty;

		public string Format;

		public Type EnumType;

		public string CssClass;

		public Delegate Renderer;

		public Delegate LabelRenderer;

		public GetCssClassDelegate GetCssClass;

		public int Width;

	}
}