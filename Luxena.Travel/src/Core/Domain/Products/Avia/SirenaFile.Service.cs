using System.Collections.Generic;

using Luxena.Base.Domain;
using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	partial class SirenaFile
	{

		public new class Service : Service<SirenaFile>
		{
		

		}

		protected override IList<Entity2> ParseProducts(Domain db)
		{
			return SirenaXmlParser.Parse(Content);
		}

	}

}