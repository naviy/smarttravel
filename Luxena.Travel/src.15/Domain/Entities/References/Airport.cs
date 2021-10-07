using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Аэропорт", "Аэропорты"), Icon("road")]
	[SupervisorPrivileges]
	public partial class Airport : Entity3
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<Airport> se)
		{
			se.For(a => a.Name)
				.Required()
				.Length(12);
		}

		[Patterns.Code, EntityName2, String(3, 3, 3), Required, Unique]
		public string Code { get; set; }

		[UiRequired]
		protected Country _Country;


		[RU("Населенный пункт (англ.)")]
		public string Settlement { get; set; }

		[RU("Населенный пункт")]
		public string LocalizedSettlement { get; set; }

		[RU("Широта")]
		public double? Latitude { get; set; }

		[RU("Долгота")]
		public double? Longitude { get; set; }


		public override string ToString()
		{
			return Code + Name.As(a => " - " + a);
		}


		public override Domain.Entity Resolve()
		{
			return db.Airports.By(a => a.Code == Code) ?? this.Save(db);
		}


		static partial void Config_(Domain.EntityConfiguration<Airport> entity)
		{
			entity.Association(a => a.Country, a => a.Airports);
		}

	}


	partial class Domain
	{
		public DbSet<Airport> Airports { get; set; }
	}

}