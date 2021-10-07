namespace Luxena.Travel.Domain
{

	public partial class CheckPaymentDto : PaymentDto
	{

	}


	public partial class CheckPaymentContractService : PaymentContractService<CheckPayment, CheckPayment.Service, CheckPaymentDto>
	{

	}

}