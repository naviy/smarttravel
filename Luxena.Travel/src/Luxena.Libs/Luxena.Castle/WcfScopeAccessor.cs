using System.Collections.Generic;
using System.ServiceModel;



namespace Luxena.Castle
{
	public class WcfScopeAccessor : IScopeAccessor
	{
		public Stack<Scope> CurrentStack
		{
			get
			{
				var operationContext = OperationContext.Current;

				if (operationContext == null)
					return null;

				var extensions = operationContext.InstanceContext.Extensions;

				var extention = extensions.Find<InstanceContextScopeExtension>();

				if (extention == null)
				{
					extention = new InstanceContextScopeExtension();

					extensions.Add(extention);
				}

				return extention.Stack;
			}
		}
	}
}