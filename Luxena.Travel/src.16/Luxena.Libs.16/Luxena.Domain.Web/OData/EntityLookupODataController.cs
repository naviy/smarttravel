using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;


namespace Luxena.Domain.Web
{

	public abstract class EntityLookupODataController<TDomain, TEntity, TKey, TSuggest> : DomainODataController<TDomain>
		where TDomain : Domain<TDomain>, new()
		where TEntity : Domain<TDomain>.Entity<TKey>
		where TSuggest : class, INameContainer, new()
	{

		protected IQueryable<TEntity> Query;
		protected Func<IQueryable<TEntity>, IEnumerable<TSuggest>> Select;
		protected Func<SuggestParams<TEntity, TSuggest>, IEnumerable<TSuggest>> Suggest;


		public PageResult<TSuggest> Get(ODataQueryOptions<TSuggest> options)
		{
			var p = new SuggestParams<TEntity, TSuggest>
			{
				Query = Query,
				Select = Select,
				Filter = options.GetFilterParams<TSuggest>().GetName(),
				SkipCount = options.Skip.As(a => a.Value),
				TakeCount = options.Top.As(a => a.Value),
			};

			if (p.TakeCount == 0) p.TakeCount = 50;

			var list = Suggest != null ? Suggest(p) : p.GetList();

			return new PageResult<TSuggest>(list, null, p.Count);
		}

		[EnableQuery]
		public IHttpActionResult Get([FromODataUri] TKey key)
		{
			var list = Select(db.Set<TEntity>().WhereIdEquals<TDomain, TEntity, TKey>(key));

			return Ok(list.One());
		}
	}

}