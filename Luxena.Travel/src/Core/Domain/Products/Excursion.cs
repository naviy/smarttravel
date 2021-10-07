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

		public override ProductType Type { get { return ProductType.Excursion; } }

		public override string Name { get { return DomainRes.Excursion; } }

		public override string PassengerName { get { return GetPassengerNames(); } }


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
