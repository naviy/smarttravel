using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class AirlineMonthCommissionDto : EntityContract
	{

		public Organization.Reference Airline { get; set; }

		public DateTime DateFrom { get; set; }

		public decimal? CommissionPc { get; set; }

	}


	public partial class AirlineMonthCommissionContractService : EntityContractService<AirlineMonthCommission, AirlineMonthCommission.Service, AirlineMonthCommissionDto>
	{
		public AirlineMonthCommissionContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Airline = r.Airline;
				c.DateFrom = r.DateFrom;
				c.CommissionPc = r.CommissionPc;
			};


			EntityFromContract += (r, c) =>
			{
				r.Airline = c.Airline + db;
				r.DateFrom = c.DateFrom + db;
				r.CommissionPc = c.CommissionPc + db;
			};
		}
	}

}