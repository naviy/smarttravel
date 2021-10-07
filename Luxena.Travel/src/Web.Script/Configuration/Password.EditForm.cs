using System;
using System.Collections;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.Services;
using LxnBase.UI;

using Luxena.Travel.Services;


namespace Luxena.Travel.Configuration
{
	public class PasswordEditForm : BaseEditForm
	{
		public PasswordEditForm()
		{
			Window.setTitle(Res.Profile_Change_Password);
			Window.cls += " password-edit-form";

			_saveButton = Form.addButton(BaseRes.Save, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			CreateFields();

			Fields = new Field[]
			{
				_oldPassword,
				_newPassword,
				_confirmPassword
			};

			ArrayList list = new ArrayList();

			list.AddRange(Fields);
			list.Add(_saveButton);
			list.Add(_cancelButton);

			ComponentSequence = (Component[]) list;
		}

		private void CreateFields()
		{
			ColumnConfig config = new ColumnConfig();
			config.Type = TypeEnum.String;
			config.IsRequired = true;
			config.Caption = Res.Password;

			_oldPassword = new TextField(new TextFieldConfig()
				.fieldLabel(Res.Password_OldPassword)
				.inputType("password")
				.name("OldPassword")
				.selectOnFocus(true)
				.width(190)
				.enableKeyEvents(true)
				.listeners(new Dictionary("keydown", new GenericTwoArgDelegate(PwdFieldKeyDown)))
				.ToDictionary());

			_newPassword = new TextField(new TextFieldConfig()
				.fieldLabel(Res.Password_NewPassword)
				.inputType("password")
				.width(190)
				.selectOnFocus(true)
				.enableKeyEvents(true)
				.listeners(new Dictionary("keydown", new GenericTwoArgDelegate(PwdFieldKeyDown)))
				.ToDictionary());

			_confirmPassword = new TextField(new TextFieldConfig()
				.fieldLabel(Res.Password_ConfirmPassword)
				.inputType("password")
				.width(190)
				.selectOnFocus(true)
				.validator(new FieldValidateDelegate1(
					delegate(object value)
					{
						bool result = true;
						Field pwd = (Field)Type.GetField(_confirmPassword, "initialPwdField");

						if (Script.IsNullOrUndefined(pwd))
							result = false;
						else
						{
							object isPwdDirty = Type.GetField(pwd, _fieldDirtyName);
							object isConfirmDirty = Type.GetField(_confirmPassword, _fieldDirtyName);

							if (!Script.IsNullOrUndefined(isPwdDirty) && Script.IsNullOrUndefined(isConfirmDirty)
								|| Script.IsNullOrUndefined(isPwdDirty) && !Script.IsNullOrUndefined(isConfirmDirty))
								result = false;
						}

						result &= value == pwd.getValue();
						if (result)
							return true;

						return Res.Password_Error_Msg;

					}))
				.enableKeyEvents(true)
				.listeners(new Dictionary("keydown", new GenericTwoArgDelegate(PwdFieldKeyDown)))
				.custom("initialPwdField", _newPassword)
				.ToDictionary());

			Panel mainDataPanel = new Panel(new PanelConfig()
				.items(new object[]
				{
					_oldPassword,
					_newPassword,
					_confirmPassword
				}).layout("form")
				.ToDictionary());

			Form.layout = "column";
			Form.add(mainDataPanel);
		}

		private static void PwdFieldKeyDown(object field, object e)
		{
			if (!(bool)Type.InvokeMethod(e, "isSpecialKey"))
				Type.SetField(field, _fieldDirtyName, true);
		}

		protected override void OnSave()
		{
			UserService.ChangeUserPassword(
				_oldPassword.getValue().ToString(),
				_newPassword.getValue().ToString(),
				delegate(object result)
				{
					bool isSuccessed = (bool) result;

					if (isSuccessed)
					{
						MessageRegister.Info(Res.Profile_Item, Res.Password_Updated_Successfully);

						CompleteSave(result);
					}
					else
					{
						_oldPassword.markInvalid();
					}
				},
				null);
		}

		public void Open()
		{
			Window.show();
		}

		private readonly Button _saveButton;
		private readonly Button _cancelButton;
		private Field _oldPassword;
		private Field _newPassword;
		private Field _confirmPassword;

		private const string _fieldDirtyName = "isFieldDirty";
	}
}