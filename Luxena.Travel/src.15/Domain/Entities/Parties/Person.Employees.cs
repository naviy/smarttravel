using System.Linq;


namespace Luxena.Travel.Domain
{


	[RU("Сотрудник", "Сотрудники")]
	public class Employee : Domain.EntityQuery<Person>
	{

		protected override IQueryable<Person> GetQuery()
		{
			return db.Persons.Where(a => a.OrganizationId != null);
		}
		
	}


	partial class Domain
	{
		public Employee Employees { get; set; }
	}
	
}