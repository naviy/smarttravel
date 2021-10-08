using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Распределение выручки по заказчикам")]
	public partial class ProfitDistributionByCustomer : ProfitDistributionTotal
	{
		[EntityName]
		public PartyReference Customer { get; set; }

		[SemanticSetup]
		public new static void SemanticSetup(SemanticSetup sm)
		{
			sm.Patterns((Product a) => new ProfitDistributionByCustomer
			{
				Customer = a.Customer,
			});
		}
	}


	public partial class ProfitDistributionByCustomerParams : ProductFilter { }


	public class ProfitDistributionByCustomerQuery : Domain.DbQuery<ProfitDistributionByCustomerParams, ProfitDistributionByCustomer>
	{
		public override IEnumerable<ProfitDistributionByCustomer> Get()
		{
			Params.AllowVoided = true;

			var query =
				from p in Params.Get(db.Products)
				group p by new
				{
					p.CustomerId,
					Currency = p.EqualFare.CurrencyId ?? "UAH",
				} into g
				select new ProfitDistributionByCustomer
				{
					Id = (g.Key.CustomerId ?? "") + "_" + (g.Key.Currency ?? ""),
					Customer = new PartyReference { Id = g.Key.CustomerId },

					Currency = g.Key.Currency,

					SellCount = g.Sum(a => !a.IsVoid && !a.IsRefund ? 1 : 0),
					RefundCount = g.Sum(a => !a.IsVoid && a.IsRefund ? 1 : 0),
					VoidCount = g.Sum(a => a.IsVoid ? 1 : 0),

					SellGrandTotal = g.Sum(a => a.IsVoid ? 0 : a.IsRefund ? 0 : a.GrandTotal.Amount),
					RefundGrandTotal = g.Sum(a => a.IsVoid ? 0 : a.IsRefund ? a.GrandTotal.Amount : 0),
					Total = g.Sum(a => a.IsVoid ? 0 : (a.IsRefund ? -1 : 1) * a.Total.Amount),
					ServiceFee = g.Sum(a => a.IsVoid ? 0 : (a.IsRefund ? -1 : 1) * a.ServiceFee.Amount),
					Commission = g.Sum(a => a.IsVoid ? 0 : (a.IsRefund ? -1 : 1) * ((a.Commission.Amount ?? 0) - (a.CommissionDiscount.Amount ?? 0))),
					Vat = g.Sum(a => a.IsVoid ? 0 : (a.IsRefund ? -1 : 1) * a.Vat.Amount),
				};

			var list = query.ToList();

			Count = list.Count;

			list.ForEach(a =>
			{
				a.Customer += db;
				a.GrandTotal = a.SellGrandTotal - a.RefundGrandTotal;
				a.AgentTotal = a.ServiceFee + a.Commission;
			});

			list = list.OrderByDescending(a => a.GrandTotal).ToList();
			list.ForEach((a, i) => a.Rank = i + 1 + SkipCount);
			list = OrderBy(list);

			return list;
		}
	}


	partial class Domain
	{
		public ProfitDistributionByCustomerQuery ProfitDistributionByCustomers { get; set; }
	}

}
