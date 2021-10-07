using System;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Проживание", "Проживания")]
	public partial class Accommodation : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Accommodation> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<Accommodation>();

			se.For(a => a.Provider)
				.Suggest<AccommodationProvider>();
		}


		public override ProductType Type => ProductType.Accommodation;

		public override string Name => DomainRes.Accommodation;

		public override string PassengerName => GetPassengerNames();


		[Patterns.StartDate]
		public virtual DateTime StartDate { get; set; }

		[Patterns.FinishDate]
		public virtual DateTime? FinishDate { get; set; }


		[RU("Гостиница")]
		public virtual string HotelName { get; set; }

		[RU("Офис гостиницы", ruShort: "офис")]
		public virtual string HotelOffice { get; set; }

		[RU("Код гостиницы", ruShort: "код")]
		public virtual string HotelCode { get; set; }


		[RU("Расположение")]
		public virtual string PlacementName { get; set; }

		[RU(ruShort: "офис")]
		public virtual string PlacementOffice { get; set; }

		[RU(ruShort: "код")]
		public virtual string PlacementCode { get; set; }


		public virtual AccommodationType AccommodationType { get; set; }

		public virtual CateringType CateringType { get; set; }


		public new partial class Service : Service<Accommodation>
		{

		}

	}

}
