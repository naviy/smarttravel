using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;
using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Accounting
{

	public class OrderItemMap : Entity2Mapping<OrderItem>
	{
		public OrderItemMap()
		{

			ManyToOne(x => x.Order, m => m.NotNullable(true));

			ManyToOne(x => x.Consignment);

			ManyToOne(x => x.Product, m => m.Lazy(LazyRelation.NoLazy));

			Property(x => x.LinkType);

			Property(x => x.Position, m => m.NotNullable(true));

			Property(x => x.Text, m => { m.NotNullable(true); m.Type(NHibernateUtil.StringClob); });

			Component(x => x.Price);

			Property(x => x.Quantity, m => m.NotNullable(true));

			Component(x => x.Discount);

			Component(x => x.GrandTotal);

			Component(x => x.GivenVat);

			Component(x => x.TaxedTotal);

			Property(x => x.HasVat, m => m.NotNullable(true));

			Property(x => x.IsForceDelivered, m => m.NotNullable(true));

		}
	}

}