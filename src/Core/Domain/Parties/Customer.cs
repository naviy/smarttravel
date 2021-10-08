using System.Linq;

using Luxena.Base.Data;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[Extends(typeof(Party))]
	public class Customer
	{
		public class Service : Entity3Service<Party>
		{
			public override RangeResponse Suggest(RangeRequest prms)
			{
				var range = Suggest3(prms, Query.Where(a => a.IsCustomer && !a.CanNotBeCustomer));

				if (range.TotalCount == 0)
					range = Suggest3(prms, Query.Where(a => !a.CanNotBeCustomer));

				return range;
			}
		}
	}

}