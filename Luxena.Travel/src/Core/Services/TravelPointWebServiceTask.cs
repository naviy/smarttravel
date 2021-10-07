using System;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;

using Common.Logging;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Services
{


	public class TravelPointWebServiceTask : ITask
	{

		//bool ITask.IsStarted { get; set; }

		public Domain.Domain db { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
		public string Key { get; set; }

		public string Robots { get { return GlobalRobots; } set { GlobalRobots = value; } }

		public static string GlobalRobots { get; set; }


		public void Execute()
		{
			if (UserName.No() || Password.No() || Key.No()) return;

			try
			{
				var loadedOn = db.Configuration.TravelPointWebService_LoadedOn ?? DateTime.Today.AddDays(-30);

				var sxml = LoadXml(loadedOn);


				//_log.Info("Loaded xml: \r\n" + sxml);

				var files = GetFilesFromXml<TravelPointXmlFile>(loadedOn, sxml);

				if (files.No())
				{
					if (loadedOn < DateTime.Today)
					{
						db.Commit(() =>
						{
							db.Configuration.TravelPointWebService_LoadedOn = loadedOn.AddDays(1);
							db.Save(db.Configuration);
						});
					}

					return;
				}


				foreach (var file in files)
				{
					try
					{
						_log.Info($"Import TravelPoint-file {file.Name}...");
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


		string LoadXml(DateTime importDate)
		{
			var client = new TravelPointWebService.ImportServiceXMLPortTypeClient(
				new BasicHttpBinding
				{
					Name = "XMLWebServiceSoap",
					MaxReceivedMessageSize = 0xfffffff,
					Security = new BasicHttpSecurity
					{
						Mode = BasicHttpSecurityMode.TransportCredentialOnly,
						Transport = new HttpTransportSecurity
						{
							ClientCredentialType = HttpClientCredentialType.Basic,
						}
					}
				},
				new EndpointAddress("http://46.4.91.251/TC/ws/ImportServiceXML.1cws")
			);

			client.ClientCredentials.UserName.UserName = UserName;
			client.ClientCredentials.UserName.Password = Password;


			var base64 = client.Import(Key, importDate.ToString("yyyy-MM-dd"), importDate.ToString("yyyy-MM-dd"));

			if (base64.No())
				return null;

			string sxml = null;

			try
			{
				sxml = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64));

				var startIndex = sxml.IndexOf('<');
				if (startIndex > 0)
					sxml = sxml.Substring(startIndex);
			}
			catch
			{
				_log.Error("TravelPointWebServiceTask: ошибка при чтении из сервиса.\r\n" + base64);
			}

			return sxml;
		}



		TGdsFile[] GetFilesFromXml<TGdsFile>(DateTime importDate, string sxml)
			where TGdsFile : GdsFile, new()
		{

			if (sxml.No()) 
				return null;


			var xml = XDocument.Parse(sxml);

			var xTickets = xml.Root.Els("avia-tickets", "avia-ticket")?.ToArray();

			if (xTickets.No()) 
				return null;
			

			var minImportDate = importDate.Date;
			var maxImportDate = minImportDate.AddDays(1).AddSeconds(-1);

			var importedFiles = (

				from a in db.GdsFile.Query

				where 
					a.TimeStamp >= minImportDate && a.TimeStamp <= maxImportDate
					&& a.FileType == GdsFileType.TravelPointXmlFile

				select a.Name

			).ToArray();


			var tickets =

				from ticket in xTickets

				let created = ticket.Value("issuing-date").As().DateTimen
				let number = ticket.Value("ticket-number")
				let status = ticket.Value("status")?.ToLower()
				let name = "Ticket " + number + ": " + status

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
		

		private static readonly ILog _log = LogManager.GetLogger(typeof(TravelPointWebServiceTask));

	}


}