using System;
using System.ServiceModel.Configuration;


namespace Luxena.ServiceModel
{
	public class AutoNamespaceExtension : BehaviorExtensionElement
	{
		protected override object CreateBehavior()
		{
			return AutoNamespaceBehavior.Instance;
		}

		public override Type BehaviorType
		{
			get { return typeof (AutoNamespaceBehavior); }
		}
	}
}