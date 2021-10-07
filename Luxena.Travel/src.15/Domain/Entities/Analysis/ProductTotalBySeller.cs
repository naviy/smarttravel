using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого по продавцу", ruShort: "По продавцу")]
	public partial class ProductTotalBySeller : ProductTotal
	{
		[Patterns.Rank]
		public int Rank { get; set; }

		public string SellerName { get; set; }

		public PersonReference Seller { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalBySeller> sm)
		{
			sm.For(a => a.Seller)
				.Pattern((Product a) => a.Seller);
		}
	}


	public partial class ProductTotalBySellerParams : ProductFilter { }


	public class ProductTotalBySellerQuery : Domain.DbQuery<ProductTotalBySellerParams, ProductTotalBySeller>
	{
		public override IEnumerable<ProductTotalBySeller> Get()
		{
			var products =
				from a in Params.Get(db.Products)
				let k = a.IsRefund ? -1 : 1
				select new
				{
					GroupKey = a.SellerId,
					Total = k * a.Total.Amount,
					ServiceFee = k * a.ServiceFee.Amount,
					GrandTotal = k * a.GrandTotal.Amount,
				};

			var products2 =
				from p in products
				where p.GrandTotal != 0
				group p by p.GroupKey into g
				select new ProductTotalBySeller
				{
					Id = g.Key ?? "",
					Seller = new PersonReference { Id = g.Key ?? "" },
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				};

			var list = products2.OrderByDescending(a => a.GrandTotal).ToList();

			list.ForEach((p, i) =>
			{
				p.Rank = i + 1;
				p.Seller += db;
				p.SellerName = p.Rank + ". " + p.Seller.Name;
			});

			Count = list.Count;

			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalBySellerQuery ProductTotalBySellers { get; set; }
	}

}