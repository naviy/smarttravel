using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;

using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public partial class LuxenaXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.LuxenaXmlFile;


		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return LuxenaXmlParser.Parse(db, Content, db.Configuration.DefaultCurrency).ToArray();
		}


		public new class Service : Service<LuxenaXmlFile>
		{
		}

	}


	public class LuxenaXmlFileMap : SubclassMapping<LuxenaXmlFile>
	{
		public LuxenaXmlFileMap()
		{
			DiscriminatorValue("LuxenaXml");
		}
	}

}