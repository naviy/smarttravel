using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Провайдер автобусных билетов", "Провайдеры автобусных билетов"), Icon("")]
	public class BusTicketProvider : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsBusTicketProvider);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsBusTicketProvider = true;
		}
	}


	partial class Domain
	{
		public BusTicketProvider BusTicketProviders { get; set; }
	}

}