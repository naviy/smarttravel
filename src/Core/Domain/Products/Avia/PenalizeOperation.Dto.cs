using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class PenalizeOperationDto : EntityContract
	{

		public PenalizeOperationType Type { get; set; }

		public PenalizeOperationStatus Status { get; set; }

		public string Description { get; set; }

	}


	public partial class PenalizeOperationContractService 
		: EntityContractService<PenalizeOperation, PenalizeOperation.Service, PenalizeOperationDto>
	{
		public PenalizeOperationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;
				c.Status = r.Status;
				c.Description = r.Description;
			};

			EntityFromContract += (r, c) =>
			{
				throw new NotImplementedException();
			};
		}

		public void Update(AviaTicket ticket, PenalizeOperationDto[] operationDtos)
		{
			if (operationDtos.No())
			{
				ticket.PenalizeOperations.Clear();
				return;
			}

			foreach (PenalizeOperationType type in Enum.GetValues(typeof(PenalizeOperationType)))
			{
				var operationType = type;

				var r = ticket.FindPenalizeOperation(operationType);
				var c = operationDtos.By(a => a.Type == operationType);

				if (c == null)
				{
					if (r != null)
						ticket.RemovePenalizeOperation(r);
				}
				else
				{
					if (r == null)
					{
						r = new PenalizeOperation { Type = operationType };
						ticket.AddPenalizeOperation(r);
					}

					r.Status = c.Status;
					r.Description = c.Description.Clip();
				}
			}
		}

	}

}