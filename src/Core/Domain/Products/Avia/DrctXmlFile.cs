using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;




namespace Luxena.Travel.Domain
{



	public partial class DrctXmlFile : GdsFile
	{


		//---g



		public override GdsFileType FileType => GdsFileType.DrctXmlFile;



		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return DrctXmlParser.Parse(
				Content,
				db.Configuration.DefaultCurrency,
				null
			).ToList();
		}

		

		//---g



		public new class Service : Service<DrctXmlFile>
		{
		}



		//---g


	}



}