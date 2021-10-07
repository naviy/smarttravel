using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{

	public class GalileoAviaXmlParser
	{

		public static IEnumerable<Entity2> Parse(string content, Currency defaultCurrency, string[] robotCodes)
		{
			if (content.No())
				throw new GdsImportException("Empty GalileoXml");

			return new GalileoAviaXmlParser(defaultCurrency, robotCodes).Parse(content);
		}

		public static IEnumerable<Entity2> Parse(XElement xml, Currency defaultCurrency, string[] robotCodes)
		{
			if (xml == null)
				throw new GdsImportException("Empty GalileoXml");

			return new GalileoAviaXmlParser(defaultCurrency, robotCodes).Parse(xml);
		}

		public GalileoAviaXmlParser(Currency defaultCurrency, string[] robotCodes)
		{
			_defaultCurrency = defaultCurrency;
			_robotCodes = robotCodes ?? new string[0];
		}


		private IEnumerable<Entity2> Parse(string content)
		{
			var xml = XDocument.Parse(content).Root;
			if (xml == null) return null;

			var name = xml.Name.LocalName.ToLower();

			if (name == "ticket" || name == "mco" || name == "emd") 
				return Parse(new[] { xml });

			var tickets = Parse(xml.Els("Tickets", "Ticket"));
			var mcos = Parse(xml.Els("MCOs", "MCO"));
			var emds = Parse(xml.Els("MCOs", "EMD"));

			return tickets.Union(mcos).Union(emds).ToArray();
		}

		private IEnumerable<Entity2> Parse(IEnumerable<XElement> xmls)
		{
			return xmls.SelectMany(Parse);
		}

		private IEnumerable<Entity2> Parse(XElement xml)
		{
			var name = xml.Name.LocalName.ToLower();
			var status = xml.Attr("Status")?.ToLower();

			if (name == "mco" || name == "emd")
			{
				yield return ParseMCO(xml);
			}
			else if (name == "ticket")
			{
				if (status == "issued" || status == "reissued" || status == "void")
				{ 
					yield return ParseTicket(xml);
					
					if (status == "void")
						yield return ParseVoid(xml);
				}
				else if (status == "refund")
					yield return ParseRefund(xml);
				//else
					//throw new NotImplementedException();
			}
			//else
				//throw new NotImplementedException();
			
		}

		private XElement ParseTicketHeader(XElement xticket, AviaDocument r)
		{
			r.Originator = GdsOriginator.Galileo;
			r.Origin = ProductOrigin.GalileoXml;

			var h = xticket.El("TicketHeader");

			xticket.Attr("No").Split('-').Do(a =>
			{
				r.AirlinePrefixCode = a.By(0);
				r.Number = a.By(1);
			});

			xticket.Attr("ReissueNo")?.Split('-').Do(a =>
			{
				r.ReissueFor = new AviaTicket
				{
					AirlinePrefixCode = a.By(0),
					Number = a.By(1),
				};
			});

			r.Producer = new Organization
			{
				AirlineIataCode = h.Value("ValidatingCarrier"),
				AirlinePrefixCode = r.AirlinePrefixCode,
				Name = h.Value("ValidAirlineName"),
			};

			r.AirlineIataCode = r.Producer.AirlineIataCode;

			xticket.El("NE").Do(a =>
				r.PassengerName =
					a.Attr("LastName")?.ToUpper() +
					a.Attr("FirstName").As(s => "/" + s.ToUpper()) +
					a.Attr("Title").As(s => " " + s.ToUpper())
			);

			h.El("OfficeID").Do(a =>
			{
				r.BookerOffice = a.Attr("Booking");
				r.TicketerOffice = a.Attr("Ticketing");
			});

			r.BookerCode = xticket.Attr("SignInBooking").Right(2);
			r.TicketerCode = xticket.Attr("SignInTicketing").Right(2);

			if (_robotCodes.Any(a => a == r.TicketerOffice + r.TicketerCode))
			{
				r.TicketerOffice = r.BookerOffice;
				r.TicketerCode = r.BookerCode;
			}


			r.TicketingIataOffice = h.Value("IATA");

			r.PnrCode = h.Value("RecordLocator");
			r.AirlinePnrCode = h.Value("AirRecLoc")?.Split('-').As(a => a.By(1) ?? a.By(0));

			r.TourCode = h.Value("TourCode");
			r.Itinerary = h.Value("Routing");


			r.Fare = NewElMoney(h, "Rate", "Currency");
			r.EqualFare = NewDefaultMoney(h.Value("RateUAH"));
			//r.Total = NewDefaultMoney(h, "Amount");

			h.El("ServiceFee").Do(a =>
			{
				r.ServiceFee = NewDefaultMoney(a.Attr("Main"));
				r.Vat = NewDefaultMoney(a.Attr("VAT"));
			});

			r.Discount = NewDefaultMoney(h.Value("Discount"));

			h.El("Remarks").Do(a =>
			{
				r.Note =
					a.Value("Remark") +
						a.Value("Remark1").As(s => "\r\n" + s) +
						a.Value("Remark2").As(s => "\r\n" + s) +
						a.Value("Remark3").As(s => "\r\n" + s);
			});

			h.El("Taxes").Do(taxes =>
			{
				foreach (var tax in taxes.Els("Tax"))
				{
					r.AddFee(new AviaDocumentFee
					{
						Code = tax.Attr("TC"),
						Amount = NewMoney(tax)
					});
				}
			});

			r.CommissionPercent = h.Value("Comission").As().Decimaln;// / 100m;
			r.Commission = NewElMoney(h, "CommisAmt");

			r.PaymentForm = h.Value("FP");

			return h;
		}


		private void Resolve(AviaDocument r)
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


		private Entity2 ParseTicket(XElement xticket)
		{
			var r = new AviaTicket();

			var h = ParseTicketHeader(xticket, r);
			r.IssueDate = h.Attr("History", "Ticketing").As().DateTime.Date;

			xticket.Els("AirSegments", "AirSegment").ForEach(seg => r.AddSegment(new FlightSegment
			{
				Position = seg.Attr("No").As().Int,
				Type = FlightSegmentType.Ticketed,
				CarrierIataCode = seg.Value("ServicingCarrier"),
				FlightNumber = seg.Value("FlightNo"),
				ServiceClassCode = seg.Value("AirClass"),

				DepartureTime = seg.El("Departure").As(a => (a.Attr("Date") + " " + a.Attr("Time")).As().DateTimen),
				FromAirportCode = seg.Attr("Board", "Point"),
				CheckInTerminal = seg.Attr("Board", "Terminal"),

				ArrivalTime = seg.El("Arrival").As(a => (a.Attr("Date") + " " + a.Attr("Time")).As().DateTimen),
				ToAirportCode = seg.Attr("Off", "Point"),
				ArrivalTerminal = seg.Attr("Off", "Terminal"),

				Duration = seg.Value("FlightDurationTime").As(a => a.Length != 4 ? a : a.Left(2) + ":" + a.Right(2)),
				Luggage = seg.Value("BaggageAllowance"),
				MealCodes = seg.Value("Meal"),
			}));

			Resolve(r);

			return r;
		}

		private Entity2 ParseRefund(XElement xrefund)
		{
			var r = new AviaRefund();

			var h = ParseTicketHeader(xrefund, r);
			r.IssueDate = h.Attr("History", "Refund").As().DateTime.Date;

			var hr = h.El("TicketRefund");
			var curr = hr.Attr("Curr");

			r.RefundServiceFee = NewMoney(curr, hr.Value("Comission"));
			r.ServiceFeePenalty = NewMoney(curr, hr.Value("CancellationFee"));

			Resolve(r);

			return r;
		}


		private Entity2 ParseMCO(XElement xml)
		{
			var r = new AviaMco();

			r.Originator = GdsOriginator.Galileo;
			r.Origin = ProductOrigin.GalileoXml;

			xml.Attr("No")?.Split('-').Do(a =>
			{
				r.AirlinePrefixCode = a.By(0);
				r.Number = a.By(1);
			});

			r.IssueDate = xml.Value("Date").As().DateTime.Date;
			r.AirlineIataCode = xml.Value("Carrier");

			r.Producer = new Organization
			{
				AirlineIataCode = r.AirlineIataCode,
				AirlinePrefixCode = r.AirlinePrefixCode,
			};

			xml.El("Passanger").Do(a =>
				r.PassengerName =
					a.Attr("LastName")?.ToUpper() +
					a.Attr("FirstName").As(s => "/" + s.ToUpper()) +
					a.Attr("Title").As(s => " " + s.ToUpper())
			);

			r.BookerOffice = xml.Value("BookingOfficeTitle");
			r.TicketerOffice = xml.Value("TktOfficeTitle");

			r.TicketerCode = xml.Value("TktAgentTitle") ?? xml.Attr("SignInHistory", "SignIn", "Value");
			//r.BookerCode = xml.Value("BookingAgentTitle") ?? r.TicketerCode;

			if (_robotCodes.Any(a => a == r.TicketerOffice + r.TicketerCode))
			{
				r.TicketerOffice = r.BookerOffice;
				r.TicketerCode = r.BookerCode;
			}


			r.TicketingIataOffice = xml.Value("IATA");

			r.PnrCode = xml.Value("RecordLocator");
			r.AirlinePnrCode = xml.Value("ACRecLoc")?.Split('-').As(a => a.By(1) ?? a.By(0));

			r.Itinerary = xml.Value("Routing");


			var curr = xml.Value("Currency");

			r.Fare = NewElMoney(xml, "Fare");
			r.EqualFare = NewMoney(curr, xml.Value("TotalAmount"));

			xml.El("ServiceFee").Do(a =>
			{
				r.ServiceFee = NewMoney(curr, a.Attr("Main"));
				r.Vat = NewMoney(curr, a.Attr("VAT"));
			});

			r.Discount = NewMoney(curr, xml.Value("Discount"));

			r.CommissionPercent = xml.Value("Comission").As().Decimaln;// / 100m;
			r.Commission = NewElMoney(xml, "CommisAmt");

			r.PaymentForm = xml.Value("FormatPayment");

			xml.El("RemarkValues").Do(a =>
			{
				r.Note = 
					a.Value("Remark") +
					a.Value("Remark1").As(s => "\r\n" + s) +
					a.Value("Remark2").As(s => "\r\n" + s) +
					a.Value("Remark3").As(s => "\r\n" + s);
			});

			Resolve(r);

			return r;
		}


		private Entity2 ParseVoid(XElement xml)
		{
			var h = xml.El("TicketHeader");

			var r = new AviaDocumentVoiding
			{
				Originator = GdsOriginator.Galileo,
				Origin = ProductOrigin.GalileoXml,
				IsVoid = true, 
				TimeStamp = h.Value("Created").As().DateTime, 

				IataOffice = h.Value("IATA"), 
				AgentOffice = h.Attr("OfficeID", "Ticketing"), 
				AgentCode = xml.Attr("SignInTicketing").Right(2),

				Document = xml.Attr("No").Split('-').As(a => new AviaTicket
				{
					AirlinePrefixCode = a.By(0),
					Number = a.By(1),
				})
			};

			if (_robotCodes.Any(a => a == r.AgentOffice + r.AgentCode))
			{
				r.AgentOffice = h.Attr("OfficeID", "Booking");
				r.AgentCode = xml.Attr("SignInBooking").Right(2);
			}

			return r;
		}


		#region Utils

		private Money NewMoney(string currency, string value)
		{
			var dvalue = value.As().Decimaln;
			return dvalue == null ? null : new Money(currency, dvalue.Value);
		}

		private Money NewMoney(XElement xml, string currencyName = "Curr", string valueName = "Value")
			=> new Money(xml.Attr(currencyName), xml.Attr(valueName).As().Decimal);

		private Money NewElMoney(XElement xml, string elName, string currencyName = "Curr", string valueName = "Value")
			=> NewMoney(xml.El(elName), currencyName, valueName);

		private Money NewDefaultMoney(string value)
			=> new Money(_defaultCurrency, value.As().Decimal);

		#endregion


		private readonly Currency _defaultCurrency;
		private readonly string[] _robotCodes;
	}

}