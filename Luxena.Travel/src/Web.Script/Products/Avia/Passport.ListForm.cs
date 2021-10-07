using System;
using System.Collections;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.UI;

using Luxena.Travel.Services;


namespace Luxena.Travel
{
	public class PassportListForm : BaseEditForm
	{
		public PassportListForm(GdsOriginator gdsOriginator, string personId)
		{
			_originator = gdsOriginator;
			_personId = personId;
		}

		public void Open()
		{
			PartyService.GetPerson(_personId,
				delegate(object result)
				{
					_person = (PersonDto) result;

					Load();

					Window.show();

				}, null );
		}

		private void Load()
		{
			Window.setTitle(string.Format(Res.Passport_List_Title, _person.Name));
			Window.addClass("passport-list");

			if (_person.Passports == null || _person.Passports.Length == 0)
			{
				Form.add(new Label("no passport"));

				return;
			}

			for (int i = 0; i < _person.Passports.Length; i++)
			{
				PassportDto passport = _person.Passports[i];

				Form.add(GetPassportComponent(passport, i == 0));
			}

			Button button = Form.addButton(new ButtonConfig()
				.text(Res.Close_Action)
				.handler(new AnonymousDelegate(
					delegate
					{
						Window.close();
					}))
				.ToDictionary());

			RegisterFocusComponent(button);
		}

		private Component GetPassportComponent(PassportDto passport, bool first)
		{
			string html = GetPassportHtml(passport);

			BoxComponent box = new BoxComponent(new BoxComponentConfig()
				.autoEl(new Dictionary("html", html))
				.cls("passport-data")
				.ToDictionary());

			TextField textField = new TextField(new TextFieldConfig()
				.readOnly(true)
				.selectOnFocus(true)
				.value(GetGdsString(passport))
				.width(420)
				.ToDictionary());

			RegisterFocusComponent(textField);

			string css = "passport";

			if (first)
				css += " first-item";

			return new Container(new ContainerConfig()
				.items(new object[]
				{
					box,
					textField
				})
				.cls(css)
				.ToDictionary());
		}

		private static string GetPassportHtml(PassportDto passport)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("<div><b>{0}</b></div>", passport.Number));

			sb.Append("<table>");

			StringBuilder name = new StringBuilder();

			if (!string.IsNullOrEmpty(passport.LastName))
				name.Append(passport.LastName);

			if (!string.IsNullOrEmpty(passport.FirstName))
				name.Append(passport.FirstName);

			if (!string.IsNullOrEmpty(passport.MiddleName))
				name.Append(passport.MiddleName);


			StringBuilder personalData = new StringBuilder();

			if (!name.IsEmpty)
				personalData.Append(name.ToString(" "));

			if (!Script.IsNullOrUndefined(passport.Birthday))
				personalData.Append(passport.Birthday.Format("d.m.Y"));

			if (!Script.IsNullOrUndefined(passport.Gender))
				personalData.Append(EnumUtility.Localize(typeof (Gender), (Gender) passport.Gender, typeof (Res)));

			if (passport.Citizenship != null)
				personalData.Append(passport.Citizenship.Name);

			if (!personalData.IsEmpty)
				sb.Append(string.Format("<tr><td class='caption'>{0}: </td><td>{1}</td></tr>", Res.Passport_PrivateData, personalData.ToString(", ")));

			if (passport.IssuedBy != null)
				sb.Append(string.Format("<tr><td class='caption'>{0}: </td><td>{1}</td></tr>", Res.Passport_IssuedBy, passport.IssuedBy.Name));

			if (!Script.IsNullOrUndefined(passport.ExpiredOn))
				sb.Append(string.Format("<tr><td class='caption'>{0}: </td><td>{1}</td></tr>", DomainRes.Passport_ExpiredOn, passport.ExpiredOn.Format("d.m.Y")));

			sb.Append("</table>");

			return sb.ToString();
		}

		private string GetGdsString(PassportDto passport)
		{
			switch (_originator)
			{
				case GdsOriginator.Amadeus:
					return passport.AmadeusString;
				case GdsOriginator.Galileo:
					return passport.GalileoString;
				default:
					return null;
			}
		}

		protected override void OnSave()
		{
		}

		private readonly GdsOriginator _originator;
		private readonly string _personId;

		private PersonDto _person;
	}
}