using System;
using System.Collections.Generic;

using Castle.MicroKernel;


namespace Luxena.Castle
{
	internal class ScopeSubsystem : AbstractSubSystem
	{
		public ScopeSubsystem(IScopeAccessor scopeAccessor)
		{
			if (scopeAccessor == null)
				throw new ArgumentNullException("scopeAccessor");

			_scopeAccessor = scopeAccessor;
		}

		public Scope GetCurrentScope()
		{
			var scopes = GetCurrentScopes();

			return scopes.Count > 0 ? scopes.Peek() : null;
		}

		public IDisposable OpenScope()
		{
			return new Scope(GetCurrentScopes());
		}

		public void CloseScope()
		{
			var scope = GetCurrentScope();

			if (scope == null)
				throw new InvalidOperationException("There are no opened scopes.");

			scope.Dispose();
		}

		private Stack<Scope> GetCurrentScopes()
		{
			var scopes = _scopeAccessor.CurrentStack;

			if (scopes == null)
				throw new InvalidOperationException("Unable to determine current scope stack. Did you provide the correct IScopeAccessor strategy?");

			return scopes;
		}

		private readonly IScopeAccessor _scopeAccessor;
	}
}