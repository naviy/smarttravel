using System;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Metamodel;


namespace Luxena.Travel.Domain
{

	[RU("Быстрый поиск")]
	[GenericPrivilegesExt(List2 = UserRole.Agent)]
	public partial class GlobalSearchEntity : Entity
	{

		public partial class Service : EntityService<GlobalSearchEntity>
		{
			public override RangeResponse Suggest(RangeRequest request)
			{
				var filter = request.Query.Clip();
				if (filter.No())
					return new RangeResponse();

				filter = "'%" + filter + "%'";

				var list = Session
					.CreateSQLQuery(
$"select id, name, class from lt_product where name like {filter} union " +
$"select id, number_, 'order' from lt_order where number_ like {filter} union " +
$"select id, number_, class from lt_payment where number_ like {filter} or documentnumber like {filter} union " +
$"select id, number_, 'invoice' from lt_invoice where number_ like {filter} union " +
$"select id, name, class from lt_party where name like {filter} or legalname like {filter} " +
$"order by 2 " +
$"limit {request.Limit} offset {request.Start}"
					)
					.List<object[]>();

				return new RangeResponse(list
					.OrderBy(a => a[1])
					.Skip(request.Start)
					.Take(request.Limit)
					.ToArray()
				);
			}
		}

	}

}