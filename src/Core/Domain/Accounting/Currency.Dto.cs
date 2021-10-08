using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class CurrencyDto : Entity3Contract
	{

		public string Code { get; set; }

		public int NumericCode { get; set; }

		public string CyrillicCode { get; set; }

	}


	public partial class CurrencyContractService : Entity3ContractService<Currency, Currency.Service, CurrencyDto>
	{
		public CurrencyContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Code = r.Code;
				c.NumericCode = r.NumericCode;
				c.CyrillicCode = r.CyrillicCode;
			};

			EntityFromContract += (r, c) =>
			{
				r.Code = c.Code + db;
				r.NumericCode = c.NumericCode + db;
				r.CyrillicCode = c.CyrillicCode + db;
			};
		}
	}

}