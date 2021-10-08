using System;
using System.Collections.Generic;


namespace Luxena.Castle
{
	public class ThreadScopeAccessor : IScopeAccessor
	{
		public Stack<Scope> CurrentStack
		{
			get
			{
				var stack = _scopes;

				if (stack == null)
				{
					stack = new Stack<Scope>();

					_scopes = stack;
				}

				return stack;
			}
		}

		[ThreadStatic]
		private static Stack<Scope> _scopes;
	}
}