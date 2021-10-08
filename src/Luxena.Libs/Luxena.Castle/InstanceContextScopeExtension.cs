using System.Collections.Generic;
using System.ServiceModel;


namespace Luxena.Castle
{
	internal class InstanceContextScopeExtension : IExtension<InstanceContext>
	{
		public InstanceContextScopeExtension()
		{
			Stack = new Stack<Scope>();
		}

		public Stack<Scope> Stack { get; private set; }

		public void Attach(InstanceContext owner)
		{
		}

		public void Detach(InstanceContext owner)
		{
		}
	}
}