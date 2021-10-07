using System;
using System.Web.Services;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class AppService : DomainWebService
	{

		[WebMethod]
		public AppStateResponse GetAppStateChanges(AppStateRequest request)
		{
			return db.Commit(() => db.App.GetAppStateChanges(request));
		}

		[WebMethod]
		public void CloseTask(object id)
		{
			db.Commit(() => db.App.CloseTask(id));
		}

		[WebMethod]
		public Party.Reference[] GetDocumentOwners()
		{
			return db.Commit(() => db.App.GetDocumentOwners());
		}

		[WebMethod]
		public void UpdateAnalytics()
		{
			db.Commit(() => db.App.UpdateAnalytics());
		}

		[WebMethod]
		public AppParameters GetAppParameters()
		{
			return db.Commit(() => db.App.GetAppParameters());
		}

		[WebMethod]
		public HomeModel GetHomeModel(DateTime? statsFrom, DateTime? statsTo)
		{
			return db.Commit(() => HomeModel.New(db, dc, statsFrom, statsTo));
		}

	}

}
