using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class CarRentalMap : SubEntityMapping<CarRental>
	{
		public CarRentalMap()
		{
			DiscriminatorValue(ProductType.CarRental);

			Property(x => x.StartDate);
			Property(x => x.FinishDate);
		}
	}

}