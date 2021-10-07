using System.Linq;
using System.Web.OData;
using System.Web.OData.Query;


namespace Luxena.Domain.Web
{

	public class EntityQueryAttribute : EnableQueryAttribute
	{
		public EntityQueryAttribute()
		{
			MaxNodeCount = 200;
		}


		//public override void ValidateQuery(HttpRequestMessage request, ODataQueryOptions queryOptions)
		//{
		//	//base.ValidateQuery(request, queryOptions);
		//}

		public override IQueryable ApplyQuery(IQueryable query, ODataQueryOptions queryOptions)
		{
			var idFilter = queryOptions.GetIdFilterValue();

			if (idFilter == "single")
				queryOptions.ClearFilter();

			if (queryOptions.SelectExpand != null && !queryOptions.Request.RequestUri.AbsoluteUri.As(a => a.Contains("usecalculated=true") || a.Contains(".usecalculated")))
			{
				query = base.ApplyQuery(query, queryOptions);
				return query;
			}


			query = query.Where(queryOptions);

			var list = query.ToList();

			if (idFilter.Yes())
			{
				list.One().Do((ICalculateContainer r) =>
				{
					r.CalculateOnLoad();
					//r.IsSaving(false);
					//r.Calculate();
					//r.IsSaving(null);
				});
			}

			query = list.AsQueryable().Select(queryOptions);

			return query;
		}
		
	}

}