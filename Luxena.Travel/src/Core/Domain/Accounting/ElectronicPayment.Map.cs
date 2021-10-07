using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Accounting
{
	public class ElectronicPaymentMap : SubclassMapping<ElectronicPayment>
	{
		public ElectronicPaymentMap()
		{
			DiscriminatorValue("Electronic");

			Property(x => x.AuthorizationCode, m => m.Length(50));
		}
	}
}