using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Luxena.Travel.Domain
{

	[RU("Авиабилет", "Авиабилеты"), UA("Авіаквиток")]
	public partial class AviaTicket : AviaDocument
	{

		public override ProductType Type => ProductType.AviaTicket;

		[Patterns.DepartureDate]
		public DateTimeOffset? Departure { get; set; }

		//		[Patterns.ArrivalDate]
		//		public DateTimeOffset? Arrival
		//		{
		//			get { return Segments.LastAs(a => a.ArrivalTime); }
		//		}

		//		public virtual DateTime? LastDeparture
		//		{
		//			get
		//			{
		//				var segments = _ticketedSegments.Get();
		//
		//				return segments.Count > 0 ? segments[segments.Count - 1].DepartureTime : null;
		//			}
		//		}

		public bool Domestic { get; set; }

		public bool Interline { get; set; }

		[RU("Классы сегментов")]
		public string SegmentClasses { get; set; }

		public string Endorsement { get; set; }

		//		public bool IsManual { get; set; }


		public Money FareTotal { get; set; }


		public virtual ICollection<FlightSegment> Segments { get; set; }


		public override string GetOrderItemText(string lang) =>
			Localization(lang) +
			GetOrderItemText2(lang) +
			GetItinerary(true, true, true).As(a => $" {Texts.Itinerary[lang]} {a}");


		public ICollection<FlightSegment> GetTicketedSegments() =>
			Number == null && Origin == ProductOrigin.AmadeusAir
				? Segments
				: Segments?.Where(a => a.Type == FlightSegmentType.Ticketed).ToList();


		public string GetItinerary(bool withSettlement, bool withSpaces, bool withDates)
		{
			var segments = GetTicketedSegments();

			if (segments.No())
				return string.Empty;

			var sb = new StringWrapper();
			var separator = string.Empty;
			string lastAirportCode = null;

			Func<Airport, string> airportToString;
			if (withSettlement)
				airportToString = a =>
				{
					if (a == null) return null;

					var settlement = a.LocalizedSettlement ?? a.Settlement;

					return settlement.No() ? a.Code : $"{settlement} ({a.Code})";
				};
			else
				airportToString = a => a?.Code;


			foreach (var seg in segments)
			{
				var airportString = airportToString(seg.FromAirport) ?? seg.FromAirportCode;

				var fromAirportCode = seg.FromAirport?.Code ?? seg.FromAirportCode;

				if (lastAirportCode.No() || airportString.Yes() && lastAirportCode != fromAirportCode)
				{
					if (lastAirportCode.Yes() && lastAirportCode != fromAirportCode)
						separator = withSpaces ? "; " : ";";

					sb += separator;
					sb += airportString;

					separator = withSpaces ? " - " : "-";
				}

				airportString = airportToString(seg.ToAirport) ?? seg.ToAirportCode;

				if (airportString.Yes())
				{
					sb += separator;
					sb += airportString;
				}

				lastAirportCode = seg.ToAirport?.Code ?? seg.ToAirportCode;
			}

			if (withDates)
			{
				var departure = Segments.OneAs(a => a.DepartureTime) ?? Departure;

				if (departure != null)
				{
					sb += ", ";
					sb += departure.ToDateString();

					var arrival = Segments.OrderByDescending(a => a.Position).OneAs(a => a.ArrivalTime);

					if (arrival != null && arrival.Value.Date != departure.Value.Date)
					{
						sb += " - ";
						sb += arrival.ToDateString();
					}
				}
			}

			return sb;
		}
	}


	partial class Domain
	{
		public DbSet<AviaTicket> AviaTickets { get; set; }
	}

}