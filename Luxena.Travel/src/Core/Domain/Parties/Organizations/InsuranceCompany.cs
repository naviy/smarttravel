using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Страховая компания", "Страховые компании")]
	[Extends(typeof(Organization))]
	public class InsuranceCompany
	{

		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsInsuranceCompany);
			}
		}

	}

}