using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	public partial class CrazyllamaPnrFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.CrazyllamaPnrFile;

		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return SabreFilParser.Parse(
				Content, 
				db.Configuration.DefaultCurrency.Id as string,
				GdsOriginator.Crazyllama
			).ToList();
		}


		public new class Service : Service<CrazyllamaPnrFile>
		{
		}

	}

}