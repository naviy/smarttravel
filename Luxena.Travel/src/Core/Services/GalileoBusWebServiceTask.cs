using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

using Common.Logging;

using Luxena.Travel.Domain;



namespace Luxena.Travel.Services
{

	public class GalileoBusWebServiceTask : ITask
	{

		// bool ITask.IsStarted { get; set; }

		public Domain.Domain db { get; set; }

		public string Domain { get; set; }
		public string Key { get; set; }
		public string Token { get; set; }


		public void Execute()
		{
			if (Domain.No() || Key.No() || Token.No()) return;

			var now = DateTime.Now;
			string url = null;


			try
			{
				//​ https://bsv.rezonuniversal.com/api/reports/busesorders?key=4f214&token=2e4c6382c0339025cd3a4240075b55bd&dateFrom=2019-10-01T00:00:00&dateTo=2019-10-15T00:00:00​
				// справка: https://help.rezonuniversal.com/metody-api/avtobusy/vygruzka-dokumentov


				var loadedOn = db.Configuration.GalileoBusWebService_LoadedOn?.AddMinutes(-15) ?? now.AddDays(-1);

				url =
					$"https://{Domain}.rezonuniversal.com/api/reports/busesorders?" +
					$"key={Key}&token={Token}&dateFrom={loadedOn:s}&dateTo={now:s}";

				//_log.Info(url);

				var request = WebRequest.Create(url) as HttpWebRequest;
				var response = request?.GetResponse() as HttpWebResponse;
				var stream = response?.GetResponseStream();
				if (stream == null) return;

				XDocument xml;

				using (var sr = new StreamReader(stream, true))
				{
					xml = XDocument.Load(sr);
				}

				//_log.Info(xml.ToString());

				//var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Luxena.Travel.Parsers.GalileoBusXmlParser.Response1.xml");
				//var xml = XDocument.Load(new StreamReader(stream));
				if (xml.Value("Response", "Status") != "OK") return;

				var docs = xml.Els("Response", "Data", "ReportBusOrder")?.ToArray();
				if (docs.No()) return;


				var importedProducts = (
					from a in db.BusTicket.Query
					where a.CreatedOn >= loadedOn
					select a.Name
				).ToArray();

				var tickets = (
					from doc in docs
					let created = doc.Value("created").As().ToDateTimen("dd.MM.yyyy HH:mm:ss")
					let number = doc.Value("order_id")
					where !importedProducts.Contains(number)
					orderby created
					select new
					{
						Created = created,
						Name = "BusTicket " + number + ": " + doc.Value("status"),
						Content = doc.ToString(),
					}
				).ToArray();

				var documents = tickets.ToArray();

				if (documents.No()) return;

				foreach (var document in documents)
				{
					try
					{
						var file = new GalileoBusXmlFile
						{
							Name = document.Name,
							Content = document.Content,
							TimeStamp = document.Created ?? DateTime.Now,
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
							db.Configuration.GalileoBusWebService_LoadedOn = ticket1.Created;
							db.Save(db.Configuration);
						});
					}
				}

			}
			catch (Exception ex)
			{
				_log.Error(ex);
				_log.Error($"url: {url}");
			}

		}


		private static readonly ILog _log = LogManager.GetLogger(typeof(GalileoBusWebServiceTask));
	}

}