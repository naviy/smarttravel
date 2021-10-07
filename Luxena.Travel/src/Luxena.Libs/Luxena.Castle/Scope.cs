using System;
using System.Collections.Generic;
using System.Linq;


namespace Luxena.Castle
{
	public class Scope : IDisposable
	{
		internal Scope(Stack<Scope> scopes)
		{
			_scopes = scopes;

			_scopes.Push(this);
		}

		public void Dispose()
		{
			if (_scopes.Peek() != this)
				throw new InvalidOperationException("The scope is not current.  Did you forget to end a child scope?");

			_scopes.Pop();

			if (_cache == null)
				return;

			foreach (var pair in _cache.Reverse())
				pair.Key.Evict(pair.Value);

			_cache.Clear();
		}

		internal void AddInstance(ScopedLifestyle id, object instance)
		{
			if (_cache == null)
				_cache = new Dictionary<ScopedLifestyle, object>();

			_cache.Add(id, instance);
		}

		internal object GetInstance(ScopedLifestyle id)
		{
			if (_cache == null)
				return null;

			object instance;

			_cache.TryGetValue(id, out instance);

			return instance;
		}

		private readonly Stack<Scope> _scopes;
		private Dictionary<ScopedLifestyle, object> _cache;
	}
}