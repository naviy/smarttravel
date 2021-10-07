using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public class CashPaymentResponse
	{

		public PaymentDto Payment { get; set; }

		public ConsignmentDto Consignment { get; set; }

	
		public CashPaymentResponse(Contracts dc, Payment payment, Consignment consignment)
		{
			Payment = dc.Payment.New(payment);
			Consignment = dc.Consignment.New(consignment);
		}

	}

}