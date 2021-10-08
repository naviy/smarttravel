using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Аренда автомобиля", "Аренды автомобилей")]
	public partial class CarRental : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<CarRental> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<CarRental>();

			se.For(a => a.Provider)
				.Suggest<CarRentalProvider>();
		}

		public override ProductType Type { get { return ProductType.CarRental; } }

		public override string Name { get { return DomainRes.CarRental; } }

		public override string PassengerName { get { return GetPassengerNames(); } }

		[Patterns.StartDate, Required]
		public virtual DateTime? StartDate { get; set; }

		[Patterns.FinishDate]
		public virtual DateTime? FinishDate { get; set; }

		[RU("Марка авто")]
		public virtual string CarBrand { get; set; }


		public new partial class Service : Service<CarRental>
		{

		}

	}

}
