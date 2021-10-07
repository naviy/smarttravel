using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Base;
using Luxena.Travel.Domain;
using Luxena.Travel.Domain.Avia;
using Luxena.Travel.Reports;

namespace Luxena.Travel.Yana
{
	public class YanaTicketPrinter : ITicketPrinter
	{
		public string MainDataTemplatePath { get; set; }
		public string FinanceDataTemplatePath { get; set; }
		public string SegmentHeaderTemplatePath { get; set; }
		public string SegmentDataTemplatePath { get; set; }

		public string CompanyInfo { get; set; }
		public string IataNumber { get; set; }

		public void Build(Stream stream, IList<AviaTicket> tickets)
		{
			_tickets = tickets;

			_document = new Document(PageSize.A4, 20, 20, 20, 20);

			PdfWriter writer = PdfWriter.GetInstance(_document, stream);

			_document.Open();

			_contentByte = writer.DirectContent;

			GenerateContent();

			_document.Close();
		}

		private void GenerateContent()
		{
			for (int i = 0; i < _tickets.Count; i++)
			{
				if (i != 0)
					_document.NewPage();

				GeneratePrintedTicket(_tickets[i]);
			}
		}

		private void GeneratePrintedTicket(AviaTicket ticket)
		{
			_currentPos = _document.PageSize.Height - _document.TopMargin;

			GenerateMainBlock(ticket);

			GenerateSegmentsBlock(ticket.Segments);

			GenerateFinanceBlock(ticket);
		}

		private void GenerateMainBlock(AviaTicket ticket)
		{
			AddImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MainDataTemplatePath), _document.LeftMargin, _currentPos);

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFont, 8);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, ticket.Passenger, 185, 731, 0);

			_contentByte.SetFontAndSize(_baseFontBold, 12);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, string.Format("ETKT {0:000} {1}", ticket.AirlinePrefixCode, ticket.Number), 185, 702, 0);

			_contentByte.SetFontAndSize(_baseFont, 8);

			string airline = ticket.Airline != null ? ticket.Airline.Name : ticket.AirlineName;

			if (!string.IsNullOrEmpty(airline))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, airline, 185, 673, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CompanyInfo, 450, 736, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, string.Format("AGENT: {0}, IATA: {1}", ticket.TicketerCode, IataNumber), 450, 727, 0);

			string booking = string.Format("{0}: {1}", ticket.Originator.ToString().ToUpper(), ticket.PnrCode);

			if (!string.IsNullOrEmpty(ticket.AirlinePnrCode))
				booking += string.Format(", AIRLINE: {0}", ticket.AirlinePnrCode);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, booking, 450, 702, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, ticket.IssueDate.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("en-US")), 450, 673, 0);

			_contentByte.EndText();
		}

		private void GenerateSegmentsBlock(IList<FlightSegment> segments)
		{
			List<FlightSegment> list = new List<FlightSegment>();

			foreach (FlightSegment segment in segments)
			{
				list.Add(segment);

				if (segment.Stopover)
				{
					GenerateSegmentsCaption(list[0], list[list.Count - 1]);

					foreach (FlightSegment flightSegment in list)
						GenerateSegment(flightSegment);

					list = new List<FlightSegment> ();
				}
			}

			if (list.Count != 0)
			{
				GenerateSegmentsCaption(list[0], list[list.Count - 1]);

				foreach (FlightSegment flightSegment in list)
					GenerateSegment(flightSegment);
			}
		}

		private void GenerateSegmentsCaption(FlightSegment first, FlightSegment last)
		{
			AddImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SegmentHeaderTemplatePath), _document.LeftMargin, _currentPos);

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFont, 10);

			string fromString = string.Empty;

			if (first.DepartureTime.HasValue)
				fromString += first.DepartureTime.Value.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("uk-UA")).ToUpper();

			string fromAirport = GetAirportShortText(first.FromAirport);

			if (string.IsNullOrEmpty(fromAirport))
				fromAirport = first.FromAirportCode;

			string separator = string.Empty;

			if (!string.IsNullOrEmpty(fromString) && !string.IsNullOrEmpty(fromAirport))
				separator = " ";

			fromString += separator + fromAirport;

			string toAirport = GetAirportShortText(last.ToAirport);

			if (string.IsNullOrEmpty(toAirport))
				toAirport = last.ToAirportCode;

			string caption = string.Format(ReportRes.YanaTicketPrinter_SegmentCaption, fromString, toAirport);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, caption, 300, _currentPos + 35, 0);

			_contentByte.EndText();
		}

		private void GenerateSegment(FlightSegment segment)
		{
			AddImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SegmentDataTemplatePath), _document.LeftMargin, _currentPos);

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFont, 7);

			if (segment.DepartureTime.HasValue)
			{
				string weekDay = segment.DepartureTime.Value.ToString("dddd", CultureInfo.GetCultureInfo("uk-UA"));
				weekDay = weekDay.Substring(0, 1).ToUpper() + weekDay.Substring(1);

				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, weekDay + ",", 28, _currentPos + 13, 0);
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, segment.DepartureTime.Value.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("uk-UA")), 28, _currentPos + 5, 0);
			}

			List<string> fromAirport = GetFromAirportText(segment);

			if (fromAirport.Count == 2)
			{
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, fromAirport[0], 88, _currentPos + 13, 0);
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, fromAirport[1], 88, _currentPos + 5, 0);
			}
			else if (fromAirport.Count == 1)
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, fromAirport[0], 88, _currentPos + 9, 0);

			List<string> toAirport = GetToAirportText(segment);

			if (toAirport.Count == 2)
			{
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, toAirport[0], 179, _currentPos + 13, 0);
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, toAirport[1], 179, _currentPos + 5, 0);
			}
			else if (toAirport.Count == 1)
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, toAirport[0], 179, _currentPos + 9, 0);

			string flight = segment.Carrier != null ? segment.Carrier.IataCode : segment.CarrierIataCode;
			string separator = string.Empty;

			if (!string.IsNullOrEmpty(flight))
				separator = " ";

			if (!string.IsNullOrEmpty(segment.FlightNumber))
				flight += separator + segment.FlightNumber;

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, flight, 285, _currentPos + 9, 0);

			if (segment.DepartureTime.HasValue)
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, segment.DepartureTime.Value.ToString("HH:mm"), 312, _currentPos + 9, 0);

			if (segment.ArrivalTime.HasValue)
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, segment.ArrivalTime.Value.ToString("HH:mm"), 345, _currentPos + 9, 0);

			if (!string.IsNullOrEmpty(segment.CheckInTime))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, segment.CheckInTime, 382, _currentPos + 9, 0);

			if (!string.IsNullOrEmpty(segment.Duration))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, segment.Duration, 420, _currentPos + 9, 0);

			string serviceClassCode = string.Empty;
			string serviceClass = string.Empty;

			if (!string.IsNullOrEmpty(segment.ServiceClassCode))
				serviceClassCode = segment.ServiceClassCode;

			if (segment.ServiceClass.HasValue && segment.ServiceClass != ServiceClass.Unknown)
				serviceClass = segment.ServiceClass.Value.ToDisplayString();

			if (!string.IsNullOrEmpty(serviceClassCode) && !string.IsNullOrEmpty(serviceClass))
			{
				serviceClassCode += ",";

				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, serviceClassCode, 445, _currentPos + 13, 0);
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, serviceClass, 445, _currentPos + 5, 0);
			}
			else if (!string.IsNullOrEmpty(serviceClassCode))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, serviceClassCode, 462, _currentPos + 9, 0);
			else if (!string.IsNullOrEmpty(serviceClass))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, serviceClass, 462, _currentPos + 9, 0);

			if (!string.IsNullOrEmpty(segment.Luggage))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, segment.Luggage, 495, _currentPos + 9, 0);

			if (segment.MealTypes.HasValue)
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, segment.MealTypes.Value.ToDisplayString(), 524, _currentPos + 9, 0);

			if (!string.IsNullOrEmpty(segment.Seat))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, segment.Seat, 553, _currentPos + 9, 0);

			_contentByte.EndText();
		}

		private void GenerateFinanceBlock(AviaTicket ticket)
		{
			AddImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FinanceDataTemplatePath), _document.LeftMargin, _currentPos);

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFont, 7);

			if (ticket.Fare != null && ticket.Fare.Currency.Code == "USD")
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(ticket.Fare), 91, _currentPos + 49, 0);

			if (ticket.Fare != null && ticket.Fare.Currency.Code == "UAH")
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(ticket.Fare), 198, _currentPos + 49, 0);
			else if (ticket.EqualFare != null && ticket.EqualFare.Currency.Code == "UAH")
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(ticket.EqualFare), 198, _currentPos + 49, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(ticket.FeesTotal), 326, _currentPos + 49, 0);

			Money serviceFee = ticket.ServiceFee;

			if (ticket.Discount != null)
				serviceFee -= ticket.Discount;

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(serviceFee), 433, _currentPos + 49, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(ticket.GrandTotal), 533, _currentPos + 49, 0);

			_contentByte.EndText();
		}

		private static string GetAirportShortText(Airport airport)
		{
			string text = string.Empty;

			if (airport == null)
				return string.Empty;

			string separator = string.Empty;

			if (!string.IsNullOrEmpty(airport.Settlement))
			{
				text = airport.Settlement;
				separator = " / ";
			}

			if (airport.Country != null)
				text += separator + airport.Country.Name;

			if (!string.IsNullOrEmpty(text))
				text += string.Format(" ({0})", airport.Code);

			return text;
		}

		private static List<string> GetFromAirportText(FlightSegment segment)
		{
			List<string> result = new List<string>();

			if (segment.FromAirport != null)
				result = GetAirportText(segment.FromAirport, segment.CheckInTerminal);
			else
			{
				string airportName = string.Empty;
				string separator = string.Empty;

				if (!string.IsNullOrEmpty(segment.FromAirportName))
				{
					airportName = segment.FromAirportName;
					separator = ", ";
				}
				else if (!string.IsNullOrEmpty(segment.FromAirportCode))
				{
					airportName = segment.FromAirportCode;
					separator = ", ";
				}

				if (!string.IsNullOrEmpty(segment.CheckInTerminal))
					airportName += separator + ReportRes.YanaTicketPrinter_Terminal + " " + segment.CheckInTerminal;

				result.Add(airportName);
			}

			return result;
		}

		private static List<string> GetToAirportText(FlightSegment segment)
		{
			List<string> result = new List<string>();

			if (segment.ToAirport != null)
				result = GetAirportText(segment.ToAirport, segment.ArrivalTerminal);
			else
			{
				string airportName = string.Empty;
				string separator = string.Empty;

				if (!string.IsNullOrEmpty(segment.ToAirportName))
				{
					airportName = segment.ToAirportName;
					separator = ", ";
				}
				else if (!string.IsNullOrEmpty(segment.ToAirportCode))
				{
					airportName = segment.ToAirportCode;
					separator = ", ";
				}

				if (!string.IsNullOrEmpty(segment.ArrivalTerminal))
					airportName += separator + ReportRes.YanaTicketPrinter_Terminal + " " + segment.ArrivalTerminal;

				result.Add(airportName);
			}

			return result;
		}

		private static List<string> GetAirportText(Airport airport, string terminal)
		{
			List<string> result = new List<string>();

			string settlement = string.Empty;
			string airportName = string.Empty;

			string separator = string.Empty;

			if (!string.IsNullOrEmpty(airport.Settlement))
			{
				settlement += airport.Settlement;
				separator = ", ";
			}

			if (airport.Country != null)
				settlement += separator + airport.Country.Name;

			separator = string.Empty;

			if (!string.IsNullOrEmpty(airport.Name))
			{
				airportName = airport.Name;
				separator = ", ";
			}

			if (!string.IsNullOrEmpty(terminal))
				airportName += separator + ReportRes.YanaTicketPrinter_Terminal + " " + terminal;

			separator = string.Empty;

			if (!string.IsNullOrEmpty(airportName))
			{
				result.Add(airportName);

				separator = " -";
			}

			if (!string.IsNullOrEmpty(settlement))
			{
				settlement += separator;

				result.Insert(0, settlement);
			}

			return result;
		}

		private static string GetMoneyString(Money money)
		{
			if (money == null)
				return string.Empty;

			return string.Format("{0} {1}", money.Currency.Code, money.Amount.ToMoneyString());
		}

		private void AddImage(string imgPath, float topX, float topY)
		{
			Image image = Image.GetInstance(imgPath);

			image.ScaleToFit(_document.PageSize.Width - _document.LeftMargin - _document.RightMargin, _document.PageSize.Height - _document.TopMargin - _document.BottomMargin);

			_currentPos -= image.ScaledHeight;

			if (_currentPos < _document.BottomMargin)
			{
				_document.NewPage();

				_currentPos = _document.PageSize.Height - _document.TopMargin - image.ScaledHeight;

				image.SetAbsolutePosition(topX, _currentPos);
			}
			else
				image.SetAbsolutePosition(topX, topY - image.ScaledHeight);

			_contentByte.AddImage(image);
		}

		private float _currentPos;

		private Document _document;
		private PdfContentByte _contentByte;
		private IList<AviaTicket> _tickets;

		private readonly BaseFont _baseFont = PdfUtility.GetBaseFont(PdfUtility.ArialNarrow, false, false, true);
		private readonly BaseFont _baseFontBold = PdfUtility.GetBaseFont(PdfUtility.ArialNarrow, true, false, true);
	}
}