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

		public override ProductType Type { get { return ProductType.Transfer; } }

		public override string Name { get { return DomainRes.Transfer; } }

		public override string PassengerName { get { return GetPassengerNames(); } }


		[Patterns.StartDate, Required]
		public virtual DateTime StartDate { get; set; }


		public new partial class Service : Service<Transfer>
		{

		}

	}

}
