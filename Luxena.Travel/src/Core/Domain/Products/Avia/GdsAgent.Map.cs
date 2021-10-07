using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Avia
{
	public class GdsAgentMap : Entity2Mapping<GdsAgent>
	{
		public GdsAgentMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Property(x => x.Origin, m => { m.NotNullable(true); m.UniqueKey("business-key"); });
			Property(x => x.Code, m => { m.Length(20); m.NotNullable(true); m.UniqueKey("business-key"); });
			Property(x => x.OfficeCode, m => { m.Length(20); m.NotNullable(true); m.UniqueKey("business-key"); });

			ManyToOne(x => x.Person, m => m.NotNullable(true));
			ManyToOne(x => x.Provider);
			ManyToOne(x => x.LegalEntity);
			ManyToOne(x => x.Office);
		}
	}
}
