using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Провайдер аренды авто", "Провайдеры аренды авто"), Icon("")]
	public class CarRentalProvider : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsCarRentalProvider);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsCarRentalProvider = true;
		}
	}


	partial class Domain
	{
		public CarRentalProvider CarRentalProviders { get; set; }
	}

}