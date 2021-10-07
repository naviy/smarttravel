using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class DocumentAccessDto : EntityContract
	{

		public Person.Reference Person { get; set; }

		public Party.Reference Owner { get; set; }

		public bool FullDocumentControl { get; set; }

	}


	public partial class DocumentAccessContractService : EntityContractService<DocumentAccess, DocumentAccess.Service, DocumentAccessDto>
	{
		public DocumentAccessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;
				c.Owner = r.Owner;
				c.FullDocumentControl = r.FullDocumentControl;
			};

			EntityFromContract += (r, c) =>
			{
				r.Person = c.Person + db;
				r.Owner = c.Owner + db;
				r.FullDocumentControl = c.FullDocumentControl + db;
			};
		}
	}

}