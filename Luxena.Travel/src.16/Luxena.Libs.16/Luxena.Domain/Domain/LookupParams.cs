using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Luxena.Domain
{

	public class LookupParams<TEntity, TLookup> : IQueryable<TEntity>
	{
		public IQueryable<TEntity> Query;
		public Func<IQueryable<TEntity>, IEnumerable<TLookup>> Select;

		public string Filter;
		public int SkipCount;
		public int TakeCount;
		public int? Count;

		
		#region IQueryable<TEntity>

		public IEnumerator<TEntity> GetEnumerator() => Query.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public Expression Expression => Query.Expression;

		public Type ElementType => Query.ElementType;

		public IQueryProvider Provider => Query.Provider;

		#endregion


		public List<TLookup> GetList(IQueryable<TEntity> query = null)
		{
			query = query ?? Query;

			Count = query.Count();

			var enumerable = query
				.As(Select)
				.Limit(SkipCount, TakeCount);

			return enumerable.ToList();
		}
	}

}
