using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Провайдер ж/д билетов", "Провайдеры ж/д билетов"), Icon("")]
	public class PasteboardProvider : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsPasteboardProvider);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsPasteboardProvider = true;
		}
	}


	partial class Domain
	{
		public PasteboardProvider PasteboardProviders { get; set; }
	}

}