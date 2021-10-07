using System.Collections.Generic;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public enum PartyType
	{
		[Localization(typeof(Department))]
		Department,

		[Localization(typeof(Organization))]
		Organization,

		[Localization(typeof(Person))]
		Person,
	}


	[RU("Контрагент", "Контрагенты")]
	[GenericPrivileges(Replace2 = UserRole.Supervisor)]
	public abstract partial class Party : Entity3
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup sm)
		{
			sm.For<Party>(a => a.Name)
				.Length(20);
		}

		[EntityType]
		public virtual PartyType Type { get; set; }

		[RU("Официальное название")]
		public string LegalName { get; set; }

		[Patterns.Code, Icon("asterisk")]
		public string Code { get; set; }


		public string NameForDocuments => LegalName ?? Name;

		[RU("Телефон 1"), Phone]
		public string Phone1 { get; set; }

		[RU("Телефон 2"), Phone]
		public string Phone2 { get; set; }

		[RU("Факс"), Fax]
		public string Fax { get; set; }

		[RU("E-mail 1"), Email]
		public string Email1 { get; set; }

		[RU("E-mail 2"), Email]
		public string Email2 { get; set; }

		[RU("Веб адрес"), Hyperlink]
		public string WebAddress { get; set; }

		[RU("Заказчик"), Icon("street-view")]
		public bool IsCustomer { get; set; }

		[RU("Поставщик"), Icon("industry")]
		public bool IsSupplier { get; set; }

		[RU("Подчиняется")]
		protected Party _ReportsTo;

		[RU("На банковский счёт по умолчанию", ruDesc: "По умолчаничанию оплачивать через выбранный банковский счёт агенства")]
		protected BankAccount _DefaultBankAccount;

		[RU("Дополнительная информация"), Text]
		public string Details { get; set; }

		[RU("Юридический адрес"), Address]
		public string LegalAddress { get; set; }

		[RU("Фактический адрес"), Address]
		public string ActualAddress { get; set; }

		[Patterns.Note, Text]
		public string Note { get; set; }


		public virtual ICollection<File> Files { get; set; }
		public int? FileCount => Files?.Count;

		public virtual ICollection<DocumentOwner> DocumentOwners { get; set; }


		static partial void Config_(Domain.EntityConfiguration<Party> entity)
		{
			entity.Association(a => a.ReportsTo);//, a => a.Parties_ReportsTo);
			entity.Association(a => a.DefaultBankAccount);//, a => a.Parties_DefaultBankAccount);
		}


		public string GetDetails(string lang)
		{
			var sb = new StringWrapper();

			if (Details.Yes())
				sb *= Details;

			if (LegalAddress.Yes())
				sb *= Texts.Address[lang] + ": " + LegalAddress;

			if (Phone1.Yes())
				sb += Texts.Phone[lang] + ": " + Phone1 + ", ";

			if (Phone2.Yes())
				sb += Texts.Phone[lang] + ": " + Phone2 + ", ";

			if (Fax.Yes())
				sb += Texts.Fax[lang] + ": " + Fax;

			return sb.ToString().TrimEnd(", ");
		}
	}


	partial class Domain
	{
		public DbSet<Party> Parties { get; set; }
	}


	//public static class PartyExtentions
	//{

	//	public static IEnumerable<object> GetContacts(this Party r)
	//	{
	//		return new[]
	//		{
	//			r.Phone1.As(a => new { Type = "Phone", Text = a, }),
	//			r.Phone2.As(a => new { Type = "Phone", Text = a, }),
	//			r.Fax.As(a => new { Type = "Fax", Text = a, }),
	//			r.Email1.As(a => new { Type = "Email", Text = a, }),
	//			r.Email2.As(a => new { Type = "Email", Text = a, }),
	//			r.WebAddress.As(a => new { Type = "Site", Text = a, }),
	//		}.Where(a => a != null);
	//	}

	//}

}