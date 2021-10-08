using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public class SabreFilFileMap : SubclassMapping<SabreFilFile>
	{
		public SabreFilFileMap()
		{
			DiscriminatorValue("SabreFil");
		}
	}

}

