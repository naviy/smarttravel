using System;
using System.Web.Services;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Web.Services
{
	public class ReportService : DomainWebService
	{
		[WebMethod]
		public int[] GetCustomerDocumentCount(object customerId, object billToId, string passenger, object airlineId, object paymentType, object dateFrom, object dateTo, bool unpayedDocumentsOnly)
		{
			return db.Commit(() => db.Report.GetCustomerDocumentCount(customerId, billToId, passenger, airlineId, paymentType, dateFrom, dateTo, unpayedDocumentsOnly));
		}

		[WebMethod]
		public byte[] PrintOrders(object[] ids)
		{
			return db.Commit(() => db.Report.PrintOrders(ids));
		}

		[WebMethod]
		public byte[] PrintTickets(object[] ids)
		{
			return db.Commit(() => db.Report.PrintTickets(ids));
		}

		public byte[] GetCustomerReport(object customerId, object billToId, object airlineId, string passenger, object paymentType, object dateFrom, object dateTo, bool includeDepartments, bool unpayedDocumentsOnly)
		{
			return db.Commit(() => db.Report.GetCustomerReport(
				customerId, billToId, airlineId, passenger, paymentType, dateFrom, dateTo, includeDepartments,
				unpayedDocumentsOnly
			));
		}

		public byte[] GetRegistryReport(ReportType reportType, object dateFrom, object dateTo, PaymentType? paymentType, PaymentForm? paymentForm, string airline)
		{
			return db.Commit(() => db.Report.GetRegistryReport(reportType, dateFrom, dateTo, paymentType, paymentForm, airline));
		}

		public byte[] GetUnbalancedReport(object customer, DateTime? dateTo, bool includeOrders)
		{
			return db.Commit(() => db.Report.GetUnbalancedReport(customer, dateTo, includeOrders));
		}

		public byte[] PrintReservation(object ticketId)
		{
			return db.Commit(() => db.Report.PrintReservation(ticketId));
		}

		public byte[] PrintBlank(object ticketId)
		{
			return db.Commit(() => db.Report.PrintBlank(ticketId));
		}

		public byte[] GetAgentReport(DateTime date)
		{
			return db.Commit(() => db.Report.GetAgentReport(date));
		}
	}
}
