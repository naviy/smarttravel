using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class BankAccountDto : Entity3DContract
	{

		public bool IsDefault { get; set; }

		public string Note { get; set; }

	}


	public partial class BankAccountContractService
		: Entity3DContractService<BankAccount, BankAccount.Service, BankAccountDto>
	{
		public BankAccountContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.IsDefault = r.IsDefault;
				c.Note = r.Note;
			};

			EntityFromContract += (r, c) =>
			{
				r.IsDefault = c.IsDefault + db;
				r.Note = c.Note + db;
			};
		}
	}

}