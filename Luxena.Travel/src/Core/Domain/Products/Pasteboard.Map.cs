using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class PasteboardMap : SubEntityMapping<Pasteboard>
	{
		public PasteboardMap()
		{
			DiscriminatorValue(ProductType.Pasteboard);

			Property(x => x.Number, m => m.Length(50));
			Property(x => x.TrainNumber);
			Property(x => x.CarNumber);

			Property(x => x.DeparturePlace);
			Property(x => x.DepartureDate);
			Property(x => x.DepartureTime);

			Property(x => x.ArrivalPlace);
			Property(x => x.ArrivalDate);
			Property(x => x.ArrivalTime);

			Property(x => x.Itinerary, m => m.Length(100));

			Property(x => x.SeatNumber);

			Property(x => x.ServiceClass, m => m.NotNullable(true));
		}
	}

	public class PasteboardRefundMap : SubEntityMapping<PasteboardRefund>
	{
		public PasteboardRefundMap()
		{
			DiscriminatorValue(ProductType.PasteboardRefund);
		}
	}

}