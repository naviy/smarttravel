using System;
using System.Web.Mvc;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Web.Controllers
{
	public class ReportsController : Controller
	{
		public Domain.Domain db { get; set; }

		[HttpGet]
		public ActionResult Agent(DateTime date, string fileName)
		{
			return File(db.Report.GetAgentReport(date), MimeTypes.OctetStream, fileName);
		}

		[HttpPost]
		public ActionResult Customer(string customer, string billTo, string passenger, string airline, int? paymentType,
			DateTime? from, DateTime? to, bool? includeDepartments, bool? unpayedDocumentsOnly, string fileName)
		{
			return File(
				db.Report.GetCustomerReport(customer, billTo, airline, passenger, paymentType, from, to, includeDepartments ?? false, unpayedDocumentsOnly ?? false),
				MimeTypes.OctetStream,
				fileName
			);
		}

		[HttpPost]
		public ActionResult Registry(
			ReportType reportType, DateTime? from, DateTime? to, 
			PaymentType? paymentType, PaymentForm? paymentForm, 
			bool? onlyWithInvoices, string airline, string fileName
		)
		{
			return File(
				db.Report.GetRegistryReport(reportType, from, to, paymentType, paymentForm, onlyWithInvoices, airline),
				MimeTypes.OctetStream,
				fileName
			);
		}

		[HttpPost]
		public ActionResult Unbalanced(string customer, DateTime? to, bool? includeOrders, string fileName)
		{
			return File(
				db.Report.GetUnbalancedReport(customer, to, includeOrders ?? false), 
				MimeTypes.OctetStream, 
				fileName
			);
		}
	}
}
