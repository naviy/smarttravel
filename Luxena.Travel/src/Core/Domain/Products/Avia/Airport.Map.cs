using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Avia
{

	public class AirportMap : Entity3Mapping<Airport>
	{
		public AirportMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Property(x => x.Code, m =>
			{
				m.Length(3);
				m.Unique(true);
				m.NotNullable(true);
			});
			ManyToOne(x => x.Country);
			Property(x => x.Settlement, m => m.Length(200));
			Property(x => x.LocalizedSettlement, m => m.Length(200));
			Property(x => x.Longitude);
			Property(x => x.Latitude);
		}
	}

}