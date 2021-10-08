using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http.Dependencies;

using SimpleInjector;


namespace Luxena.Travel.Web.Support
{
	public sealed class WebApiDependencyResolver : IDependencyResolver
	{
		public WebApiDependencyResolver(Container container)
		{
			_container = container;
		}

		[DebuggerStepThrough]
		public IDependencyScope BeginScope()
		{
			return this;
		}

		[DebuggerStepThrough]
		public object GetService(Type serviceType)
		{
			return ((IServiceProvider) _container).GetService(serviceType);
		}

		[DebuggerStepThrough]
		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _container.GetAllInstances(serviceType);
		}

		[DebuggerStepThrough]
		public void Dispose()
		{
		}

		private readonly Container _container;
	}
}