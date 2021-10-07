using System;
using System.Globalization;

using Luxena.Base.Domain;


namespace Luxena.Travel.Domain
{

	[AgentPrivileges]
	[RU("Паспорт", "Паспорта")]
	public partial class Passport : Entity2
	{
		[RU("")]
		public virtual Person Owner { get; set; }

		[EntityName]
		[Patterns.Number]
		public virtual string Number { get; set; }

		[RU("Имя")]
		public virtual string FirstName { get; set; }

		[RU("Отчество")]
		public virtual string MiddleName { get; set; }

		[RU("Фамилия")]
		public virtual string LastName { get; set; }

		[RU("Гражданство")]
		public virtual Country Citizenship { get; set; }

		[RU("Дата рождения")]
		public virtual DateTime? Birthday { get; set; }

		[RU("Пол")]
		public virtual Gender? Gender { get; set; }


		[RU("Выдавшая страна")]
		public virtual Country IssuedBy { get; set; }

		[RU("Действителен до")]
		public virtual DateTime? ExpiredOn { get; set; }

		[Patterns.Note]
		public virtual string Note { get; set; }

		[RU("Данные для Amadeus")]
		public virtual string AmadeusString =>
			$"SRDOCS HK1-P-{Citizenship?.ThreeCharCode}-{Number}-{IssuedBy?.ThreeCharCode}-{Birthday.AsString("ddMMMyy", _formatCulture)}-{GenderString}-{ExpiredOn.AsString("ddMMMyy", _formatCulture)}-{LastName}-{FirstName}-{MiddleName}".TrimEnd('-');

		[RU("Данные для Galileo")]
		public virtual string GalileoString =>
			$"SI.P1/SSRDOCSYYHK1/P/{Citizenship?.TwoCharCode}/{Number}/{IssuedBy?.TwoCharCode}/{Birthday.AsString("ddMMMyy", _formatCulture)}/{GenderString}/{ExpiredOn.AsString("ddMMMyy", _formatCulture)}/{LastName}/{FirstName}/{MiddleName}".TrimEnd('/');

		private string GenderString =>
			Gender.HasValue ? (Gender == Travel.Domain.Gender.Male ? "M" : "F") : string.Empty;


		private static readonly CultureInfo _formatCulture = CultureInfo.CreateSpecificCulture("en-US");


		public class Service : Entity2Service<Passport>
		{

			public Passport ByNumber(string number)
			{
				return number.No() ? null : By(p => p.Number == number);
			}

			public Service()
			{
				Validating += r =>
				{
					//var id = r.Id;
					//var number = r.Number;
					//var issuedBy = r.IssuedBy?.Id;
					//var owner = r.Owner?.Id;

					//var isNew = db.IsNew(r);
					//var old = isNew
					//	? db.Passport.Query.By(a => a.Number == number && a.IssuedBy.Id == issuedBy && r.Owner.Id != owner)
					//	: db.Passport.Query.By(a => a.Number == number && a.IssuedBy.Id == issuedBy && a.Id != id);

					//if (old != null)
					//	throw new DomainException(@"Паспорт с таким номером и выдавшей страной уже добавлен" + old.Owner.As(a => " для персоны " + a.Name));
					//P3578655 02.07.2019
				};
			}

		}

	}
}