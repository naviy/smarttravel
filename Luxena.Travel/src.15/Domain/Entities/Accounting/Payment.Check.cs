using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Кассовый чек")]
	public partial class CheckPayment : Payment
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<CheckPayment> se)
		{
			se.For(a => a.DocumentNumber)
				.RU("№ чека")
				.Add(new Patterns.Auto());
		}


		public override PaymentForm PaymentForm => PaymentForm.Check;

	}


	partial class Domain
	{
		public DbSet<CheckPayment> CheckPayments { get; set; }
	}

}