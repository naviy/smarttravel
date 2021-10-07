using System.Web.Services;

using Luxena.Base.Data;


namespace Luxena.Travel.Web.Services
{
	public class GdsFileService : DomainWebService
	{
		[WebMethod]
		public object Reimport(object[] ids, RangeRequest @params)
		{
			return db.GdsFile.Reimport(ids, @params);
		}

		//public void SaveFile(GdsFile file)
		//{
			
		//}

		//public void SaveFile(GdsFile file, out string userOutput)
		//{
		//	userOutput = null;
		//}
	}
}
