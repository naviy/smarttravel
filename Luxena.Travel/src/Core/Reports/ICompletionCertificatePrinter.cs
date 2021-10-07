using System;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{



	public interface ICompletionCertificatePrinter
	{

		byte[] Build(Order order, string number, DateTime issueDate, Person issuedBy, bool showPaid);
	}



}
