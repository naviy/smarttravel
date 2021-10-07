using System;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Тур (готовый)", "Туры (готовые)")]
	public partial class Tour : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Tour> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<Tour>();

			se.For(a => a.Provider)
				.Suggest<TourProvider>();
		}

		public override ProductType Type { get { return ProductType.Tour; } }

		public override string Name { get { return DomainRes.Tour; } }

		public override string PassengerName { get { return GetPassengerNames(); } }

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


		[RU("Авиа (описание)")]
		public virtual string AviaDescription { get; set; }

		[RU("Трансфер (описание)")]
		public virtual string TransferDescription { get; set; }
		

		public new partial class Service : Service<Tour>
		{

		}

	}

}
