using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Экскурсия", "Экскурсии")]
	public partial class Excursion : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Excursion> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<Excursion>();
		}

		public override ProductType Type => ProductType.Excursion;

		public override string Name => DomainRes.Excursion;

		public override string PassengerName => GetPassengerNames();


		[Patterns.StartDate]
		public virtual DateTime StartDate { get; set; }


		[Patterns.FinishDate]
		public virtual DateTime? FinishDate { get; set; }

		[RU("Название тура"), Required]
		public virtual string TourName { get; set; }


		public new partial class Service : Service<Excursion>
		{

		}

	}

}
