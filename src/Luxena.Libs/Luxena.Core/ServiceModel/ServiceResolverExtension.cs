using System;
using System.ServiceModel.Configuration;


namespace Luxena.ServiceModel
{
	public class ServiceResolverExtension : BehaviorExtensionElement
	{
		protected override object CreateBehavior()
		{
			return ServiceResolverBehavior.Instance;
		}

		public override Type BehaviorType
		{
			get { return typeof (ServiceResolverBehavior); }
		}
	}
}