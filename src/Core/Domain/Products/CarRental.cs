using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("Аренда автомобиля", "Аренды автомобилей")]
	public partial class CarRental : Product
	{

		//---g



		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<CarRental> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<CarRental>();

			se.For(a => a.Provider)
				.Suggest<CarRentalProvider>();
		}



		//---g



		public override ProductType Type => ProductType.CarRental;

		public override string Name => DomainRes.CarRental;

		public override string PassengerName => GetPassengerNames();

		[Patterns.StartDate, Required]
		public virtual DateTime? StartDate { get; set; }

		[Patterns.FinishDate]
		public virtual DateTime? FinishDate { get; set; }

		[RU("Марка авто")]
		public virtual string CarBrand { get; set; }



		//---g


		public new partial class Service : Service<CarRental>
		{

		}




		//---g

	}






	//===g



}
