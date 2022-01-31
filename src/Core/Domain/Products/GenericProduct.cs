using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Дополнительная услуга", "Дополнительные услуги")]
	public partial class GenericProduct : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<GenericProduct> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<GenericProduct>();

			se.For(a => a.Provider)
				.Suggest<GenericProductProvider>();
		}

		public override ProductType Type => ProductType.GenericProduct;

		public override string Name => (GenericType != null ? GenericType.Name : DomainRes.GenericProduct) +
			(Number.Yes() ? " #" + Number : "");

		public override string PassengerName => GetPassengerNames();

		[RU("Вид услуги"), Required]
		public virtual GenericProductType GenericType { get; set; }

		[Patterns.Number]
		public virtual string Number { get; set; }

		[Patterns.StartDate]
		public virtual DateTime? StartDate { get; set; }

		[Patterns.FinishDate]
		public virtual DateTime? FinishDate { get; set; }


		public new partial class Service : Service<GenericProduct>
		{

		}

	}

}
