using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Трансфер", "Трансферы")]
	public partial class Transfer : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Transfer> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<Transfer>();

			se.For(a => a.Provider)
				.Suggest<TransferProvider>();
		}

		public override ProductType Type => ProductType.Transfer;

		public override string Name => PnrCode ?? DomainRes.Transfer;

		public override string PassengerName => GetPassengerNames();


		[Patterns.StartDate, Required]
		public virtual DateTime StartDate { get; set; }


		public new partial class Service : Service<Transfer>
		{

		}

	}

}
