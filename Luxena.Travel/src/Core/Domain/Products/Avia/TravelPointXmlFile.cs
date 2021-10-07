using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;
using Luxena.Travel.Services;


namespace Luxena.Travel.Domain
{

	public partial class TravelPointXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.TravelPointXmlFile;


		protected override IList<Entity2> ParseProducts(Domain db)
		{
			// return new Entity2[0];

			return TravelPointXmlParser.Parse(
				Content,
				db.Configuration.DefaultCurrency, 
				TravelPointWebServiceTask.GlobalRobots?.Split(',').Clip()
			).ToArray();
		}


		public new class Service : Service<TravelPointXmlFile>
		{
		}

	}

}