using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;


namespace Luxena.ServiceModel
{
	public class AutoNamespaceBehavior : IServiceBehavior
	{
		public static readonly AutoNamespaceBehavior Instance = new AutoNamespaceBehavior();

		private AutoNamespaceBehavior()
		{
			
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			if (IsDefaultNamespace(serviceDescription.Namespace))
				serviceDescription.Namespace = ResolveNamespace(serviceDescription.ServiceType);

			foreach (var endpoint in serviceDescription.Endpoints)
			{
				if (IsDefaultNamespace(endpoint.Contract.Namespace))
					endpoint.Contract.Namespace = ResolveNamespace(endpoint.Contract.ContractType);

				if (IsDefaultNamespace(endpoint.Binding.Namespace))
					endpoint.Binding.Namespace = endpoint.Contract.Namespace;
			}
		}

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
			Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		private static bool IsDefaultNamespace(string @namespace)
		{
			return string.IsNullOrEmpty(@namespace) || @namespace == "http://tempuri.org/";
		}

		private static string ResolveNamespace(Type type)
		{
			var contractNamespaces = type.Assembly.GetCustomAttributes(typeof(ContractNamespaceAttribute), false);

			var contractNamespace = (ContractNamespaceAttribute) contractNamespaces.Find(x => ((ContractNamespaceAttribute) x).ClrNamespace == type.Namespace);

			return contractNamespace != null ? contractNamespace.ContractNamespace : type.Namespace;
		}
	}
}