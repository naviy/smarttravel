using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[Extends(typeof(Organization))]
	public class RoamingOperator
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsRoamingOperator);
			}
		}
	}

}