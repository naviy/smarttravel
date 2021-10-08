using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Accounting
{

	public class CashOutOrderPaymentMap : SubclassMapping<CashOutOrderPayment>
	{
		public CashOutOrderPaymentMap()
		{
			DiscriminatorValue("CashOutOrder");
		}
	}

}