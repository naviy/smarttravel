namespace Luxena.Travel.Domain
{

	public partial class CashOutOrderPaymentDto : PaymentDto
	{

		public bool SavePosted { get; set; }

	}


	public partial class CashOutOrderPaymentContractService : PaymentContractService<CashOutOrderPayment, CashOutOrderPayment.Service, CashOutOrderPaymentDto>
	{

		public CashOutOrderPaymentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.SavePosted = r.SavePosted;
			};

			EntityFromContract += (r, c) =>
			{
				r.SavePosted = c.SavePosted + db;
			};
		}
		
	}

}