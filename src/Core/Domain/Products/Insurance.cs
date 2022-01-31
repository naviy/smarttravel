using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Страховка", "Страховки")]
	public partial class Insurance : Product
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<Insurance> se)
		{

			se.For(a => a.ReissueFor)
				.Suggest<Insurance>();


			se.For(a => a.Producer)
				.Suggest<InsuranceCompany>()
				.RU("Страховая компания")
				.Required();


//			se.For(a => a.Provider)
//				.Suggest<InsuranceProvider>();

		}

		public override ProductType Type => ProductType.Insurance;

		public override string Name => Number ?? PnrCode;

		public override string PassengerName => GetPassengerNames();

		[Patterns.Number, EntityName2, Required]
		public virtual string Number { get; set; }

		[Patterns.StartDate, Required]
		public virtual DateTime? StartDate { get; set; }

		[Patterns.FinishDate]
		public virtual DateTime? FinishDate { get; set; }


		public new partial class Service : Service<Insurance>
		{

		}

	}


	public partial class InsuranceRefund : Insurance
	{
		public override ProductType Type => ProductType.InsuranceRefund;

		public override bool IsRefund => true;

		public override string Name => string.Format(DomainRes.ProductRefund_NameFormat, base.Name);




		public new partial class Service : Service<InsuranceRefund> { }
	}

}
