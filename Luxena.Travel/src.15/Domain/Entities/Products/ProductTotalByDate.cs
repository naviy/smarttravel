using System;
using System.Linq;
using System.Linq.Expressions;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public class ProductTotalByDate
	{
		[RU("Дата услуги")]
		public DateTimeOffset? Date { get; set; }

		[RU("Итого к оплате")]
		public decimal? GrandTotal { get; set; }

		[RU("Всего за период к оплате")]
		public decimal? SumGrandTotal { get; set; }


		public static ProductTotalByDate[] Get(Domain db, Expression<Func<Product, bool>> masterExpr)
		{
			var query1 = db.Products.Where(masterExpr);

			var list = (
				from a in query1
				where !a.IsVoid
				group a by a.IssueDate into g
				let grandTotal = g.Sum(a => a.GrandTotal.Amount)
				where grandTotal != null && grandTotal != 0
				orderby g.Key
				select new ProductTotalByDate
				{
					Date = g.Key,
					GrandTotal = grandTotal,
				}
				).ToArray();

			list.Aggregate((decimal?)0, (sum, a) => a.SumGrandTotal = sum + (a.GrandTotal ?? 0));

			return list;
		}

	}


	partial class Domain
	{

		[EntityAction, RU("Динамика поставленных услуг")]
		public ProductTotalByDate[] ProductByProvider_TotalByIssueDate(string ProviderId)
		{
			return ProductTotalByDate.Get(db, a => a.ProviderId == ProviderId || a.ProducerId == ProviderId);
		}

		[EntityAction, RU("Динамика проданных услуг")]
		public ProductTotalByDate[] ProductBySeller_TotalByIssueDate(string SellerId)
		{
			return ProductTotalByDate.Get(db, a => a.SellerId == SellerId);
		}

		[EntityAction, RU("Динамика забронированных услуг")]
		public ProductTotalByDate[] ProductByBooker_TotalByIssueDate(string BookerId)
		{
			return ProductTotalByDate.Get(db, a => a.BookerId == BookerId);
		}

		[EntityAction, RU("Динамика тикетированных услуг")]
		public ProductTotalByDate[] ProductByTicketer_TotalByIssueDate(string TicketerId)
		{
			return ProductTotalByDate.Get(db, a => a.TicketerId == TicketerId);
		}

	}

}