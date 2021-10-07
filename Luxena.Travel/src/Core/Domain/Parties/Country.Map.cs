using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;
using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain
{
	public class CountryMap : Entity2Mapping<Country>
	{
		public CountryMap()
		{
			Cache(m => m.Usage(CacheUsage.ReadWrite));

			Property(x => x.Name, m => { m.Length(100); m.NotNullable(true); m.Unique(true); });
			Property(x => x.TwoCharCode, m => { m.Length(2); m.Unique(true); });
			Property(x => x.ThreeCharCode, m => { m.Length(3); m.Unique(true); });
			Property(x => x.Note, m => m.Type(NHibernateUtil.StringClob));

		}
	}
}
