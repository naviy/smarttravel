using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract(Name = "AviaRefund", Namespace = "")]
	[XmlType(TypeName = "AviaRefund")]
	public class AviaRefundContract : AviaDocumentContract
	{
		public AviaRefundContract() { }

		public AviaRefundContract(AviaRefund r) : base(r)
		{
			RefundedDocument = r.RefundedDocument.As(a => a.Name);
			CancelFee = r.CancelFee;
			CancelCommissionPercent = r.CancelCommissionPercent;
			CancelCommission = r.CancelCommission;
			ServiceFeePenalty = r.ServiceFeePenalty;
			RefundServiceFee = r.RefundServiceFee;
			ServiceTotal = r.ServiceTotal;
		}

		[DataMember] public string RefundedDocument { get; set; }
		[DataMember] public MoneyContract CancelFee { get; set; }
		[DataMember] public decimal? CancelCommissionPercent { get; set; }
		[DataMember] public MoneyContract CancelCommission { get; set; }
		[DataMember] public MoneyContract ServiceFeePenalty { get; set; }
		[DataMember] public MoneyContract RefundServiceFee { get; set; }
		[DataMember] public MoneyContract ServiceTotal { get; set; }
	}
}