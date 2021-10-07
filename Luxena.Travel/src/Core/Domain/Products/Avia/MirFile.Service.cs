using System.Collections.Generic;

using Luxena.Base.Domain;
using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	partial class MirFile
	{

		public new class Service : Service<MirFile>
		{

		}


		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return MirParser.Parse(Content, db.Configuration.DefaultCurrency);
		}

	}

}