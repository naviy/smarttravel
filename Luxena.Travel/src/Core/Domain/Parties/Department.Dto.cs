namespace Luxena.Travel.Domain
{

	public partial class DepartmentDto : PartyDto
	{

		public Organization.Reference Organization { get; set; }

	}


	public partial class DepartmentContractService : PartyContractService<Department, Department.Service, DepartmentDto>
	{
		public DepartmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Organization = r.Organization;
			};

			EntityFromContract += (r, c) =>
			{
				r.Organization = c.Organization + db;
			};
		}
	}

}