using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Аренда автомобиля", "Аренды автомобилей"), Icon("car")]
	[UA("Оренда автомобіля")]
	public partial class CarRental : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<CarRental> se)
		{
			se.For(a => a.ReissueFor)
				.Lookup<CarRental>();

			se.For(a => a.Provider)
				.Lookup<CarRentalProvider>();
		}

		public override ProductType Type => ProductType.CarRental;

		[RU("Марка авто")]
		public string CarBrand { get; set; }

	}


	partial class Domain
	{
		public DbSet<CarRental> CarRentals { get; set; }
	}

}
