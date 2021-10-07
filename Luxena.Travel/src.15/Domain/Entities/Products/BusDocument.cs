using System;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Автобусный билет или возврат", "Автобусные билеты и возвраты"), Icon("bus")]
	public abstract partial class BusDocument : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<BusDocument> se)
		{
			se.For(a => a.Type)
				.Enum(ProductType.BusTicket, ProductType.BusTicketRefund);

			se.For(a => a.RefundedProduct)
				.Lookup<BusTicket>();

			se.For(a => a.ReissueFor)
				.Lookup<BusTicket>();

			se.For(a => a.Provider)
				.Lookup<BusTicketProvider>();
		}


		[Patterns.Number, EntityName2]
		public string Number { get; set; }

		[RU("Начальная станция"), Patterns.DeparturePlace]
		public string DeparturePlace { get; set; }

		[Patterns.DepartureDate]
		public DateTimeOffset? DepartureDate { get; set; }

		[Patterns.DepartureTime]
		public string DepartureTime { get; set; }


		[RU("Конечная станция"), Patterns.ArrivalPlace]
		public string ArrivalPlace { get; set; }

		[Patterns.ArrivalDate]
		public DateTimeOffset? ArrivalDate { get; set; }

		[Patterns.ArrivalTime]
		public string ArrivalTime { get; set; }


		[RU("Номер места")]
		public string SeatNumber { get; set; }


		public override string GetOrderItemText(string lang) =>
			Localization(lang) +
			(DeparturePlace + ArrivalPlace.As(a => " - " + a)).As(a => " " + a) +
			DepartureDate.AsDateString(a => ", " + Texts.DepartureDate[lang] + " " + a) +
			SeatNumber.As(a => ", " + Texts.SeatNumber + " " + a) +
			GetPassengerNames().As(a => ", " + a);

	}


	[RU("Автобусный билет", "Автобусные билеты")]
	[UA("Автобусний квиток")]
	public partial class BusTicket : BusDocument
	{
		public override ProductType Type => ProductType.BusTicket;
	}


	[RU("Возврат автобусного билета", "Возвраты автобусных билетов")]
	[UA("Повернення автобусного квитка")]
	public partial class BusTicketRefund : BusDocument
	{
		public override ProductType Type => ProductType.BusTicketRefund;

		public override bool IsRefund => true;
	}



	partial class Domain
	{
		public DbSet<BusDocument> BusDocuments { get; set; }
		public DbSet<BusTicket> BusTickets { get; set; }
		public DbSet<BusTicketRefund> BusTicketRefunds { get; set; }
	}

}
