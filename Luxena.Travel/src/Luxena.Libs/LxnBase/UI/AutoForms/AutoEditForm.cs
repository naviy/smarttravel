using System;
using System.Collections;

using Ext.form;

using jQueryApi;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI.AutoControls;


namespace LxnBase.UI.AutoForms
{
	public class AutoEditForm : BaseClassEditForm
	{
		public AutoEditForm(EditFormArgs args, ItemConfig itemConfig) : base(args, itemConfig)
		{
		}

		protected override void LoadInstance(AjaxCallback onLoaded)
		{
			GenericService.Get(Args.Type, Args.IdToLoad, false, onLoaded, null);
		}

		protected override void OnLoadForm()
		{
			Window.setWidth(CalculateWindowWidth(Fields));
		}

		protected override void OnLoaded()
		{
			if (Instance != null)
				_instanceDictionary = Dictionary.GetDictionary(Instance);
		}

		protected override Field[] AddFields()
		{
			Field[] fields = new Field[InstanceConfig.Columns.Length];

			for (int i = 0; i < InstanceConfig.Columns.Length; i++)
			{
				fields[i] = ControlFactory.CreateEditor(InstanceConfig.Columns[i]);

				Form.add(fields[i]);
			}

			return fields;
		}

		protected override void SetFieldValues()
		{
			ColumnConfig[] columns = InstanceConfig.Columns;

			for (int i = 0; i < columns.Length; i++)
			{
				object value = GetInstancePropertyValue(columns[i].Name);

				if (!Script.IsNullOrUndefined(value))
					Fields[i].setValue(value);
			}
		}

		protected override void OnSave()
		{
			Dictionary data = GetFieldValues();

			object version = Instance != null ? _instanceDictionary[ObjectPropertyNames.Version] : null;

			if (LocalMode)
			{
				data["Id"] = Args.Id;

				CompleteSave(ItemResponse.Create(data));
			}
			else if (data.Count != 0)
			{
				GenericService.Update(Args.Type, Args.Id, version, data, Args.RangeRequest, CompleteSave, null);
			}
			else
			{
				Cancel();
			}
		}

		protected override void OnSaved(object result)
		{
			ItemResponse response = (ItemResponse) result;

			if (!LocalMode && Script.IsNullOrUndefined(Args.RangeRequest))
			{
				string text = (string) Type.GetField(response.Item, ObjectPropertyNames.Reference) ?? InstanceConfig.Caption;

				MessageRegister.Info(InstanceConfig.ListCaption, (Args.IsNew ? BaseRes.Created : BaseRes.Updated) + " " + text.ToLocaleString());
			}

			base.OnSaved(result);
		}

		protected virtual Dictionary GetFieldValues()
		{
			Dictionary data = new Dictionary();

			for (int i = 0; i < Fields.Length; i++)
				GetFieldValue(Fields[i], data);

			return data;
		}

		protected void GetFieldValue(Field field, Dictionary values)
		{
			object value = field.getValue();

			if (!Script.IsValue(value) || string.IsNullOrEmpty(value.ToString()) && value != (object) 0)
				value = null;

			if (_instanceDictionary == null || LocalMode || (bool) Type.InvokeMethod(field, "isDirty") && _instanceDictionary[field.name] != value)
				values[field.name] = value;
		}

		private double CalculateWindowWidth(Field[] formFields)
		{
			double width = 0;

			for (int i = 0; i < formFields.Length; i++)
				if (formFields[i].width != 0 && width < formFields[i].width)
					width = formFields[i].width;

			return Form.labelWidth + width + 50;
		}

		private Dictionary _instanceDictionary;
	}
}