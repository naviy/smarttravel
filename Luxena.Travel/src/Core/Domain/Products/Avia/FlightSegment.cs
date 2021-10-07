using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[GenericPrivileges]
	[RU("������� �������", "������� ��������")]
	public partial class FlightSegment : Entity2
	{

		[RU("���������")]
		public virtual AviaTicket Ticket { get; set; }

		[EntityName]
		public virtual int Position { get; set; }

		[RU("��� ��������")]
		public virtual FlightSegmentType Type { get; set; }

		[RU("�� ���������")]
		public virtual string FromAirportCode { get; set; }

		public virtual string FromAirportName { get; set; }

		[RU("�� ���������")]
		public virtual Airport FromAirport { get; set; }

		public virtual Country FromCountry => FromAirport?.Country;

		[RU("� ��������")]
		public virtual string ToAirportCode { get; set; }

		public virtual string ToAirportName { get; set; }

		[RU("� ��������")]
		public virtual Airport ToAirport { get; set; }

		public virtual Country ToCountry => ToAirport?.Country;

		public virtual string CarrierIataCode { get; set; }

		[RU("���")]
		public virtual string CarrierPrefixCode { get; set; }

		[RU("����������")]
		public virtual string CarrierName { get; set; }

		[RU("����������"), Suggest(typeof(Airline))]
		public virtual Organization Carrier { get; set; }

		[RU("����������� ����������"), Suggest(typeof(Airline))]
		public virtual Organization Operator { get; set; }

		[RU("����")]
		public virtual string FlightNumber { get; set; }

		[RU("��� �����")]
		public virtual AirplaneModel Equipment { get; set; }

		[RU("��� ������-������")]
		public virtual string ServiceClassCode { get; set; }

		[RU("������-�����"), DefaultValue(1)]
		public virtual ServiceClass? ServiceClass { get; set; }

		[RU("�����������"), DateTime]
		public virtual DateTime? DepartureTime { get; set; }

		[RU("��������"), DateTime]
		public virtual DateTime? ArrivalTime { get; set; }

		public virtual string MealCodes { get; set; }

		[RU("�������")]
		public virtual MealType? MealTypes { get; set; }

		public virtual string MealNames(string separator = ", ")
		{
			if (MealTypes == null)
				return MealCodes;

			var mealTypes = (int)MealTypes.Value;
			if (mealTypes == 0)
				return null;

			var sb = new StringWrapper();

			var re1 = new Regex(@"[A-Z]");

			foreach (var type in Enum.GetValues(typeof(MealType)))
			{
				if ((mealTypes & (int)type) == 0) continue;

				if (sb > 0)
					sb += separator;
				sb += re1.Replace(type.ToString(), m => " " + m.Value.ToLower()).Trim();
			}

			return sb;
		}



		[RU("���-�� ���������")]
		public virtual int? NumberOfStops { get; set; }

		[RU("�����")]
		public virtual string Luggage { get; set; }

		[RU("��������")]
		public virtual string CheckInTerminal { get; set; }

		[RU("�����������")]
		public virtual string CheckInTime { get; set; }

		[RU("����� ��������")]
		public virtual string Duration { get; set; }

		[RU("��������")]
		public virtual string ArrivalTerminal { get; set; }

		[RU("�����")]
		public virtual string Seat { get; set; }

		[RU("���� ������")]
		public virtual string FareBasis { get; set; }

		[RU("��� �������� �����")]
		public virtual bool Stopover { get; set; }


		public virtual decimal? Surcharges { get; set; }

		public virtual bool IsInclusive { get; set; }

		public virtual decimal? Fare { get; set; }

		public virtual decimal? StopoverOrTransferCharge { get; set; }

		public virtual bool IsSideTrip { get; set; }

		/// <summary>
		/// ���������� � ����������
		/// </summary>
		public virtual double Distance { get; set; }

		public virtual Money Amount { get; set; }

		/// <summary>
		/// ����� ������
		/// </summary>
		public virtual Money CouponAmount { get; set; }


		public virtual string GetFromAirportCode()
		{
			return FromAirport != null ? FromAirport.Code : FromAirportCode;
		}

		public virtual string GetToAirportCode()
		{
			return ToAirport != null ? ToAirport.Code : ToAirportCode;
		}

		public static void Copy(FlightSegment source, FlightSegment target)
		{
			target.Position = source.Position;
			target.Type = source.Type;
			target.FromAirportCode = source.FromAirportCode;
			target.FromAirportName = source.FromAirportName;
			target.FromAirport = source.FromAirport;
			target.ToAirportCode = source.ToAirportCode;
			target.ToAirportName = source.ToAirportName;
			target.ToAirport = source.ToAirport;
			target.CarrierIataCode = source.CarrierIataCode;
			target.CarrierPrefixCode = source.CarrierPrefixCode;
			target.CarrierName = source.CarrierName;
			target.Carrier = source.Carrier;
			target.Operator = source.Operator;
			target.FlightNumber = source.FlightNumber;
			target.Equipment = source.Equipment;
			target.ServiceClassCode = source.ServiceClassCode;
			target.ServiceClass = source.ServiceClass;
			target.DepartureTime = source.DepartureTime;
			target.ArrivalTime = source.ArrivalTime;
			target.MealCodes = source.MealCodes;
			target.MealTypes = source.MealTypes;
			target.NumberOfStops = source.NumberOfStops;
			target.Luggage = source.Luggage;
			target.CheckInTerminal = source.CheckInTerminal;
			target.CheckInTime = source.CheckInTime;
			target.Duration = source.Duration;
			target.ArrivalTerminal = source.ArrivalTerminal;
			target.Seat = source.Seat;
			target.FareBasis = source.FareBasis;
			target.Stopover = source.Stopover;
			target.CouponAmount = source.CouponAmount;
		}

		public override string ToString()
		{
			return $"{Ticket} #{Position}: {FromAirportCode} {CarrierIataCode} {ToAirportCode} ({Distance:0.0} km, {Surcharges:0.00} + {Fare:0.00} + {StopoverOrTransferCharge:0.00} = {Amount:0.00})";
		}


		public override Entity Resolve(Domain db)
		{
			var r = this;

			if (r.Carrier == null)
			{
				if (r.CarrierIataCode.Yes())
					r.Carrier = db.Airline.ByIataCode(r.CarrierIataCode);
				else if (r.CarrierPrefixCode.Yes())
					r.Carrier = db.Airline.ByPrefixCode(r.CarrierPrefixCode);
			}
			else
				r.Carrier += db.Airline;

			r.Operator += db.Airline;


			if (r.FromAirport == null)
			{
				if (r.FromAirportCode.Yes())
					r.FromAirport = db.Airport.ByCode(r.FromAirportCode);
			}
			else
				r.FromAirport += db;

			if (r.ToAirport == null)
			{
				if (r.ToAirportCode.Yes())
					r.ToAirport = db.Airport.ByCode(r.ToAirportCode);
			}
			else
				r.ToAirport += db;


			if (r.Carrier != null && r.ServiceClassCode.Yes())
				r.ServiceClass = db.AirlineServiceClass.GetServiceClass(r.ServiceClassCode, r.Carrier);

			r.Equipment += db.AirplaneModel;

			r.Distance = Airport.GetDistance(r.FromAirport, r.ToAirport);

			if (r.MealCodes.Yes())
			{
				foreach (var mealType in r.MealCodes.Select(a => _mealCodeMapping.By(a)))
				{
					r.MealTypes = (r.MealTypes ?? 0) | mealType;
				}
			}

			CouponAmount += db;

			return r;
		}

		private static readonly IDictionary<char, MealType> _mealCodeMapping = new Dictionary<char, MealType>
		{
			{ 'B', MealType.Breakfast },
			{ 'K', MealType.ContinentalBreakfast },
			{ 'L', MealType.Lunch },
			{ 'D', MealType.Dinner },
			{ 'S', MealType.Snack },
			{ 'O', MealType.ColdMeal },
			{ 'H', MealType.HotMeal },
			{ 'M', MealType.Meal },
			{ 'R', MealType.Refreshment },
			{ 'C', MealType.AlcoholicComplimentaryBeverages },
			{ 'F', MealType.FoodForPurchase },
			{ 'P', MealType.AlcoholicBeveragesForPurchase },
			{ 'Y', MealType.DutyFree }
		};


		public class Service : Entity2Service<FlightSegment>
		{
			public Service()
			{
				Modifing += r =>
				{
					r.FromAirport.Do(a =>
					{
						r.FromAirportCode = a.Code;
						r.FromAirportName = a.Name;
					});
					r.ToAirport.Do(a =>
					{
						r.ToAirportCode = a.Code;
						r.ToAirportName = a.Name;
					});

					r.Carrier.Do(a =>
					{
						r.CarrierIataCode = a.AirlineIataCode;
						r.CarrierName = a.Name;
						r.CarrierPrefixCode = a.AirlinePrefixCode;
					});
				};
			}
		}

	}

}