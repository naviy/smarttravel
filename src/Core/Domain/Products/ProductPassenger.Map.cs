using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class ProductPassengerMap : Entity2Mapping<ProductPassenger>
	{

		public ProductPassengerMap()
		{
			ManyToOne(x => x.Product);

			Property(x => x.PassengerName, m => m.Length(100));
			ManyToOne(x => x.Passenger);
		}

	}

}