namespace Luxena.Travel.Domain
{

	public partial class AviaMcoProcessDto : AviaDocumentProcessDto
	{

		public string Description { get; set; }

	}

	public partial class AviaMcoProcessContractService
		: AviaDocumentProcessContractService<AviaMco, AviaMco.Service, AviaMcoProcessDto>
	{
		public AviaMcoProcessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Description = r.Description;
			};

			EntityFromContract += (r, c) =>
			{
				r.Description = c.Description + db;
			};
		}
	}

}