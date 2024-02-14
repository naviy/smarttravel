using Luxena.Domain.Contracts;




namespace Luxena.Travel.Domain
{



	public partial class BankAccountDto : Entity3DContract
	{

		public bool IsDefault { get; set; }

		public string CompanyDetails { get; set; }
		
		public string TotalSuffix { get; set; }

		public bool DisallowVat { get; set; }

		public string Note { get; set; }

	}




	public partial class BankAccountContractService : Entity3DContractService<BankAccount, BankAccount.Service, BankAccountDto>
	{

		public BankAccountContractService()
		{

			ContractFromEntity += (r, c) =>
			{
				c.IsDefault = r.IsDefault;
				c.CompanyDetails = r.CompanyDetails;
				c.TotalSuffix = r.TotalSuffix;
				c.DisallowVat = r.DisallowVat;
				c.Note = r.Note;
			};


			EntityFromContract += (r, c) =>
			{
				r.IsDefault = c.IsDefault + db;
				r.CompanyDetails = c.CompanyDetails + db;
				r.TotalSuffix = c.TotalSuffix + db;
				r.DisallowVat = c.DisallowVat + db;
				r.Note = c.Note + db;
			};

		}

	}



}