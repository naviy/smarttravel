using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;




namespace Luxena.Travel.Domain
{



	//===g






	partial class AviaTicket
	{

		//---g



		public override Entity Resolve(Domain db)
		{

			base.Resolve(db);

			var r = this;


			r.ReissueFor = db.AviaDocument.ByFullNumber(r.ReissueFor);
			
			r.FareTotal += db;

			Segments.ForEach(a => a.Resolve(db));


			r.CalculateFares();


			if (r.Origin == ProductOrigin.AmadeusAir && r.IsManual)
				r.Origin = ProductOrigin.AmadeusPrint;


			r.SetDomestic();


			if (r.Origin == ProductOrigin.AmadeusPrint)
				db.AviaDocument.ResolvePrintDocumentCommission(r);


			if (r.OriginalDocument == null)
				return r;


			var reservations = db.AviaTicket.FindReservation(r);

			if (reservations == null || reservations.Count != 1)
				return r;


			var reservation = reservations[0];
			
			r = db.Unproxy(r);

			db.AviaTicket.UpdateReservation(reservation, r);

			db.AviaTicket.Save(reservation);


			reservation.Order?.Recalculate(db);


			return reservation;

		}




		//---g




		public new class Service : Service<AviaTicket>
		{

			//---g



			public IList<AviaTicket> FindReservation(AviaTicket ticket)
			{

				var passenger = ticket.Passengers.One(a => a.PassengerName);

				if (passenger.No()) 
					return null;


				var passengerMatch = _reFindReservation.Match(passenger);

				if (passengerMatch.Yes())
					passenger = passengerMatch.Groups["passenger"].Value;// + "%";


				var list = ListBy(a =>
					a.PnrCode == ticket.PnrCode &&
					a.AirlinePrefixCode == ticket.AirlinePrefixCode &&
					a.Number == null &&
					!a.IsVoid &&
					a.Passengers.FirstOrDefault(b =>
						passengerMatch.Yes() ? b.PassengerName.StartsWith(passenger) : b.PassengerName == passenger
					) != null
				);


				if (list?.Count > 1 && ticket.EqualFare.Yes())
					list = list.Where(a => Equals(a.EqualFare, ticket.EqualFare)).ToList();


				return list;

			}



			private static readonly Regex _reFindReservation = new Regex(@"(?<passenger>.*[^\s])\s?(MR|MRS|MSTR|MISS)$", RegexOptions.Compiled);



			//---g



			public void UpdateReservation(AviaTicket reservation, AviaTicket ticket)
			{

				reservation.IssueDate = ticket.IssueDate;
				reservation.Number = ticket.Number;
				reservation.PassengerName = ticket.PassengerName;
				reservation.ConjunctionNumbers = ticket.ConjunctionNumbers;

				reservation.Producer = ticket.Producer;
				reservation.AirlineIataCode = ticket.AirlineIataCode;
				reservation.AirlinePrefixCode = ticket.AirlinePrefixCode;
				reservation.AirlineName = ticket.AirlineName;

				reservation.PaymentType = ticket.PaymentType;
				reservation.PaymentForm = ticket.PaymentForm;
				reservation.PaymentDetails = ticket.PaymentDetails;

				reservation.Booker = ticket.Booker;
				reservation.BookerOffice = ticket.BookerOffice;
				reservation.BookerCode = ticket.BookerCode;

				reservation.Ticketer = ticket.Ticketer;
				reservation.TicketerOffice = ticket.TicketerOffice;
				reservation.TicketingIataOffice = ticket.TicketingIataOffice;
				reservation.TicketerCode = ticket.TicketerCode;

				reservation.Seller = ticket.Seller;

				reservation.Owner = ticket.Owner;

				reservation.Originator = ticket.Originator;
				reservation.Origin = ticket.Origin;
				reservation.OriginalDocument = ticket.OriginalDocument;

				reservation.AirlinePnrCode = ticket.AirlinePnrCode;
				reservation.TourCode = ticket.TourCode;

				reservation.Departure = ticket.Departure;
				reservation.Itinerary = ticket.Itinerary;
				reservation.SegmentClasses = ticket.SegmentClasses;
				reservation.ReissueFor = ticket.ReissueFor;

				reservation.Fare = ticket.Fare;
				reservation.EqualFare = ticket.EqualFare;
				reservation.Vat = ticket.Vat;
				reservation.FeesTotal = ticket.FeesTotal;
				reservation.Commission = ticket.Commission;
				reservation.CommissionPercent = ticket.CommissionPercent;

				reservation.Domestic = ticket.Domestic;
				reservation.Interline = ticket.Interline;


				if (!Equals(reservation.Total, ticket.Total))
				{
					//reservation.Total = ticket.Total;

					reservation.ServiceFee = null;
					reservation.Discount = null;
					reservation.GrandTotal = null;
				}


				if (ticket.Customer != null)
					reservation.SetCustomer(db, ticket.Customer);


				if (ticket.ServiceFee != null)
					reservation.ServiceFee = ticket.ServiceFee;


				if (ticket.Discount != null)
					reservation.Discount = ticket.Discount;


				//if (ticket.GrandTotal != null)
				//	reservation.GrandTotal = ticket.GrandTotal;


				reservation.Total = reservation.GetTotal();
				reservation.GrandTotal = reservation.GetGrandTotal();

				CopyFees(reservation, ticket);
				CopySegments(reservation, ticket);

			}



			private static void CopyFees(AviaTicket reservation, AviaTicket ticket)
			{

				for (var i = 0; i < ticket.Fees.Count; i++)
				{

					var fee = ticket.Fees[i];


					if (reservation.Fees.Count <= i)
					{
						var clone = (AviaDocumentFee)fee.Clone();
						clone.Document = reservation;

						reservation.Fees.Add(clone);
					}
					else
					{
						AviaDocumentFee.Copy(fee, reservation.Fees[i]);
					}

				}


				while (reservation.Fees.Count > ticket.Fees.Count)
				{
					reservation.Fees.RemoveAt(reservation.Fees.Count - 1);
				}

			}



			private static void CopySegments(AviaTicket reservation, AviaTicket ticket)
			{

				for (var i = 0; i < ticket.Segments.Count; i++)
				{

					var segment = ticket.Segments[i];


					if (reservation.Segments.Count <= i)
					{
						var clone = (FlightSegment)segment.Clone();
						clone.Ticket = reservation;

						reservation.AddSegment(clone);
					}
					else
					{
						FlightSegment.Copy(segment, reservation.Segments[i]);
					}

				}


				while (reservation.Segments.Count > ticket.Segments.Count)
				{
					reservation.Segments.RemoveAt(reservation.Segments.Count - 1);
				}

			}


		}

	}

}