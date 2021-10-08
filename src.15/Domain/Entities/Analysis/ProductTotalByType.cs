using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуги итого по видам услуг", ruShort: "По видам услуг")]
	public partial class ProductTotalByType : ProductTotal
	{
		[Patterns.Rank]
		public int Rank { get; set; }

		public ProductType Type { get; set; }

		public string TypeName { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByType> sm)
		{
			sm.For(a => a.Type)
				.Length(20)
				.Pattern((Product a) => a.Type);
		}
	}


	public partial class ProductTotalByTypeParams : ProductFilter { }


	public class ProductTotalByTypeQuery : Domain.DbQuery<ProductTotalByTypeParams, ProductTotalByType>
	{
		public override IEnumerable<ProductTotalByType> Get()
		{
			//db.BeginLog();

			var lng = new DefaultLocalizationTypesSource();

			var products =
				from a in Params.Get(db.Products)
				let k = a.IsRefund ? -1 : 1
				select new
				{
					GroupKey = a.Type,
					Total = k * a.Total.Amount,
					ServiceFee = k * a.ServiceFee.Amount,
					GrandTotal = k * a.GrandTotal.Amount,
				};

			var products2 =
				from p in products
				where p.GrandTotal != 0
				group p by p.GroupKey into g
				select new ProductTotalByType
				{
					Id = g.Key + "",
					Type = g.Key,
					Total = g.Sum(a => a.Total),
					ServiceFee = g.Sum(a => a.ServiceFee),
					GrandTotal = g.Sum(a => a.GrandTotal),
				};

			var list = products2.OrderByDescending(a => a.GrandTotal).ToList();

			list.ForEach((p, i) =>
			{
				p.Rank = i + 1;
				p.TypeName = p.Rank + ". " + p.Type.EnumLocalization(lng).Default.Many;
			});

			Count = list.Count;

			return list;
		}
	}


	partial class Domain
	{
		public ProductTotalByTypeQuery ProductTotalByTypes { get; set; }
	}

}