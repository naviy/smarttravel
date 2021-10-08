using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("ПКО")]
	public partial class CashInOrderPayment : Payment
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<CashInOrderPayment> se)
		{
			se.For(a => a.DocumentNumber)
				.RU("№ ПКО");
		}


		public override PaymentForm PaymentForm => PaymentForm.CashInOrder;

		public override string DocumentUniqueCode => 
			IsVoid || DocumentNumber.No() ? null : $"{DocumentNumber}_{Date:yyyy}";

	}


	partial class Domain
	{
		public DbSet<CashInOrderPayment> CashInOrderPayments { get; set; }
	}

}