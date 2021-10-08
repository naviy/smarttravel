using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Accounting
{
	public class OpeningBalanceMap : Entity2Mapping<OpeningBalance>
	{
		public OpeningBalanceMap()
		{
			Cache(x=>x.Usage(CacheUsage.ReadWrite));

			Property(x => x.Number, m => { m.Length(20); m.NotNullable(true); m.Unique(true); });

			Property(x => x.Date, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); m.UniqueKey("business key");});

			ManyToOne(x => x.Party, m => { m.NotNullable(true); m.UniqueKey("business key");});

			Property(x => x.Balance, m => m.NotNullable(true));
		}
	}
}