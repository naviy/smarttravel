using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using Luxena.Travel.Domain;

using System.IO.Compression;


namespace Luxena.Travel.Web.Controllers
{

	public class RepairController : Controller
	{
		public Domain.Domain db { get; set; }


		//persey.travel.luxena.com/Repair/Reexport?startDate=2018-08-07&endDate=2018-08-07
		//bsv.travel.luxena.com/Repair/Reexport?startDate=2020-01-01&endDate=2020-03-15

		public string Reexport(string startDate, string endDate)
		{
			var date1 = startDate.As().DateTime;
			var date2 = endDate.As().DateTimen;

			try
			{
				var invoices = db.Invoice.ListBy(a => a.IssueDate >= date1 && (date2 == null || a.IssueDate <= date2));

				var orders =
					db.Order.ListBy(a => a.IssueDate >= date1 && (date2 == null || a.IssueDate <= date2))
					.Concat(invoices.Select(a => a.Order))
					.Distinct().ToList();

				var docs = orders
					.SelectMany(a => a.Items.Select(b => b.Product))
					.Where(a => a != null)
					.Distinct()
					.Union(db.Product.ListBy(a => a.IssueDate >= date1 && (date2 == null || a.IssueDate <= date2)))
					.ToList();

				db.Export(invoices);
				db.Export(orders);
				db.Export(docs);

				return "Invoices: {0}, Orders: {1}, Documents: {2}".AsFormat(invoices.Count, orders.Count, docs.Count);
				// http://travel/Repair/Reexport/2016-08-10/2016-08-10
				// http://ufsa.travel.luxena.com/Repair/Reexport/2016-08-10/2016-08-10
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public string PartyReexport()
		{
			try
			{
				var organizations = db.Organization.Query.ToList();
				organizations.ForEach(a => db.Organization.Export(a));

				var persons = db.Person.Query.ToList();
				persons.ForEach(a => db.Person.Export(a));

				return $"Organizations: {organizations.Count}, Persons: {persons.Count}";
				// http://travel/Repair/PartyReexport/
				// http://ufsa.travel.luxena.com/Repair/PartyReexport
			}
			catch (Exception ex)
			{
				throw;
			}
		}



		public string OrderRefresh(string startDate, string orderNumber)
		{
			var orders =
				startDate.Yes() ? db.Order.ListBy(a => a.IssueDate >= DateTime.Parse(startDate)) :
				orderNumber.Yes() ? db.Order.ListBy(a => a.Number == orderNumber) :
				null;

			if (orders == null)
				throw new Exception("Должен быть указан параметр startDate или orderNumber");

			try
			{
				return db.Commit(() =>
				{
					foreach (var order in orders)
					{
						var product = order.Items.Select(a => a.Product).FirstOrDefault(a => a?.Order != null);
						if (product != null)
						{
							product.RefreshOrder(db);
							db.Save(product);
						}
						else
						{
							order.Recalculate(db);
							db.Save(order);
						}
					}

					//db.Save(orders);

					return "Orders: {0}".AsFormat(orders.Count);
				});
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		public string OrderRecalculate(string startDate, string orderNumber)
		{
			var orders =
				startDate.Yes() ? db.Order.ListBy(a => a.IssueDate >= DateTime.Parse(startDate)) :
				orderNumber.Yes() ? db.Order.ListBy(a => a.Number == orderNumber) :
				null;

			if (orders == null)
				throw new Exception("Должен быть указан параметр startDate или orderNumber");

			try
			{
				return db.Commit(() =>
				{
					foreach (var order in orders)
					{
						order.Recalculate(db);
					}

					return "Orders: {0}".AsFormat(orders.Count);
				});
			}
			catch (Exception ex)
			{
				throw;
			}
		}



		// http://travel/Repair/AviaTicketDirections01
		public ActionResult AviaTicketDirections01()
		{
			return View(db);
		}




		public ActionResult AirExport1(string startDate, string endDate, string iata, string prefix)
		{
			var date1 = startDate.As().DateTime;
			var date2 = endDate.As().DateTimen;

			var files = (
				from a in db.AviaTicket.Query
				let date = a.Departure
				let file = a.OriginalDocument
				where
					date >= date1 && (date2 == null || date <= date2) &&
					(iata == null || a.Producer.AirlineIataCode == iata) &&
					(prefix == null || a.Producer.AirlinePrefixCode == prefix) &&
					(a.Type == ProductType.AviaTicket || a.Type == ProductType.AviaRefund) &&
					file != null && file.FileType == GdsFileType.AirFile
				select new GdsFileInfo(file.Name, file.Content)
			).ToArray();

			return GetZipGdsFiles(files, $"air1-{iata ?? prefix}-{startDate:yyyyMMdd}.zip");
		}

		public ActionResult AirExport2(string startDate, string endDate, string iata, string prefix)
		{
			var date1 = startDate.As().DateTime;
			var date2 = endDate.As().DateTimen;

			var files = (
				from a in db.AviaDocument.Query
				let date = a.IssueDate
				let file = a.OriginalDocument
				where
					date >= date1 && (date2 == null || date <= date2) &&
					(iata == null || a.Producer.AirlineIataCode == iata) &&
					(prefix == null || a.Producer.AirlinePrefixCode == prefix) &&
					(a.Type == ProductType.AviaTicket || a.Type == ProductType.AviaRefund) &&
					file != null && file.FileType == GdsFileType.AirFile
				select new GdsFileInfo(file.Name, file.Content)
			).ToArray();

			return GetZipGdsFiles(files, $"air2-{iata ?? prefix}-{startDate:yyyyMMdd}.zip");
		}

		public ActionResult MirExport2(string startDate, string endDate, string iata, string prefix)
		{
			var date1 = startDate.As().DateTime;
			var date2 = endDate.As().DateTimen;

			var files = (
				from a in db.AviaDocument.Query
				let date = a.IssueDate
				let file = a.OriginalDocument
				where
					date >= date1 && (date2 == null || date <= date2) &&
					(iata == null || a.Producer.AirlineIataCode == iata) &&
					(prefix == null || a.Producer.AirlinePrefixCode == prefix) &&
					(a.Type == ProductType.AviaTicket || a.Type == ProductType.AviaRefund) &&
					file != null && file.FileType == GdsFileType.MirFile
				select new GdsFileInfo(file.Name, file.Content)
			).ToArray();

			return GetZipGdsFiles(files, $"mir2-{iata ?? prefix}-{startDate:yyyyMMdd}.zip");
		}


		private class GdsFileInfo
		{
			public GdsFileInfo(string name, string content)
			{
				Name = name;
				Content = content;
			}

			public string Name { get; }
			public string Content { get; }

			public override string ToString() => Name;
		}


		ActionResult GetZipGdsFiles(IEnumerable<GdsFileInfo> files, string archiveName)
		{
			using (var fileStream = new MemoryStream())
			{
				using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Update))
				{
					foreach (var file in files)
					{
						var readmeEntry = archive.CreateEntry(file.Name);
						using (var writer = new StreamWriter(readmeEntry.Open()))
						{
							writer.Write(file.Content);
						}
					}
				}

				var buffer = fileStream.ToArray();
				return File(buffer, "application/zip", archiveName);
			}
		}

	}

}
