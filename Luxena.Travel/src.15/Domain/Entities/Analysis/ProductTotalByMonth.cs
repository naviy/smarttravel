using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого помесячно", ruShort: "Помесячно")]
	public partial class ProductTotalByMonth : ProductTotal
	{
		[Patterns.Month, MonthAndYear]
		public DateTimeOffset IssueDate { get; set; }

		public int Year, Month;
	}


	public partial class ProductTotalByMonthParams : ProductFilter { }


	public class ProductTotalByMonthQuery : Domain.DbQuery<ProductTotalByMonthParams, ProductTotalByMonth>
	{
		public override IEnumerable<ProductTotalByMonth> Get()
		{
			//			db.BeginLog();

			var products =
				from a in Params.Get(db.Products)
				let k = a.IsRefund ? -1 : 1
				select new
				{
					a.IssueDate,
					Total = k * a.Total.Amount,
					ServiceFee = k * a.ServiceFee.Amount,
					GrandTotal = k * a.GrandTotal.Amount,
				};

			var list = (
				from p in products
				where p.GrandTotal != 0
				group p by new { p.IssueDate.Year, p.IssueDate.Month } into g
				orderby g.Key
				select new ProductTotalByMonth
				{
					Id = g.Key.Year + "-" + g.Key.Month,
					Year = g.Key.Year,
					Month = g.Key.Month,
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				}
			).ToList();

			list.ForEach(a =>
			{
				a.Id = a.Year + "-" + a.Month;
				a.IssueDate = new DateTime(a.Year, a.Month, 1, 0, 0, 0);
			});

			Count = list.Count;

			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalByMonthQuery ProductTotalByMonths { get; set; }
	}

}