using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого по годам", ruShort: "По годам")]
	public partial class ProductTotalByYear : ProductTotal
	{
		[Key, Patterns.Year]
		public int Year { get; set; }
	}


	public partial class ProductTotalByYearParams : ProductFilter { }

	public class ProductTotalByYearQuery : Domain.DbQuery<ProductTotalByYearParams, ProductTotalByYear>
	{
		public override IEnumerable<ProductTotalByYear> Get()
		{
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
				group p by p.IssueDate.Year into g
				orderby g.Key
				select new ProductTotalByYear
				{
					Id = g.Key + "",
					Year = g.Key,
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				}
			).ToList();

			Count = list.Count;

			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalByYearQuery ProductTotalByYears { get; set; }
	}

}