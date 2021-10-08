using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{
	[DataContract]
	public class AviaPaymentResponse
	{
		public EntityReference Payer { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto Vat { get; set; }

		public object[] DocumentIds { get; set; }

		public OrderItemDto[] OrderItems { get; set; }
	}
}