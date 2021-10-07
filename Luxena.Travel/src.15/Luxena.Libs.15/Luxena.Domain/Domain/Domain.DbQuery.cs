using System.Collections.Generic;
using System.Linq;


namespace Luxena.Domain
{

	partial class Domain<TDomain>
	{

		public abstract class DbQuery<TParams, TResult>
			where TParams : class
			where TResult : class
		{

			public abstract IEnumerable<TResult> Get();


			public TDomain db;
			public TParams Params;
			public int SkipCount, TakeCount;
			public int Count = -1;
			public Dictionary<string[], bool> OrderByColumns;


			public IQueryable<T> OrderBy<T>(IQueryable<T> query)
			{
				if (OrderByColumns.No())
					return query;

				var useThen = false;
				foreach (var col in OrderByColumns)
				{
					query = query.OrderBy(col.Key, col.Value, useThen);
					useThen = true;
				}
				
				return query;
			}

			public List<T> OrderBy<T>(List<T> list)
			{
				return OrderByColumns.No() ? list : OrderBy(list.AsQueryable()).ToList();
			}


			public IQueryable<T> Limit<T>(IQueryable<T> query)
			{
				return query.Limit(SkipCount, TakeCount);
			}

			public IEnumerable<T> Limit<T>(IEnumerable<T> query)
			{
				return query.Limit(SkipCount, TakeCount);
			}

		}
	
	}

}
