using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Персона", "Персоны"), Icon("user")]
	[AgentPrivileges]
	public partial class Person : Party
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<Person> bag)
		{
			bag.For(a => a.Name)
				.RU("Ф.И.О.")
				.Length(20);
		}

		public override PartyType Type => PartyType.Person;

		[RU("Номера мильных карт")]
		public string MilesCardsString { get; set; }

		[RU("Дата рождения")]
		public DateTime? Birthday { get; set; }

		protected Organization _Organization;

		[RU("Должность")]
		public string Title { get; set; }

		[RU("№ бонусной карты")]
		public string BonusCardNumber { get; set; }


		public virtual ICollection<MilesCard> MilesCards { get; set; }

		public virtual ICollection<Passport> Passports { get; set; }

		public virtual ICollection<DocumentAccess> DocumentAccesses { get; set; }

		public virtual ICollection<GdsAgent> GdsAgents { get; set; }


		public Party GetDefaultOwner(Domain domain)
		{
			this.Domain(domain);

			var owners = db.DocumentAccesses.Where(a => a.PersonId == Id).Select(a => a.Owner).ToList();

			return owners.Count == 1 ? owners[0] : owners.No() ? db.DefaultOwner : null;
		}


		public override void Calculate()
		{
			base.Calculate();

			MilesCardsString = MilesCards.AsSelect(a => a.Number).Join(", ");
		}


		static partial void Config_(Domain.EntityConfiguration<Person> entity)
		{
			entity.Association(a => a.Organization);
		}

	}


	partial class Domain
	{
		public DbSet<Person> Persons { get; set; }
	}

}