using System.Web;

using Luxena.Base.Data.NHibernate;

using SimpleInjector;
using SimpleInjector.Integration.Web;


namespace Luxena.Travel.Web
{

	public partial class AppConfig
	{

		public static void RegisterDomain(HttpApplication webApplication, Container container)
		{
			var requestLifestyle = new WebRequestLifestyle();

			//var sessionFactory = ConfigurationFactory.Create(webApplication.Server.MapPath("~/nhibernate.config")).BuildSessionFactory();
			var cfg = Config.ConfigurationBuilder.Build(webApplication.Server.MapPath("~/nhibernate.config"));
			var sessionFactory = cfg.BuildSessionFactory();
			container.RegisterSingle(sessionFactory);

			//container.RegisterManyForOpenGeneric(typeof(Entity.Service<>), requestLifestyle, typeof(Entity).Assembly);
			//container.RegisterManyForOpenGeneric(typeof(IEventHandler<>), container.RegisterAll, typeof(Entity).Assembly);
			//container.Register<SessionManager, SessionManager>(requestLifestyle);

			container.Register(() => new Domain.Domain(container.GetInstance)
			{
				GetIdentityName = () => HttpContext.Current.User.Identity.Name,
			}, requestLifestyle);

			container.Register<TransactionManager, TransactionManager>(requestLifestyle);

			Domain.Domain.Configure();
		}

	}

}