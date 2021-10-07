using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class FlightSegmentDto : EntityContract
	{

		//public string StateInfo { get; set; }

		public Airport.Reference FromAirport { get; set; }

		public string FromAirportName { get; set; }

		public Airport.Reference ToAirport { get; set; }

		public string ToAirportName { get; set; }

		public string CarrierCode { get; set; }

		public Organization.Reference Carrier { get; set; }

		public Organization.Reference Operator { get; set; }

		public string FlightNumber { get; set; }

		public AirplaneModel.Reference Equipment { get; set; }

		public string ServiceClassCode { get; set; }

		public int ServiceClass { get; set; }

		public string ServiceClassName { get; set; }

		public DateTime? DepartureTime { get; set; }

		public DateTime? ArrivalTime { get; set; }

		public int? MealTypes { get; set; }

		public int? NumberOfStops { get; set; }

		public string Luggage { get; set; }

		public string CheckInTerminal { get; set; }

		public string CheckInTime { get; set; }

		public string Duration { get; set; }

		public string ArrivalTerminal { get; set; }

		public string Seat { get; set; }

		public string FareBasis { get; set; }

		public int Position { get; set; }

		public bool Stopover { get; set; }

		public FlightSegmentType Type { get; set; }

		public MoneyDto CouponAmount { get; set; }
	}


	public partial class FlightSegmentContractService
		: EntityContractService<FlightSegment, FlightSegment.Service, FlightSegmentDto>
	{
		public FlightSegmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				//c.StateInfo = "O";
				c.Position = r.Position;

				c.FromAirport = ((Airport.Reference)r.FromAirport).Do(a => a.Name = r.FromAirport.Code + " - " + a);
				c.FromAirportName = r.FromAirport?.Name ?? r.FromAirportName;

				c.ToAirport = ((Airport.Reference)r.ToAirport).Do(a => a.Name = r.ToAirport.Code + " - " + a);
				c.ToAirportName = r.ToAirport?.Name ?? r.ToAirportName;

				c.CarrierCode = r.CarrierIataCode;
				c.Carrier = r.Carrier;
				c.Operator = r.Operator;

				c.FlightNumber = r.FlightNumber;
				c.Equipment = r.Equipment;

				c.ServiceClassCode = r.ServiceClassCode;

				if (r.ServiceClass.HasValue && r.ServiceClass.Value != ServiceClass.Unknown)
					c.ServiceClassName = r.ServiceClass.ToDisplayString();

				if (r.ServiceClass != null)
					c.ServiceClass = (int)r.ServiceClass;


				c.DepartureTime = r.DepartureTime;
				c.ArrivalTime = r.ArrivalTime;

				c.MealTypes = (int?)r.MealTypes;

				c.NumberOfStops = r.NumberOfStops;

				c.Luggage = r.Luggage;
				c.CheckInTerminal = r.CheckInTerminal;
				c.CheckInTime = r.CheckInTime;
				c.Duration = r.Duration;
				c.ArrivalTerminal = r.ArrivalTerminal;
				c.Seat = r.Seat;
				c.FareBasis = r.FareBasis;
				c.Stopover = r.Stopover;
				c.Type = r.Type;

				c.CouponAmount = r.CouponAmount;
			};

			EntityFromContract += (r, c) =>
			{
				r.Position = c.Position + db;

				r.FromAirport = c.FromAirport + db;

				r.ToAirport = c.ToAirport + db;
				r.Carrier = c.Carrier + db;
				r.CarrierIataCode = c.CarrierCode + db;
				r.Operator = c.Operator + db;

				r.FlightNumber = c.FlightNumber + db;
				r.Equipment = c.Equipment + db;

				r.ServiceClassCode = c.ServiceClassCode + db;
				r.ServiceClass = (ServiceClass)c.ServiceClass + db;

				r.DepartureTime = c.DepartureTime + db;
				r.ArrivalTime = c.ArrivalTime + db;

				r.MealTypes = (MealType?)c.MealTypes + db;

				r.NumberOfStops = c.NumberOfStops + db;

				r.Luggage = c.Luggage + db;
				r.CheckInTerminal = c.CheckInTerminal + db;
				r.CheckInTime = c.CheckInTime + db;
				r.Duration = c.Duration + db;
				r.ArrivalTerminal = c.ArrivalTerminal + db;
				r.Seat = c.Seat + db;
				r.FareBasis = c.FareBasis + db;
				r.Stopover = c.Stopover + db;
				r.Type = c.Type + db;

				r.Ticket?.UpdateSegment(r);
			};
		}
	}

}