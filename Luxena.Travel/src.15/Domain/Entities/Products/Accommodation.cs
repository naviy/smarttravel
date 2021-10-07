using System;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Проживание", "Проживания"), Icon("bed")]
	[UA("Готель")]
	public partial class Accommodation : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Accommodation> se)
		{
			se.For(a => a.ReissueFor)
				.Lookup<Accommodation>()
				.Add(new Patterns.ReissueFor());

			se.For(a => a.Provider)
				.Lookup<AccommodationProvider>();
		}


		public override ProductType Type => ProductType.Accommodation;


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


		static partial void Config_(Domain.EntityConfiguration<Accommodation> entity)
		{
			entity.Association(a => a.AccommodationType);//, a => a.Accommodations);
			entity.Association(a => a.CateringType);//, a => a.Accommodations);
		}


		public override string GetOrderItemText(string lang) =>
			Localization(lang) +
			HotelName.As(a => ": " + Texts.Hotel[lang] + " " + a) +
			PlacementName.As(a => ", " + a) +
			Country.As(a => ", " + a) +
			AccommodationType.As(a => ", " + a) +
			", " + StartDate.ToDateString() + FinishDate.AsDateString(a => " - " + a) +
			CateringType.As(a => ", " + a) +
			GetPassengerNames().As(a => ", " + a);

	}


	partial class Domain
	{
		public DbSet<Accommodation> Accommodations { get; set; }
	}

}
