using System;


namespace Luxena
{
	public interface IServiceResolver
	{
		IDisposable OpenScope();

		void CloseScope();

		T Resolve<T>();
		T Resolve<T>(string name);

		object Resolve(Type type);
		object Resolve(Type type, string name);

		void Resolve(object instance);

		void Release(object instance);
	}
}