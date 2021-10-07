using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Accounting
{

	public class CashInOrderPaymentMap : SubclassMapping<CashInOrderPayment>
	{
		public CashInOrderPaymentMap()
		{
			DiscriminatorValue("CashOrder");
		}
	}

}