using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Мобильный оператор", "Мобильные операторы"), Icon("mobile"), Small]
	public class RoamingOperator : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsRoamingOperator);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsRoamingOperator = true;
		}
	}


	partial class Domain
	{
		public RoamingOperator RoamingOperators { get; set; }
	}

}