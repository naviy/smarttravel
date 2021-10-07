using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.NHibernate.Mapping
{
	public class MoneyMapping : ComponentMapping<Money>
	{
		public MoneyMapping()
		{
			ManyToOne(m => m.Currency);
			Property(m => m.Amount);
		}
	}
}