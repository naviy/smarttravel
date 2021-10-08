using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.Avia
{
	public class AviaTicketMap : SubEntityMapping<AviaTicket>
	{
		public AviaTicketMap()
		{
			DiscriminatorValue(ProductType.AviaTicket);

			Property(x => x.Domestic, m => m.NotNullable(true));
			Property(x => x.Interline, m => m.NotNullable(true));
			Property(x => x.SegmentClasses, m => m.Length(50));
			Property(x => x.Departure);
			Property(x => x.Endorsement, m => m.Length(150));

			BagAggregate(x => x.Segments, i => i.Ticket, x => x.Position);

			BagAggregate(x => x.PenalizeOperations, i => i.Ticket);

			Component(x => x.FareTotal);
		}
	}
}