using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Fgr
{
	public class FgrTicketPrinter : ITicketPrinter
	{
		public string HeaderPath { get; set; }
		public string SegmentHeaderPartPath { get; set; }
		public string SegmentTopPartPath { get; set; }
		public string SegmentMiddlePartPath { get; set; }
		public string SegmentBottomPartPath { get; set; }
		public string FooterPath { get; set; }
		public string ConditionsPath { get; set; }
		public string CompanyName { get; set; }
		public string CompanyFooter { get; set; }

		public void Build(Stream stream, IList<AviaTicket> tickets)
		{
			_tickets = tickets;

			_document = new Document(PageSize.A4, 10, 10, 0, 0);

			_writer = PdfWriter.GetInstance(_document, stream);

			_writer.SetFullCompression();

			_document.Open();

			_contentByte = _writer.DirectContent;

			GenerateContent();

			_document.Close();
		}

		private void GenerateContent()
		{
			var newPage = false;

			foreach (var ticket in _tickets)
			{
				if (newPage)
					_document.NewPage();
				newPage = true;

				GenerateTicket(ticket);
			}
		}

		private void GenerateTicket(AviaTicket ticket)
		{
			_currentPos = _document.PageSize.Height;
			_currentTicket = ticket;

			GenerateHeader();

			GenerateSegments();

			GenerateFooter();

			AddConditions();
		}

		private void GenerateFooter()
		{
			AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FooterPath)));

			_contentByte.BeginText();

			var yPosCustom = _currentPos - 7;

			_contentByte.SetFontAndSize(_baseFontBold, CustomFontSize);
			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CompanyFooter, 542, yPosCustom, 0);

			_contentByte.SetFontAndSize(_baseFont, BaseFontSize);

			var yPos = _currentPos + 102;


			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(_currentTicket.GrandTotal), 110, yPos, 0);

			if (!_currentTicket.PenalizeOperations.IsNullOrEmpty())
			{
				_contentByte.SetFontAndSize(_baseFontCond, BaseFontSize);

				yPos -= 55;

				DisplayPenalizeOperation(PenalizeOperationType.ChangesBeforeDeparture, 147, yPos);
				DisplayPenalizeOperation(PenalizeOperationType.RefundBeforeDeparture, 300, yPos);
				DisplayPenalizeOperation(PenalizeOperationType.NoShowBeforeDeparture, 485, yPos);

				yPos -= 21;

				if (_currentTicket.Segments.Count > 1)
				{
					DisplayPenalizeOperation(PenalizeOperationType.ChangesAfterDeparture, 147, yPos);
					DisplayPenalizeOperation(PenalizeOperationType.RefundAfterDeparture, 300, yPos);
					DisplayPenalizeOperation(PenalizeOperationType.NoShowAfterDeparture, 485, yPos);
				}
			}

			_contentByte.EndText();
		}

		private void AddConditions()
		{
			var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConditionsPath));

			var reader = new PdfReader(path);

			for (var i = 1; i <= reader.NumberOfPages; ++i)
			{
				_document.NewPage();
				var template = _writer.GetImportedPage(reader, i);
				_contentByte.AddTemplate(template, 0, _document.PageSize.Height - template.Height);
			}
		}

		private void GenerateSegments()
		{
			var list = new List<FlightSegment>();

			foreach (var segment in _currentTicket.GetTicketedSegments())
			{
				list.Add(segment);

				if (segment.Stopover)
				{
					GenerateSegmentHeader(list[0]);
					list.RemoveAt(0);

					foreach (var flightSegment in list)
					{
						AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SegmentTopPartPath)));
						GenerateSegment(flightSegment);
					}

					list = new List<FlightSegment>();
				}
			}

			if (list.Count != 0)
			{
				GenerateSegmentHeader(list[0]);
				list.RemoveAt(0);

				foreach (var flightSegment in list)
				{
					AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SegmentTopPartPath)));
					GenerateSegment(flightSegment);
				}
			}
		}

		private void GenerateHeader()
		{
			AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HeaderPath)));

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFont, BaseFontSize);

			var xPos = 185;

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _currentTicket.PassengerName, xPos, 752, 0);

			if (_currentTicket.Number.Yes())
			{
				_contentByte.SetFontAndSize(_baseFontBold, 12);
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
					$"{_currentTicket.AirlinePrefixCode} {_currentTicket.Number}", xPos, 728, 0);
			}

			_contentByte.SetFontAndSize(_baseFont, BaseFontSize);

			var airline = _currentTicket.Producer != null ? _currentTicket.Producer.Name : _currentTicket.AirlineName;

			if (airline.Yes())
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, airline, xPos, 705, 0);

			xPos = 470;

			_contentByte.SetFontAndSize(_baseFontBold, HeaderFieldFontSize);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CompanyName, xPos, 752, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _currentTicket.PnrCode, xPos, 729, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _currentTicket.IssueDate.ToString("dd MMMM yyyy"), xPos, 706, 0);

			_contentByte.EndText();
		}

		private void GenerateSegmentHeader(FlightSegment segment)
		{
			AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SegmentHeaderPartPath)));

			var fromString = "";

			if (segment.DepartureTime.HasValue)
				fromString += segment.DepartureTime.Value.ToString("dd MMMM yyyy").ToUpper();

			var separator = "";
			var fromAirport = "";

			if (segment.FromAirport != null)
			{
				if (segment.FromAirport.Country != null)
				{
					fromAirport = segment.FromAirport.Country.Name;
					separator = ", ";
				}

				if (!string.IsNullOrEmpty(segment.FromAirport.Settlement))
					fromAirport += separator + segment.FromAirport.Settlement;
			}

			fromAirport = string.IsNullOrEmpty(fromAirport) ? segment.FromAirportCode :
				$"{fromAirport} ({segment.FromAirportCode})";

			if (!string.IsNullOrEmpty(fromAirport) && !string.IsNullOrEmpty(fromString))
				fromString = $"{fromString} - {fromAirport}";

			var caption = $"{ReportRes.TicketPrinter_SegmentCaption} {fromString}";

			_contentByte.BeginText();

			_contentByte.SetRGBColorFill(0, 65, 132);

			_contentByte.SetFontAndSize(_baseFontBold, SegmentHeaderFontSize);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, caption, 300, _currentPos + 35, 0);

			_contentByte.SetRGBColorFill(0, 0, 0);

			_contentByte.EndText();

			GenerateSegment(segment);
		}

		private void GenerateSegment(FlightSegment segment)
		{
			var font = new Font(_baseFontCond, BaseFontSize);

			var table = new PdfPTable(15);

			table.DefaultCell.PaddingTop = 5;
			table.DefaultCell.PaddingRight = 3;
			table.DefaultCell.PaddingBottom = 1;
			table.DefaultCell.PaddingLeft = 5;
			table.DefaultCell.Border = Rectangle.NO_BORDER;
			table.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
			table.SetTotalWidth(new[] { 53, 6, 90, 6, 90, 6, 90, 6, 62, 6, 46.5f, 6, 46.5f, 6, 46.5f });

			var departureDate = "";
			if (segment.DepartureTime.HasValue)
				departureDate = segment.DepartureTime.Value.ToString("dd.MM.yyyy");

			var flight = "";
			if (!string.IsNullOrEmpty(segment.FlightNumber))
				flight = string.Format("{0} {1}", segment.Carrier != null ? segment.Carrier.AirlineIataCode : segment.CarrierIataCode,
					segment.FlightNumber);

			var serviceClass = "";
			if (segment.ServiceClass.HasValue && segment.ServiceClass.Value != ServiceClass.Unknown)
				serviceClass = segment.ServiceClass.Value.ToDisplayString();
			else if (!string.IsNullOrEmpty(segment.ServiceClassCode))
				serviceClass = segment.ServiceClassCode;

			AddCell(table, $"{departureDate}{Environment.NewLine}{flight}{Environment.NewLine}{serviceClass}",
				font, Element.ALIGN_CENTER);

			AddCell(table, "", font, Element.ALIGN_CENTER);
			AddCell(table, GetFromAirportText(segment) ?? "", font, Element.ALIGN_LEFT);
			AddCell(table, "", font, Element.ALIGN_CENTER);
			AddCell(table, GetToAirportText(segment) ?? "", font, Element.ALIGN_LEFT);

			var departureTime = "";
			if (segment.DepartureTime.HasValue)
				departureTime = segment.DepartureTime.Value.ToString("HH:mm");

			AddCell(table, "", font, Element.ALIGN_CENTER);
			AddCell(table, $"{departureTime}{Environment.NewLine}{ReportRes.TicketPrinter_Terminal} {segment.CheckInTerminal}",
				font, Element.ALIGN_CENTER);

			var arrivalTime = "";
			if (segment.ArrivalTime.HasValue)
				arrivalTime = segment.ArrivalTime.Value.ToString("HH:mm");

			AddCell(table, "", font, Element.ALIGN_CENTER);
			AddCell(table, $"{arrivalTime}{Environment.NewLine}{ReportRes.TicketPrinter_Terminal} {segment.ArrivalTerminal}",
				font, Element.ALIGN_CENTER);

			AddCell(table, "", font, Element.ALIGN_CENTER);
			AddCell(table, segment.Duration ?? "", font, Element.ALIGN_CENTER);

			AddCell(table, "", font, Element.ALIGN_CENTER);
			AddCell(table, segment.Luggage ?? "", font, Element.ALIGN_CENTER);

			AddCell(table, "", font, Element.ALIGN_CENTER);
			AddCell(table, segment.MealTypes.HasValue ? segment.MealTypes.Value.ToDisplayString() : "", font, Element.ALIGN_CENTER);

			var rowsCount = GetTableLinesCount(table);
			float cellHeight = 0;
			for (var i = 0; i < rowsCount; i++)
				cellHeight += AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SegmentMiddlePartPath)));

			cellHeight += AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SegmentBottomPartPath)));

			table.WriteSelectedRows(0, -1, 12, _currentPos + cellHeight + 7, _contentByte);
		}

		private static string GetFromAirportText(FlightSegment segment)
		{
			if (segment.FromAirport != null)
				return GetAirportText(segment.FromAirport);

			if (!string.IsNullOrEmpty(segment.FromAirportName))
				return segment.FromAirportName;

			if (!string.IsNullOrEmpty(segment.FromAirportCode))
				return segment.FromAirportCode;

			return null;
		}

		private static string GetToAirportText(FlightSegment segment)
		{
			if (segment.ToAirport != null)
				return GetAirportText(segment.ToAirport);

			if (!string.IsNullOrEmpty(segment.ToAirportName))
				return segment.FromAirportName;

			if (!string.IsNullOrEmpty(segment.ToAirportCode))
				return segment.FromAirportCode;

			return null;
		}

		private static string GetAirportText(Airport airport)
		{
			var builder = new StringBuilder();

			var settlement = "";
			var separator = "";

			if (!string.IsNullOrEmpty(airport.Settlement))
			{
				settlement += airport.Settlement;
				separator = ", ";
			}

			if (airport.Country != null)
				settlement += separator + airport.Country.Name;

			if (!string.IsNullOrEmpty(settlement) && !string.IsNullOrEmpty(airport.Name))
				settlement += ",";

			if (!string.IsNullOrEmpty(settlement))
				builder.AppendLine(settlement);

			if (!string.IsNullOrEmpty(airport.Name))
				builder.AppendLine(airport.Name);

			return builder.ToString();
		}

		private static void AddCell(PdfPTable table, string text, Font font, int horizontalAlignment)
		{
			table.DefaultCell.HorizontalAlignment = horizontalAlignment;

			table.AddCell(new Phrase(text, font));
		}

		private float AddTemplate(string path, string relatedPath = null, float extraHeight = 0)
		{
			var reader = new PdfReader(path);

			var template = _writer.GetImportedPage(reader, 1);

			_currentPos -= template.Height;

			var position = _currentPos;

			if (!string.IsNullOrEmpty(relatedPath))
			{
				var relatedTemplate = _writer.GetImportedPage(new PdfReader(relatedPath), 1);
				position = _currentPos - relatedTemplate.Height;
			}

			if (position - extraHeight < _document.BottomMargin)
			{
				_currentPos = _document.PageSize.Height - _document.TopMargin - template.Height;
			}

			_contentByte.AddTemplate(template, 0, _currentPos);

			return template.Height;
		}

		private void DisplayPenalizeOperation(PenalizeOperationType type, float xPos, float yPos)
		{
			var operation = _currentTicket.PenalizeOperations.Find(op => op.Type == type);

			if (operation == null)
				return;

			var text = "";

			switch (operation.Status)
			{
				case PenalizeOperationStatus.NotAllowed:

					if (type == PenalizeOperationType.ChangesBeforeDeparture || type == PenalizeOperationType.ChangesAfterDeparture)
						text = ReportRes.TicketPrinter_ChangesNotAllowed;
					if (type == PenalizeOperationType.RefundBeforeDeparture || type == PenalizeOperationType.RefundAfterDeparture)
						text = ReportRes.TicketPrinter_RefundNotAllowed;

					break;

				case PenalizeOperationStatus.NotChargeable:

					if (type == PenalizeOperationType.RefundBeforeDeparture || type == PenalizeOperationType.RefundAfterDeparture)
						text = ReportRes.TicketPrinter_RefundNoCharge;
					else
						text = ReportRes.TicketPrinter_NoCharge;

					break;

				case PenalizeOperationStatus.Chargeable:

					text = ReportRes.TicketPrinter_Charge;

					if (!string.IsNullOrEmpty(operation.Description))
						text = $"{ReportRes.TicketPrinter_Charge} {operation.Description}";

					break;
			}

			if (!string.IsNullOrEmpty(text))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, xPos, yPos, 0);
		}

		private static string GetMoneyString(Money money)
		{
			// need to replace no-break symbol by regular space, since using Myriad Pro it is displayed as artifact in PDF
			return money?.Amount.ToMoneyString().Replace(' ', ' ') ?? "";
		}

		private static int GetTableLinesCount(PdfPTable table)
		{
			if (table != null && table.TotalHeight > 0)
				return ((int) Math.Truncate(table.TotalHeight/BaseFontSize));

			return 3;
		}

		private IList<AviaTicket> _tickets;
		private Document _document;
		private PdfWriter _writer;
		private PdfContentByte _contentByte;

		private readonly BaseFont _baseFont = PdfUtility.GetBaseFont("Myriad Pro", false, false, true);
		private readonly BaseFont _baseFontCond = PdfUtility.GetBaseFont("Myriad Pro Cond", false, false, true);
		private readonly BaseFont _baseFontBold = PdfUtility.GetBaseFont("Myriad Pro", true, false, true);

		private const int BaseFontSize = 10;
		private const int CustomFontSize = 6;
		private const int SegmentHeaderFontSize = 11;
		private const int HeaderFieldFontSize = 12;

		private float _currentPos;
		private AviaTicket _currentTicket;
	}
}