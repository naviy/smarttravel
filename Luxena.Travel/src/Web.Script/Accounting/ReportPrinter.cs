using System.Collections;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{
	public static class ReportPrinter
	{
		public static void GetOrderDocument(object invoiceId, string number, InvoiceType type)
		{
			ReportLoader.Load(
				string.Format("files/invoice/{0}_{1}.xls", type.ToString(), number),
				new Dictionary("invoice", invoiceId));
		}

		public static void GetCashOrder(object paymentId, string number)
		{
			ReportLoader.Load(string.Format("files/payment/CashOrder_{0}.xls", number), new Dictionary("payment", paymentId));
		}

		public static void GetLastIssuedConsignment(object consignmentId, string number)
		{
			ReportLoader.Load(string.Format("files/consignment/Consignment_{0}.xls", number), new Dictionary("consignment", consignmentId));
		}

		public static void GetIssuedConsignment(object fileId, string number)
		{
			ReportLoader.Load(string.Format("files/consignment/Consignment_{0}.xls", number), new Dictionary("file", fileId));
		}
	}
}