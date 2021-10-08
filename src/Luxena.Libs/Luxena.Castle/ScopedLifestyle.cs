using System;

using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;


namespace Luxena.Castle
{
	public class ScopedLifestyle : AbstractLifestyleManager
	{
		public override object Resolve(CreationContext context)
		{
			var scope = Kernel.GetScopeSubsystem().GetCurrentScope();

			if (scope == null)
				throw new InvalidOperationException(string.Format("Component '{0}' has scoped lifestyle, and it could not be resolved because no scope is accessible. Did you forget to call container.BeginScope()?", Model.Name));

			var instance = scope.GetInstance(this);

			if (instance == null)
			{
				instance = base.Resolve(context);

				scope.AddInstance(this, instance);
			}

			return instance;
		}

		public override bool Release(object instance)
		{
			return _evicting && base.Release(instance);
		}

		public override void Dispose()
		{
		}

		internal void Evict(object instance)
		{
			if (_evicting) return;

			_evicting = true;
			try
			{
				// that's not really thread safe, should we care about it? (c) Castle
				Kernel.ReleaseComponent(instance);
			}
			//Fake http://stackoverflow.com/questions/8926586/castle-windsor-3-interceptor-not-releasing-components-created-by-a-typed-factory
			catch
			{
				
			}
			finally
			{
				_evicting = false;
			}
		}

		private bool _evicting;
	}
}