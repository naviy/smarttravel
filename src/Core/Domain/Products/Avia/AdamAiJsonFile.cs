using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;




namespace Luxena.Travel.Domain
{



	public partial class AdamAiJsonFile : GdsFile
	{


		//---g



		public override GdsFileType FileType => GdsFileType.AdamAiJsonFile;



		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return AdamAiJsonParser.Parse(
				Content,
				db.Configuration.DefaultCurrency
			).ToList();
		}

		

		//---g



		public new class Service : Service<AdamAiJsonFile>
		{
		}



		//---g


	}



}