using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class AviaDocumentFeeDto
	{

		public string Code { get; set; }

		public MoneyDto Amount { get; set; }

	
		public AviaDocumentFeeDto()
		{
		}

		public AviaDocumentFeeDto(AviaDocumentFee r)
		{
			Code = r.Code;
			Amount = r.Amount;
		}

	}

}