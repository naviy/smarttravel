using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого поквартально", ruShort: "Поквартально")]
	public partial class ProductTotalByQuarter : ProductTotal
	{
		[Patterns.Quarter, QuarterAndYear]
		public DateTimeOffset IssueDate { get; set; }

		public int Year, Quarter;
	}


	public partial class ProductTotalByQuarterParams : ProductFilter { }


	public class ProductTotalByQuarterQuery : Domain.DbQuery<ProductTotalByQuarterParams, ProductTotalByQuarter>
	{
		public override IEnumerable<ProductTotalByQuarter> Get()
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
				group p by new { p.IssueDate.Year, Quarter = (p.IssueDate.Month - 1) / 3 } into g
				orderby g.Key.Year, g.Key.Quarter
				select new ProductTotalByQuarter
				{
					Id = g.Key.Year + "-" + g.Key.Quarter,
					Year = g.Key.Year,
					Quarter = g.Key.Quarter,
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				}
			).ToList();

			list.ForEach(a => a.IssueDate = new DateTime(a.Year, a.Quarter * 3 + 2, 15, 0, 0, 0));

			Count = list.Count;

			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalByQuarterQuery ProductTotalByQuarters { get; set; }
	}

}