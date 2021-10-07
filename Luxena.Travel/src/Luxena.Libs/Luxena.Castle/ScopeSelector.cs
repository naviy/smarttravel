using System.Collections.Generic;
using System.Linq;


namespace Luxena.Castle
{
	public class ScopeSelector : IScopeAccessor
	{
		public ScopeSelector()
		{
			_scopes = new List<IScopeAccessor>();
		}

		public Stack<Scope> CurrentStack
		{
			get
			{
				return _scopes.Select(scope => scope.CurrentStack).FirstOrDefault(stack => stack != null) ?? _default.CurrentStack;
			}
		}

		public ScopeSelector Add(IScopeAccessor scope)
		{
			_scopes.Add(scope);
			return this;
		}

		private readonly IScopeAccessor _default = new ThreadScopeAccessor();
		private readonly List<IScopeAccessor> _scopes;
	}
}