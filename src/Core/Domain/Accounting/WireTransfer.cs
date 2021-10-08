using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Безналичный платеж")]
	public partial class WireTransfer : Payment
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<WireTransfer> se)
		{
			se.For(a => a.DocumentNumber)
				.RU("№ платежного поручения");

			se.For(a => a.Invoice)
				.RU("Счёт")
				.Suggest<Invoice>();
		}

		public override PaymentForm PaymentForm => PaymentForm.WireTransfer;


		public new class Service : Service<WireTransfer>
		{
		}

	}

}