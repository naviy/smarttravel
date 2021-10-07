using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;

using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public partial class GalileoRailXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.GalileoRailXmlFile;


		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return GalileoRailXmlParser.Parse(
				Content,
				db.Configuration.DefaultCurrency
			).ToArray();
		}


		public new class Service : Service<GalileoRailXmlFile>
		{
		}

	}


	public class GalileoRailXmlFileMap : SubclassMapping<GalileoRailXmlFile>
	{
		public GalileoRailXmlFileMap()
		{
			DiscriminatorValue("GalileoRailXml");
		}
	}

}