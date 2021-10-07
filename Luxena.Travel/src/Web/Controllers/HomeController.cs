using System.Web.Mvc;
using System.Web.Security;

using Luxena.Travel.Security;
using Luxena.Travel.Web.Models;



namespace Luxena.Travel.Web.Controllers
{


	public class HomeController : Controller
	{

		public Domain.Domain db { get; set; }


		public ActionResult Index()
		{
			var ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Else(() => Request.ServerVariables["REMOTE_ADDR"]);
			var requestInfo = Request.UserAgent;

			db.Commit(() => db.UserVisit.Add(ip, requestInfo));

			return View();
		}

	}


}
