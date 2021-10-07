using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class InternalTransferDto : EntityContract
	{

		public DateTime Date { get; set; }

		public Party.Reference FromParty { get; set; }

		public Order.Reference FromOrder { get; set; }

		public Party.Reference ToParty { get; set; }

		public Order.Reference ToOrder { get; set; }

		public decimal Amount { get; set; }
		
	}


	public partial class InternalTransferContractService : EntityContractService<InternalTransfer, InternalTransfer.Service, InternalTransferDto>
	{
		public InternalTransferContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Date = r.Date;
				c.FromOrder = r.FromOrder;
				c.FromParty = r.FromParty;
				c.ToOrder = r.ToOrder;
				c.ToParty = r.ToParty;
				c.Amount = r.Amount;
			};

			EntityFromContract += (r, c) =>
			{
				r.Date = c.Date + db;
				r.FromOrder = c.FromOrder + db;
				r.FromParty = c.FromParty + db;
				r.ToOrder = c.ToOrder + db;
				r.ToParty = c.ToParty + db;
				r.Amount = c.Amount + db;
			};
		}
	}

}