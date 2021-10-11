using System;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{



	//===g





	public interface IInvoicePrinter
	{

		//---g



		int FormNumber { get; set; }

		string ServiceFeeTitle { get; }



		byte[] Build(
			Order order, 
			string number,
			DateTime issueDate,
			Person issuedBy,
			int? formNumber,
			bool showPaid, 
			out string fileExtension
		);



		//---g

	}






	//===g



}
