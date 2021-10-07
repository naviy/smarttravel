using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.form;
using Ext.ux.form;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI.AutoControls;

using Field = Ext.form.Field;


namespace Luxena.Travel
{

	public class EnumSemanticType : SemanticType
	{

		public override string GetString(SemanticMember sm, object value)
		{
			if (!Script.IsValue(value)) return "";

			foreach (object[] values in sm._enumItems)
			{
				if (values[0] == value)
					return (string)values[1];
			}

			return (string)value;
		}



		public override void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
			cfg.renderer(new RenderDelegate(delegate(object value)
			{
				if (!Script.IsValue(value)) return "";

				foreach (object[] values in sm._enumItems)
				{
					if (values[0] == value)
						return values[1];
				}

				return value;
			}));
		}

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			ComboBoxConfig config = new ComboBoxConfig()
				.store(new ArrayStore(new ArrayStoreConfig()
					.fields(new object[] { ObjectPropertyNames.Id, ObjectPropertyNames.Name })
					.data(sm._enumItems)
					.ToDictionary()
				))
				.mode("local")
				.displayField(ObjectPropertyNames.Name)
				.valueField(ObjectPropertyNames.Id)
				.editable(false)
				.hideLabel(false)
				.hideTrigger(false)
				.triggerAction("all")
				.pageSize(0)
				.selectOnFocus(true);

			if (Script.IsValue(sm._enumItems))
				config.value(sm._enumItems[0][0]);

			InitTextFieldConfig(form, sm, width, config);

			FormMember member = form.Members.Add2(form, sm, config, initMember);

			return member.SetField(new ComboBox(config.ToDictionary()));
		}

	}


	public class EnumsSemanticType : SemanticType
	{

		public override string GetString(SemanticMember sm, object value)
		{
			if (!Script.IsValue(value)) return "";

			ArrayList items = new ArrayList();

			foreach (object[] values in sm._enumItems)
			{
				if (((int)values[0] & (int)value) != 0)
					items.Add(values[1]);
			}

			return items.Join(", ");
		}


		public override void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
			cfg.renderer(new RenderDelegate(delegate (object value)
			{
				if (!Script.IsValue(value)) return "";

				ArrayList items = new ArrayList();

				foreach (object[] values in sm._enumItems)
				{
					if (((int)values[0] & (int)value) != 0)
						items.Add(values[1]);
				}

				return items.Join(", ");
			}));
		}

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			width = ConvertWidth(form, width);

			CheckboxGroupConfig config = new CheckboxGroupConfig()
				.items(sm._enumItems
					.Filter(delegate(object value) { return (int)((Array)value)[0] != 0; })
					.Map(delegate(object item)
					{
						object id = ((Array)item)[0];
						string name = ((Array)item)[1].ToString();

						return new Checkbox(new CheckboxConfig()
							.data(id)
							.boxLabel(name)
							.ToDictionary()
						);
					})
				)
				.width(width)
				.columns(2)
			;


			InitFieldConfig(form, sm, width, config);

			FormMember member = form.Members.Add2(form, sm, config, initMember);
			CheckboxGroup field = new CheckboxGroup(config.ToDictionary());

			member.OnLoadValue += delegate
			{
				int value = (int)form.GetValue(sm._name);
				if (!Script.IsValue(value)) return;

				foreach (Checkbox item in field.items)
				{
					item.setValue(((int)item.data & value) != 0);
				}
			};

			member.OnSaveValue += delegate
			{
				int value = 0;

				foreach (Checkbox item in field.getValue())
				{
					value = value | (int)item.data;
				}

				form.SetValue(sm._name, value);
			};


			return member.SetField(field);
		}

	}

	public static partial class ViewTypes
	{
		public static SemanticType Enumeration = new EnumSemanticType();
		public static SemanticType Enumerations = new EnumsSemanticType();
	}

	public partial class SemanticMember
	{
		public bool IsEnum;

		public object[][] _enumItems;

		public string GetEnumItemName(object value)
		{
			if (Script.IsNullOrUndefined(_enumItems)) return "";

			foreach (object[] item in _enumItems)
			{
				if (item[0] == value)
					return (string)item[1];
			}

			return "";
		}


		public SemanticMember EnumItems(object[][] items)
		{
			_type = ViewTypes.Enumeration;
			_enumItems = items;
			IsEnum = true;
			return this;
		}

		public SemanticMember EnumsItems(object[][] items)
		{
			_type = ViewTypes.Enumerations;
			_enumItems = items;
			IsEnum = true;
			return this;
		}
	}

}