using System.Collections.Generic;

using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	public partial class AirFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.AirFile;

		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return AirParser.Parse(Content, db.Configuration.AmadeusRizUsingMode, db.Configuration.DefaultCurrency);
		}


		public new class Service : Service<AirFile>
		{
		}

	}

}