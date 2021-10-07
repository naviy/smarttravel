using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

using Common.Logging;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Services
{



	//===g

	
	
	
	
	
	public class GalileoRailWebServiceTask : ITask
	{
		
		//---g



		// bool ITask.IsStarted { get; set; }

		public Domain.Domain db { get; set; }

		public string Domain { get; set; }
		public string Key { get; set; }
		public string Token { get; set; }



		//---g



		public void Execute()
		{

			if (Domain.No() || Key.No() || Token.No()) 
				return;


			var now = DateTime.Now;
			string url = null;


			try
			{
				//return;


				//- OLD - https://bsv.galileo.com.ua/api/reports/raildocs?key=4f214&token=2e4c6382c0339025cd3a4240075b55bd&dateFrom=2019-05-07&dateTo=2018-05-07
				//NEW - https://bsv.rezonuniversal.com/api/reports/raildocs?key=4f214&token=2e4c6382c0339025cd3a4240075b55bd&dateFrom=2019-05-07&dateTo=2018-05-07
				//NEW - https://fgr-rail.rezonuniversal.com/api/reports/raildocs?key=b1e21&token=44ecca1baa8b232f49213dd9f29fcece&dateFrom=2021-09-20&dateTo=2021-09-20
				//справка: https://help.rezonuniversal.com/metody-api/zhd


				var loadedOn = db.Configuration.GalileoRailWebService_LoadedOn?.AddMinutes(-15) ?? now.AddDays(-1);

				url =
					$"https://{Domain}.rezonuniversal.com/api/reports/raildocs?" +
					$"key={Key}&token={Token}&dateFrom={loadedOn:s}&dateTo={now:s}";

				//_log.Info(url);

				var request = WebRequest.Create(url) as HttpWebRequest;
				var response = request?.GetResponse() as HttpWebResponse;
				var stream = response?.GetResponseStream();

				if (stream == null) 
					return;


				XDocument xml;

				using (var sr = new StreamReader(stream, true))
				{
					xml = XDocument.Load(sr);
				}

				//_log.Info(xml.ToString());

				//var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Luxena.Travel.Parsers.GalileoRailXmlParser.Response1.xml");
				//var xml = XDocument.Load(new StreamReader(stream));
				if (xml.Value("Response", "Status") != "OK")
					return;


				var docs = xml.Els("Response", "Data", "ReportRailDoc")?.ToArray();

				if (docs.No()) 
					return;


				var importedProducts = (
					from a in db.Pasteboard.Query
					where a.CreatedOn >= loadedOn
					select a.Name
				).ToArray();


				var tickets = (
					from doc in docs
					let created = doc.Value("sysdate").As().DateTimen
					let number = doc.Value("uz_ordernumber")
					where !importedProducts.Contains(number)
					orderby created
					select new
					{
						Created = created,
						Name = "Pasteboard " + number + ": " + doc.Attr("Status"),
						Content = doc.ToString(),
					}
				).ToArray();


				var documents = tickets.ToArray();

				if (documents.No())
					return;


				foreach (var document in documents)
				{

					try
					{
						var file = new GalileoRailXmlFile
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
							db.Configuration.GalileoRailWebService_LoadedOn = ticket1.Created;
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



		//---g



		private static readonly ILog _log = LogManager.GetLogger(typeof(GalileoRailWebServiceTask));



		//---g

	}






	//===g



}