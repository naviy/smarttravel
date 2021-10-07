using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public class DrctXmlFileMap : SubclassMapping<DrctXmlFile>
	{
		public DrctXmlFileMap()
		{
			DiscriminatorValue("DrctXml");
		}
	}

}