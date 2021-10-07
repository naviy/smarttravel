using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class UserDto : Entity3DContract
	{

		public Person.Reference Person { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public bool Active { get; set; }

		public bool IsAdministrator { get; set; }

		public bool IsSupervisor { get; set; }

		public bool IsAgent { get; set; }

		public bool IsCashier { get; set; }

		public bool IsAnalyst { get; set; }

		public bool IsSubAgent { get; set; }

		public bool AllowCustomerReport { get; set; }
		public bool AllowRegistryReport { get; set; }
		public bool AllowUnbalancedReport { get; set; }

		public string SessionId { get; set; }

		public string Roles { get; set; }

	}


	public partial class UserContractService : Entity3DContractService<User, User.Service, UserDto>
	{
		public UserContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;

				c.Active = r.Active;
				c.IsAdministrator = r.IsAdministrator;
				c.IsSupervisor = r.IsSupervisor;
				c.IsAgent = r.IsAgent;
				c.IsCashier = r.IsCashier;
				c.IsAnalyst = r.IsAnalyst;
				c.IsSubAgent = r.IsSubAgent;

				c.AllowCustomerReport = r.AllowCustomerReport;
				c.AllowRegistryReport = r.AllowRegistryReport;
				c.AllowUnbalancedReport = r.AllowUnbalancedReport;

				c.SessionId = r.SessionId;
				c.Roles = r.Roles;
			};

			EntityFromContract += (r, c) =>
			{
				r.Person = c.Person + db;

				r.Password = c.Password + db;
				r.ConfirmPassword = c.ConfirmPassword + db;

				r.Active = c.Active + db;
				r.IsAdministrator = c.IsAdministrator + db;
				r.IsSupervisor = c.IsSupervisor + db;
				r.IsAgent = c.IsAgent + db;
				r.IsCashier = c.IsCashier + db;
				r.IsAnalyst = c.IsAnalyst + db;
				r.IsSubAgent = c.IsSubAgent + db;

				r.AllowCustomerReport = c.AllowCustomerReport + db;
				r.AllowRegistryReport = c.AllowRegistryReport + db;
				r.AllowUnbalancedReport = c.AllowUnbalancedReport + db;
			};
		}
	}

}