using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;
using Luxena.Travel.Services;


namespace Luxena.Travel.Domain
{

	public partial class GalileoXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.GalileoXmlFile;


		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return GalileoAviaXmlParser.Parse(
				Content,
				db.Configuration.DefaultCurrency, 
				GalileoWebServiceTask.GlobalRobots?.Split(',').Clip()
			).ToArray();
		}


		public new class Service : Service<GalileoXmlFile>
		{
		}

	}

}