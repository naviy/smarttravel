using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class PaymentSystemDto : Entity3Contract
	{

	}


	public partial class PaymentSystemContractService : Entity3ContractService<PaymentSystem, PaymentSystem.Service, PaymentSystemDto>
	{

	}

}