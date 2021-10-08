using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Дополнительная услуга", "Дополнительные услуги")]
	[UA("Додаткова послуга")]
	public partial class GenericProduct : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<GenericProduct> se)
		{
			se.For(a => a.ReissueFor)
				.Lookup<GenericProduct>();

			se.For(a => a.Provider)
				.Lookup<GenericProductProvider>();
		}

		public override ProductType Type => ProductType.GenericProduct;

		[RU("Вид услуги"), Required]
		protected GenericProductType _GenericType;

		[Patterns.Number]
		public string Number { get; set; }


		public override string GetOrderItemText(string lang) =>
			GenericType.As(a => a.Name + " ") +
			Number.As(a => Texts.Number_Short[lang] + " " + a + " ") +
			Texts.From[lang] + " " + IssueDate.ToDateString() +
			GetPassengerNames().As(a => ", " + a);



		static partial void Config_(Domain.EntityConfiguration<GenericProduct> entity)
		{
			entity.Association(a => a.GenericType);//, a => a.GenericProducts);
		}

	}


	partial class Domain
	{
		public DbSet<GenericProduct> GenericProducts { get; set; }
	}

}
