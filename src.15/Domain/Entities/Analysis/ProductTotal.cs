using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public partial class ProductTotal
	{
		public string Id { get; set; }

		[Float(2)]
		public decimal? Total { get; set; }

		[Float(2)]
		public decimal? ServiceFee { get; set; }

		[Float(2)]
		public decimal? GrandTotal { get; set; }

		[Patterns.Note]
		public string Note { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup sm)
		{
			sm.Patterns((Product a) => new ProductTotal
			{
				Total = a.Total,
				ServiceFee = a.ServiceFee,
				GrandTotal = a.GrandTotal,
			});
		}
	}

}