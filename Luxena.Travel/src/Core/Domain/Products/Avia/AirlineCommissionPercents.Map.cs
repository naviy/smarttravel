using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Avia
{
	public class AirlineCommissionPercentsMap : ClassMapping<AirlineCommissionPercents>
	{
		public AirlineCommissionPercentsMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Id(x => x.Id, Uuid.Mapping);

			Version(x => x.Version, m => { });

			ManyToOne(x => x.Airline);

			Property(x => x.Domestic, m => m.NotNullable(true));

			Property(x => x.InterlineDomestic, m => m.NotNullable(true));

			Property(x => x.InterlineInternational, m => m.NotNullable(true));

			Property(x => x.International, m => m.NotNullable(true));
		}
	}
}