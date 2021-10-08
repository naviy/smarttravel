using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class MilesCardDto : EntityContract
	{

		public string Number { get; set; }

		public Organization.Reference Organization { get; set; }

	}


	public partial class MilesCardContractService : EntityContractService<MilesCard, MilesCard.Service, MilesCardDto>
	{
		public MilesCardContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.Organization = r.Organization;
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.Organization = c.Organization + db;
			};
		}
	}

}