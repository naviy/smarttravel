// https://developers.drct.aero/docs/api/b3A6MTUyNTY5NzA-generates-a-report




using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Parsers
{



	//===g






	public class DrctXmlParser
	{

		//---g



		public static IEnumerable<Entity2> Parse(string content, Currency defaultCurrency, string[] robotCodes)
		{

			if (content.No())
				throw new GdsImportException("Empty DrctXml");


			return new DrctXmlParser(defaultCurrency, robotCodes).Parse(content);

		}



		public static IEnumerable<Entity2> Parse(XElement xml, Currency defaultCurrency, string[] robotCodes)
		{

			if (xml == null)
				throw new GdsImportException("Empty DrctXml");


			return new DrctXmlParser(defaultCurrency, robotCodes).Parse(xml);

		}



		private DrctXmlParser(Currency defaultCurrency, string[] robotCodes)
		{
			_defaultCurrency = defaultCurrency;
			_robotCodes = robotCodes ?? Array.Empty<string>();
		}



		//---g



		private IEnumerable<Entity2> Parse(string content)
		{

			var xticket = XDocument.Parse(content).Root;

			if (xticket == null)
				return null;


			var tickets = Parse(xticket);

			return tickets.ToArray();

		}



		private IEnumerable<Entity2> Parse(XElement xml)
		{

			var type = xml.Value("type").ToLower();
			var status = xml.Value("status")?.ToLower();


			if (type == "ticket")
			{

				if (status == "issued" || status == "reissued")
				{
					yield return ParseAviaTicket(xml);
				}

				else if (status == "void")
				{
					yield return ParseVoid<AviaTicket>(xml);
				}

				//else if (status == "refund")
				//	yield return ParseRefund(xml);

				else
				{
					throw new NotImplementedException();
				}

			}

			else if (type == "emd")
			{

				if (status == "issued" || status == "reissued")
				{
					yield return ParseMco(xml);
				}

				else if (status == "void")
				{
					yield return ParseVoid<AviaMco>(xml);
				}

				//else if (status == "refund")
				//	yield return ParseRefund(xml);

				else
				{
					throw new NotImplementedException();
				}

			}

			else
			{
				throw new NotImplementedException();
			}

		}



		//---g



		private TAviaDocument ParseAviaDocument<TAviaDocument>(XElement xdoc)
			where TAviaDocument : AviaDocument, new()
		{

			var r = new TAviaDocument();


			r.Originator = GdsOriginator.Drct;
			r.Origin = ProductOrigin.Drct;

			r.IssueDate = xdoc.Value("last_transaction_at").As().DateTime.Date;


			var number = xdoc.Value("number") ?? xdoc.Value("locator");
			var passenger = xdoc.Value("passenger", "last_name") + " " + xdoc.Value("passenger", "first_name");

			r.Number = number + " - " + passenger;


			r.PnrCode = xdoc.Value("locator");

			//x.Attr("reissue-old-no")?.Split('-').Do(a =>
			//{
			//	r.ReissueFor = new AviaTicket
			//	{
			//		AirlinePrefixCode = a.By(0),
			//		Number = a.By(1),
			//	};
			//});

			r.BookerOffice = xdoc.Value("booking_office") ?? xdoc.Value("company", "id");
			r.BookerCode = xdoc.Value("originator", "id");

			r.TicketerOffice = xdoc.Value("ticketing_office") ?? xdoc.Value("company", "id");
			r.TicketerCode = xdoc.Value("originator", "id");


			r.PassengerName = passenger;


			//r.TourCode = x.Value("tour-code");
			//r.Itinerary = x.Value("route");


			r.EqualFare = NewMoney(xdoc, "fare");
			r.Fare = r.EqualFare.Clone();
			r.FeesTotal = NewMoney(xdoc, "taxes");
			r.Total = NewMoney(xdoc, "total_price");

			if (r.EqualFare.No() && r.Total.Yes())
				r.EqualFare = r.Total - r.FeesTotal;

			r.Commission = new Money(r.Total?.Currency, xdoc.Value("commission_equivalent").As().Decimal);


			return r;

		}



		private Entity2 ParseAviaTicket(XElement xticket)
		{

			var r = ParseAviaDocument<AviaTicket>(xticket);


			xticket.Els("segments", "segment").ForEach((seg, index) => r.AddSegment(new FlightSegment
			{

				Position = index + 1,
				Type = FlightSegmentType.Ticketed,

				CarrierIataCode = seg.Value("carrier"),
				FlightNumber = seg.Value("flight_number"),
				ServiceClassCode = seg.Value("flight_class"),

				DepartureTime = seg.Value("departured_at").As().DateTimen,
				FromAirportCode = seg.Value("departure_airport_code"),
				//CheckInTerminal = seg.Value("terminal-check-in"),

				ArrivalTime = seg.Value("arrived_at").As().DateTimen,
				ToAirportCode = seg.Value("arrival_airport_code"),
				//ArrivalTerminal = seg.Value("terminal-arrival"),

				Duration = seg.Value("duration"),
				Luggage = seg.Value("baggage"),
				MealCodes = seg.Value("booking_class"),
				FareBasis = seg.Value("fare_basis"),

			}));


			var carrier = r.Segments.One()?.CarrierIataCode;

			r.Producer = new Organization { AirlineIataCode = carrier };
			r.AirlineIataCode = carrier;


			ResolveAviaDoument(r);


			return r;

		}



		private Entity2 ParseMco(XElement xmco)
		{

			var r = ParseAviaDocument<AviaMco>(xmco);

			
			var iataCode = xmco.Els("segments", "segment").One().Value("carrier");

			r.Producer = new Organization { AirlineIataCode = iataCode };
			r.AirlineIataCode = iataCode;


			ResolveAviaDoument(r);


			return r;

		}



		private void ResolveAviaDoument(AviaDocument r)
		{
			
			//r.PaymentType =
			//	r.PaymentForm == "CASH" ? PaymentType.Cash :
			//	r.PaymentForm == "CHEQUE" ? PaymentType.Check :
			//	r.PaymentForm == "CREDIT CARD" ? PaymentType.CreditCard :
			//	r.PaymentForm == "INVOICE" ? PaymentType.Invoice :
			//	PaymentType.Unknown;


			r.Total = r.GetTotal();
			r.GrandTotal = r.GetGrandTotal();

		}



		private Entity2 ParseVoid<TDocument>(XElement xvoid)
			where TDocument: AviaDocument, new()
		{

			var number = xvoid.Value("number") ?? xvoid.Value("locator");

			var passenger = xvoid.Value("passenger", "last_name") + " " + xvoid.Value("passenger", "first_name");

			var iataCode = xvoid.Els("segments", "segment").One().Value("carrier");


			var r = new AviaDocumentVoiding
			{

				Originator = GdsOriginator.Drct,
				Origin = ProductOrigin.Drct,
				IsVoid = true,
				TimeStamp = xvoid.Value("last_transaction_at").As().DateTime.Date,

				//IataOffice = xvoid.Value("iata-agency-code"),
				
				AgentOffice = xvoid.Value("ticketing_office") ?? xvoid.Value("company", "id"),
				AgentCode = xvoid.Value("originator", "id"),

				Document = new TDocument
				{
					AirlineIataCode = iataCode,
					Number = number + " - " + passenger,
				},

			};


			return r;

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

		//	r.Originator = GdsOriginator.Drct;
		//	r.Origin = ProductOrigin.DrctXml;

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



		//---g



		private Money NewMoney(XElement x, string name)
		{
			var currency = x.Value(name, "currency");
			var dvalue = x.Value(name, "amount").As().Decimaln;

			return dvalue == null ? null : new Money(currency ?? _defaultCurrency?.Code, dvalue.Value);
		}



		//---g



		private readonly string[] _robotCodes;
		private readonly Currency _defaultCurrency;



		//---g

	}






	//===g



}