using System.Web.Mvc;
using System.Web.Security;

using Luxena.Travel.Security;
using Luxena.Travel.Web.Models;



namespace Luxena.Travel.Web.Controllers
{


	public class LoginController : Controller
	{

		public Domain.Domain db { get; set; }


		public ActionResult LogIn()
		{

			return View();

		}


		[HttpPost]
		public ActionResult LogIn(LoginModel model)
		{
			var sessionId = db.Commit(() => db.User.Login(model.UserName, model.Password));

			if (sessionId == null)
			{
				ModelState.AddModelError("", "Некорректное имя пользователя или пароль");
				return View();
			}

			FormsAuthentication.SetAuthCookie(new AuthenticationToken(model.UserName, sessionId).ToString(), model.RememberMe, Request.ApplicationPath);

			return Redirect(Request.ApplicationPath);
		}


		[HttpPost]
		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("login");
		}

	}


}
