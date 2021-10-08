using System;
using System.Collections.Generic;
using System.Linq;


namespace Luxena.Travel.Domain
{

	[RU("Авиабилет", "Авиабилеты")]
	public partial class AviaTicket : AviaDocument
	{

		public AviaTicket()
		{
			SetupCalculations();
		}

		public override ProductType Type => ProductType.AviaTicket;

		public override string Itinerary
		{
			get { return _itinerary.Get(); }
			set { _itinerary.Set(value); }
		}

		[Patterns.DepartureDate]
		public virtual DateTime? Departure
		{
			get { return _departure.Get(); }
			set { _departure.Set(value); }
		}

		[Patterns.ArrivalDate]
		public virtual DateTime? Arrival
			=> _segments.LastAs(a => a.ArrivalTime);

		public virtual DateTime? LastDeparture
			=> _ticketedSegments.Get().LastOrDefault()?.DepartureTime;

		// Для эспорта в эксель
		public virtual string FareBasises
			=> _ticketedSegments.Get().Select(a => a.FareBasis).Where(a => a.Yes()).Distinct().OrderBy(a => a).Join(", ").Clip();


		public virtual bool Domestic { get; set; }

		public virtual bool Interline
		{
			get { return _interline.Get(); }
			set { _interline.Set(value); }
		}

		[RU("Классы сегментов")]
		public virtual string SegmentClasses
		{
			get { return _segmentClasses.Get(); }
			set { _segmentClasses.Set(value); }
		}

		public virtual string Endorsement { get; set; }

		public virtual bool IsManual { get; set; }

		public virtual IList<FlightSegment> Segments
		{
			get { return _segments; }
			set { _segments = value; }
		}

		public virtual Money FareTotal
		{
			get { return _fareTotal; }
			set { _fareTotal = value; }
		}

		public virtual IList<FlightSegment> GetTicketedSegments()
		{
			return _ticketedSegments.Get();
		}

		public virtual IList<PenalizeOperation> PenalizeOperations
		{
			get { return _penalizeOperations; }
			set { _penalizeOperations = value; }
		}


		public override object Clone()
		{
			var ticket = (AviaTicket)base.Clone();

			ticket._fareTotal = _fareTotal.Clone();

			ticket.SetupCalculations();

			ticket._segments = new List<FlightSegment>();
			ticket._penalizeOperations = new List<PenalizeOperation>();

			foreach (var segment in _segments)
				ticket.AddSegment((FlightSegment)segment.Clone());

			foreach (var operation in _penalizeOperations)
				ticket.AddPenalizeOperation((PenalizeOperation)operation.Clone());

			return ticket;
		}

		public virtual void AddSegment(FlightSegment segment)
		{
			segment.Ticket = this;

			_segments.Add(segment);

			OnItineraryChange();
		}

		public virtual void RemoveSegment(FlightSegment segment)
		{
			_segments.Remove(segment);

			OnItineraryChange();
		}

		//public virtual void UpdateSegmentPosition(FlightSegment segment, int position)
		//{
		//	segment.Ticket = this;

		//	var index = _segments.IndexOf(segment);
		//	_segments[index].Position = position;

		//	OnItineraryChange();
		//}

		public virtual void UpdateSegment(FlightSegment segment)
		{
			segment.Ticket = this;
			OnItineraryChange();
		}

		public virtual PenalizeOperation FindPenalizeOperation(PenalizeOperationType type)
		{
			return _penalizeOperations.Find(op => op.Type == type);
		}

		public virtual void AddPenalizeOperation(PenalizeOperation operation)
		{
			operation.Ticket = this;

			_penalizeOperations.Add(operation);
		}

		public virtual void InsertSegment(int index, FlightSegment segment)
		{
			segment.Ticket = this;

			_segments.Insert(index, segment);

			OnItineraryChange();
		}

		public override string GetItinerary(Func<Airport, string> airportToString, bool withSpaces, bool withDates)
		{
			var segments = _ticketedSegments.Get();

			if (segments.No())
				return string.Empty;

			var sb = new StringWrapper();
			var separator = string.Empty;
			string lastAirportCode = null;

			foreach (var segment in segments)
			{
				var airportString = airportToString(segment.FromAirport) ?? segment.FromAirportCode;

				if ((lastAirportCode.No() || lastAirportCode != segment.GetFromAirportCode()) &&
					airportString.Yes())
				{
					if (lastAirportCode.Yes() && lastAirportCode != segment.GetFromAirportCode())
						separator = withSpaces ? "; " : ";";

					sb += separator;
					sb += airportString;

					separator = withSpaces ? " - " : "-";
				}

				airportString = airportToString(segment.ToAirport) ?? segment.ToAirportCode;

				if (airportString.Yes())
				{
					sb += separator;
					sb += airportString;
				}

				lastAirportCode = segment.GetToAirportCode();
			}

			if (withDates)
			{
				var departure = segments.One(a => a.DepartureTime) ?? Departure;

				if (departure != null)
				{
					sb += ", ";
					sb += departure.AsDateString();

					// нужно в УФСА
					var arrival = segments.LastAs(a => a.ArrivalTime);

					if (arrival != null && arrival.Value.Date != departure.Value.Date)
					{
						sb += " - ";
						sb += arrival.AsDateString();
					}
				}
			}

			return sb;
		}

		public virtual void SetDomestic()
		{
			Domestic = Vat != null && Vat.Amount != 0;
		}

		//		public virtual void SetInterline()
		//		{
		//			Interline = _ticketedSegments.Get().Any(segment => !Equals(segment.Carrier, Producer));
		//		}

		public virtual void RemovePenalizeOperation(PenalizeOperation operation)
		{
			_penalizeOperations.Remove(operation);
		}


		public virtual Airport GetDirection()
		{
			if (_segments.No()) return null;

			return (
				from s in Segments
				let next = Segments.By(a => a.Position == s.Position + 1)

				where s.Stopover && s.Type == FlightSegmentType.Ticketed

				orderby
					next == null ? 0 : (next.DepartureTime - s.ArrivalTime)?.Seconds

				select
					s.ToAirport?.Code != "KBP" ? s.ToAirport :
					Segments.By(s2 =>
						s2.Position <= s.Position &&
						s2.Position > (Segments.Where(s3 => s3.Stopover && s3.Position < s.Position).Max(s3 => (int?)s3.Position) ?? -1)
					)?.FromAirport

			).One();
		}


		public virtual void CalculateFares()
		{
			if (_segments.Count == 0) return;

			if (!CalculateFaresByFareTotal())
			{
				CalculateFaresByTotal();
			}

		}

		public virtual bool CalculateFaresByTotal()
		{
			var useFare = Fare != null && Fare.Amount > 0 && Fare.Currency != null;
			var useTotal = Total != null && Total.Amount > 0 && Total.Currency != null;

			if (!useFare && !useTotal)
				return false;

			var totalAmount = useFare ? Fare.Amount : Total.Amount - (FeesTotal != null ? FeesTotal.Amount : 0);
			var totalCurrency = useFare ? Fare.Currency : Total.Currency;

			var fareSegments = _segments.Where(a => a.Type == 0).ToList();
			var normalFareSegments = fareSegments.Where(a => !a.IsSideTrip).ToList();

			var totalDistance = normalFareSegments.Sum(a => a.Distance);
			if (totalDistance < 0.01) return false;

			var farePerMeter = (double)totalAmount / totalDistance;
			var remaining = totalAmount;

			foreach (var seg in normalFareSegments)
			{
				seg.Amount = new Money(totalCurrency, (decimal)Math.Round(farePerMeter * seg.Distance, 2));
				remaining -= seg.Amount.Amount;
			}

			fareSegments[0].Amount.Amount += remaining;

			foreach (var seg in fareSegments.Where(a => a.IsSideTrip))
			{
				seg.Amount = new Money(
					totalCurrency,
					(seg.Surcharges ?? 0) + (seg.Fare ?? 0) + (seg.StopoverOrTransferCharge ?? 0)
				);
			}

			return true;
		}

		public virtual bool CalculateFaresByFareTotal()
		{
			if (FareTotal == null || FareTotal.Amount <= 0 || FareTotal.Currency == null)
				return false;

			var fareSegments = _segments.Where(a => a.Type == 0).ToList();

			var unusedFareTotal = FareTotal.Amount - fareSegments.Sum(a => (a.Surcharges ?? 0) + (a.Fare ?? 0) + (a.StopoverOrTransferCharge ?? 0));
			if (unusedFareTotal < 0)
				unusedFareTotal = 0;

			var normalFareSegments = fareSegments.Where(a => !a.IsSideTrip).ToList();

			// Общая дистанция inclusives (т.е. без fare), для которых будет необходимо распределить остаток от FareTotal
			var unusedDistanceTotal = 0.0;
			var fareDistance = 0.0;
			for (int i = 0, len = normalFareSegments.Count; i < len; i++)
			{
				var seg = normalFareSegments[i];
				fareDistance += seg.Distance;

				if ((seg.IsInclusive || i == len - 1) && (seg.Fare ?? 0) == 0)
					unusedDistanceTotal += fareDistance;

				if (seg.IsInclusive || seg.Fare > 0)
					fareDistance = 0;
			}

			var startFareIndex = 0;
			var lastInclusiveFareIndex = -1;
			var unusedRamining = unusedFareTotal;
			fareDistance = 0.0;

			for (int i = 0, len = normalFareSegments.Count; i < len; i++)
			{
				var seg0 = normalFareSegments[i];
				fareDistance += seg0.Distance;

				if (!seg0.IsInclusive && !(seg0.Fare > 0) && i != len - 1) continue;

				var useUnused = seg0.Fare == null || seg0.Fare == 0;

				if (useUnused && unusedDistanceTotal > 0.01 || !useUnused && fareDistance > 0.01)
				{
					var farePerMeter = useUnused ? (double)unusedFareTotal / unusedDistanceTotal : (double)seg0.Fare.Value / fareDistance;

					var sumFare = 0m;
					for (var j = startFareIndex; j <= i; j++)
					{
						var seg = normalFareSegments[j];

						var fare = (decimal)Math.Round(farePerMeter * seg.Distance, 2);
						seg.Amount = new Money(FareTotal.Currency, fare + (seg.Surcharges ?? 0) + (seg.StopoverOrTransferCharge ?? 0));
						sumFare += fare;
					}

					if (useUnused)
					{
						lastInclusiveFareIndex = startFareIndex;
						unusedRamining -= sumFare;
					}
					else
						normalFareSegments[startFareIndex].Amount.Amount += seg0.Fare.Value - sumFare;
				}

				startFareIndex = i + 1;
				fareDistance = 0;
			}

			if (lastInclusiveFareIndex >= 0)
				normalFareSegments[lastInclusiveFareIndex].Amount.Amount += unusedRamining;


			foreach (var seg in fareSegments.Where(a => a.IsSideTrip))
			{
				seg.Amount = new Money(FareTotal.Currency,
					(seg.Surcharges ?? 0) + (seg.Fare ?? 0) + (seg.StopoverOrTransferCharge ?? 0)
				);
			}

			return true;
		}

		private void SetupCalculations()
		{
			_ticketedSegments = new Calculated<List<FlightSegment>>(() =>
			{
				var value = Segments.ToList();

				if (Number == null && Origin == ProductOrigin.AmadeusAir)
				{

				}
				else
					value = value.FindAll(a => a.Type == FlightSegmentType.Ticketed);

				_ticketedSegments.Set(value);
			});

			_itinerary = new Calculated<string>(UpdateCalculated);
			_departure = new Calculated<DateTime?>(UpdateCalculated);
			_interline = new Calculated<bool>(UpdateCalculated);
			_segmentClasses = new Calculated<string>(UpdateCalculated);
		}

		private void OnItineraryChange()
		{
			_ticketedSegments.Reset();
			_itinerary.Reset();
			_departure.Reset();
			_interline.Reset();
			_segmentClasses.Reset();
		}


		private void UpdateCalculated()
		{
			Itinerary = GetItinerary(airport => airport?.Code, false, false);

			var segments = _ticketedSegments.Get();

			SegmentClasses = string.Join("-", segments.FindAll(s => s.ServiceClassCode.Yes()).Select(s => s.ServiceClassCode));

			Departure = segments.Count > 0 ? segments[0].DepartureTime : null;

			Interline = segments.Any(segment => !Equals(segment.Carrier, Producer));
		}


		private IList<FlightSegment> _segments = new List<FlightSegment>();
		private IList<PenalizeOperation> _penalizeOperations = new List<PenalizeOperation>();

		private Calculated<List<FlightSegment>> _ticketedSegments;
		private Calculated<string> _itinerary;
		private Calculated<DateTime?> _departure;
		private Calculated<bool> _interline;
		private Calculated<string> _segmentClasses;

		private Money _fareTotal;
	}

}