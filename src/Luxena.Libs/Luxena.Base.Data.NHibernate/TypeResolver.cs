using Luxena.Base.Metamodel;

using NHibernate;
using NHibernate.Proxy;


namespace Luxena.Base.Data.NHibernate
{
	public class TypeResolver : ITypeResolver
	{
		public System.Type Resolve(object obj)
		{
			return obj.IsProxy() ? NHibernateUtil.GetClass(obj) : NHibernateProxyHelper.GuessClass(obj);
		}
	}
}