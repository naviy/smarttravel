using System;
using System.Linq.Expressions;
using System.Reflection;

using NHibernate.Mapping.ByCode;


namespace Luxena.Base.Data.NHibernate.Mapping
{
	public static class CollectionMappingExtensions
	{
		public static void Inverse<T1, T2>(this ICollectionPropertiesMapper<T1, T2> mapper, Expression<Func<T2, T1>> property) where T1 : class
		{
			mapper.Inverse(true);
			mapper.Key(property);
		}

		public static void Key<T1, T2>(this ICollectionPropertiesMapper<T1, T2> mapper, Expression<Func<T2, T1>> property) where T1 : class
		{
			var expression = property.Body as MemberExpression;

			if (expression == null)
				throw new ArgumentException("The lambda expression should be a member expression");

			var propertyInfo = expression.Member as PropertyInfo;

			if (propertyInfo == null)
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");

			mapper.Key(k => k.Column(propertyInfo.Name));
		}
	}
}