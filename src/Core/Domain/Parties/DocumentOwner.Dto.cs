using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class DocumentOwnerDto : EntityContract
	{

		public Party.Reference Owner { get; set; }

		public bool IsActive { get; set; }

		public bool IsDefault { get; set; }

	}


	public partial class DocumentOwnerContractService : EntityContractService<DocumentOwner, DocumentOwner.Service, DocumentOwnerDto>
	{
		public DocumentOwnerContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Owner = r.Owner;
				c.IsActive = r.IsActive;
				c.IsDefault = r.IsDefault;
			};

			EntityFromContract += (r, c) =>
			{
				r.Owner = c.Owner + db;
				r.IsActive = c.IsActive + db;
				r.IsDefault = c.IsDefault + db;
			};
		}
	}

}