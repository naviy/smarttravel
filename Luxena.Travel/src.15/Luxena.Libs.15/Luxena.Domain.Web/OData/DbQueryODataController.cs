using System;
using System.Linq;
using System.Web.OData;
using System.Web.OData.Query;

using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Core.UriParser.Semantic;


namespace Luxena.Domain.Web
{

	public abstract class DbQueryODataController<TDomain, TParams, TResult, TQuery> : DomainODataController<TDomain>
		where TDomain : Domain<TDomain>, new()
		where TParams : class, new()
		where TResult : class
		where TQuery : Domain<TDomain>.DbQuery<TParams, TResult>, new()
	{

		public TQuery Query { get { return _query ?? (_query = new TQuery { db = db }); } }
		private TQuery _query;

		public PageResult<TResult> Get(ODataQueryOptions<TParams> options)
		{
			if (options.Skip != null)
				Query.SkipCount = options.Skip.Value;

			if (options.Top != null)
				Query.TakeCount = options.Top.Value;

			Query.Params = options.GetFilterParams<TParams>();

			if (options.OrderBy != null)
			{
				
				Query.OrderByColumns = options.OrderBy.OrderByNodes.ToDictionary(
					a =>
					{
						var node = (OrderByPropertyNode)a;
						var name = node.Property.Name;
						var source = ((SingleValuePropertyAccessNode)node.OrderByClause.Expression).Source as SingleValuePropertyAccessNode;

						return source != null ? new[] { source.Property.Name, name } : new[] { name };
					},
					a => a.Direction == OrderByDirection.Descending
				);
			}

			var list = Query.Get();

			if (Query.Count < 0)
				throw new Exception("В методе " + typeof(TQuery).Name + ".Get() необходимо присвоить свойству Count общее количество записей");

			return new PageResult<TResult>(list, null, Query.Count);
		}




	}


	//	public class ProductReport2Controller : QueryODataController2<Domain, Product, ProductReport2, ProductReport2Service> { }

}
