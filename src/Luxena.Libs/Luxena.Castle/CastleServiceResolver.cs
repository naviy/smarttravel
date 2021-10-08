using System;

using Castle.Windsor;


namespace Luxena.Castle
{
	public class CastleServiceResolver : IServiceResolver
	{
		public CastleServiceResolver(IWindsorContainer container)
		{
			_container = container;
		}

		public IDisposable OpenScope()
		{
			return _container.OpenScope();
		}

		public void CloseScope()
		{
			_container.CloseScope();
		}

		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		public T Resolve<T>(string name)
		{
			return _container.Resolve<T>(name);
		}

		public object Resolve(Type type)
		{
			return _container.Resolve(type);
		}

		public object Resolve(Type type, string name)
		{
			return _container.Resolve(type, name);
		}

		public void Resolve(object instance)
		{
			_container.Resolve(instance);
		}

		public void Release(object instance)
		{
			_container.Release(instance);
		}

		private readonly IWindsorContainer _container;
	}
}