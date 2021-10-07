using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Провайдер туров (готовых)", "Провайдеры туров (готовых)"), Icon("")]
	public class TourProvider : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsTourProvider);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsTourProvider = true;
		}
	}


	partial class Domain
	{
		public TourProvider TourProviders { get; set; }
	}

}