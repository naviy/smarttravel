using System.Web.Services;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class ClosedPeriodService : DomainWebService
	{
		[WebMethod]
		public ClosedPeriodDto GetLastClosedPeriod()
		{
			return db.Commit(() => dc.ClosedPeriod.Last());
		}

		[WebMethod]
		public bool CanUpdate(ClosedPeriodDto dto)
		{
			return db.Commit(() => dc.CanUpdate(dto));
		}

	}

}
