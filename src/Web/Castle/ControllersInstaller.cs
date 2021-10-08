using System.Web.Mvc;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Luxena.Castle;
using Luxena.Travel.Web.Controllers;


namespace Luxena.Travel.Web.Castle
{
	public class ControllersInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(AllTypes.FromThisAssembly()
				.BasedOn<IController>()
				.If(Component.IsInSameNamespaceAs<LoginController>())
				.If(t => t.Name.EndsWith("Controller"))
				.Configure(c => c.LifeStyle.Scoped()));
		}
	}
}