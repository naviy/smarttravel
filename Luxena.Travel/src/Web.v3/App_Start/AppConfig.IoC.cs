using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

//using Luxena.Travel.NHibernate;
using Luxena.Travel.Web.Support;

using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;



namespace Luxena.Travel.Web
{

	public partial class AppConfig
	{

		public static void RegisterIoC(HttpApplication webApplication)
		{
			var container = new Container();
			
			RegisterDomain(webApplication, container);

			container.RegisterInitializer<ApiController>(container.InjectProperties);
			container.RegisterInitializer<Controller>(container.InjectProperties);
			container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			container.RegisterMvcAttributeFilterProvider();
			
			container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new WebApiDependencyResolver(container);

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
		}


	}

}