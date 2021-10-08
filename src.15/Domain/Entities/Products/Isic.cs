using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Студенческий билет", "Студенческие билеты"), Icon("graduation-cap")]
	[UA("Студентський квиток")]
	public partial class Isic : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Isic> se)
		{
			se.For(a => a.ReissueFor)
				.Lookup<Isic>();
		}

		public override ProductType Type => ProductType.Isic;


		[RU("Тип карты"), Required, DefaultValue(1)]
		public IsicCardType CardType { get; set; }

		[Patterns.Number, MaxLength(12), Required]
		public string Number1 { get; set; }

		[Patterns.Number, MaxLength(1), Required]
		public string Number2 { get; set; }

	}


	partial class Domain
	{
		public DbSet<Isic> Isics { get; set; }
	}

}
