using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Экскурсия", "Экскурсии"), Icon("photo")]
	[UA("Екскурсія")]
	public partial class Excursion : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Excursion> se)
		{
			se.For(a => a.ReissueFor)
				.Lookup<Excursion>();
		}

		public override ProductType Type => ProductType.Excursion;


		[RU("Название тура"), Required]
		public string TourName { get; set; }

	}


	partial class Domain
	{
		public DbSet<Excursion> Excursions { get; set; }
	}

}
