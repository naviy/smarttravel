//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//
//
//namespace Luxena.Domain
//{
//
//	public class RangeRequest
//	{
//
//		public int? PageIndex { get; set; }
//		public int? PageSize { get; set; }
//		public int? Skip { get; set; }
//
//		public SortInfo[] Sorts { get; set; }
//
//		public string SearchText { get; set; }
//
//		/// <summary>
//		/// По каким полям проводить текстовый поиск
//		/// </summary>
//		public MemberInfo[] TextSearchMembers { get; set; }
//
//
//		public class SortInfo
//		{
//			public string Name { get; set; }
//			public bool Descending { get; set; }
//		}
//
//
//		public IQueryable<TEntity> Query<TEntity>(IQueryable<TEntity> query)
//		{
//			query = FilterQuery(query);
//
//			var useOrderThen = false;
//			foreach (var sort in Sorts)
//			{
//				query = query.Sort(sort.Name, sort.Descending, useOrderThen);
//				useOrderThen = true;
//			}
//
//			if (PageSize != null && PageIndex != null)
//				query = query.Skip(PageIndex.Value * PageSize.Value);
//			else if (Skip != null)
//				query = query.Skip(Skip.Value);
//
//			if (PageSize != null)
//				query = query.Take(PageSize.Value);
//
//
//			return query;
//		}
//
//		public IQueryable<TEntity> FilterQuery<TEntity>(IQueryable<TEntity> query)
//		{
//			if (SearchText.Yes())
//				query = query.TextSearch(SearchText, TextSearchMembers);
//			return query;
//		}
//
//	}
//
//
//	public static class RangeRequestExtentions
//	{
//
//		public static IQueryable<TEntity> Range<TEntity>(this IQueryable<TEntity> query, RangeRequest request)
//		{
//			return request == null ? query : request.Query(query);
//		}
//
//		public static IQueryable<TEntity> Range<TEntity>(this IEnumerable<TEntity> query, RangeRequest request)
//		{
//			return request == null ? query.AsQueryable() : request.Query(query.AsQueryable());
//		}
//
//
//		public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> query, RangeRequest request)
//		{
//			return request == null ? query : request.FilterQuery(query);
//		}
//
//		public static IQueryable<TEntity> Filter<TEntity>(this IEnumerable<TEntity> query, RangeRequest request)
//		{
//			return request == null ? query.AsQueryable() : request.FilterQuery(query.AsQueryable());
//		}
//
//
//		public static RangeRequest TextSearchMembers<TEntity>(this RangeRequest request, Expression<Func<TEntity, object>> memberSource)
//		{
//			if (memberSource == null) return request;
//
//			var newExpr = (NewExpression)memberSource.Body;
//
//			request.TextSearchMembers = newExpr.Arguments
//				.OfType<MemberExpression>()
//				.Select(a => a.Member)
//				.ToArray();
//
//			return request;
//		}
//
//	}
//
//}
