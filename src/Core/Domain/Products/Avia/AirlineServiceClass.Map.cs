using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{
	public class AirlineServiceClassMap : Entity2Mapping<AirlineServiceClass>
	{
		public AirlineServiceClassMap()
		{
			ManyToOne(x => x.Airline, m => m.NotNullable(true));
			Property(x => x.Code, m => { m.Length(1); m.NotNullable(true); });
			Property(x => x.ServiceClass, x => x.NotNullable(true));
		}
	}
}
