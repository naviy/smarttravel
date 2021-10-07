using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Avia
{
	public class SirenaFileMap : SubclassMapping<SirenaFile>
	{
		public SirenaFileMap()
		{
			DiscriminatorValue("Sirena");
		}
	}
}