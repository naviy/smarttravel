using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class GdsAgentDto : EntityContract
	{

		public Person.Reference Person { get; set; }

		public int Origin { get; set; }

		public string Code { get; set; }

		public string OfficeCode { get; set; }

		public Organization.Reference Provider { get; set; }

		public Party.Reference Office { get; set; }

		public Organization.Reference LegalEntity { get; set; }

	}


	public partial class GdsAgentContractService : EntityContractService<GdsAgent, GdsAgent.Service, GdsAgentDto>
	{
		public GdsAgentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;
				c.Origin = (int)r.Origin;
				c.Code = r.Code;
				c.OfficeCode = r.OfficeCode;
				c.Provider = r.Provider;
				c.Office = r.Office;
				c.LegalEntity = r.LegalEntity;
			};

			EntityFromContract += (r, c) =>
			{
				r.Person = c.Person + db;
				r.Origin = (ProductOrigin)c.Origin + db;
				r.Code = c.Code + db;
				r.OfficeCode = c.OfficeCode + db;
				r.Provider = c.Provider+db;
				r.Office = c.Office + db;
				r.LegalEntity = c.LegalEntity + db;
			};
		}
	}

}