using System;
using System.Linq;
using System.Linq.Expressions;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public class OrderTotalByDate
	{

		[RU("Дата заказа")]
		public DateTimeOffset? Date { get; set; }

		[RU("Итого к оплате")]
		public decimal? Total { get; set; }

		[RU("Всего за период к оплате")]
		public decimal? SumTotal { get; set; }


		public static OrderTotalByDate[] Get(Domain db, Expression<Func<Order, bool>> masterExpr)
		{
			var query1 = db.Orders.Where(masterExpr);

			var list = (
				from a in query1
				where !a.IsVoid
				group a by a.IssueDate into g
				let total = g.Sum(a => a.Total.Amount)
				where total != null && total != 0
				orderby g.Key
				select new OrderTotalByDate
				{
					Date = g.Key,
					Total = total,
				}
				).ToArray();

			list.Aggregate((decimal?)0, (sum, a) => a.SumTotal = sum + (a.Total ?? 0));

			return list;
		}

	}


	partial class Domain
	{

		[EntityAction, RU("Динамика заказов заказчика")]
		public OrderTotalByDate[] OrderByCustomer_TotalByIssueDate(string CustomerId)
		{
			return OrderTotalByDate.Get(db, a => a.CustomerId == CustomerId);
		}

		[EntityAction, RU("Динамика заказов владельца")]
		public OrderTotalByDate[] OrderByOwner_TotalByIssueDate(string OwnerId)
		{
			return OrderTotalByDate.Get(db, a => a.OwnerId == OwnerId);
		}

		[EntityAction, RU("Динамика заказов ответственного")]
		public OrderTotalByDate[] OrderByAssignedTo_TotalByIssueDate(string AssignedToId)
		{
			return OrderTotalByDate.Get(db, a => a.AssignedToId == AssignedToId);
		}

	}

}