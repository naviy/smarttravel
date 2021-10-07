using System;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Полетный сегмент", "Полетные сегменты")]
	[GenericPrivileges]
	public partial class FlightSegment : Entity2
	{

		[EntityName]
		public string Name { get { return Ticket.As(a => a.Name + " ") + " #" + Position; } }

		[Localization(typeof(AviaTicket))]
		protected AviaTicket _Ticket;

		[EntityPosition, Patterns.Position]
		public int Position { get; set; }

		public FlightSegmentType Type { get; set; }

		[RU("Из аэропорта (код)", ruShort: "код")]
		public string FromAirportCode { get; set; }

		[RU("Из аэропорта (название)", ruShort: "название")]
		public string FromAirportName { get; set; }

		[RU("Из аэропорта")]
		protected Airport _FromAirport;

		//public Country FromCountry { get { return FromAirport.As(a => a.Country); } }

		[RU("В аэропорт (код)", ruShort: "код")]
		public string ToAirportCode { get; set; }

		[RU("В аэропорт (название)", ruShort: "название")]
		public string ToAirportName { get; set; }

		[RU("В аэропорт")]
		protected Airport _ToAirport;

		//public Country ToCountry { get { return ToAirport.As(a => a.Country); } }

		public string CarrierIataCode { get; set; }

		public string CarrierPrefixCode { get; set; }

		public string CarrierName { get; set; }

		[RU("Перевозчик")]
		protected Organization _Carrier;

		[RU("Рейс"), Length(4)]
		public string FlightNumber { get; set; }

		[RU("Код сервис-класса")]
		public string ServiceClassCode { get; set; }

		public  ServiceClass? ServiceClass { get; set; }

		[RU("Дата/время отправления"), DateTime]
		public  DateTimeOffset? DepartureTime { get; set; }

		[RU("Дата/время прибытия"), DateTime]
		public DateTimeOffset? ArrivalTime { get; set; }

		[RU("Коды питания")]
		public string MealCodes { get; set; }

		[RU("Питание")]
		public MealType? MealTypes { get; set; }

		[RU("Остановки")]
		public int? NumberOfStops { get; set; }

		[RU("Багаж"), Length(3)]
		public string Luggage { get; set; }

		[RU("Терминал отправления", ruShort: "терминал")]
		public string CheckInTerminal { get; set; }

		[RU("Регистрация")]
		public string CheckInTime { get; set; }

		[RU("Перелет"), Length(4)]
		public string Duration { get; set; }

		[RU("Терминал прибытия", ruShort: "терминал")]
		public string ArrivalTerminal { get; set; }

		[RU("Место"), Length(3)]
		public string Seat { get; set; }

		[RU("База тарифа"), Length(10)]
		public string FareBasis { get; set; }

		[RU("Это конечный пункт")]
		public bool Stopover { get; set; }


		public decimal? Surcharges { get; set; }

		public bool IsInclusive { get; set; }

		public decimal? Fare { get; set; }

		public decimal? StopoverOrTransferCharge { get; set; }

		public bool IsSideTrip { get; set; }

		[RU("Расстояние, км")]
		public double Distance { get; set; }

		public Money Amount { get; set; }

		public Money CouponAmount { get; set; }


		static partial void Config_(Domain.EntityConfiguration<FlightSegment> entity)
		{
			entity.Association(a => a.Ticket, a => a.Segments);
			entity.Association(a => a.FromAirport);
			entity.Association(a => a.ToAirport);
			entity.Association(a => a.Carrier);
		}

	}


	partial class Domain
	{
		public DbSet<FlightSegment> FlightSegments { get; set; }
	}

}
