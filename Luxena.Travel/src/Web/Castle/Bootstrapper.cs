using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Common.Logging;

using Luxena.Travel.Security;

using NHibernate;

using Luxena.Castle;
using Luxena.Travel.Domain;
using Luxena.Travel.Reports;
using Luxena.Travel.Services;

using InstallerConfiguration = Castle.Windsor.Installer.Configuration;


namespace Luxena.Travel.Web.Castle
{
	public static class Bootstrapper
	{
		public static WindsorContainer WebContainer { get; private set; }

		public static WindsorContainer TaskContainer { get; private set; }

		public static Exception Exception { get; private set; }

		public static void Configure()
		{
			try
			{
				CreateConfigWatchers();

				bool.TryParse(ConfigurationManager.AppSettings.Get("showDebugOnBrowser"), out _showDebugOnBrowser);

				CreateContainers();
			}
			catch (Exception ex)
			{
				Exception = new Exception("Bootstrapper.Configure exception", ex); 

				_log.Error(ex);
			}
		}

		public static void Release()
		{
			WebContainer?.Dispose();

			TaskContainer?.Dispose();
		}

		private static void CreateContainers()
		{
			PdfUtility.SetFontsPath("static/fonts");

			var cfg = Config.ConfigurationBuilder.Build(NHibernateConfigFilePath);

			var sessionFactory = cfg.BuildSessionFactory();

			_resolverProvider = new ServiceResolverProvider();

			CreateWebContainer(cfg, sessionFactory);
			CreateTaskContainer(cfg, sessionFactory);

			ServiceResolver.Provider = _resolverProvider;
		}

		private static void CreateWebContainer(NHibernate.Cfg.Configuration cfg, ISessionFactory sessionFactory)
		{
			WebContainer = new WindsorContainer();
			WebContainer.AddScopeSubsystem(new WebScopeAccessor());

			_resolverProvider.WebResolver = new CastleServiceResolver(WebContainer);

			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(WebContainer.Kernel));

			WebContainer.Install(new Config.DataInstaller(cfg, sessionFactory, () => HttpContext.Current.User.Identity.Name));
			WebContainer.Install(new Config.ManagersInstaller());
			WebContainer.Install(new Config.ServicesInstaller(_showDebugOnBrowser));
			WebContainer.Install(new ControllersInstaller());
			WebContainer.Install(InstallerConfiguration.FromXmlFile(ClientConfigFilePath));

			WebContainer.Register(
				Component.For<IServiceResolver>().Instance(_resolverProvider.WebResolver),
				Component.For<ModificationInterceptor, IInterceptor>()
					.OnCreate((k, x) =>
					{
						var token = AuthenticationToken.Parse(HttpContext.Current.User.Identity.Name);
						x.Initialize(token == null ? "" : token.UserName);
					})
					.LifeStyle.Scoped()
				);
		}

		private static void CreateTaskContainer(NHibernate.Cfg.Configuration cfg, ISessionFactory sessionFactory)
		{
			TaskContainer = new WindsorContainer();
			TaskContainer.AddScopeSubsystem(new ThreadScopeAccessor());

			var tasks = new List<string>();

			TaskContainer.Kernel.ComponentRegistered += (key, handler) =>
			{
				if (handler.ComponentModel.Service.Is<BaseTaskRunner>())
					tasks.Add(key);
			};

			_resolverProvider.TaskResolver = new CastleServiceResolver(TaskContainer);

			TaskContainer.Install(InstallerConfiguration.FromXmlFile(ConfigurationManager.AppSettings["client.config"].ResolvePath()));
			TaskContainer.Install(new Config.DataInstaller(cfg, sessionFactory, () => "SYSTEM"));
			TaskContainer.Install(new Config.ManagersInstaller());
			TaskContainer.Install(new Config.ServicesInstaller(true));

			TaskContainer.Register(
				Component.For<IServiceResolver>().Instance(_resolverProvider.TaskResolver),
				Component.For<ModificationInterceptor, IInterceptor>()
					.OnCreate((k, x) => x.Initialize("SYSTEM"))
					.LifeStyle.Scoped()
			);

			foreach (var task in tasks)
				TaskContainer.Resolve<BaseTaskRunner>(task).Start();
		}

		private static string NHibernateConfigFilePath => 
			ConfigurationManager.AppSettings["nhibernate.config"].ResolvePath();

		private static string ClientConfigFilePath => 
			ConfigurationManager.AppSettings["client.config"].ResolvePath();

		private static void CreateConfigWatchers()
		{
			var application = HttpContext.Current.Application;

			application.Add("client.config.watcher", CreateWatcher(ClientConfigFilePath));
			application.Add("nhibernate.config.watcher", CreateWatcher(NHibernateConfigFilePath));
		}

		private static FileSystemWatcher CreateWatcher(string filePath)
		{
			var watcher = new FileSystemWatcher
			{
				Path = Path.GetDirectoryName(filePath),
				Filter = Path.GetFileName(filePath),
				EnableRaisingEvents = true
			};

			watcher.Changed += delegate { HttpRuntime.UnloadAppDomain(); };

			return watcher;
		}

		private static readonly ILog _log = LogManager.GetLogger(typeof (Bootstrapper));

		private static bool _showDebugOnBrowser;

		private static ServiceResolverProvider _resolverProvider;
	}
}