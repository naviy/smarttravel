using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого по владельцу", ruShort: "По владельцу")]
	public partial class ProductTotalByOwner : ProductTotal
	{
		[Patterns.Rank]
		public int Rank { get; set; }

		public string OwnerName { get; set; }

		public PartyReference Owner { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByOwner> sm)
		{
			sm.For(a => a.Owner)
				.Pattern((Product a) => a.Owner);
		}
	}


	public partial class ProductTotalByOwnerParams : ProductFilter { }


	public class ProductTotalByOwnerQuery : Domain.DbQuery<ProductTotalByOwnerParams, ProductTotalByOwner>
	{
		public override IEnumerable<ProductTotalByOwner> Get()
		{
			var products =
				from a in Params.Get(db.Products)
				let k = a.IsRefund ? -1 : 1
				select new
				{
					GroupKey = a.OwnerId,
					Total = k * a.Total.Amount,
					ServiceFee = k * a.ServiceFee.Amount,
					GrandTotal = k * a.GrandTotal.Amount,
				};

			var products2 =
				from p in products
				where p.GrandTotal != 0
				group p by p.GroupKey into g
				select new ProductTotalByOwner
				{
					Id = g.Key ?? "",
					Owner = new PartyReference { Id = g.Key ?? "" },
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				};

			var list = products2.OrderByDescending(a => a.GrandTotal).ToList();

			list.ForEach((p, i) =>
			{
				p.Rank = i + 1;
				p.Owner += db;
				p.OwnerName = p.Rank + ". " + p.Owner.Name;
			});

			Count = list.Count;

			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalByOwnerQuery ProductTotalByOwners { get; set; }
	}

}