using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	public partial class SabreFilFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.SabreFilFile;

		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return SabreFilParser.Parse(Content, db.Configuration.DefaultCurrency.Id as string).ToList();
		}


		public new class Service : Service<SabreFilFile>
		{
		}

	}

}