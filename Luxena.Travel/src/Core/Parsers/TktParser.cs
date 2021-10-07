using System;

using Luxena.Base.Domain;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{
	public class TktParser
	{
		private TktParser(string tkt)
		{
			_data = tkt.Replace(",\",", "\",").Split(',');
		}

		public static Entity2 Parse(string tkt)
		{
			if (tkt.No())
				throw new GdsImportException("Empty TKT");

			return new TktParser(tkt).Parse();
		}

		private Entity2 Parse()
		{
			var type = GetString(7);

			if (type == TicketTypeSale)
				return ParseTicket();

			if (type == TicketTypeRefund)
				return ParseRefund();
			
			if (type == TicketTypeVoid)
				return ParseVoid();

			throw new Exception(string.Format("Unsupported type of document: \"{0}\"", type));
		}

		private AviaTicket ParseTicket()
		{
			var ticket = new AviaTicket();

			ParseDocument(ticket, Positions.Ticket);

			ticket.IssueDate = GetDate(3).Value;

			ticket.BookerCode = GetString(9);
			ticket.TicketerCode = ticket.BookerCode;

			ParseSegments(ticket, Positions.Ticket);

			return ticket;
		}

		private AviaRefund ParseRefund()
		{
			var refund = new AviaRefund();

			ParseDocument(refund, Positions.Refund);

			refund.IssueDate = GetDate(92).Value;

			refund.BookerCode = GetString(93);
			refund.TicketerCode = refund.BookerCode;

			return refund;
		}

		private AviaDocumentVoiding ParseVoid()
		{
			var voiding = new AviaDocumentVoiding();

			voiding.Document = new AviaTicket
			{
				AirlinePrefixCode = GetString(0),
				Number = GetString(1)
			};

			voiding.IsVoid = true;

			voiding.TimeStamp = GetDate(3).Value;

			voiding.Originator = GdsOriginator.Galileo;
			voiding.Origin = ProductOrigin.GalileoTkt;
			voiding.AgentCode = GetString(9);

			return voiding;
		}

		private void ParseDocument(AviaDocument document, Positions positions)
		{
			document.AirlinePrefixCode = GetString(0);
			document.Number = GetString(1);

			document.PassengerName = GetString(73);

			document.TourCode = GetString(51);

			string originator = null;

			var pnr = GetString(169);

			if (pnr != null)
			{
				var pnrParts = pnr.Split('/');

				if (pnrParts.Length == 2)
				{
					originator = pnrParts[0].Length == 2 ? pnrParts[0] : pnrParts[1];
					document.PnrCode = pnrParts[0].Length == 2 ? pnrParts[1] : pnrParts[0];
				}
				else
					document.PnrCode = pnrParts[0];
			}

			document.Origin = ProductOrigin.GalileoTkt;

			if (originator == "1A")
				document.Originator = GdsOriginator.Amadeus;
			else if (originator == "1G")
				document.Originator = GdsOriginator.Galileo;
			else
				document.Originator = GdsOriginator.Unknown;

			_originalCurrency = GetString(positions.OriginalCurrencyPos);

			document.Fare = GetMoney(positions.NotPublishedFareOriginalPos);

			if (document.Fare == null)
				document.Fare = GetMoney(positions.FareOriginalPos);

			document.FeesTotal = new Money(_originalCurrency, 0);

			for (var i = 0; i < FeeMaxCount; i++)
			{
				var code = GetString(positions.FeePos + 2*FeeMaxCount + i);

				if (code == null)
					break;

				document.AddFee(new AviaDocumentFee
				{
					Code = code,
					Amount = GetMoney(positions.FeePos + FeeMaxCount + i)
				});
			}

			document.Total = GetMoney(76);

			document.CommissionPercent = GetDecimal(72);

			if (document.CommissionPercent.HasValue)
				document.Commission = (document.Fare * document.CommissionPercent.Value) / 100;

			document.PaymentForm = GetString(53);

			if (document.PaymentForm == PaymentTypeInvoice)
				document.PaymentType = PaymentType.Invoice;
			else if (document.PaymentForm == PaymentTypeCash)
				document.PaymentType = PaymentType.Cash;
			else if (document.PaymentForm == PaymentTypeCreditCard)
				document.PaymentType = PaymentType.CreditCard;
		}

		private void ParseSegments(AviaTicket ticket, Positions positions)
		{
			for (var i = 0; i < SegmentMaxCount - 1; i++)
			{
				var toAirport = GetString(positions.SegmentPos + i + 1);

				if (toAirport == null)
					break;

				var segment = new FlightSegment
				{
					FromAirportCode = GetString(positions.SegmentPos + i),
					ToAirportCode = toAirport,
					ServiceClassCode = GetString(171)
				};

				segment.FromAirport = new Airport
				{
					Code = segment.FromAirportCode
				};

				segment.ToAirport = new Airport
				{
					Code = toAirport
				};

				if (i == 0)
				{
					segment.CarrierIataCode = GetString(166);
					segment.FlightNumber = GetString(170);
					segment.DepartureTime = GetDate(167);
					segment.FareBasis = GetString(174);
				}

				segment.Position = i + 1;

				ticket.AddSegment(segment);
			}
		}

		private Money GetMoney(int pos)
		{
			var amount = GetDecimal(pos);

			if (amount.HasValue)
				return new Money(_originalCurrency, amount.Value);

			return null;
		}

		private string GetString(int pos)
		{
			var s = _data[pos];

			if (s[0] == '"' && s[s.Length - 1] == '"')
				return s.Substring(1, s.Length - 2).TrimOrNull();

			return s.TrimOrNull();
		}

		private DateTime? GetDate(int pos)
		{
			var value = GetString(pos);

			if (value == null)
				return null;

			return _zeroDay.AddDays(int.Parse(value));
		}

		private decimal? GetDecimal(int pos)
		{
			var value = GetString(pos);

			if (value == null)
				return null;

			return Utility.ParseDecimal(value);
		}

		private const string TicketTypeSale = "SALE";
		private const string TicketTypeVoid = "VOID";
		private const string TicketTypeRefund = "REFUND";

		private const string PaymentTypeInvoice = "In";
		private const string PaymentTypeCash = "Ca";
		private const string PaymentTypeCreditCard = "CC";

		private const int FeeMaxCount = 12;
		private const int SegmentMaxCount = 12;

		private static DateTime _zeroDay = new DateTime(1800, 12, 28);

		private readonly string[] _data;

		private string _originalCurrency;

		private class Positions
		{
			public static readonly Positions Ticket = new Positions
			{
				OriginalCurrencyPos = 78,
				NotPublishedFareOriginalPos = 12,
				FareOriginalPos = 13,
				FeePos = 15,
				SegmentPos = 54
			};

			public static readonly Positions Refund = new Positions
			{
				OriginalCurrencyPos = 146,
				FareOriginalPos = 107,
				FeePos = 110,
				SegmentPos = 94
			};

			private Positions()
			{
			}

			public int OriginalCurrencyPos;
			public int NotPublishedFareOriginalPos;
			public int FareOriginalPos;
			public int FeePos;
			public int SegmentPos;
		}
	}
}