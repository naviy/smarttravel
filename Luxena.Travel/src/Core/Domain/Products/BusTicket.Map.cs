using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class BusTicketMap : SubEntityMapping<BusTicket>
	{
		public BusTicketMap()
		{
			DiscriminatorValue(ProductType.BusTicket);

			Property(x => x.Number);

			Property(x => x.DeparturePlace);
			Property(x => x.DepartureDate);
			Property(x => x.DepartureTime);

			Property(x => x.ArrivalPlace);
			Property(x => x.ArrivalDate);
			Property(x => x.ArrivalTime);

			Property(x => x.SeatNumber);
		}
	}
	
	public class BusTicketRefundMap : SubEntityMapping<BusTicketRefund>
	{
		public BusTicketRefundMap()
		{
			DiscriminatorValue(ProductType.BusTicketRefund);
		}
	}

}