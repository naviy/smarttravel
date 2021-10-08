using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Security;


namespace Luxena.Travel.Web
{
	using Domain;


	public class HomeController : Controller
	{

		public ActionResult Index()
		{
			return View();
		}


		public ActionResult LogIn()
		{
			return View();
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult LogIn(LoginModel model)
		{
			string sessionId;

			using (var db = new Domain())
			{
				sessionId = db.Login(model.UserName, model.Password);
			}

			if (sessionId == null)
			{
				if (model.JsonResult)
					return Json(false);

				ModelState.AddModelError("", "Некорректное имя пользователя или пароль");
				return View();
			}

			var userName = new AuthenticationToken(model.UserName, sessionId).ToString();

			FormsAuthentication.SetAuthCookie(userName, model.RememberMe, Request.ApplicationPath);

			if (model.JsonResult)
				return Json(true);

			return Redirect(Request.ApplicationPath);
		}


		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("LogIn");
		}

	}


	public class LoginModel
	{

		[Required]
		public string UserName { get; set; }

		public string Password { get; set; }

		public bool RememberMe { get; set; }

		public bool JsonResult { get; set; }

	}

}
