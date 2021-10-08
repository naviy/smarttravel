using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Luxena.Base.Data;
using Luxena.Base.Data.NHibernate;
using Luxena.Base.Managers;
using Luxena.Castle;
using Luxena.Travel.Domain;
using Luxena.Travel.Reports;

using NHibernateConfiguration = NHibernate.Cfg.Configuration;
using CastleConfiguration = Castle.Windsor.Installer.Configuration;
using LuxenaBaseService = Luxena.Base.Services.BaseService;


namespace Luxena.Travel.Config
{
	public class ManagersInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Kernel.ComponentRegistered += (key, handler) =>
			{
				var type = handler.ComponentModel.Service;

				if (type.Is<GenericManager>() && !ClassManagerProvider.Managers.Contains(type))
					ClassManagerProvider.Managers.Add(type);
			};

			container.Register(
				Component.For<TransactionManager, ITransactionManager>().LifeStyle.Scoped(),
				Component.For<GenericManager>().LifeStyle.Scoped(),
				Component.For<ClassManagerProvider, IClassManagerProvider>()
					.DependsOn(Property.ForKey("TypeNamePattern").Eq("{0}Manager"))
					.LifeStyle.Scoped(),
				AllTypes.FromAssemblyNamed("Luxena.Travel")
					.BasedOn<GenericManager>()
					.Configure(conf => conf.LifeStyle.Scoped())//,
				//Component.For<InvoicePrinter, IInvoicePrinter>().LifeStyle.Scoped(),
				//Component.For<ReceiptPrinter, IReceiptPrinter>().LifeStyle.Scoped()
			);
		}
	}
}
