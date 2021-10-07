using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	public partial class AmadeusXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.AmadeusXmlFile;

		protected override IList<Entity2> ParseProducts(Domain db)
		{
			var products = 
				AmadeusXmlParser.Parse(Content, db.Configuration.DefaultCurrency, new string[0])
				.ToList();
			return products;
		}


		public new class Service : Service<AmadeusXmlFile>
		{
		}

	}

}