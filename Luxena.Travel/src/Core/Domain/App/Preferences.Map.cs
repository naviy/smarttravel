using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
{
	public class PreferencesMap : Entity2Mapping<Preferences>
	{
		public PreferencesMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			ManyToOne(x => x.Identity, m => m.NotNullable(true));
		}
	}
}
