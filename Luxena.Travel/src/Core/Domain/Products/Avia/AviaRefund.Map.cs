using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.Avia
{

	public class AviaRefundMap : SubEntityMapping<AviaRefund>
	{
		public AviaRefundMap()
		{
			DiscriminatorValue(ProductType.AviaRefund);
		}
	}

}