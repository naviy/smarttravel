using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Страховая компания", "Страховые компании"), Icon("")]
	public class InsuranceCompany : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsInsuranceCompany);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsInsuranceCompany = true;
		}
	}


	partial class Domain
	{
		public InsuranceCompany InsuranceCompanies { get; set; }
	}

}