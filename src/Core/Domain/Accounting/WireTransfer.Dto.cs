namespace Luxena.Travel.Domain
{

	public partial class WireTransferDto : PaymentDto
	{

	}


	public partial class WireTransferContractService : PaymentContractService<WireTransfer, WireTransfer.Service, WireTransferDto>
	{

	}

}