using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;


namespace Luxena.ServiceModel
{
	public class ServiceResolverInstanceProvider : IInstanceProvider
	{
		public ServiceResolverInstanceProvider(Type serviceType)
		{
			_serviceType = serviceType;
		}

		public object GetInstance(InstanceContext instanceContext)
		{
			return GetInstance(instanceContext, null);
		}

		public object GetInstance(InstanceContext instanceContext, Message message)
		{
			var serviceResolver = ServiceResolver.Current;

			var scope = serviceResolver.OpenScope();

			try
			{
				return serviceResolver.Resolve(_serviceType);
			}
			catch (Exception)
			{
				scope.Dispose();

				throw;
			}
		}

		public void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
			var serviceResolver = ServiceResolver.Current;

			try
			{
				serviceResolver.Release(instance);
			}
			finally
			{
				serviceResolver.CloseScope();
			}
		}

		private readonly Type _serviceType;
	}
}