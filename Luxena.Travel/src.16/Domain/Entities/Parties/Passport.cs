using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Паспорт", "Паспорта"), Icon("certificate")]
	[AgentPrivileges]
	public partial class Passport : Entity2
	{
		[Patterns.Owner, Required]
		protected Person _Owner;

		[EntityName]
		[Patterns.Number]
		public string Number { get; set; }

		[RU("Имя")]
		public string FirstName { get; set; }

		[RU("Отчество")]
		public string MiddleName { get; set; }

		[RU("Фамилия")]
		public string LastName { get; set; }

		[RU("Ф.И.О.")]
		public string Name { get { return string.Join(" ", new[] { LastName, FirstName, MiddleName }.Where(a => a.Yes())); } }


		[RU("Гражданство")]
		protected Country _Citizenship;

		[RU("Дата рождения")]
		public DateTime? Birthday { get; set; }

		[RU("Пол")]
		public Gender? Gender { get; set; }


		[RU("Выдавшая страна")]
		protected Country _IssuedBy;

		[RU("Действителен до")]
		public DateTime? ExpiredOn { get; set; }

		[Patterns.Note, Text]
		public string Note { get; set; }


		[RU("Данные для Amadeus")]
		public string AmadeusString => 
			$"SRDOCS HK1-P-{Citizenship.As(a => a.ThreeCharCode)}-{Number}-{IssuedBy.As(a => a.ThreeCharCode)}-{Birthday.AsString("ddMMMyy", _formatCulture)}-{GenderString}-{ExpiredOn.AsString("ddMMMyy", _formatCulture)}-{LastName}-{FirstName}-{MiddleName}".TrimEnd('-');

		[RU("Данные для Galileo")]
		public string GalileoString => 
			$"SI.P1/SSRDOCSYYHK1/P/{Citizenship.As(a => a.TwoCharCode)}/{Number}/{IssuedBy.As(a => a.TwoCharCode)}/{Birthday.AsString("ddMMMyy", _formatCulture)}/{GenderString}/{ExpiredOn.AsString("ddMMMyy", _formatCulture)}/{LastName}/{FirstName}/{MiddleName}".TrimEnd('/');

		private string GenderString => 
			Gender.HasValue ? (Gender == Travel.Domain.Gender.Male ? "M" : "F") : string.Empty;


		private static readonly CultureInfo _formatCulture = CultureInfo.CreateSpecificCulture("en-US");


		static partial void Config_(Domain.EntityConfiguration<Passport> entity)
		{
			entity.Association(a => a.Owner, a => a.Passports);
			entity.Association(a => a.Citizenship);//, a => a.Passports_Citizenship);
			entity.Association(a => a.IssuedBy);//, a => a.Passports_IssuedBy);
		}

	}


	partial class Domain
	{
		public DbSet<Passport> Passports { get; set; }
	}

}