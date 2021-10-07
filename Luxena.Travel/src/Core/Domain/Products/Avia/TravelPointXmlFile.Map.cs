using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public class TravelPointXmlFileMap : SubclassMapping<TravelPointXmlFile>
	{
		public TravelPointXmlFileMap()
		{
			DiscriminatorValue("TravelPointXml");
		}
	}

}