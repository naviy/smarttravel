using System;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public class CustomerReportParams
	{
		public Party Customer { get; set; }

		public Party BillTo { get; set; }

		public Party Client => Customer ?? BillTo;

		public string Passenger { get; set; }

		public PaymentType? PaymentType { get; set; }

		public DateTime? DateFrom { get; set; }

		public DateTime? DateTo { get; set; }

		public bool UnpayedDocumentsOnly { get; set; }

		public Organization Airline { get; set; }
	}
}