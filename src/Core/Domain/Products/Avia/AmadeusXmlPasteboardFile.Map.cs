using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Avia
{
	public class AmadeusXmlFileMap : SubclassMapping<AmadeusXmlFile>
	{
		public AmadeusXmlFileMap()
		{
			DiscriminatorValue("AmadeusXml");
		}
	}
}

