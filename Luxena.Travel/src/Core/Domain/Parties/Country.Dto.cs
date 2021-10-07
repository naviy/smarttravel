using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class CountryDto : Entity3Contract
	{

		public string TwoCharCode { get; set; }

		public string ThreeCharCode { get; set; }

		public string Note { get; set; }

	}


	public partial class CountryContractService : Entity3ContractService<Country, Country.Service, CountryDto>
	{
		public CountryContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.TwoCharCode = r.TwoCharCode;
				c.ThreeCharCode = r.ThreeCharCode;
				c.Note = r.Note;
			};

			EntityFromContract += (r, c) =>
			{
				r.TwoCharCode = c.TwoCharCode + db;
				r.ThreeCharCode = c.ThreeCharCode + db;
				r.Note = c.Note + db;
			};
		}
	}

}