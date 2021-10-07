using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{
	[DataContract(Name = "Fee")]
	[XmlType(TypeName = "Fee")]
	public class AviaDocumentFeeContract
	{
		public AviaDocumentFeeContract() { }

		public AviaDocumentFeeContract(AviaDocumentFee fee)
		{
			Code = fee.Code;
			Currency = fee.Amount.Currency.Code;
			Amount = fee.Amount.Amount;
		}

		[DataMember] public string Code { get; set; }
		[DataMember] public string Currency { get; set; }
		[DataMember] public decimal Amount { get; set; }
	}
}