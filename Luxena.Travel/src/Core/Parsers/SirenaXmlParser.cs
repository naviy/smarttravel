using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Luxena.Base.Domain;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{
	public class SirenaXmlParser
	{
		public static IList<Entity2> Parse(string xmlDocument)
		{
			return new SirenaXmlParser().Parse(XDocument.Parse(xmlDocument));
		}

		private IList<Entity2> Parse(XDocument document)
		{
			var entities = new List<Entity2>();

			if (document.Root == null)
				return entities;

			foreach (var element in document.Root.Elements())
			{
				var entity = Parse(element);

				if (entity != null)
					entities.Add(entity);
				else
					throw new GdsImportException("No avia documents");
			}

			return entities;
		}

		private Entity2 Parse(XElement ticketEl)
		{
			_ticketEl = ticketEl;

			var entity = Create();

			if (entity == null)
				return null;

			if (entity is AviaDocumentVoiding)
			{
				_voiding = (AviaDocumentVoiding) entity;

				ParseVoiding();

				return _voiding;
			}

			_document = (AviaDocument) entity;
			_ticket = _document as AviaTicket;

			_nfare = 0;
			_fare = 0;
			_vat = 0;

			ParseAviaDocument();

			ParseFees();

			ParsePayments();

			ParseSegments();

			SetFinanceData();

			return entity;
		}

		private Entity2 Create()
		{
			_currentEl = _ticketEl;

			var opType = Read("OPTYPE");
			var transType = Read("TRANS_TYPE");
			var mcoType = Read("MCO_TYPE");

			var link = _documentTypeMap.FirstOrDefault(l => l.Match(opType, transType, mcoType));

			if (link == null)
				throw new GdsImportException(string.Format("Unknown document type"));

			return link.CreateInstance();
		}

		private void ParseVoiding()
		{
			_voiding.IsVoid = true;

			_voiding.TimeStamp = DateTime.ParseExact(Read("DEALDATE") + Read("DEALTIME"), "ddMMyyyyHHmm", CultureInfo.InvariantCulture);

			_voiding.Originator = GdsOriginator.Sirena;
			_voiding.Origin = ProductOrigin.SirenaXml;

			_voiding.AgentCode = Read("DISP");

			var ticket = new AviaTicket
			{
				AirlinePrefixCode = Read<string>("GENERAL_CARRIER"),
				Number = ParseNumber()
			};

			ticket.AddVoiding(_voiding);
		}

		private void ParseAviaDocument()
		{
			_currentEl = _ticketEl;

			_currencyCode = Read("CURRENCY");
			_ncurrencyCode = Read("NCURRENCY");

			_document.Origin = ProductOrigin.SirenaXml;
			_document.Originator = GdsOriginator.Sirena;

			_document.IssueDate = DateTime.ParseExact(Read("DEALDATE") + Read("DEALTIME"), "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
			_document.AirlinePrefixCode = Read<string>("GENERAL_CARRIER");

			_document.Number = ParseNumber();
			_document.ConjunctionNumbers = Read("CONJ");
			_document.PassengerName = Read("FIO");
			_document.GdsPassport = Read("PASS");

			var element = _ticketEl.Element("BOOK");

			if (element != null)
			{
				_document.BookerOffice = ReadAttribute("agency", element);
				_document.BookerCode = ReadAttribute("disp", element);
			}

			_document.TicketerOffice = Read("SALEAG");
			_document.TicketingIataOffice = Read("SALESTAMP");
			_document.TicketerCode = Read("DISP");
			_document.PnrCode = Read("PNR_LAT");

			ParseCommission(_document);
		}

		private string ParseNumber()
		{
			const string pattern = @"^(\d{3}\s|\w*?)(?<number>\d+)$";

			var match = Regex.Match(Read("BSONUM"), pattern);

			var str = match.Groups["number"].Value;

			if (str.No())
				throw new GdsImportException("Document number not found");

			return str;
		}

		private void ParseCommission(AviaDocument document)
		{
			var element = _ticketEl.Element("COMISSION");

			if (element == null)
				return;

			var type = ReadAttribute("type", element);

			if (type == "fixed")
			{
				document.Commission = new Money(ReadAttribute("currency", element), Utility.ParseDecimal(ReadAttribute("amount", element)));
			}
			else if (type == "percent" & document.EqualFare != null)
			{
				document.CommissionPercent = Utility.ParseDecimal(ReadAttribute("rate", element));

				var amount = Math.Round(document.EqualFare.Amount*document.CommissionPercent.Value/100, 2);

				document.Commission = new Money(_currencyCode, amount);
			}
		}

		private void ParseSegments()
		{
			var element = _ticketEl.Element("SEGMENTS");

			if (element == null)
				return;

			foreach (var segmentEl in element.Elements("SEGMENT"))
			{
				_currentEl = segmentEl;

				ParseSegment();
			}

			ParseVat(element);
		}

		private void ParseSegment()
		{
			var segmentType = Read("IS_VOID") == "F" ? FlightSegmentType.Ticketed : FlightSegmentType.Voided;

			if (segmentType == FlightSegmentType.Ticketed)
			{
				_fare += ReadDecimal("FARE");
				_nfare += ReadDecimal("NFARE");
			}

			if (_ticket == null)
				return;

			var segment = new FlightSegment
			{
				Position = Read<int>("SEGNO"),
				Type = segmentType,
				FromAirportCode = Read("PORT1CODE")
			};

			if (segment.FromAirportCode.Yes())
				segment.FromAirport = new Airport
				{
					Code = segment.FromAirportCode,
					Name = Read("PORT1NAME"),
					Settlement = Read("CITY1CODE")
				};


			segment.ToAirportCode = Read("PORT2CODE");

			if (segment.ToAirportCode.Yes())
				segment.ToAirport = new Airport
				{
					Code = segment.ToAirportCode,
					Name = Read("PORT2NAME"),
					Settlement = Read("CITY2CODE")
				};

			segment.CarrierIataCode = Read("CARRIER");

			segment.FlightNumber = Read("REIS");
			segment.ServiceClassCode = Read("CLASS");

			segment.DepartureTime = DateTime.ParseExact(Read("FLYDATE") + Read("FLYTIME"), "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
			segment.ArrivalTime = DateTime.ParseExact(Read("ARRDATE") + Read("ARRTIME"), "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
			segment.CheckInTerminal = Read("TERM1");
			segment.ArrivalTerminal = Read("TERM2");

			segment.FareBasis = Read("BASICFARE");

			segment.Stopover = Read("STPO") == "0";

			_ticket.AddSegment(segment);
		}

		private void ParseFees()
		{
			var element = _ticketEl.Element("TAXES");

			if (element == null)
				return;

			_document.FeesTotal = new Money(_currencyCode, 0);

			foreach (var taxEl in element.Elements("TAX"))
			{
				_currentEl = taxEl;

				ParseFee();
			}

			ParseVat(element);
		}

		private void ParseFee()
		{
			var fee = new AviaDocumentFee
			{
				Code = Read("CODE"),
				Amount = new Money(_currencyCode, ReadDecimal("AMOUNT"))
			};

			_document.AddFee(fee);

			ParseVat(_currentEl);
		}

		private void ParsePayments()
		{
			_total = 0;

			var element = _ticketEl.Element("FOPS");

			if (element == null)
				return;

			foreach (var paymentEl in element.Elements("FOP"))
				ParsePayment(paymentEl);
		}

		private void ParsePayment(XElement paymentEl)
		{
			_currentEl = paymentEl;

			if (_document.PaymentForm.No())
			{
				var paymentForm = Read("TYPE");

				_document.PaymentForm = paymentForm;

				if (paymentForm.Yes() && _paymentTypeMap.ContainsKey(paymentForm))
					_document.PaymentType = _paymentTypeMap[paymentForm];
			}

			_total += ReadDecimal("AMOUNT");
		}

		private void ParseVat(XElement element)
		{
			var amount = ReadAttribute("vat_amount", element);

			if (amount != null)
				_vat += Utility.ParseDecimal(amount);
		}

		private void SetFinanceData()
		{
			_document.Fare = new Money(_ncurrencyCode, _nfare);
			_document.EqualFare = new Money(_currencyCode, _fare);
			_document.Total = new Money(_currencyCode, _total);

			if (_vat > 0)
				_document.Vat = new Money(_currencyCode, _vat);

			if (_document is AviaRefund)
			{
				var refund = _document as AviaRefund;

				var cancelFee = refund.EqualFare + refund.FeesTotal - refund.Total;

				if (cancelFee != null && cancelFee.Amount > 0)
					refund.CancelFee = cancelFee;
			}
		}

		private T Read<T>(string name)
		{
			var value = Read(name);

			if (value == null)
				return default(T);

			return (T) Convert.ChangeType(value, typeof (T));
		}

		private decimal ReadDecimal(string name)
		{
			var value = Read(name);

			return value == null ? 0 : Utility.ParseDecimal(value);
		}

		private string Read(string name)
		{
			var el = _currentEl.Element(name);

			if (el == null)
				return null;

			return el.Value.No() ? null : el.Value;
		}

		private static string ReadAttribute(string name, XElement element)
		{
			var attribute = element.Attribute(name);

			if (attribute == null)
				return null;

			return attribute.Value.No() ? null : attribute.Value;
		}

		private class TypeLink
		{
			public TypeLink(string opType, string transType, string mcoType, Type type)
			{
				_opType = opType;
				_transType = transType;
				_mcoType = mcoType;
				_type = type;
			}

			public bool Match(string opType, string transType, string mcoType)
			{
				mcoType = mcoType == string.Empty ? null : mcoType;

				return _opType == opType && _transType == transType && _mcoType == mcoType;
			}

			public Entity2 CreateInstance()
			{
				if (_type == null)
					return null;

				return (Entity2)Activator.CreateInstance(_type);
			}

			private readonly string _opType;
			private readonly string _transType;
			private readonly string _mcoType;
			private readonly Type _type;
		}

		private readonly IList<TypeLink> _documentTypeMap = new List<TypeLink>
		{
			new TypeLink("SALE", "SALE", null, typeof (AviaTicket)),
			new TypeLink("SALE", "SALE", "PTA", typeof (AviaMco)),
			new TypeLink("REFUND", "CANCEL", null, typeof (AviaDocumentVoiding)),
			new TypeLink("REFUND", "REFUND", null, typeof (AviaRefund)),
			new TypeLink("SALE", "REFUND", "RECEIPT", null),
			new TypeLink("REFUND", "EXCHANGE", null, null),
			new TypeLink("SALE", "EXCHANGE", null, null),
			new TypeLink("SALE", "EXCHANGE", "PENALTY", null),
			new TypeLink("SALE", "REFUND", "PENALTY", null),
			new TypeLink("SALE", "ERASE ", null, null),
		};

		private static readonly Dictionary<string, PaymentType> _paymentTypeMap = new Dictionary<string, PaymentType>
		{
			{ "CA", PaymentType.Cash },
			{ "Õ¿", PaymentType.Cash },
			{ "CC", PaymentType.CreditCard },
			{ "INV", PaymentType.Invoice },
			{ "œ ", PaymentType.Cash } // TODO: what is "œ "?
		};

		private XElement _ticketEl;
		private XElement _currentEl;

		private AviaDocument _document;
		private AviaTicket _ticket;

		private AviaDocumentVoiding _voiding;

		private string _currencyCode;
		private string _ncurrencyCode;

		private decimal _nfare;
		private decimal _fare;
		private decimal _total;
		private decimal _vat;
	}
}