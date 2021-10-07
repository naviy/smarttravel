using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Accounting
{
	public class CheckPaymentMap : SubclassMapping<CheckPayment>
	{
		public CheckPaymentMap()
		{
			DiscriminatorValue("Check");
		}
	}
}