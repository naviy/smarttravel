using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;

using Common.Logging;

using Luxena.Base.Managers;
using Luxena.Base.Metamodel;
using Luxena.Castle;
using Luxena.Travel.Domain;
using Luxena.Travel.Web.Castle;



namespace Luxena.Travel.Web
{


	public class MvcApplication : HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			log4net.Util.LogLog.EmitInternalMessages = true;

			_log.Info("Application started");

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			Bootstrapper.Configure();

			Class.Of<GlobalSearchEntity>();

			System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender1, certificate, chain, sslPolicyErrors) => true;
		}


		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			if (Bootstrapper.Exception != null)
			{
				try
				{
					throw Bootstrapper.Exception;
				}
				finally
				{
					Bootstrapper.Configure();
				}
			}

			var webContainer = Bootstrapper.WebContainer;
			Context.Items[ScopeId] = webContainer.OpenScope();
		}


		private void Application_AuthenticateRequest(object sender, EventArgs e)
		{
			if (User == null) return;

			var sc = Bootstrapper.WebContainer.Resolve<ISecurityContext>();

			if (sc.IsValid)
				return;

			HttpContext.Current.User = null;
		}


		protected void Application_EndRequest(object sender, EventArgs e)
		{
			if (Context.Items.Contains(ScopeId))
				((IDisposable)Context.Items[ScopeId]).Dispose();

			if (Response.RedirectLocation.No())
				return;

			var login = Request.ApplicationPath;

			login = login.No() ? "/login" : login?.TrimEnd('/') + "/login";

			if (Response.RedirectLocation.StartsWith(login + "?ReturnUrl="))
				Response.RedirectLocation = login;
		}


		protected void Application_Error(object sender, EventArgs e)
		{
			_log.Error(Server.GetLastError());
		}


		protected void Application_End(object sender, EventArgs e)
		{
			Bootstrapper.Release();

			_log.Info("Application finished");
		}


		private static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}


		private static void RegisterRoutes(RouteCollection routes)
		{
			routes.MapRoute("LogIn", "login", new { controller = "Login", action = "LogIn" });
			routes.MapRoute("LogOut", "logout", new { controller = "Login", action = "LogOut" });
			routes.MapRoute("Home", "", new { controller = "Home", action = "Index" });
			routes.MapRoute("Reports", "reports/{action}/{fileName}", new { controller = "Reports" });
			routes.MapRoute("AgentReport", "reports/agent/{date}/{fileName}", new { controller = "Reports", action = "Agent" });
			routes.MapRoute("Export", "export/{className}/{fileName}", new { controller = "Export", action = "Export" });
			routes.MapRoute("Import", "import/{action}", new { controller = "Import" });
			routes.MapRoute("Print", "print/{action}/{fileName}", new { controller = "Print" });
			routes.MapRoute("Files", "files/{action}/{fileName}", new { controller = "Files" });
			routes.MapRoute("Bonus", "bonus/{action}", new { controller = "Bonus" });
			routes.MapRoute("Repair", "repair/{action}/{startDate}/{endDate}", new { controller = "Repair" });
			routes.MapRoute("Repair0", "repair/{action}", new { controller = "Repair" });
		}


		private const string ScopeId = "ScopeId";

		private static readonly ILog _log = LogManager.GetLogger(typeof (MvcApplication).Namespace);

	}



}