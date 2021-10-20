using System.Collections.Generic;
using System.Linq;

using DelegateDecompiler;

using Luxena.Base.Metamodel;
using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	//===g






	public enum PartyType { Department, Organization, Person }






	[GenericPrivileges(Replace = new object[] { UserRole.Administrator, UserRole.Supervisor })]
	public abstract partial class Party : Entity3
	{

		//---g



		public abstract PartyType Type { get; }

		[RU("Официальное название")]
		public virtual string LegalName { get; set; }

		[RU("Подпись")]
		public virtual string Signature { get; set; }


		[Patterns.Code]
		public virtual string Code { get; set; }


		[Computed, Hidden(false)]
		public virtual string NameForDocuments => LegalName ?? Name;


		[RU("№ бонусной карты")]
		public virtual string BonusCardNumber { get; set; }

		[RU("Накоплено бонусов")]
		public virtual decimal? BonusAmount { get; set; }


		[RU("Телефон 1")]
		public virtual string Phone1 { get; set; }

		[RU("Телефон 2")]
		public virtual string Phone2 { get; set; }

		[RU("Факс")]
		public virtual string Fax { get; set; }

		[RU("E-mail 1")]
		public virtual string Email1 { get; set; }

		[RU("E-mail 2")]
		public virtual string Email2 { get; set; }

		[RU("Веб адрес")]
		public virtual string WebAddress { get; set; }

		[RU("Заказчик")]
		public virtual bool IsCustomer { get; set; }

		[RU("Не может быть заказчиком")]
		public virtual bool CanNotBeCustomer { get; set; }

		[RU("Поставщик")]
		public virtual bool IsSupplier { get; set; }

		[RU("Подчиняется")]
		public virtual Party ReportsTo { get; set; }

		[RU("Банковский счёт агенства по умолчанию", ruDesc: "По умолчаничанию оплачивать через банковский счёт агенства")]
		public virtual BankAccount DefaultBankAccount { get; set; }

		[RU("Дополнительная информация")]
		[Text]
		public virtual string Details { get; set; }

		[RU("Юридический адрес")]
		[Text(4)]
		public virtual string LegalAddress { get; set; }

		[RU("Фактический адрес")]
		[Text(4)]
		public virtual string ActualAddress { get; set; }

		[Patterns.Note, LineCount(6)]
		public virtual string Note { get; set; }

		[RU("Добавлять окончание к номеру счёта")]
		public virtual string InvoiceSuffix { get; set; }


		public virtual IList<File> Files
		{
			get => _files;
			set => _files = value;
		}
		private IList<File> _files = new List<File>();



		//---g



		public override string ToString()
		{
			return Name;
		}



		//---g

	}






	//===g






	public static class PartyExtentions
	{

		//---g



		[Computed]
		public static IEnumerable<object> GetContacts(this Party r)
		{

			return new[]
			{
				r.Phone1.As(a => new { Type = "Phone", Text = a, }),
				r.Phone2.As(a => new { Type = "Phone", Text = a, }),
				r.Fax.As(a => new { Type = "Fax", Text = a, }),
				r.Email1.As(a => new { Type = "Email", Text = a, }),
				r.Email2.As(a => new { Type = "Email", Text = a, }),
				r.WebAddress.As(a => new { Type = "Site", Text = a, }),
			}.Where(a => a != null);

		}



		//---g

	}






	//===g



}