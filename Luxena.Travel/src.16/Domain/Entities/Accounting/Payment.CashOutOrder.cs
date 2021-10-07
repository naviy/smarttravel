using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("РКО")]
	public partial class CashOutOrderPayment : Payment
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<CashOutOrderPayment> se)
		{
			se.For(a => a.DocumentNumber)
				.RU("№ РКО");
		}


		public override PaymentForm PaymentForm => PaymentForm.CashOutOrder;

		public override string DocumentUniqueCode => 
			IsVoid || DocumentNumber.No() ? null : $"{DocumentNumber}_{Date:yyyy}";

	}


	partial class Domain
	{
		public DbSet<CashOutOrderPayment> CashOutOrderPayments { get; set; }
	}

}