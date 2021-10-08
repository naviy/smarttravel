using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Luxena.Travel.Web.Controllers
{
	public class PrintController : Controller
	{
		public Domain.Domain db { get; set; }

		[HttpPost]
		public ActionResult Order(string orders, string fileName)
		{
			return File(
				db.Report.PrintOrders(new JavaScriptSerializer().Deserialize<object[]>(orders)),
				MimeTypes.OctetStream,
				fileName
			);
		}

		[HttpPost]
		public ActionResult Ticket(string tickets, string fileName)
		{
			return File(
				db.Report.PrintTickets(new JavaScriptSerializer().Deserialize<object[]>(tickets)),
				MimeTypes.OctetStream,
				fileName
			);
		}

		[HttpPost]
		public ActionResult Reservation(string ticket, string fileName)
		{
			return File(db.Report.PrintReservation(ticket), MimeTypes.OctetStream, fileName);
		}

		[HttpPost]
		public ActionResult Blank(string ticketId, string fileName)
		{
			return File(db.Report.PrintBlank(ticketId), MimeTypes.OctetStream, fileName);
		}
	}
}
