using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Электронный платеж")]
	public partial class ElectronicPayment : Payment
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<ElectronicPayment> se)
		{
			se.For(a => a.DocumentNumber)
				.RU("№ транзакции");
		}

		public override PaymentForm PaymentForm => PaymentForm.Electronic;

		[RU("Код авторизации")]
		public virtual string AuthorizationCode { get; set; }


		public new class Service : Service<ElectronicPayment>
		{

		}

	}

}