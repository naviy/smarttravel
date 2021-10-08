using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


namespace Luxena.ServiceModel
{
	public class ServiceResolverBehavior : IServiceBehavior
	{
		public static readonly ServiceResolverBehavior Instance = new ServiceResolverBehavior();

		private ServiceResolverBehavior()
		{
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (var cdb in serviceHostBase.ChannelDispatchers)
			{
				var channelDispatcher = cdb as ChannelDispatcher;

				if (channelDispatcher == null)
					return;

				var instanceProvider = new ServiceResolverInstanceProvider(serviceDescription.ServiceType);

				foreach (var endpointDispatcher in channelDispatcher.Endpoints)
					endpointDispatcher.DispatchRuntime.InstanceProvider = instanceProvider;
			}
		}

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
			Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}
	}
}