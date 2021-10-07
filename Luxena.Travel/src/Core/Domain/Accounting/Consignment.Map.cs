using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;


namespace Luxena.Travel.Domain.Accounting
{
	public class ConsignmentMap : Entity2Mapping<Consignment>
	{
		public ConsignmentMap()
		{
			Property(x => x.Number, m =>
			{
				m.NotNullable(true);
				m.Unique(true);
				m.Length(20);
			});

			Property(x => x.IssueDate, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); });

			ManyToOne(x => x.Supplier);

			ManyToOne(x => x.Acquirer, m => m.NotNullable(true));

			BagPersist(x => x.OrderItems, i => i.Consignment);

			BagAggregate(x => x.IssuedConsignments, i => i.Consignment);

			Property(x => x.TotalSupplied);

			Component(x => x.GrandTotal);
			Component(x => x.Vat);
			Component(x => x.Discount);
		}
	}
}