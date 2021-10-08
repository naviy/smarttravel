using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Mapping
{
	public class MilesCardMap : Entity2Mapping<MilesCard>
	{
		public MilesCardMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			ManyToOne(x => x.Owner, m => m.NotNullable(true));
			Property(x => x.Number, m => { m.Length(20); m.NotNullable(true); });
			ManyToOne(x => x.Organization);
		}
	}
}