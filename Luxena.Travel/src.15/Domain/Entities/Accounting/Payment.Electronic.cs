using System.Data.Entity;

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
		public string AuthorizationCode { get; set; }

	}


	partial class Domain
	{
		public DbSet<ElectronicPayment> ElectronicPayments { get; set; }
	}

}