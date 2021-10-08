using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain
{
	public class PaymentSystemMap : Entity2Mapping<PaymentSystem>
	{
		public PaymentSystemMap()
		{
			Cache(m => m.Usage(CacheUsage.ReadWrite));

			Property(x => x.Name, m =>
			{
				m.Length(100);
				m.Unique(true);
				m.NotNullable(true);
			});
		}
	}
}