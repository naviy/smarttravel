using System;
using System.Collections;

using Ext;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;


namespace Luxena.Travel
{
	public class PassportCompareForm : BaseEditForm
	{
		public PassportCompareForm(PassportDto personPassport, PassportDto gdsPassport)
		{
			_personPassport = personPassport;
			_gdsPassport = gdsPassport;
		}

		public void Open()
		{
			Window.setTitle(Res.PassportCompare_Caption);
			Window.addClass("passport-compare");

			StringBuilder builder = new StringBuilder();

			builder.Append(string.Format(@"
				<table class='passports'>
					<tr>
						<td></td>
						<td class='main-caption'>{0}</td>
						<td class='main-caption'>{1}</td>
					</tr>", Res.PassportCompare_PersonPassport, Res.PassportCompare_GdsPassport));

			AddRow(builder, DomainRes.Common_Number, _personPassport.Number, _gdsPassport.Number, !string.Equals(_personPassport.Number, _gdsPassport.Number, true));
			AddRow(builder, DomainRes.Passport_LastName, _personPassport.LastName, _gdsPassport.LastName, !string.Equals(_personPassport.LastName,_gdsPassport.LastName, true));
			AddRow(builder, DomainRes.Passport_FirstName, _personPassport.FirstName, _gdsPassport.FirstName, !string.Equals(_personPassport.FirstName, _gdsPassport.FirstName, true));
			AddRow(builder, DomainRes.Passport_MiddleName, _personPassport.MiddleName, _gdsPassport.MiddleName, !string.Equals(_personPassport.MiddleName, _gdsPassport.MiddleName, true));
			AddRow(builder, DomainRes.Passport_Citizenship, StringUtility.ToString(_personPassport.Citizenship), StringUtility.ToString(_gdsPassport.Citizenship), !Reference.Equals(_personPassport.Citizenship, _gdsPassport.Citizenship));
			AddRow(builder, DomainRes.Passport_Birthday, StringUtility.ToString(_personPassport.Birthday), StringUtility.ToString(_gdsPassport.Birthday), StringUtility.ToString(_personPassport.Birthday) != StringUtility.ToString(_gdsPassport.Birthday));
			AddRow(builder, DomainRes.Passport_Gender, EnumUtility.Localize(typeof(Gender), (Gender)_personPassport.Gender, typeof(DomainRes)), EnumUtility.Localize(typeof(Gender), (Gender)_gdsPassport.Gender, typeof(DomainRes)), _personPassport.Gender != _gdsPassport.Gender);
			AddRow(builder, DomainRes.Passport_IssuedBy, StringUtility.ToString(_personPassport.IssuedBy), StringUtility.ToString(_gdsPassport.IssuedBy), !Reference.Equals(_personPassport.IssuedBy, _gdsPassport.IssuedBy));
			AddRow(builder, DomainRes.Passport_ExpiredOn, StringUtility.ToString(_personPassport.ExpiredOn), StringUtility.ToString(_gdsPassport.ExpiredOn), StringUtility.ToString(_personPassport.ExpiredOn) != StringUtility.ToString(_gdsPassport.ExpiredOn));

			builder.Append("</table>");

			BoxComponent box = new BoxComponent(new BoxComponentConfig()
				.autoEl(new Dictionary("html", builder.ToString()))
				.cls("passport-data")
				.ToDictionary());

			Form.add(box);

			Button button1 = Form.addButton(new ButtonConfig()
				.text(Res.PassportCompare_UsePersonPassport)
				.handler(new AnonymousDelegate(delegate { CompleteSave(true); }))
				.ToDictionary());

			Button button2 = Form.addButton(new ButtonConfig()
				.text(Res.PassportCompare_UseGdsPassport)
				.handler(new AnonymousDelegate(delegate { CompleteSave(false); }))
				.ToDictionary());

			Button button3 = Form.addButton(new ButtonConfig()
				.text(Res.Cancel)
				.handler(new AnonymousDelegate(Cancel))
				.ToDictionary());

			ComponentSequence = new Component[] { button1, button2, button3 };

			Window.show();
		}

		private static void AddRow(StringBuilder builder, string caption, object value1, object value2, bool different)
		{
			string css = string.Empty;

			if (different)
				css = " different";

			builder.Append(string.Format(@"
					<tr>
						<td class='caption {3}'>{0}:</td>
						<td class='value {3}'>{1}</td>
						<td class='value {3}'>{2}</td>
					</tr>", caption, value1, value2, css));
		}

		protected override void OnSave()
		{
		}

		private readonly PassportDto _personPassport;
		private readonly PassportDto _gdsPassport;
	}
}