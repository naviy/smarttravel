using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext.data;
using Ext.form;
using Ext.menu;

using LxnBase;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;

using Field = Ext.form.Field;
using FieldConfig = Ext.form.FieldConfig;


namespace Luxena.Travel
{

	public class FormMember
	{

		public FormMember(IEditForm form, SemanticMember semantic, FieldConfig config)
		{
			Config = config;
			Form = form;
			Semantic = semantic;
		}


		public IEditForm Form;
		public SemanticMember Semantic;
		public FieldConfig Config;


		protected Field Field;
		protected Field[] Fields;
		
		public Field[] GetFields()
		{
			return Fields;
		}


		[AlternateSignature]
		public extern Field SetField(Field value);

		public Field SetField(Field value, Field[] fields)
		{
			Field = value;
			if (Script.IsUndefined(fields))
				fields = new Field[] { value };
			Fields = fields;

			AddEvents();

			return value;
		}


		public event AnonymousDelegate OnLoadValue;
		public event AnonymousDelegate OnSaveValue;

		public void LoadValue()
		{
			if (OnLoadValue != null)
				OnLoadValue();
			else if (Field != null)
			{
				object value = Form.GetValue(Semantic._name);

				if (Script.IsValue(value))
					Field.setValue(value);
			}
		}

		public void SaveValue()
		{
			if (OnSaveValue != null)
				OnSaveValue();
			else if (Field != null)
			{
				object value = Field.getValue();
				if (Script.IsNullOrUndefined(value))
					value = null;

				Form.SetValue(Semantic._name, value);
			}
		}

		public event AnonymousBoolDelegate OnIsModified;

		public bool IsModified()
		{
			if (OnIsModified != null)
				return OnIsModified();

			if (Fields == null) return false;

			foreach (Field field in Fields)
			{
				if (field != null && field.isDirty()) return true;
			}

			return false;
		}


		public void BoldLabel()
		{
			Config.labelStyle("font-weight:bold;");
		}

		public void DataProxy(DataProxy proxy)
		{
			((ObjectSelectorConfig)Config).setDataProxy(proxy);
		}

		public void HideLabel()
		{
			Config.hideLabel(true);
		}

		public void Label(string value)
		{
			Config.fieldLabel(value);
		}

		public void EmptyText(string value)
		{
			((TextFieldConfig)Config).emptyText(value);
		}

		
		public void ValueProperties(string[] value)
		{
			((ObjectSelectorConfig)Config).valueProperties(value);
		}

		public void Width(int value)
		{
			value = Semantic._type.ConvertWidth(Form, value);
			Config.width(value);
		}

		public void Height(int value)
		{
			Config.height(value);
		}

		public void ItemCls(string value)
		{
			Config.itemCls(value);
		}

		public void Required(bool value)
		{
			((TextFieldConfig)Config).allowBlank(!value);
		}

		#region Events

		public void AddEvents()
		{
			if (_onBlur != null)
				Field.addListener("blur", _onBlur);

			if (_onKeyPress != null)
			{
				TextField textField = Field as TextField;
				if (textField != null)
					textField.enableKeyEvents = true;

				Field.addListener("keypress", _onKeyPress);
			}

			if (_onChangeValue != null)
				Field.addListener("changeValue", _onChangeValue);
		}

		private Delegate _onBlur;
		public void OnBlur(AnonymousDelegate value)
		{
			_onBlur = value;
		}

		private Delegate _onKeyPress;
		public void OnKeyPress(GenericTwoArgDelegate value)
		{
			_onKeyPress = value;
		}

		FieldChangeDelegate _onChangeValue;
		public void OnChangeValue(FieldChangeDelegate value)
		{
			MoneyControlConfig moneyControlConfig = Config as MoneyControlConfig;
			if (moneyControlConfig != null)
				moneyControlConfig.SetAmountChangeHandler(value);
			else
				_onChangeValue = value;
		}

		public void MenuOnChange(ArrayList items)
		{
			MoneyControlConfig moneyControlConfig = Config as MoneyControlConfig;
			if (moneyControlConfig != null)
				moneyControlConfig.SetOnChangeMenu(new MenuConfig().items(items));
		}


		#endregion

	}

}