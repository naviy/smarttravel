using System.Collections.Generic;

using Luxena.Travel.Parsers;




namespace Luxena.Travel.Domain
{



	//===g






	public partial class AirFile : GdsFile
	{

		//---g



		public override GdsFileType FileType => GdsFileType.AirFile;



		//---g



		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return AirParser.Parse(
				Content, 
				db.Configuration.AmadeusRizUsingMode,
				db.Configuration.DefaultCurrency,
				db.Configuration.DefaultConsolidatorCommission
			);
		}



		//---g



		public new class Service : Service<AirFile>
		{
		}




		//---g

	}






	//===g



}