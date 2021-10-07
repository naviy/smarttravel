using System.Data.Entity;


namespace Luxena.Travel.Domain
{

	[RU("Подразделение", "Подразделения")]
	public partial class Department : Party
	{

		public override PartyType Type => PartyType.Department;

		protected Organization _Organization;


		//public override string ToString()
		//{
		//	return Name + Organization.As(a => " в " + a);
		//}


		static partial void Config_(Domain.EntityConfiguration<Department> entity)
		{
			entity.Association(a => a.Organization, a => a.Departments);
		}

	}


	partial class Domain
	{
		public DbSet<Department> Departments { get; set; }
	}

}