using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Турпакет", "Турпакеты")]
	[UA("Турпакет")]
	public partial class Tour : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Tour> se)
		{
			se.For(a => a.ReissueFor)
				.Lookup<Tour>();

			se.For(a => a.Provider)
				.Lookup<TourProvider>();
		}

		public override ProductType Type => ProductType.Tour;

		[RU("Гостиница")]
		public string HotelName { get; set; }

		[RU("Офис гостиницы", ruShort: "офис")]
		public string HotelOffice { get; set; }

		[RU("Код гостиницы", ruShort: "код")]
		public string HotelCode { get; set; }


		[RU("Расположение")]
		public string PlacementName { get; set; }

		[RU(ruShort: "офис")]
		public string PlacementOffice { get; set; }

		[RU(ruShort: "код")]
		public string PlacementCode { get; set; }


		protected AccommodationType _AccommodationType;

		protected CateringType _CateringType;


		[RU("Авиа (описание)")]
		public string AviaDescription { get; set; }

		[RU("Трансфер (описание)")]
		public string TransferDescription { get; set; }


		static partial void Config_(Domain.EntityConfiguration<Tour> entity)
		{
			entity.Association(a => a.AccommodationType);//, a => a.Tours);
			entity.Association(a => a.CateringType);//, a => a.Tours);
		}

	}


	partial class Domain
	{
		public DbSet<Tour> Tours { get; set; }
	}
	
}
