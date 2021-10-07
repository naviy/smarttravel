using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого по провайдеру", ruShort: "По провайдеру")]
	public partial class ProductTotalByProvider : ProductTotal
	{
		[Patterns.Rank]
		public int Rank { get; set; }

		public string ProviderName { get; set; }

		public OrganizationReference Provider { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByProvider> sm)
		{
			sm.For(a => a.Provider)
				.Pattern((Product a) => a.Provider);
		}
	}


	public partial class ProductTotalByProviderParams : ProductFilter { }


	public class ProductTotalByProviderQuery : Domain.DbQuery<ProductTotalByProviderParams, ProductTotalByProvider>
	{
		public override IEnumerable<ProductTotalByProvider> Get()
		{
			var products =
				from a in Params.Get(db.Products)
				let k = a.IsRefund ? -1 : 1
				select new
				{
					GroupKey = a.ProviderId ?? a.ProducerId,
					Total = k * a.Total.Amount,
					ServiceFee = k * a.ServiceFee.Amount,
					GrandTotal = k * a.GrandTotal.Amount,
				};

			var products2 =
				from p in products
				where p.GrandTotal != 0
				group p by p.GroupKey into g
				select new ProductTotalByProvider
				{
					Id = g.Key ?? "",
					Provider = new OrganizationReference { Id = g.Key ?? "" },
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				};

			var list = products2.OrderByDescending(a => a.GrandTotal).ToList();

			list.ForEach((p, i) =>
			{
				p.Rank = i + 1;
				p.Provider += db;
				p.ProviderName = p.Rank + ". " + p.Provider.Name;
			});

			Count = list.Count;

			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalByProviderQuery ProductTotalByProviders { get; set; }
	}

}