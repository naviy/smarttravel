using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Avia
{
	public class AirFileMap : SubclassMapping<AirFile>
	{
		public AirFileMap()
		{
			DiscriminatorValue("Air");
		}
	}
}

