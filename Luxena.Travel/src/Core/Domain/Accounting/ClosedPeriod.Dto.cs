using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class ClosedPeriodDto : EntityContract
	{

		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }

		public PeriodState PeriodState { get; set; }

	}


	public partial class ClosedPeriodContractService
		: EntityContractService<ClosedPeriod, ClosedPeriod.Service, ClosedPeriodDto>
	{
		public ClosedPeriodContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.DateFrom = r.DateFrom;
				c.DateTo = r.DateTo;
				c.PeriodState = r.PeriodState;
			};

			EntityFromContract += (r, c) =>
			{
				r.DateFrom = c.DateFrom + db;
				r.DateTo = c.DateTo + db;
				r.PeriodState = c.PeriodState + db;
			};
		}

		public bool CanUpdate(ClosedPeriodDto c)
		{
			return c.DateFrom < c.DateTo && !db.ClosedPeriod.IsPeriodOverlap(c.Id, c.DateFrom, c.DateTo);
		}

		public ClosedPeriodDto Last()
		{
			return New(db.ClosedPeriod.Last());
		}
	}

}