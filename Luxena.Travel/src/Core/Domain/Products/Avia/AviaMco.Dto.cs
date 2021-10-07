namespace Luxena.Travel.Domain
{

	public partial class AviaMcoDto : AviaDocumentDto
	{

		public string Description { get; set; }

		public AviaDocument.Reference InConnectionWith { get; set; }

	}

	public partial class AviaMcoContractService : AviaDocumentContractService<AviaMco, AviaMco.Service, AviaMcoDto>
	{
		public AviaMcoContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Description = r.Description;

				c.InConnectionWith = r.InConnectionWith;
				//c.Refund = db.AviaRefund.ByFullNumber(r.Number);
			};

			EntityFromContract += (r, c) =>
			{
				r.Description = c.Description + db;
			};
		}
	}

}