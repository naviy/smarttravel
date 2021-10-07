namespace Luxena.Travel.Domain
{

	public partial class ElectronicPaymentDto : PaymentDto
	{

		public string AuthorizationCode { get; set; }

		public PaymentSystem.Reference PaymentSystem { get; set; }

	}


	public partial class ElectronicPaymentContractService : PaymentContractService<ElectronicPayment, ElectronicPayment.Service, ElectronicPaymentDto>
	{

		public ElectronicPaymentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.AuthorizationCode = r.AuthorizationCode;
				c.PaymentSystem = r.PaymentSystem;
			};

			EntityFromContract += (r, c) =>
			{
				r.AuthorizationCode = c.AuthorizationCode + db;
				r.PaymentSystem = c.PaymentSystem + db;
			};			
		}

	}

}