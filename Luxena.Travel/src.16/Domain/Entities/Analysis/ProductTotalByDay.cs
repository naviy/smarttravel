using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого посуточно", ruShort: "Посуточно")]
	public partial class ProductTotalByDay : ProductTotal
	{
		[Key, Patterns.Date]
		public DateTimeOffset IssueDate { get; set; }
	}


	public partial class ProductTotalByDayParams : ProductFilter { }


	public class ProductTotalByDayQuery : Domain.DbQuery<ProductTotalByDayParams, ProductTotalByDay>
	{
		public override IEnumerable<ProductTotalByDay> Get()
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

			var query =
				from p in products
				where p.GrandTotal != 0
				group p by p.IssueDate
					into g
				orderby g.Key
				select g;

			Count = query.Count();

			var list = query
				.As(Limit)
				.Select(g => new ProductTotalByDay
				{
					Id = g.Key + "",
					IssueDate = g.Key,
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				})
				.ToList();


			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalByDayQuery ProductTotalByDays { get; set; }
	}

}