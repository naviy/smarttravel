using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.Avia
{

	public class FlightSegmentMap : Entity2Mapping<FlightSegment>
	{
		public FlightSegmentMap()
		{
			ManyToOne(x => x.Ticket, m => m.NotNullable(true));
			Property(x => x.Position, m => m.NotNullable(true));

			Property(x => x.Type, m => m.NotNullable(true));
			Property(x => x.FromAirportCode, m => m.Length(3));
			Property(x => x.FromAirportName, m => m.Length(200));
			ManyToOne(x => x.FromAirport);
			Property(x => x.ToAirportCode, m => m.Length(3));
			Property(x => x.ToAirportName, m => m.Length(200));
			ManyToOne(x => x.ToAirport);
			Property(x => x.CarrierIataCode, m => m.Length(2));
			Property(x => x.CarrierPrefixCode, m => m.Length(3));
			Property(x => x.CarrierName, m => m.Length(100));
			ManyToOne(x => x.Carrier);
			ManyToOne(x => x.Operator);
			Property(x => x.FlightNumber, m => m.Length(4));
			ManyToOne(x => x.Equipment);
			Property(x => x.ServiceClassCode, m => m.Length(1));
			Property(x => x.ServiceClass);
			Property(x => x.DepartureTime);
			Property(x => x.ArrivalTime);
			Property(x => x.MealCodes, m => m.Length(4));
			Property(x => x.MealTypes);
			Property(x => x.NumberOfStops);
			Property(x => x.Luggage, m => m.Length(3));
			Property(x => x.CheckInTerminal, m => m.Length(5));
			Property(x => x.CheckInTime, m => m.Length(5));
			Property(x => x.Duration, m => m.Length(6));
			Property(x => x.ArrivalTerminal, m => m.Length(5));
			Property(x => x.Seat, m => m.Length(10));
			Property(x => x.FareBasis, m => m.Length(20));
			Property(x => x.Stopover, m => m.NotNullable(true));

			Property(x => x.Surcharges);
			Property(x => x.IsInclusive, m => m.NotNullable(true));
			Property(x => x.Fare);
			Property(x => x.StopoverOrTransferCharge);
			Property(x => x.IsSideTrip, m => m.NotNullable(true));
			Property(x => x.Distance, m => m.NotNullable(true));
			Component(x => x.Amount);
			Component(x => x.CouponAmount);
		}
	}

}