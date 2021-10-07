using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Avia
{

	public class AirplaneModelMap : Entity3Mapping<AirplaneModel>
	{
		public AirplaneModelMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Property(x => x.IataCode, m => { m.Length(3); m.Unique(true); m.NotNullable(true); });
			Property(x => x.IcaoCode, m => { m.Length(4); });
		}
	}

}