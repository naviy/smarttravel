using System;


namespace Luxena.Base.Metamodel
{
	public sealed class DefaultTypeResolver : ITypeResolver
	{
		public static readonly DefaultTypeResolver Instance = new DefaultTypeResolver();

		private DefaultTypeResolver()
		{
		}

		public Type Resolve(object obj)
		{
			return obj.GetType();
		}
	}
}