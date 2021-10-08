using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
{

	public class IdentityMap : Entity3DMapping<Identity>
	{
		public IdentityMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			BagAggregate(x => x.Preferences, i => i.Identity);
		}
	}

}
