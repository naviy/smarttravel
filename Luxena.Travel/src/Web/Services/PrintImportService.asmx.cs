using System.Web.Services;


namespace Luxena.Travel.Web.Services
{
	[WebService(Namespace = "http://travel.luxena.com/services")]
	public class PrintImportService : DomainWebService
	{
		public Travel.Services.PrintImportService Service { get; set; }

		[WebMethod]
		public bool ImportTickets(string user, string password, string office, byte[] exportFilesZip, out string output)
		{
			return Service.ImportTickets(user, password, office, exportFilesZip, out output);
		}

		[WebMethod]
		public bool ImportTicketsLocally(string path)
		{
			return Service.ImportTicketsLocally(path);
		}

		[WebMethod]
		public bool GetUpdate(string currentVersion, out byte[] update)
		{
			return Service.GetUpdate(currentVersion, out update);
		}
	}
}
