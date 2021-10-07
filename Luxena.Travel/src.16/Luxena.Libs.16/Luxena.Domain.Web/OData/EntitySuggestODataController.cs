using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;


namespace Luxena.Domain.Web
{

	public abstract class EntityLookupODataController<TDomain, TEntity, TKey, TLookup> : DomainODataController<TDomain>
		where TDomain : Domain<TDomain>, new()
		where TEntity : Domain<TDomain>.Entity<TKey>
		where TLookup : class, INameContainer, new()
	{

		protected IQueryable<TEntity> Query;
		protected Func<IQueryable<TEntity>, IEnumerable<TLookup>> Select;
		protected Func<LookupParams<TEntity, TLookup>, IEnumerable<TLookup>> Lookup;


		public PageResult<TLookup> Get(ODataQueryOptions<TLookup> options)
		{
			var p = new LookupParams<TEntity, TLookup>
			{
				Query = Query,
				Select = Select,
				Filter = options.GetFilterParams<TLookup>().GetName(),
				SkipCount = options.Skip.As(a => a.Value),
				TakeCount = options.Top.As(a => a.Value),
			};

			if (p.TakeCount == 0) p.TakeCount = 50;

			var list = Lookup != null ? Lookup(p) : p.GetList();

			return new PageResult<TLookup>(list, null, p.Count);
		}

		[EnableQuery]
		public IHttpActionResult Get([FromODataUri] TKey key)
		{
			var list = Select(db.Set<TEntity>().WhereIdEquals<TDomain, TEntity, TKey>(key));

			return Ok(list.One());
		}
	}

}