using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Xml.Linq;

using Common.Logging;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Services
{
	


	//===g






	public class DrctWebServiceTask : ITask
	{

		//---g



		//bool ITask.IsStarted { get; set; }

		public Domain.Domain db { get; set; }


		public string Key { get; set; }

		public string Robots { get => GlobalRobots; set => GlobalRobots = value; }

		public static string GlobalRobots { get; set; }


		public List<GdsFileTaskReimport> Reimports { get; set; }



		//---g



		public void Execute()
		{

			if (Key.No())
				return;

			try
			{
				var loadedOn = db.Configuration.DrctWebService_LoadedOn ?? DateTime.Today;

				var xml = LoadXml(loadedOn, Key);
				
				//_log.Info("Loaded xml: \r\n" + xml);
				

				var files = GetFilesFromXml<DrctXmlFile>(loadedOn, xml);

				if (files.No())
				{
					if (loadedOn < DateTime.Today)
					{
						db.Commit(() =>
						{
							db.Configuration.DrctWebService_LoadedOn = loadedOn.AddDays(1);
							db.Save(db.Configuration);
						});
					}

					return;
				}


				foreach (var file in files)
				{
					try
					{

						_log.Info($"Import DRCT-file {file.Name}...");


						var reimport = Reimports.By(a => file.Content.Contains(a.OfficeCode));

						if (reimport != null)
							file.SaveToInboxFolder(reimport.InboxPath);
						else
							db.GdsFile.AddFile(file);

					}
					catch (Exception ex)
					{
						_log.Error(ex);
					}
				}

			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}
		}



		static XDocument LoadXml(DateTime importDate, string key)
		{
			/*
{
	"method": "get",
	"url": "https://api.drct.aero/reporting/tickets",
	"query": {
		"date_from": "2021-07-30",
		"date_to": "2021-07-31"
	},
	"headers": {
		"Accept": "application/xml",
		"authorization": "Bearer CSWTG7_MT6UiQmezfuaqndGamgZfzJPM2TYHikc4momSDTsSjpMQyfwmXZetsG8"
	}
}
			*/

			var url =
				"https://api.drct.aero/reporting/tickets"
				+ $"?date_from={importDate:yyyy-MM-dd}"
				+ $"&date_to={importDate.AddDays(1):yyyy-MM-dd}"
			;

			// https://api.drct.aero/reporting/tickets?date_from=20211007&date_to=20211008

			var request = WebRequest.Create(url) as HttpWebRequest;

			if (request == null)
				return null;


			request.Accept = "application/xml";
			request.Headers.Add("authorization", "Bearer "+ key);
			

			//ServicePointManager.Expect100Continue = true; 
			ServicePointManager.SecurityProtocol = 
				SecurityProtocolType.Ssl3 |
				SecurityProtocolType.Tls |
				SecurityProtocolType.Tls11 | 
				SecurityProtocolType.Tls12 //|
				//SecurityProtocolType.Tls13
			;


			var response = request.GetResponse() as HttpWebResponse;

			var stream = response?.GetResponseStream();

			if (stream == null)
				return null;


			XDocument xml;

			using (var sr = new StreamReader(stream, true))
			{
				xml = XDocument.Load(sr);
			}
			

			return xml;

		}



		TGdsFile[] GetFilesFromXml<TGdsFile>(DateTime importDate, XDocument xml)
			where TGdsFile : GdsFile, new()
		{

			if (xml == null)
				return null;


			var xTickets = xml.Els("tickets", "ticket")?.ToArray();

			if (xTickets.No()) 
				return null;



			var minImportDate = importDate.Date;
			var maxImportDate = minImportDate.AddDays(1).AddSeconds(-1);


			var importedFiles = (

				from a in db.GdsFile.Query

				where 
					a.TimeStamp >= minImportDate && a.TimeStamp <= maxImportDate
					&& a.FileType == GdsFileType.DrctXmlFile

				select a.Name

			).ToArray();



			var tickets =

				from ticket in xTickets

				let created = ticket.Value("last_transaction_at").As().DateTimen
				let number = ticket.Value("number") ?? ticket.Value("locator")
				let passenger = ticket.Value("passenger", "last_name") + "_" + ticket.Value("passenger", "first_name")
				let status = ticket.Value("status")?.ToLower()
				let name = $"AviaTicket_{number}-{passenger}-{status}.xml"

				where !importedFiles.Contains(name)

				select new TGdsFile
				{
					TimeStamp = created ?? DateTime.Now,
					Name = name,
					Content = ticket.ToString(),
				}

			;



			var files = tickets.ToArray();


			return files;

		}



		//---g



		private static readonly ILog _log = LogManager.GetLogger(typeof(DrctWebServiceTask));



		//---g
		
	}






	//===g



}