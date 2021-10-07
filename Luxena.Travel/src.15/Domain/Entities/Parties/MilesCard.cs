using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Мильная карта", "Мильные карты"), Icon("desktop")]
	[AgentPrivileges]
	public partial class MilesCard : Entity2
	{

		[Patterns.Owner, UiRequired]
		protected Person _Owner;

		[EntityName, Patterns.Number]
		public string Number { get; set; }

		protected Organization _Organization;


		static partial void Config_(Domain.EntityConfiguration<MilesCard> entity)
		{
			entity.Association(a => a.Owner, a => a.MilesCards);
			entity.Association(a => a.Organization, a => a.MilesCards);
		}

	}


	partial class Domain
	{
		public DbSet<MilesCard> MilesCards { get; set; }
	}

}