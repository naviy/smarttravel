using System;
using System.ComponentModel;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Ж/д билет или возврат", "Ж/д билеты и возвраты"), Icon("subway")]
	public abstract partial class RailwayDocument : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<RailwayDocument> se)
		{
			se.For(a => a.Type)
				.Enum(ProductType.Pasteboard, ProductType.PasteboardRefund);

			se.For(a => a.Number)
				.Length(24);

			se.For(a => a.Provider)
				.Lookup<PasteboardProvider>();
			
			se.For(a => a.RefundedProduct)
				.Lookup<Pasteboard>();

			se.For(a => a.ReissueFor)
				.Lookup<Pasteboard>();
		}


		[Patterns.Number, EntityName2]
		public string Number { get; set; }



		[RU("Начальная станция"), Patterns.DeparturePlace, Length(20)]
		public string DeparturePlace { get; set; }

		[Patterns.DepartureDate]
		public DateTimeOffset? DepartureDate { get; set; }

		[Patterns.DepartureTime]
		public string DepartureTime { get; set; }


		[RU("Конечная станция"), Patterns.ArrivalPlace, Length(20)]
		public string ArrivalPlace { get; set; }

		[Patterns.ArrivalDate]
		public DateTimeOffset? ArrivalDate { get; set; }

		[Patterns.ArrivalTime]
		public string ArrivalTime { get; set; }


		[RU("Номер поезда")]
		public string TrainNumber { get; set; }

		[RU("Номер вагона")]
		public string CarNumber { get; set; }

		[RU("Номер места")]
		public string SeatNumber { get; set; }


		[Patterns.ServiceClass, DefaultValue(0)]
		public PasteboardServiceClass ServiceClass { get; set; }


		public override string GetOrderItemText(string lang) =>
			Localization(lang) +
			(DeparturePlace + ArrivalPlace.As(a => " - " + a)).As(a => " " + a) +
			DepartureDate.AsDateString(a => ", " + Texts.DepartureDate[lang] + " " + a) +
			ServiceClass.EnumLocalization()[lang].As(a => ", " + a) +
			TrainNumber.As(a => ", " + Texts.Train[lang] + " " + a) +
			CarNumber.As(a => ", " + Texts.Wagon[lang] + " " + a) +
			GetPassengerNames().As(a => ", " + a);
	}


	[RU("Ж/д билет", "Ж/д билеты")]
	[UA("Залізничний квиток")]
	public partial class Pasteboard : RailwayDocument
	{
		public override ProductType Type => ProductType.Pasteboard;
	}


	[RU("Возврат ж/д билета", "Возвраты ж/д билетов")]
	[UA("Повернення залізничного квитка")]
	public partial class PasteboardRefund : RailwayDocument
	{
		public override ProductType Type => ProductType.PasteboardRefund;

		public override bool IsRefund => true;
	}


	partial class Domain
	{
		public DbSet<RailwayDocument> RailwayDocuments { get; set; }
		public DbSet<Pasteboard> Pasteboards { get; set; }
		public DbSet<PasteboardRefund> PasteboardRefunds { get; set; }
	}

}
