namespace Luxena.Travel.Domain
{

	public partial class AviaRefundDto : AviaDocumentDto
	{

	}

	public partial class AviaRefundContractService : AviaDocumentContractService<AviaRefund, AviaRefund.Service, AviaRefundDto>
	{
	}

}