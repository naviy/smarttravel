using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{


	public class TravelPointXmlParser
	{

		public static IEnumerable<Entity2> Parse(string content, Currency defaultCurrency, string[] robotCodes)
		{
			if (content.No())
				throw new GdsImportException("Empty TravelPointXml");

			return new TravelPointXmlParser(defaultCurrency, robotCodes).Parse(content);
		}

		public static IEnumerable<Entity2> Parse(XElement xml, Currency defaultCurrency, string[] robotCodes)
		{
			if (xml == null)
				throw new GdsImportException("Empty TravelPointXml");

			return new TravelPointXmlParser(defaultCurrency, robotCodes).Parse(xml);
		}

		public TravelPointXmlParser(Currency defaultCurrency, string[] robotCodes)
		{
			_defaultCurrency = defaultCurrency;
			_robotCodes = robotCodes ?? new string[0];
		}


		private IEnumerable<Entity2> Parse(string content)
		{
			var xticket = XDocument.Parse(content).Root;
			if (xticket == null) return null;

			var tickets = Parse(xticket);
			return tickets.ToArray();

			//var name = xml.Name.LocalName.ToLower();

			//if (name == "ticket" || name == "mco" || name == "emd") 
			//	return Parse(new[] { xml });

			//var tickets = Parse(xml.Els("Tickets", "Ticket"));
			//var mcos = Parse(xml.Els("MCOs", "MCO"));
			//var emds = Parse(xml.Els("MCOs", "EMD"));

			//return tickets.Union(mcos).Union(emds).ToArray();
		}


		//private IEnumerable<Entity2> Parse(IEnumerable<XElement> xmls)
		//{
		//	return xmls.SelectMany(Parse);
		//}


		private IEnumerable<Entity2> Parse(XElement xml)
		{
			var name = xml.Name.LocalName.ToLower();
			var status = xml.Value("status")?.ToLower();

			//if (name == "mco" || name == "emd")
			//{
			//	yield return ParseMCO(xml);
			//}
			//else 
			if (name == "avia-ticket")
			{
				if (status == "issue" || status == "reissue" || status == "void")
				{
					yield return ParseAviaTicket(xml);

					if (status == "void")
						yield return ParseVoid(xml);
				}
				//else if (status == "refund")
				//	yield return ParseRefund(xml);
				//else
				//throw new NotImplementedException();
			}
			//else
			//throw new NotImplementedException();

		}


		private TAviaDocument ParseAviaDocument<TAviaDocument>(XElement x)
			where TAviaDocument : AviaDocument, new()
		{
			var r = new TAviaDocument();

			r.Originator = GdsOriginator.TravelPoint;
			r.Origin = ProductOrigin.TravelPointXml;

			r.IssueDate = x.Value("issuing-date").As().DateTime.Date;

			x.Value("ticket-number").Split('-').Do(a =>
			{
				r.AirlinePrefixCode = a.By(0);
				r.Number = a.By(1);
			});

			r.PnrCode = x.Value("booking-number");
			r.AirlinePnrCode = x.Value("AirRecLoc")?.Split('-').As(a => a.By(1) ?? a.By(0));

			x.Attr("reissue-old-no")?.Split('-').Do(a =>
			{
				r.ReissueFor = new AviaTicket
				{
					AirlinePrefixCode = a.By(0),
					Number = a.By(1),
				};
			});

			r.TicketerOffice = x.Value("office-id-ticketing");
			r.TicketerCode = x.Value("agent-id-ticketing");

			r.BookerCode = x.Value("agent-id-booking");
			if (r.BookerCode.Yes())
				r.BookerOffice = r.TicketerOffice;

			x.Value("carrier").Do(code =>
			{
				r.Producer = new Organization { AirlineIataCode = code };
				r.AirlineIataCode = code;
			});


			r.PassengerName = x.Value("passenger");

			r.PaymentForm = x.Value("form-of-payment");

			r.TicketingIataOffice = x.Value("iata-agency-code");


			r.TourCode = x.Value("tour-code");
			r.Itinerary = x.Value("route");

			_currency = x.Value("currency");

			r.Fare = NewMoney(x, "fare-eq");

			r.EqualFare = NewMoney(x, "fare-eq");
			r.FeesTotal = NewMoney(x, "taxes");
			r.Total = NewMoney(x, "total-amount");

			if (r.EqualFare.No() && r.Total.Yes())
				r.EqualFare = r.Total - r.FeesTotal;

			r.CancelFee = NewMoney(x, "cancellation-fee");
			r.ServiceFee = NewMoney(x, "sf-total");
			r.Commission = NewMoney(x, "our-commission");

			return r;
		}


		private Entity2 ParseAviaTicket(XElement xticket)
		{
			var r = ParseAviaDocument<AviaTicket>(xticket);

			xticket.Els("air-segments", "air-segment").ForEach(seg => r.AddSegment(new FlightSegment
			{
				Position = seg.Attr("no").As().Int,
				Type = FlightSegmentType.Ticketed,

				CarrierIataCode = seg.Value("carrier"),
				FlightNumber = seg.Value("number"),
				ServiceClassCode = seg.Value("air-class"),

				DepartureTime = $"{seg.Value("departure-date")}T{seg.Value("departure-time")}".As().DateTimen,
				FromAirportCode = seg.Value("origin-airport-code"),
				CheckInTerminal = seg.Value("terminal-check-in"),

				ArrivalTime = $"{seg.Value("arrival-date")}T{seg.Value("arrival-time")}".As().DateTimen,
				ToAirportCode = seg.Value("destination-airport-code"),
				ArrivalTerminal = seg.Value("terminal-arrival"),

				Duration = seg.Value("flight-duration-time").As().Intn().As(a => $"{a / 100}:{a % 100:00}"),
				Luggage = seg.Value("baggage"),
				MealCodes = seg.Value("meal"),
				FareBasis = seg.Value("fare-basis"),
			}));

			ResolveAviaDoument(r);

			return r;
		}


		private void ResolveAviaDoument(AviaDocument r)
		{
			r.PaymentType =
				r.PaymentForm == "CASH" ? PaymentType.Cash :
				r.PaymentForm == "CHEQUE" ? PaymentType.Check :
				r.PaymentForm == "CREDIT CARD" ? PaymentType.CreditCard :
				r.PaymentForm == "INVOICE" ? PaymentType.Invoice :
				PaymentType.Unknown;

			r.Total = r.GetTotal();
			r.GrandTotal = r.GetGrandTotal();
		}


		//private Entity2 ParseRefund(XElement xrefund)
		//{
		//	var r = new AviaRefund();

		//	var h = ParseProduct(xrefund, r);
		//	r.IssueDate = h.Attr("History", "Refund").As().DateTime.Date;

		//	var hr = h.El("TicketRefund");
		//	var curr = hr.Attr("Curr");

		//	r.RefundServiceFee = NewMoney(curr, hr.Value("Comission"));
		//	r.ServiceFeePenalty = NewMoney(curr, hr.Value("CancellationFee"));

		//	ResolveAviaDoument(r);

		//	return r;
		//}


		//private Entity2 ParseMCO(XElement x)
		//{
		//	var r = new AviaMco();

		//	r.Originator = GdsOriginator.TravelPoint;
		//	r.Origin = ProductOrigin.TravelPointXml;

		//	x.Attr("No")?.Split('-').Do(a =>
		//	{
		//		r.AirlinePrefixCode = a.By(0);
		//		r.Number = a.By(1);
		//	});

		//	r.IssueDate = x.Value("Date").As().DateTime.Date;
		//	r.AirlineIataCode = x.Value("Carrier");

		//	r.Producer = new Organization
		//	{
		//		AirlineIataCode = r.AirlineIataCode,
		//		AirlinePrefixCode = r.AirlinePrefixCode,
		//	};

		//	x.El("Passanger").Do(a =>
		//		r.PassengerName =
		//			a.Attr("LastName")?.ToUpper() +
		//			a.Attr("FirstName").As(s => "/" + s.ToUpper()) +
		//			a.Attr("Title").As(s => " " + s.ToUpper())
		//	);

		//	r.BookerOffice = x.Value("BookingOfficeTitle");
		//	r.TicketerOffice = x.Value("TktOfficeTitle");

		//	r.TicketerCode = x.Value("TktAgentTitle") ?? x.Attr("SignInHistory", "SignIn", "Value");
		//	//r.BookerCode = xml.Value("BookingAgentTitle") ?? r.TicketerCode;

		//	if (_robotCodes.Any(a => a == r.TicketerOffice + r.TicketerCode))
		//	{
		//		r.TicketerOffice = r.BookerOffice;
		//		r.TicketerCode = r.BookerCode;
		//	}


		//	r.TicketingIataOffice = x.Value("IATA");

		//	r.PnrCode = x.Value("RecordLocator");
		//	r.AirlinePnrCode = x.Value("ACRecLoc")?.Split('-').As(a => a.By(1) ?? a.By(0));

		//	r.Itinerary = x.Value("Routing");


		//	_currency = x.Value("currency");

		//	r.Fare = NewMoney(x, "Fare");
		//	r.EqualFare = NewMoney(curr, x.Value("fare-eq"));

		//	x.El("ServiceFee").Do(a =>
		//	{
		//		r.ServiceFee = NewMoney(curr, a.Attr("Main"));
		//		r.Vat = NewMoney(curr, a.Attr("VAT"));
		//	});

		//	r.Discount = NewMoney(curr, x.Value("Discount"));

		//	r.CommissionPercent = x.Value("Comission").As().Decimaln;// / 100m;
		//	r.Commission = NewElMoney(x, "CommisAmt");

		//	r.PaymentForm = x.Value("FormatPayment");

		//	x.El("RemarkValues").Do(a =>
		//	{
		//		r.Note = 
		//			a.Value("Remark") +
		//			a.Value("Remark1").As(s => "\r\n" + s) +
		//			a.Value("Remark2").As(s => "\r\n" + s) +
		//			a.Value("Remark3").As(s => "\r\n" + s);
		//	});

		//	ResolveAviaDoument(r);

		//	return r;
		//}


		private Entity2 ParseVoid(XElement x)
		{

			var r = new AviaDocumentVoiding
			{
				Originator = GdsOriginator.TravelPoint,
				Origin = ProductOrigin.TravelPointXml,
				IsVoid = true,
				TimeStamp = x.Value("Created").As().DateTime,

				IataOffice = x.Value("iata-agency-code"),
				AgentOffice = x.Value("office-id-ticketing"),
				AgentCode = x.Value("agent-id-ticketing"),

				Document = x.Value("ticket-number").Split('-').As(a => new AviaTicket
				{
					AirlinePrefixCode = a.By(0),
					Number = a.By(1),
				}),
			};

			//if (_robotCodes.Any(a => a == r.AgentOffice + r.AgentCode))
			//{
			//	r.AgentOffice = h.Attr("OfficeID", "Booking");
			//	r.AgentCode = xml.Attr("SignInBooking").Right(2);
			//}

			return r;
		}


		#region Utils

		private Money NewMoney(XElement x, string name)
		{
			var dvalue = x.Value(name).As().Decimaln;
			return dvalue == null ? null : new Money(_currency ?? _defaultCurrency?.Code, dvalue.Value);
		}

		private Money NewDefaultMoney(string value)
			=> new Money(_defaultCurrency, value.As().Decimal);

		#endregion


		private readonly string[] _robotCodes;
		private readonly Currency _defaultCurrency;
		private string _currency;
	}


}