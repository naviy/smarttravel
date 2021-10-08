using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Провайдер трансферов", "Провайдеры трансферов"), Icon("")]
	public class TransferProvider : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsTransferProvider);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsTransferProvider = true;
		}
	}


	partial class Domain
	{
		public TransferProvider TransferProviders { get; set; }
	}

}