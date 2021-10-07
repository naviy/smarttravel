using System;

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Luxena.Base.Managers;
using Luxena.Castle;

using NHibernate;

using Luxena.Base.Data;

using GenericDao = Luxena.Travel.Domain.GenericDao;


namespace Luxena.Travel.Config
{
	public class DataInstaller : IWindsorInstaller
	{
		public DataInstaller(NHibernate.Cfg.Configuration cfg, ISessionFactory factory, Func<string> getIdentityName)
		{
			_cfg = cfg;
			_factory = factory;
			_getIdentityName = getIdentityName;
		}

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<ISessionFactory>().Instance(_factory),

				Component.For<NHibernate.Cfg.Configuration>().Instance(_cfg),
				Component.For<GenericDao, IGenericDao>().LifeStyle.Scoped(),

				AllTypes.FromAssemblyNamed("Luxena.Travel")
					.Pick()
					.If(type => type.Name.EndsWith("Dao"))// || type == typeof(ConfigurationSource))
					.WithService.Self().WithService.DefaultInterface()
					.Configure(conf => conf.LifeStyle.Scoped()
				),

				Component.For<Domain.Domain>()
					.LifeStyle.Scoped()
					.UsingFactoryMethod(() => new Domain.Domain(type =>
					{
						try
						{
							return container.Resolve(type);
						}
						catch (ComponentNotFoundException)
						{
							return null;
						}
					})
					{
						GetIdentityName = _getIdentityName,
					}),

				Component.For<ISecurityContext>()
					.LifeStyle.Scoped()
					.UsingFactoryMethod(() =>
					{
						var db = container.Resolve<Domain.Domain>();
						var service = db.Security;
						return service;
					})

			);


			Domain.Domain.Configure();

			foreach (var serviceType_ in Domain.Domain.ServiceTypes())
			{
				var serviceType = serviceType_;

				container.Register(
					Component.For(serviceType)
						.LifeStyle.Scoped()
						.UsingFactoryMethod(() =>
						{
							var db = container.Resolve<Domain.Domain>();
							var service = db.Service(serviceType);
							return service;
						})
				);
			}
		}


		private readonly NHibernate.Cfg.Configuration _cfg;
		private readonly ISessionFactory _factory;
		private readonly Func<string> _getIdentityName;
	}

}