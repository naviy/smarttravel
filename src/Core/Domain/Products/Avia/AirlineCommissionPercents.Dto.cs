using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class AirlineCommissionPercentsDto : EntityContract
	{

		public Organization.Reference Airline { get; set; }

		public decimal Domestic { get; set; }

		public decimal International { get; set; }

		public decimal InterlineDomestic { get; set; }

		public decimal InterlineInternational { get; set; }

	}


	public partial class AirlineCommissionPercentsContractService : EntityContractService<AirlineCommissionPercents, AirlineCommissionPercents.Service, AirlineCommissionPercentsDto>
	{
		public AirlineCommissionPercentsContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Airline = r.Airline;
				c.Domestic = r.Domestic;
				c.International = r.International;
				c.InterlineDomestic = r.InterlineDomestic;
				c.InterlineInternational = r.InterlineInternational;
			};

			EntityFromContract += (r, c) =>
			{
				r.Airline = c.Airline + db;
				r.Domestic = c.Domestic + db;
				r.International = c.International + db;
				r.InterlineDomestic = c.InterlineDomestic + db;
				r.InterlineInternational = c.InterlineInternational + db;
			};
		}
	}

}