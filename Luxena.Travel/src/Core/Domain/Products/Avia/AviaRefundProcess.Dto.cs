namespace Luxena.Travel.Domain
{

	public partial class AviaRefundProcessDto : AviaDocumentProcessDto
	{

		public MoneyDto ServiceFeePenalty { get; set; }

		public MoneyDto RefundServiceFee { get; set; }

	}

	public partial class AviaRefundProcessContractService 
		: AviaDocumentProcessContractService<AviaRefund, AviaRefund.Service, AviaRefundProcessDto>
	{
		public AviaRefundProcessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.RefundServiceFee = r.RefundServiceFee;
				c.ServiceFeePenalty = r.ServiceFeePenalty;
			};

			EntityFromContract += (r, c) =>
			{
				r.RefundServiceFee = c.RefundServiceFee + db;
				r.ServiceFeePenalty = c.ServiceFeePenalty + db;
			};
		}
	}

}