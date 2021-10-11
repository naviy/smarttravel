using System.Collections;

using LxnBase.UI.AutoControls;




namespace Luxena.Travel
{



	//===g






	public static class ReportPrinter
	{

		//---g



		public static void GetOrderDocument(object invoiceId, string number, InvoiceType type, string fileExtension)
		{

			ReportLoader.Load(

				string.Format(
					"files/invoice/{0}_{1}.{2}", 
					type.ToString(),
					number,
					fileExtension ?? "xls"
				//AppManager.AppParameters.InvoiceFileExtension
				),
				new Dictionary("invoice", invoiceId)
			);

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



		//---g

	}






	//===g




}