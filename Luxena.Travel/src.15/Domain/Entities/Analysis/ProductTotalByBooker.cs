using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого по бронировщику", ruShort: "По бронировщику")]
	public partial class ProductTotalByBooker : ProductTotal
	{
		[Patterns.Rank]
		public int Rank { get; set; }

		public string BookerName { get; set; }

		public PersonReference Booker { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByBooker> sm)
		{
			sm.For(a => a.Booker)
				.Pattern((Product a) => a.Booker);
		}
	}


	public partial class ProductTotalByBookerParams : ProductFilter { }


	public class ProductTotalByBookerQuery : Domain.DbQuery<ProductTotalByBookerParams, ProductTotalByBooker>
	{
		public override IEnumerable<ProductTotalByBooker> Get()
		{
			var products =
				from a in Params.Get(db.Products)
				let k = a.IsRefund ? -1 : 1
				select new
				{
					GroupKey = a.BookerId,
					Total = k * a.Total.Amount,
					ServiceFee = k * a.ServiceFee.Amount,
					GrandTotal = k * a.GrandTotal.Amount,
				};

			var products2 =
				from p in products
				where p.GrandTotal != 0
				group p by p.GroupKey into g
				select new ProductTotalByBooker
				{
					Id = g.Key ?? "",
					Booker = new PersonReference { Id = g.Key ?? "" },
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				};

			var list = products2.OrderByDescending(a => a.GrandTotal).ToList();

			list.ForEach((p, i) =>
			{
				p.Rank = i + 1;
				p.Booker += db;
				p.BookerName = p.Rank + ". " + p.Booker.Name;
			});

			Count = list.Count;

			return list;
		}
	}

	partial class Domain
	{
		public ProductTotalByBookerQuery ProductTotalByBookers { get; set; }
	}

}