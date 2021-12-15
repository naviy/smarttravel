using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;




namespace Luxena
{



	//===g






	public static class LinqExtensions
	{

		//---g



		#region Enumerable

		//		[DebuggerStepThrough]
		//		public static void ForEach<T, T2>(this IEnumerable<T> query, Func<T, T2> method)
		//		{
		//			if (query == null) return;
		//
		//			foreach (var item in query)
		//			{
		//				method(item);
		//			}
		//		}

		[DebuggerStepThrough]
		public static void ForEach<T>(this IEnumerable<T> me, Action<T> method)
		{
			if (me == null) return;

			foreach (var item in me)
			{
				method(item);
			}
		}

		[DebuggerStepThrough]
		public static void ForEach<T>(this IEnumerable<T> me, Action<T, int> method)
		{
			if (me == null) return;

			var i = 0;
			foreach (var item in me)
			{
				method(item, i++);
			}
		}



		//		[DebuggerStepThrough]
		//		public static bool Exists<T>(this IEnumerable<T> query, Func<T, bool> match)
		//		{
		//			if (query == null || match == null) return false;
		//
		//			return query.Any(match);
		//		}
		//
		//		[DebuggerStepThrough]
		//		public static bool Exists<T>(this IQueryable<T> query, Func<T, bool> match)
		//		{
		//			if (query == null || match == null) return false;
		//
		//			return query.Any(match);
		//		}

		#endregion



		//---g



		#region ICollection

		[DebuggerStepThrough]
		public static void AddRange<T>(this ICollection<T> me, IEnumerable<T> items)
		{
			if (me == null || items == null) return;

			foreach (var item in items)
			{
				me.Add(item);
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<T> Take<T>(this ICollection<T> me, int count)
		{
			if (me == null) return null;

			if (count < 0)
				count = me.Count + count;

			if (me.Count == count)
				return me;

			return ((IEnumerable<T>)me).Take(count);
		}

		#endregion



		//---g



		#region IList

		public static IEnumerable<T> Range<T>(this IList<T> me, int start, int end)
		{
			if (me == null || start == 0 && end == me.Count - 1) return me;
			return ListIterator(me, start, end);
		}

		[DebuggerStepThrough]
		public static IEnumerable<T> Left<T>(this IList<T> me, int count)
		{
			if (me == null || me.Count == count) return me;
			return ListIterator(me, 0, count - 1);
		}


		[DebuggerStepThrough]
		public static IEnumerable<T> Right<T>(this IList<T> me, int count)
		{
			if (me == null || me.Count == count) return me;
			return ListIterator(me, me.Count - count, me.Count - 1);
		}


		private static IEnumerable<T> ListIterator<T>(IList<T> list, int start, int end)
		{
			if (start < 0) start = 0;
			var count = list.Count;
			if (end >= count) end = count - 1;

			for (var i = start; i <= end; i++)
				yield return list[i];
		}

		#endregion



		//---g



		#region IQueriable

		public static List<TEntity> ToList<TEntity>(this IQueryable<TEntity> query)
		{
			return query == null ? null : new List<TEntity>(query);
		}

		public static IList ToList(this IQueryable query)
		{
			return query == null ? null :
				(IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(query.ElementType), query);
		}

		public static IQueryable SelectExpr(this IQueryable query, LambdaExpression selectExpr)
		{
			return query.Provider.CreateQuery(Expression.Call(
				typeof(Queryable),
				"Select",
				new[] { query.ElementType, selectExpr.ReturnType },
				query.Expression,
				Expression.Quote(selectExpr)
			));
		}

		//public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TEntity>> where)
		//{
		//	if (query == null) return null;
		//	if (where == null) return query;

		//	return where(query);
		//}

		#endregion



		//---g



		#region GetMembers

		public static PropertyInfo GetProperty<TEntity, TValue>(
			this Expression<Func<TEntity, TValue>> getProperties
			)
		{
			return GetMembers(getProperties).OfType<PropertyInfo>().FirstOrDefault();
		}

		public static IEnumerable<PropertyInfo> GetProperties<TEntity, TValue>(
			this Expression<Func<TEntity, TValue>> getProperties
			)
		{
			return GetMembers(getProperties).OfType<PropertyInfo>();
		}

		public static IEnumerable<MemberInfo> GetMembers<TEntity, TValue>(
			this Expression<Func<TEntity, TValue>> getMembers
			)
		{
			var memberExpr = getMembers.Body as MemberExpression;
			if (memberExpr != null)
			{
				return new[] { memberExpr.Member };
			}

			var newExpr = getMembers.Body as NewExpression;
			if (newExpr != null)
			{
				return newExpr.Arguments.OfType<MemberExpression>().Select(a => a.Member);
			}

			var unaryExpr = getMembers.Body as UnaryExpression;
			if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
			{
				memberExpr = unaryExpr.Operand as MemberExpression;
				if (memberExpr != null)
				{
					return new[] { memberExpr.Member };
				}
			}

			throw new NotImplementedException();
		}

		#endregion



		//---g



		#region TextSearch

		public static IEnumerable<TEntity> TextSearch<TEntity>(
			this IEnumerable<TEntity> query,
			string searchText,
			params MemberInfo[] members)
		{
			var exp = GetTextSearchExpression<TEntity>(searchText, members);
			return exp == null ? query : query.AsQueryable().Where(exp);
		}

		public static IQueryable<TEntity> TextSearch<TEntity>(
			this IQueryable<TEntity> query,
			string searchText,
			params MemberInfo[] members)
		{
			var exp = GetTextSearchExpression<TEntity>(searchText, members);
			return exp == null ? query : query.Where(exp);
		}


		private static Expression<Func<TEntity, bool>> GetTextSearchExpression<TEntity>(
			string searchText,
			MemberInfo[] members)
		{
			if (searchText.No()) return null;

			if (members.No())
				members = typeof(TEntity).GetProperties();

			Expression exp = null;
			var param = Expression.Parameter(typeof(TEntity), "a");

			foreach (var member in members.Reverse())
			{
				if (member.ResultType() != typeof(string)) continue;

				var prop = Expression.PropertyOrField(param, member.Name);

				Expression eq = Expression.Call(prop, _stringContainsMethod, Expression.Constant(searchText));

				exp = exp == null ? eq : Expression.OrElse(eq, exp);
			}

			return exp == null ? null : Expression.Lambda<Func<TEntity, bool>>(exp, param);
		}


		static readonly MethodInfo _stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

		#endregion



		//---g



		#region OrderBy

		internal static IOrderedQueryable<TEntity> OrderBy_<TEntity, TKey>(
			IQueryable<TEntity> query,
			Expression<Func<TEntity, TKey>> orderSelector,
			bool descending = false,
			bool useOrderThen = false
		)
		{
			var orderedQuery = query as IOrderedQueryable<TEntity>;

			return useOrderThen && orderedQuery != null
				? descending
					? orderedQuery.ThenByDescending(orderSelector)
					: orderedQuery.ThenBy(orderSelector)
				: descending
					? query.OrderByDescending(orderSelector)
					: query.OrderBy(orderSelector);
		}

		private static readonly MethodInfo orderByGenericMethod =
			typeof(LinqExtensions).GetMethod("OrderBy_", BindingFlags.NonPublic | BindingFlags.Static);


		internal static IOrderedQueryable<TEntity> InvokeOrderBy<TEntity>(
			IQueryable<TEntity> query,
			LambdaExpression orderSelector,
			bool descending = false,
			bool useOrderThen = false
		)
		{
			var method = orderByGenericMethod.MakeGenericMethod(typeof(TEntity), orderSelector.ReturnType);

			var orderedQuery = method.Invoke(null, new object[] { query, orderSelector, descending, useOrderThen });

			return (IOrderedQueryable<TEntity>)orderedQuery;
		}


		public static LambdaExpression ToExpression(Type entityType, string[] propertyNames)
		{
			if (entityType == null) throw new ArgumentNullException("entityType");
			if (propertyNames.No()) throw new ArgumentNullException("propertyNames");

			var param = Expression.Parameter(entityType, "a");
			var type = entityType;
			Expression propExpr = param;

			foreach (var propName in propertyNames)
			{
				propExpr = Expression.PropertyOrField(propExpr, propName);
				var prop = type.PropertyOrField(propName);
				type = prop.ResultType();
			}

			return Expression.Lambda(propExpr, param);
		}

		public static LambdaExpression ToExpression(Type entityType, string propertyName)
		{
			if (propertyName == null) throw new ArgumentNullException("propertyName");

			var propertyNames = propertyName.Split(new[] { '.', '/' }, StringSplitOptions.RemoveEmptyEntries);

			return ToExpression(entityType, propertyNames);
		}

		public static LambdaExpression ToExpression(Type entityType, MemberInfo member)
		{
			if (entityType == null) throw new ArgumentNullException("entityType");
			if (member == null) throw new ArgumentNullException("member");

			var param = Expression.Parameter(entityType, "a");

			return Expression.Lambda(
				Expression.PropertyOrField(param, member.Name),
				param
			);
		}


		public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
			this IQueryable<TEntity> query,
			MemberInfo member,
			bool descending = false,
			bool useOrderThen = false
		)
		{
			return InvokeOrderBy(query, ToExpression(typeof(TEntity), member), descending, useOrderThen);
		}

		public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
			this IQueryable<TEntity> query,
			string members,
			bool descending = false,
			bool useOrderThen = false
		)
		{
			return InvokeOrderBy(query, ToExpression(typeof(TEntity), members), descending, useOrderThen);
		}

		public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
			this IQueryable<TEntity> query,
			string[] members,
			bool descending = false,
			bool useOrderThen = false
		)
		{
			return InvokeOrderBy(query, ToExpression(typeof(TEntity), members), descending, useOrderThen);
		}

		#endregion



		//---g



		#region Limit

		public static IQueryable<T> Limit<T>(this IQueryable<T> query, int skipCount, int takeCount)
		{
			if (!(query is IOrderedQueryable<T>))
				return query;

			if (skipCount > 0 && takeCount > 0)
				query = query.Skip(skipCount);

			if (takeCount > 0)
				query = query.Take(takeCount);

			return query;
		}

		public static IEnumerable<T> Limit<T>(this IEnumerable<T> query, int skipCount, int takeCount)
		{
			//if (!(query is IOrderedQueryable<T>))
			//	return query;

			if (skipCount > 0 && takeCount > 0)
				query = query.Skip(skipCount);

			if (takeCount > 0)
				query = query.Take(takeCount);

			return query;
		}

		#endregion



		//---g



		public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
		{
			if (y == null) return x;

			foreach (var pair in y)
			{
				x.Add(pair);
			}

			return x;
		}

		public static IDictionary<TKey, TValue> Concat<TKey, TValue>(this IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
		{
			var result = new Dictionary<TKey, TValue>();

			return result.AddRange(x).AddRange(y);
		}



		//---g

	}






	//===g



}


//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;


//namespace Luxena
//{

//	public static class LinqExtentions
//	{


//		public static IEnumerable<PropertyInfo> GetEntityProperties<TEntity, TValue>(
//			this Expression<Func<TEntity, TValue>> getProperties
//		)
//		{
//			return GetEntityMembers(getProperties).OfType<PropertyInfo>();
//		}

//		public static IEnumerable<MemberInfo> GetEntityMembers<TEntity, TValue>(
//			this Expression<Func<TEntity, TValue>> getMembers
//		)
//		{
//			var memberExpr = getMembers.Body as MemberExpression;
//			if (memberExpr != null)
//			{
//				return new[] { memberExpr.Member };
//			}

//			var newExpr = getMembers.Body as NewExpression;
//			if (newExpr != null)
//			{
//				return newExpr.Arguments.OfType<MemberExpression>().Select(a => a.Member);
//			}

//			var unaryExpr = getMembers.Body as UnaryExpression;
//			if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
//			{
//				memberExpr = unaryExpr.Operand as MemberExpression;
//				if (memberExpr != null)
//				{
//					return new[] { memberExpr.Member };
//				}
//			}

//			throw new NotImplementedException();
//		}


//		#region Enumerable

//		[DebuggerStepThrough]
//		public static void ForEach<T>(this IEnumerable<T> me, Action<T> method)
//		{
//			if (me == null) return;

//			foreach (var item in me)
//			{
//				method(item);
//			}
//		}

//		[DebuggerStepThrough]
//		public static bool Exists<T>(this IEnumerable<T> query, Func<T, bool> match)
//		{
//			if (query == null || match == null) return false;

//			// ReSharper disable once CompareNonConstrainedGenericWithNull
//			return query.FirstOrDefault(match) != null;
//		}

//		#endregion


//		#region IList

//		[DebuggerStepThrough]
//		public static void AddRange<T>(this IList<T> me, IEnumerable<T> items)
//		{
//			if (me == null || items == null) return;

//			foreach (var item in items)
//			{
//				me.Add(item);
//			}
//		}

//		#endregion


//		#region TextSearch

//		public static IEnumerable<TEntity> TextSearch<TEntity>(
//			this IEnumerable<TEntity> query,
//			string searchText,
//			params MemberInfo[] members)
//		{
//			var exp = GetTextSearchExpression<TEntity>(searchText, members);
//			return exp == null ? query : query.AsQueryable().Where(exp);
//		}

//		public static IQueryable<TEntity> TextSearch<TEntity>(
//			this IQueryable<TEntity> query,
//			string searchText,
//			params MemberInfo[] members)
//		{
//			var exp = GetTextSearchExpression<TEntity>(searchText, members);
//			return exp == null ? query : query.Where(exp);
//		}


//		private static Expression<Func<TEntity, bool>> GetTextSearchExpression<TEntity>(
//			string searchText,
//			MemberInfo[] members)
//		{
//			if (searchText.No()) return null;

//			if (members.No())
//				members = typeof(TEntity).GetProperties();

//			Expression exp = null;
//			var param = Expression.Parameter(typeof(TEntity), "a");

//			foreach (var member in members.Reverse())
//			{
//				if (member.ResultType() != typeof(string)) continue;

//				var prop = Expression.PropertyOrField(param, member.Name);

//				Expression eq = Expression.Call(prop, _stringContainsMethod, Expression.Constant(searchText));

//				exp = exp == null ? eq : Expression.OrElse(eq, exp);
//			}

//			return exp == null ? null : Expression.Lambda<Func<TEntity, bool>>(exp, param);
//		}

//		static readonly MethodInfo _stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

//		#endregion


//	}

//}
