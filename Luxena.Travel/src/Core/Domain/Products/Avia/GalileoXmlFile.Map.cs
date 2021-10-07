using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public class GalileoXmlFileMap : SubclassMapping<GalileoXmlFile>
	{
		public GalileoXmlFileMap()
		{
			DiscriminatorValue("GalileoXml");
		}
	}

}

