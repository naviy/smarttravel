using System;

using Luxena.Base.Metamodel;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class OpeningBalanceDto : EntityContract
	{

		public string Number { get; set; }

		public DateTime Date { get; set; }

		public Party.Reference Party { get; set; }

		public decimal Balance { get; set; }

	}


	public partial class OpeningBalanceContractService : EntityContractService<OpeningBalance, OpeningBalance.Service, OpeningBalanceDto>
	{
		public OpeningBalanceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.Date = r.Date;
				c.Party = r.Party;
				c.Balance = r.Balance;
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.Date = c.Date + db;
				r.Party = c.Party + db;
				r.Balance = c.Balance + db;
			};
		}
	}

}