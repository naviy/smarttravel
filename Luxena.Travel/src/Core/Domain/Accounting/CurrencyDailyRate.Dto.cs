using System;

using Luxena.Base.Serialization;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class CurrencyDailyRateDto : EntityContract
	{
		public virtual DateTime Date { get; set; }

		public decimal? UAH_EUR { get; set; }
		public decimal? UAH_RUB { get; set; }
		public decimal? UAH_USD { get; set; }
		public decimal? RUB_EUR { get; set; }
		public decimal? RUB_USD { get; set; }
		public decimal? EUR_USD { get; set; }
	}


	public partial class CurrencyDailyRateContractService
		: EntityContractService<CurrencyDailyRate, CurrencyDailyRate.Service, CurrencyDailyRateDto>
	{
		public CurrencyDailyRateContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Date = r.Date;
				c.UAH_EUR = r.UAH_EUR;
				c.UAH_RUB = r.UAH_RUB;
				c.UAH_USD = r.UAH_USD;
				c.RUB_EUR = r.RUB_EUR;
				c.RUB_USD = r.RUB_USD;
				c.EUR_USD = r.EUR_USD;
			};

			EntityFromContract += (r, c) =>
			{
				r.Date = c.Date + db;
				r.UAH_EUR = c.UAH_EUR + db;
				r.UAH_RUB = c.UAH_RUB + db;
				r.UAH_USD = c.UAH_USD + db;
				r.RUB_EUR = c.RUB_EUR + db;
				r.RUB_USD = c.RUB_USD + db;
				r.EUR_USD = c.EUR_USD + db;
			};
		}

	}

}
