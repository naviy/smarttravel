using System;
using System.Collections;

using Ext;
using Ext.form;

using jQueryApi;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI.AutoControls;


namespace LxnBase.UI
{
	public abstract class BaseClassEditForm : BaseEditForm
	{
		protected BaseClassEditForm(EditFormArgs args, ItemConfig instanceConfig)
		{
			Init(args, instanceConfig);
		}

		protected EditFormArgs Args
		{
			get { return _args; }
		}

		protected ItemConfig InstanceConfig
		{
			get { return _instanceConfig; }
			set { _instanceConfig = value; }
		}

		protected object Instance
		{
			get { return _instance; }
			set { _instance = value; }
		}

		protected bool LocalMode
		{
			get { return _localMode; }
		}

		protected void Init(EditFormArgs args, ItemConfig itemConfig)
		{
			_args = args;
			_instanceConfig = itemConfig;

			_localMode = Script.IsValue(_args.Mode) && _args.Mode == LoadMode.Local;

			if (_args.OnSave != null)
				Saved += _args.OnSave;

			if (_args.OnCancel != null)
				Canceled += _args.OnCancel;
		}

		public void Open()
		{
			OnWindowOpen();
		}

		protected virtual void OnWindowOpen()
		{
			if (_localMode)
				OnInstanceLoaded(_args.FieldValues);
			else if (Script.IsValue(_args.IdToLoad))
				LoadInstance(OnInstanceLoaded);
			else
				OnInstanceLoaded(null);
		}

		protected abstract void LoadInstance(AjaxCallback onLoaded);

		protected void OnInstanceLoaded(object result)
		{
			if (!Args.IsCopy)
				_instance = result;
			else
				Args.FieldValues = Dictionary.GetDictionary(result);

			string title = string.Format("{0} ({1})", GetCaption(), Args.IsNew ? BaseRes.Creation : BaseRes.Editing);

			Window.setTitle(title);

			LoadForm();

			SetFieldValues();

			OnLoaded();

			Window.show();
		}

		protected virtual string GetCaption()
		{
			return InstanceConfig.Caption;
		}

		protected void LoadForm()
		{
			Fields = AddFields();
			Button[] buttons = AddButtons();

			InitComponentSequence(Fields, buttons);

			OnLoadForm();
		}

		protected abstract Field[] AddFields();

		protected virtual Button[] AddButtons()
		{
			SaveButton = Form.addButton(BaseRes.Save, new AnonymousDelegate(Save));
			CancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			return new Button[] { SaveButton, CancelButton };
		}

		protected virtual void InitComponentSequence(Field[] fields, Button[] buttons)
		{
			ArrayList list = new ArrayList();

			list.AddRange(fields);
			list.AddRange(buttons);

			ComponentSequence = (Component[]) list;
		}

		protected virtual void OnLoadForm()
		{
		}

		protected abstract void SetFieldValues();

		protected virtual void OnLoaded()
		{
		}

		protected object GetInstancePropertyValue(string fieldName)
		{
			return _instance != null ? Dictionary.GetDictionary(_instance)[fieldName] : GetArgsValue(fieldName);
		}

		protected object GetArgsValue(string fieldName)
		{
			if (_args.FieldValues != null && !Script.IsNullOrUndefined(_args.FieldValues[fieldName]))
				return _args.FieldValues[fieldName];

			return null;
		}

		protected ColumnConfig GetFieldConfig(string fieldName)
		{
			for (int i = 0; i < _instanceConfig.Columns.Length; i++)
				if (_instanceConfig.Columns[i].Name == fieldName)
					return _instanceConfig.Columns[i];

			return null;
		}

		protected Field CreateEditor(string fieldName)
		{
			return ControlFactory.CreateEditor(GetFieldConfig(fieldName));
		}

		protected OperationStatus GetCustomActionStatus(string actionName)
		{
			Dictionary permissions = InstanceConfig.CustomActionPermissions;

			if (permissions != null && permissions.ContainsKey(actionName))
				return (OperationStatus)permissions[actionName];

			OperationStatus status = new OperationStatus();
			status.Visible = false;

			return status;
		}


		protected void StartSave()
		{
			SaveButton.disable();
		}

		protected override void FailSave(object result)
		{
			try
			{
				base.FailSave(result);
			}
			finally
			{
				SaveButton.enable();
			}
		}


		protected Button SaveButton;
		protected Button CancelButton;

		private EditFormArgs _args;
		private ItemConfig _instanceConfig;

		private object _instance;
		private bool _localMode;
	}
}