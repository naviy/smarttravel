using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class IssuedConsignmentDto : EntityContract
	{

		public string Number { get; set; }

		public DateTime TimeStamp { get; set; }

		public Consignment.Reference Consignment { get; set; }

		public Person.Reference IssuedBy { get; set; }

	}


	public partial class IssuedConsignmentContractService 
		: EntityContractService<IssuedConsignment, IssuedConsignment.Service, IssuedConsignmentDto>
	{
		public IssuedConsignmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.TimeStamp = r.TimeStamp;
				c.Consignment = r.Consignment;
				c.IssuedBy = r.IssuedBy;
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.TimeStamp = c.TimeStamp + db;
				r.Consignment = c.Consignment + db;
				r.IssuedBy = c.IssuedBy + db;
			};
		}
	}

}