using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Провайдер проживания", "Провайдеры проживания"), Icon("")]
	public class AccommodationProvider : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsAccommodationProvider);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsAccommodationProvider = true;
		}
	}


	partial class Domain
	{
		public AccommodationProvider AccommodationProviders { get; set; }
	}

}