using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class ContractDto : EntityContract
	{

		public Organization.Reference Customer { get; set; }

		public string Number { get; set; }

		public DateTime? IssueDate { get; set; }

		public Decimal DiscountPc { get; set; }

		public string Note { get; set; }

	}


	public partial class ContractContractService
		: EntityContractService<Contract, Contract.Service, ContractDto>
	{
		public ContractContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Customer = r.Customer;
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.DiscountPc = r.DiscountPc;
				c.Note = r.Note;
			};

			EntityFromContract += (r, c) =>
			{
				r.Customer = c.Customer + db;
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				r.DiscountPc = c.DiscountPc + db;
				r.Note = c.Note + db;
			};
		}
	}

}
