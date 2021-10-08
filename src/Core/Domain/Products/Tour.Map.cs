using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class TourMap : SubEntityMapping<Tour>
	{
		public TourMap()
		{
			DiscriminatorValue(ProductType.Tour);

			Property(x => x.StartDate, m => m.NotNullable(true));
			Property(x => x.FinishDate);

			Property(x => x.HotelName);
			Property(x => x.HotelOffice);
			Property(x => x.HotelCode);

			Property(x => x.PlacementName);
			Property(x => x.PlacementOffice);
			Property(x => x.PlacementCode);

			ManyToOne(x => x.AccommodationType);
			ManyToOne(x => x.CateringType);

			Property(x => x.AviaDescription);
			Property(x => x.TransferDescription);
		}
	}

}