using System;


namespace Luxena.Base.Metamodel
{
	public static class ClassExtensions
	{
		public static Class GetClass(this Type type)
		{
			return Class.Of(type);
		}

		public static Class GetClass(this object obj)
		{
			return Class.Of(obj);
		}
	}
}