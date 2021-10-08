using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract(Name = "Segment")]
	[XmlType(TypeName = "Segment")]
	public class FlightSegmentContract
	{

		[DataMember]
		public FlightSegmentType Type { get; set; }
		[DataMember]
		public int Position { get; set; }
		[DataMember]
		public string FromAirportCode { get; set; }
		[DataMember]
		public string FromAirportName { get; set; }
		[DataMember]
		public AirportContract FromAirport { get; set; }
		//[DataMember] public EntityReference FromCountry { get; set; }
		[DataMember]
		public string ToAirportCode { get; set; }
		[DataMember]
		public string ToAirportName { get; set; }
		[DataMember]
		public AirportContract ToAirport { get; set; }
		//[DataMember] public EntityReference ToCountry { get; set; }
		[DataMember]
		public string CarrierIataCode { get; set; }
		[DataMember]
		public string CarrierPrefixCode { get; set; }
		[DataMember]
		public string CarrierName { get; set; }
		[DataMember]
		public PartyReference Carrier { get; set; }
		[DataMember]
		public string FlightNumber { get; set; }
		[DataMember]
		public string FlightNumber2 { get; set; }
		[DataMember]
		public string ServiceClassCode { get; set; }
		[DataMember]
		public ServiceClass? ServiceClass { get; set; }
		[DataMember]
		public DateTime? DepartureTime { get; set; }
		[DataMember]
		public DateTime? ArrivalTime { get; set; }
		[DataMember]
		public string MealCodes { get; set; }
		[DataMember]
		public MealType? MealTypes { get; set; }
		[DataMember]
		public int? NumberOfStops { get; set; }
		[DataMember]
		public string Luggage { get; set; }
		[DataMember]
		public string CheckInTerminal { get; set; }
		[DataMember]
		public string CheckInTime { get; set; }
		[DataMember]
		public string Duration { get; set; }
		[DataMember]
		public string ArrivalTerminal { get; set; }
		[DataMember]
		public string Seat { get; set; }
		[DataMember]
		public string FareBasis { get; set; }
		[DataMember]
		public bool Stopover { get; set; }


		//===


		public FlightSegmentContract() { }


		public FlightSegmentContract(FlightSegment r)
		{
			Type = r.Type;
			Position = r.Position;
			FromAirportCode = r.FromAirportCode;
			FromAirportName = r.FromAirportName;
			FromAirport = r.FromAirport;
			//FromCountry = r.FromCountry;
			ToAirportCode = r.ToAirportCode;
			ToAirportName = r.ToAirportName;
			ToAirport = r.ToAirport;
			//ToCountry = r.ToCountry;
			CarrierIataCode = r.CarrierIataCode;
			CarrierPrefixCode = r.CarrierPrefixCode;
			CarrierName = r.CarrierName;
			Carrier = r.Carrier;
			FlightNumber = r.FlightNumber;
			FlightNumber2 = r.CarrierIataCode + r.FlightNumber;
			ServiceClassCode = r.ServiceClassCode;
			ServiceClass = r.ServiceClass;
			DepartureTime = r.DepartureTime;
			ArrivalTime = r.ArrivalTime;
			MealCodes = r.MealCodes;
			MealTypes = r.MealTypes;
			NumberOfStops = r.NumberOfStops;
			Luggage = r.Luggage;
			CheckInTerminal = r.CheckInTerminal;
			CheckInTime = r.CheckInTime;
			Duration = r.Duration;
			ArrivalTerminal = r.ArrivalTerminal;
			Seat = r.Seat;
			FareBasis = r.FareBasis;
			Stopover = r.Stopover;
		}


		//===


		public void AssignTo(Domain.Domain db, FlightSegment r)
		{
			r.Type = Type;
			r.Position = Position;
			r.FromAirportCode = FromAirportCode;
			r.FromAirportName = FromAirportName;
			r.FromAirport = db.Airport.ByCode(FromAirport?.Code);
			r.ToAirportCode = ToAirportCode;
			r.ToAirportName = ToAirportName;
			r.ToAirport = db.Airport.ByCode(ToAirport?.Code);
			r.CarrierIataCode = CarrierIataCode;
			r.CarrierPrefixCode = CarrierPrefixCode;
			r.CarrierName = CarrierName;
			r.Carrier = db.Organization.ByName(Carrier?.Text);
			r.FlightNumber = FlightNumber;
			//r.FlightNumber2 = CarrierIataCode + FlightNumber;
			r.ServiceClassCode = ServiceClassCode;
			r.ServiceClass = ServiceClass;
			r.DepartureTime = DepartureTime;
			r.ArrivalTime = ArrivalTime;
			r.MealCodes = MealCodes;
			r.MealTypes = MealTypes;
			r.NumberOfStops = NumberOfStops;
			r.Luggage = Luggage;
			r.CheckInTerminal = CheckInTerminal;
			r.CheckInTime = CheckInTime;
			r.Duration = Duration;
			r.ArrivalTerminal = ArrivalTerminal;
			r.Seat = Seat;
			r.FareBasis = FareBasis;
			r.Stopover = Stopover;
		}

	}


}