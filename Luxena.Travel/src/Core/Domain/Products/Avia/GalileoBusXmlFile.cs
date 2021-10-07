using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;

using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public partial class GalileoBusXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.GalileoBusXmlFile;


		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return GalileoBusXmlParser.Parse(
				Content,
				db.Configuration.DefaultCurrency
			).ToArray();
		}


		public new class Service : Service<GalileoBusXmlFile>
		{
		}

	}


	public class GalileoBusXmlFileMap : SubclassMapping<GalileoBusXmlFile>
	{
		public GalileoBusXmlFileMap()
		{
			DiscriminatorValue("GalileoBusXml");
		}
	}

}