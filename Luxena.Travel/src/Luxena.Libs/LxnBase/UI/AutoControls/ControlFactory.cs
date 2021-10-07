using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext.data;
using Ext.form;
using Ext.util;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI.Controls;

using Field = Ext.form.Field;
using Record = Ext.data.Record;


namespace LxnBase.UI.AutoControls
{
	public delegate object RenderDelegate(object value);

	public delegate object GridRenderDelegate(object value, object metadata, Record record, int rowIndex, int colIndex, Store store);

	public delegate bool StoreSearchDelegate(Record record, object id);

	public delegate Field CreateEditorDelegate(ColumnConfig columnConfig, bool isListMode);

	public static class ControlFactory
	{
		public static Delegate CreateRenderer(ColumnConfig columnConfig)
		{
			if (columnConfig.Type == TypeEnum.Bool)
				return new RenderDelegate(BoolRenderer);

			if (columnConfig.Type == TypeEnum.Number)
				return GetNumberRenderer((NumberColumnConfig) columnConfig, 2);

			if (columnConfig.Type == TypeEnum.Date)
				return GetDateRenderer((DateTimeColumnConfig) columnConfig);

			if (columnConfig.Type == TypeEnum.List)
				return GetListRenderer((ListColumnConfig) columnConfig);

			if (columnConfig.Type == TypeEnum.Object)
				return GetClassRenderer((ClassColumnConfig) columnConfig);

			if (columnConfig.Type == TypeEnum.Custom)
			{
				CustomTypeColumnConfig config = (CustomTypeColumnConfig) columnConfig;

				return (Delegate) _customRenderers[config.TypeName];
			}

			return null;
		}

		public static Delegate CreateBooleanRenderer()
		{
			ColumnConfig booleanConfig = new ColumnConfig();
			booleanConfig.Type = TypeEnum.Bool;

			return CreateRenderer(booleanConfig);
		}



		[AlternateSignature]
		public static extern Field CreateEditor(ColumnConfig columnConfig);

		public static Field CreateEditor(ColumnConfig columnConfig, bool isListMode)
		{
			Field field = null;

			switch (columnConfig.Type)
			{
				case TypeEnum.Number:
				{
					NumberColumnConfig cfg = (NumberColumnConfig) columnConfig;

					NumberFieldConfig config = new NumberFieldConfig()
						.width(DefaultNumberFieldWidth)
						.allowDecimals(!cfg.IsInteger)
						.decimalPrecision(2)
						.allowBlank(!columnConfig.IsRequired)
						.selectOnFocus(true);

					field = cfg.IsInteger ? new NumberField(config.ToDictionary()) : new DecimalField(config.ToDictionary());
				}
					break;

				case TypeEnum.Bool:
					if (Script.IsNullOrUndefined(isListMode) || !isListMode)
						field = new Checkbox(new CheckboxConfig().ToDictionary());
					else
					{
						object[] falseVal = { false, BaseRes.Filter_False };
						object[] trueVal = { true, BaseRes.Filter_True };

						field = new Ext.form.ComboBox(new ComboBoxConfig()
							.store(new ArrayStore(new ArrayStoreConfig()
								.fields(new string[] { "Id", "Name" })
								.data(new object[] { falseVal, trueVal })
								.ToDictionary()))
							.mode("local")
							.width(50)
							.displayField("Name")
							.valueField("Id")
							.editable(false)
							.hideLabel(false)
							.triggerAction("all")
							.allowBlank(!columnConfig.IsRequired)
							.pageSize(0)
							.selectOnFocus(true)
							.ToDictionary());
					}
					break;

				case TypeEnum.String:
				{
					TextFieldConfig config = new TextFieldConfig()
						.selectOnFocus(true)
						.allowBlank(!columnConfig.IsRequired);

					int maxLength = ((TextColumnConfig) columnConfig).Length;

					if (maxLength > 0)
					{
						config.maxLength(maxLength);

						if (((TextColumnConfig) columnConfig).Lines == 0)
						{
							int width = maxLength*6;

							if (width < MinWidth)
								width = MinWidth;
							else if (width > MaxWidth)
								width = MaxWidth;

							config.width(width);
						}
					}
					else
						config.width(DefaultTextFieldWidth);

					if (((TextColumnConfig) columnConfig).Lines != 0)
					{
						config.width(MaxWidth);
						config.height(100);

						field = new TextArea(config.ToDictionary());
					}
					else
						field = new TextField(config.ToDictionary());
				}
					break;

				case TypeEnum.Date:
				{
					DateFieldConfig config = new DateFieldConfig()
						.allowBlank(!columnConfig.IsRequired)
						.selectOnFocus(true);

					if (string.IsNullOrEmpty(((DateTimeColumnConfig) columnConfig).FormatString))
						config.format("d.m.Y");
					else
						config.format(((DateTimeColumnConfig) columnConfig).FormatString);

					field = new DateField(config.ToDictionary());
				}
					break;

				case TypeEnum.Object:
					field = GetPersistentEditor((ClassColumnConfig) columnConfig).Widget;
					break;

				case TypeEnum.List:
					field = new Ext.form.ComboBox(new ComboBoxConfig()
						.store(new ArrayStore(new ArrayStoreConfig()
							.fields(new object[] { ObjectPropertyNames.Id, ObjectPropertyNames.Name })
							.data(((ListColumnConfig)columnConfig).Items)
							.ToDictionary()))
						.mode("local")
						.displayField(ObjectPropertyNames.Name)
						.valueField(ObjectPropertyNames.Id)
						.editable(false)
						.hideLabel(false)
						.hideTrigger(false)
						.triggerAction("all")
						.allowBlank(!columnConfig.IsRequired)
						.pageSize(0)
						.width(DefaultTextFieldWidth)
						.selectOnFocus(true)
						.ToDictionary());
					break;
				case TypeEnum.Custom:

					string typeName = ((CustomTypeColumnConfig) columnConfig).TypeName;

					if (_customEditors.ContainsKey(typeName))
					{
						CreateEditorDelegate editor = (CreateEditorDelegate) _customEditors[typeName];

						field = editor.Invoke(columnConfig, isListMode);
					}

					break;
			}

			if (field == null)
			{
				TextFieldConfig config = new TextFieldConfig().selectOnFocus(true);
				field = new TextField(config.ToDictionary());
			}

			field.name = columnConfig.Name;
			field.fieldLabel = columnConfig.Caption;

			if (!Script.IsNullOrUndefined(columnConfig.DefaultValue) && Script.IsNullOrUndefined(field.value))
				field.setValue(columnConfig.DefaultValue);

			return field;
		}



		public static GridRenderDelegate CreateRefrenceRenderer(string type)
		{
			return delegate(object value, object metadata, Record record, int rowIndex, int colIndex, Store store)
			{
				if (value == null)
					return null;

				object[] values = new object[3];

				values[Reference.IdPos] = record.id;
				values[Reference.NamePos] = value;

				object tp = Type.GetField(record.data, ObjectPropertyNames.ObjectClass);

				values[Reference.TypePos] = Script.IsNullOrUndefined(tp) ? type : tp;

				return ObjectLink.RenderArray(values);
			};
		}



		public static ObjectSelector GetPersistentEditor(ClassColumnConfig cfg)
		{
			int width = DefaultTextFieldWidth;

			if (cfg.Length != 0)
			{
				width = cfg.Length * 6;
				if (width < MinWidth)
					width = MinWidth;
				else if (width > MaxWidth)
					width = MaxWidth;
			}

			ObjectSelectorConfig config = (ObjectSelectorConfig) new ObjectSelectorConfig()
				.setClass(cfg.Clazz)
				.hideLabel(false)
				.width(width)
				.selectOnFocus(true)
				.allowBlank(!cfg.IsRequired)
				.name(cfg.Name)
				.fieldLabel(cfg.Caption);

			return new ObjectSelector(config);
		}

		public static void RegisterCustomRenderer(string typeName, GridRenderDelegate renderer)
		{
			_customRenderers[typeName] = renderer;
		}

		public static void RegisterCustomEditor(string typeName, CreateEditorDelegate editor)
		{
			_customEditors[typeName] = editor;
		}

		private static object BoolRenderer(object value)
		{
			if ((bool) value)
				return @"<div class='checkBoxDisabled checked'></div>";

			return string.Empty;
		}

		private static RenderDelegate GetDateRenderer(DateTimeColumnConfig config)
		{
			return delegate(object value) { return Format.date(value, config.FormatString); };
		}

		private static RenderDelegate GetClassRenderer(ClassColumnConfig config)
		{
			return delegate(object value)
			{
				if (value == null)
					return null;

				object[] values = value as object[];

				if (values != null)
				{
					if (config.RenderAsString)
						return values[Reference.NamePos];

					if (Script.IsNullOrUndefined(values[Reference.TypePos]))
						values[Reference.TypePos] = config.Clazz;

					return ObjectLink.RenderArray(values);
				}

				Reference info = (Reference) value;

				if (config.RenderAsString)
					return info.Name;

				if (Script.IsNullOrUndefined(info.Type))
					info.Type = config.Clazz;

				return ObjectLink.RenderInfo(info);
			};
		}

		private static RenderDelegate GetListRenderer(ListColumnConfig config)
		{
			return delegate(object value)
			{
				if (Script.IsNullOrUndefined(value))
					return null;

				if (value is Array)
					return ((Array) value)[Reference.NamePos];

				for (int i = 0; i < config.Items.Length; i++)
				{
					object[] values = (object[]) config.Items[i];

					if (!Script.IsNullOrUndefined(values[Reference.IdPos]) && values[Reference.IdPos] == value)
						return values[Reference.NamePos];
				}

				return value;
			};
		}

		public static RenderDelegate GetNumberRenderer(NumberColumnConfig config, int precision)
		{
			return
				delegate(object value)
				{
					if (Script.IsNullOrUndefined(value))
						return string.Empty;

					if (!Script.IsValue(precision))
						precision = 0;

					string str = config.IsInteger ? value.ToString() : ((decimal)value).Format("N" + precision);
					return string.Format("<div style='text-align: right'>{0}</div>", str);
				};
		}

		public const int MinWidth = 70;
		public const int MaxWidth = 230;

		public const int DefaultTextFieldWidth = 180;
		public const int DefaultNumberFieldWidth = 87;

		private static readonly Dictionary _customRenderers = new Dictionary();
		private static readonly Dictionary _customEditors = new Dictionary();
	}
}