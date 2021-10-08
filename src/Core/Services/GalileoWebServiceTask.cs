using System;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;

using Common.Logging;

using Luxena.Travel.Domain;
using Luxena.Travel.GalileoWebService;


namespace Luxena.Travel.Services
{

	public class GalileoWebServiceTask : ITask
	{

		// bool ITask.IsStarted { get; set; }

		public Domain.Domain db { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public string Robots { get { return GlobalRobots; } set { GlobalRobots = value; } }

		public static string GlobalRobots { get; set; }


		public void Execute()
		{
			if (UserName.No() || Password.No()) return;

			var now = DateTime.Now;

			try
			{
				var client = new XMLWebServiceSoapClient(
					new BasicHttpBinding { Name = "XMLWebServiceSoap", MaxReceivedMessageSize = 0xfffffff },
					new EndpointAddress("http://golink.galileo.com.ua/XMLWebService.asmx")
				);

				var loadedOn = db.Configuration.GalileoWebService_LoadedOn;
				var dayCount = loadedOn == null ? 30 : (now.Date - loadedOn.Value.Date).Days + 1;
				//var dayCount = 4;

				//dayCount = 1;


				var sxml = client.GetTicketsXML(UserName, Password, dayCount, "", "", "", "", "");

				if (sxml.No()) return;

				//return;

				var importDate = now.Date.AddDays(-dayCount + 1 - 1);

				var importedProducts = (
					from a in db.AviaDocument.Query
					where a.CreatedOn >= importDate
					select new { a.Type, a.Name, a.IsVoid }
				).ToArray();

				var xml = XDocument.Parse(sxml);

				var tickets = (
					from a in xml.Root.Els("Tickets", "Ticket").Sure()
					let created = a.Value("TicketHeader", "Created").As().DateTimen
					let number = a.Attr("No")
					let status = a.Attr("Status")?.ToLower()

					//where created != null && created > loadedOn
					where
						status == "issued" || status == "reissued" ? !importedProducts.Any(b => b.Name == number && b.Type == ProductType.AviaTicket) :
						status == "refund" ? !importedProducts.Any(b => b.Name == number && b.Type == ProductType.AviaRefund) :
						status == "void" && !importedProducts.Any(b => b.Name == number && b.Type == ProductType.AviaTicket && b.IsVoid)
					orderby created
					select new
					{
						Created = created,
						Name = "Ticket " + number + ": " + a.Attr("Status"),
						Content = a.ToString(),
						Xml = a,
					}
				).ToArray();

				var mcos = (
					from a in xml.Root.Els("MCOs", "MCO").AsUnion(xml.Root.Els("MCOs", "EMD")).Sure()
					let created = a.Value("Created").As().DateTimen
					let number = a.Attr("No")
					//where created != null && created > loadedOn
					where importedProducts.All(b => b.Name != number)
					orderby created
					select new
					{
						Created = created,
						Name = a.Name + " " + a.Attr("No"),
						Content = a.ToString(),
						Xml = a,
					}
				).ToArray();

				var documents = tickets.AsConcat(mcos).ToArray();

				if (documents.No()) return;

				foreach (var document in documents)
				{
					try
					{
						var file = new GalileoXmlFile
						{
							Name = document.Name,
							Content = document.Content,
							TimeStamp = document.Created.Value,
						};

						db.GdsFile.AddFile(file);
					}
					catch (Exception ex)
					{
						_log.Error(ex);
					}
					finally
					{
						var ticket1 = document;
						db.Commit(() =>
						{
							db.Configuration.GalileoWebService_LoadedOn = ticket1.Created;
							db.Save(db.Configuration);
						});
					}
				}

			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}

		}


		private static readonly ILog _log = LogManager.GetLogger(typeof(GalileoWebServiceTask));
	}

}