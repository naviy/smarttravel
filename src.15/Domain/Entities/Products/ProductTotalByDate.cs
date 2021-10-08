using System;
using System.Linq;
using System.Linq.Expressions;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public class ProductTotalByDate
	{
		[RU("���� ������")]
		public DateTimeOffset? Date { get; set; }

		[RU("����� � ������")]
		public decimal? GrandTotal { get; set; }

		[RU("����� �� ������ � ������")]
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

		[EntityAction, RU("�������� ������������ �����")]
		public ProductTotalByDate[] ProductByProvider_TotalByIssueDate(string ProviderId)
		{
			return ProductTotalByDate.Get(db, a => a.ProviderId == ProviderId || a.ProducerId == ProviderId);
		}

		[EntityAction, RU("�������� ��������� �����")]
		public ProductTotalByDate[] ProductBySeller_TotalByIssueDate(string SellerId)
		{
			return ProductTotalByDate.Get(db, a => a.SellerId == SellerId);
		}

		[EntityAction, RU("�������� ��������������� �����")]
		public ProductTotalByDate[] ProductByBooker_TotalByIssueDate(string BookerId)
		{
			return ProductTotalByDate.Get(db, a => a.BookerId == BookerId);
		}

		[EntityAction, RU("�������� �������������� �����")]
		public ProductTotalByDate[] ProductByTicketer_TotalByIssueDate(string TicketerId)
		{
			return ProductTotalByDate.Get(db, a => a.TicketerId == TicketerId);
		}

	}

}