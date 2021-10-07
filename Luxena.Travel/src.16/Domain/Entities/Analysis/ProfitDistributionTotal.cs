using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public partial class ProfitDistributionTotal
	{

		public string Id { get; set; }

		[Patterns.Rank, EntityPosition]
		public int Rank { get; set; }

		[RU("Кол-во продаж")]
		public int SellCount { get; set; }

		[RU("Кол-во возвратов")]
		public int RefundCount { get; set; }

		[RU("Кол-во ануляций")]
		public int VoidCount { get; set; }

		[Patterns.Currency]
		public string Currency { get; set; }

		[RU("Продано"), Float(2)]
		public decimal? SellGrandTotal { get; set; }

		[RU("Возврат"), Float(2)]
		public decimal? RefundGrandTotal { get; set; }

		[Float(2)]
		public decimal? GrandTotal { get; set; }

		[Float(2)]
		public decimal? Total { get; set; }

		[Float(2)]
		public decimal? ServiceFee { get; set; }

		[Float(2)]
		public decimal? Commission { get; set; }

		[RU("Итого по агенту"), Float(2)]
		public decimal? AgentTotal { get; set; }

		[Float(2)]
		public decimal? Vat { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup sm)
		{
			sm.Patterns((Product a) => new ProfitDistributionTotal
			{
				GrandTotal = a.GrandTotal,
				Total = a.Total,
				ServiceFee = a.ServiceFee,
				Commission = a.Commission,
				Vat = a.Vat,
			});
		}

	}

}