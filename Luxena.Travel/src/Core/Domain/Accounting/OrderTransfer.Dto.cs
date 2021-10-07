using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class OrderTransferDto
	{

		public InternalTransfer.Reference Transfer { get; set; }

		public DateTime Date { get; set; }

		public Party.Reference Party { get; set; }

		public Order.Reference Order { get; set; }

		public decimal Amount { get; set; }


		public OrderTransferDto(InternalTransfer r, bool incoming)
		{
			Transfer = r;
			Date = r.Date;

			if (incoming)
			{
				Party = r.FromParty;
				Order = r.FromOrder;
				Amount = r.Amount;
			}
			else
			{
				Party = r.ToParty;
				Order = r.ToOrder;
				Amount = -r.Amount;
			}
		}

	}

}