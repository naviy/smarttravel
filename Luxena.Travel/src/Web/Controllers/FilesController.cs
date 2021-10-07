using System.IO;
using System.Web.Mvc;

using Luxena.Travel.Reports;


namespace Luxena.Travel.Web.Controllers
{

	public class FilesController : Controller
	{
		public Domain.Domain db { get; set; }

		public IInvoicePrinter InvoicePrinter { get; set; }


		[HttpPost]
		public ActionResult Party(string file, string fileName)
		{
			return File(db.File.ContentBy(file), MimeTypes.OctetStream, fileName);
		}

		[HttpPost]
		public ActionResult Invoice(string invoice, string fileName)
		{
			//var templatePrinter = InvoicePrinter as TemplatePrinter;
			//if (templatePrinter != null)
			//	fileName = Path.ChangeExtension(fileName, Path.GetExtension(templatePrinter.TemplateFileName));

			return File(db.Invoice.GetFile(invoice), MimeTypes.OctetStream, fileName);
		}

		[HttpPost]
		public ActionResult Payment(string payment, string fileName)
		{
			return File(db.Commit(() => db.CashInOrderPayment.GetFile(payment)), MimeTypes.OctetStream, fileName);
		}

		[HttpPost]
		public ActionResult Consignment(string consignment, string file, string fileName)
		{
			return File(
				db.Commit(() => file.No() ? db.Consignment.GetLastFileBy(consignment) : db.IssuedConsignment.GetFile(file)),
				MimeTypes.OctetStream,
				fileName);
		}

	}

}
