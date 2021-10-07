namespace Luxena.Travel.Domain
{

	public partial class CashInOrderPaymentDto : PaymentDto
	{

		public bool SavePosted { get; set; }

	}


	public partial class CashInOrderPaymentContractService : PaymentContractService<CashInOrderPayment, CashInOrderPayment.Service, CashInOrderPaymentDto>
	{

		public CashInOrderPaymentContractService()
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