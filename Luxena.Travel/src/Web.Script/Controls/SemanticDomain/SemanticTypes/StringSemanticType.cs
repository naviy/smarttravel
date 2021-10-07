using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext.form;

using LxnBase;


namespace Luxena.Travel
{

	public delegate object FieldValidateDelegate1(object value);


	public class StringSemanticType : SemanticType
	{

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			TextFieldConfig config = NewConfig(form, sm);

			int maxLength = sm._maxLength;

			if (maxLength > 0)
			{
				config.maxLength(maxLength);

				if (!Script.IsValue(width) && sm._lineCount == 0)
				{
					width = maxLength * 6;

					if (width < MinWidth)
						width = MinWidth;
					else if (width > MaxWidth)
						width = MaxWidth;

					if (width > form.FieldMaxWidth)
						width = form.FieldMaxWidth;
				}
			}

			InitTextFieldConfig(form, sm, width, config);

			if (sm._lineCount != 0)
			{
				config.height(100);
			}

			FormMember member = form.Members.Add2(form, sm, config, initMember);

			return member.SetField(sm._lineCount != 0 
				? new TextArea(config.ToDictionary()) 
				: new TextField(config.ToDictionary())
			);
		}

		protected virtual TextFieldConfig NewConfig(IEditForm form, SemanticMember sm)
		{
			return new TextFieldConfig();
		}

	}


	public class PasswordSemanticType : StringSemanticType
	{

		protected override TextFieldConfig NewConfig(IEditForm form, SemanticMember sm)
		{
			return new TextFieldConfig()
				.selectOnFocus(true)
				.allowBlank(false)
				.enableKeyEvents(true)
				.listeners(new Dictionary("keydown", new GenericTwoArgDelegate(PwdFieldKeyDown)))
				.inputType("password");
		}

		private static void PwdFieldKeyDown(object field, object e)
		{
			if (!(bool)Type.InvokeMethod(e, "isSpecialKey"))
				Type.SetField(field, "isFieldDirty", true);
		}

	}


	public class ConfirmPasswordSemanticType : PasswordSemanticType
	{
		protected override TextFieldConfig NewConfig(IEditForm form, SemanticMember sm)
		{
			return base.NewConfig(form, sm)
				.validator(new FieldValidateDelegate1(
					delegate(object value)
					{
						object password = form.GetCurrentValue(sm._passwordFieldName);

						return value == password ? (object)true : Res.Password_Error_Msg;
					}));
		}
	}


	public static partial class ViewTypes
	{
		public static SemanticType String = new StringSemanticType();
		public static SemanticType Password = new PasswordSemanticType();
		public static SemanticType ConfirmPassword = new ConfirmPasswordSemanticType();
	}


	public partial class SemanticMember
	{
		[AlternateSignature]
		public extern SemanticMember String();

		public SemanticMember String(int maxLength)
		{
			_type = ViewTypes.String;
			_maxLength = Script.IsNullOrUndefined(maxLength) ? 0 : maxLength;
			return this;
		}

		public SemanticMember Text(int lineCount)
		{
			_type = ViewTypes.String;
			_lineCount = lineCount > 0 ? lineCount : 3;
			return this;
		}

		public SemanticMember Password()
		{
			_type = ViewTypes.Password;
			return this;
		}


		public string _passwordFieldName;

		[AlternateSignature]
		public extern SemanticMember ConfirmPassword();

		public SemanticMember ConfirmPassword(string passwordFieldName)
		{
			_type = ViewTypes.ConfirmPassword;
			_passwordFieldName = passwordFieldName ?? "Password";
			return this;
		}

	}

}