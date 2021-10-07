using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain
{
	public class CurrencyMap : Entity3Mapping<Currency>
	{
		public CurrencyMap()
		{
			Cache(m => m.Usage(CacheUsage.ReadWrite));

			Property(x => x.Code, m => { m.Unique(true); m.NotNullable(true); m.Length(3); });

			Property(x => x.CyrillicCode, m => { m.Unique(true); m.Length(3); });

			Property(x => x.NumericCode, m => { m.Unique(true); m.NotNullable(true); });
		}
	}
}
