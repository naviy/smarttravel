using System;

using Ext.form;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public abstract class SemanticType
	{

		public virtual string GetString(SemanticMember sm, object value)
		{
			return Script.IsValue(value) ? value.ToString() : "";
		}

		public abstract Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember);


		public void InitFieldConfig(IEditForm form, SemanticMember semantic, int width, FieldConfig config)
		{
			config.name(semantic._name);

			if (!Script.IsValue(width) || width == 0)
				width = DefaultWidth > 0 ? DefaultWidth : form.FieldMaxWidth;

			if (width > 0)
				config.width(width);

			if (!Script.IsValue(semantic._title))
				config.labelSeparator("");
			else
				config.fieldLabel(semantic._title);

			if (semantic._required)
				config.labelStyle("font-weight:bold;");

			if (Script.IsValue(semantic._defaultValue))
				config.value(semantic._defaultValue);
			//if (!Script.IsNullOrUndefined(columnConfig.DefaultValue) && Script.IsNullOrUndefined(field.value))
			//	field.setValue(columnConfig.DefaultValue);

		}

		public void InitTextFieldConfig(IEditForm form, SemanticMember semantic, int width, TextFieldConfig config)
		{
			width = ConvertWidth(form, width);

			InitFieldConfig(form, semantic, width, config);

			config.selectOnFocus(true);
			config.allowBlank(!semantic._required);

			if (Script.IsValue(semantic._emptyText))
				config.emptyText(semantic._emptyText);

			if (semantic._maxLength > 0)
				config.maxLength(semantic._maxLength);
		}
		
		public virtual int ConvertWidth(IEditForm form, int width)
		{
			if (Script.IsNullOrUndefined(width) || width == 0)
				return 
					DefaultWidth > 0 ? DefaultWidth : 
					form.FieldMaxWidth < ControlFactory.DefaultTextFieldWidth ? form.FieldMaxWidth : 
					ControlFactory.DefaultTextFieldWidth;
			
			if (width == -1)
				return form.FieldMaxWidth;
			
			if (width == -2)
				return MinWidth;
			
			if (width == -3)
				return MaxWidth;

			return width;
		}

		public const int MinWidth = 104;

		public const int MaxWidth = 336;

		public virtual int DefaultWidth { get { return 0; } }

		public virtual void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
		}
	}

}