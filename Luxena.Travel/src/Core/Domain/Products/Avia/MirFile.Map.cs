using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Avia
{
	public class MirFileMap : SubclassMapping<MirFile>
	{
		public MirFileMap()
		{
			DiscriminatorValue("Mir");
		}
	}
}