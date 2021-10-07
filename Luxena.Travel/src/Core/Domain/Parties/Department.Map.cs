using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.Mapping
{

	public class DepartmentMap : SubEntityMapping<Department>
	{
		public DepartmentMap()
		{
			DiscriminatorValue(PartyType.Department);

			ManyToOne(x => x.Organization, m => m.NotNullable(true));
		}
	}

}