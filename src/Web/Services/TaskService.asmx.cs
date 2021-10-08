using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{
	public class TaskService : DomainWebService
	{
		[WebMethod]
		public object ChangeStatus(object[] ids, TaskStatus status, RangeRequest @params)
		{
			return db.Commit(() => db.Task.ChangeStatus(ids, status, @params));
		}
	}
}
