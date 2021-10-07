using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Bsv
{
	public class BsvTicketPrinter : ITicketPrinter
	{
		public string FramePath { get; set; }
		public string HeaderPath { get; set; }
		public string SegmentHeaderTemplatePath { get; set; }
		public string SegmentDataTemplatePath { get; set; }
		public string MealInfoPath { get; set; }
		public string CalculationPath { get; set; }
		public string PenaltiesPath { get; set; }
		public string ServicesPath { get; set; }

		public string CompanyName { get; set; }

		public void Build(Stream stream, IList<AviaTicket> tickets)
		{
			_tickets = tickets;

			_document = new Document(PageSize.A4, 10, 10, 33, 30);

			_writer = PdfWriter.GetInstance(_document, stream);

			_writer.SetFullCompression();

			_document.Open();

			_contentByte = _writer.DirectContent;

			GenerateContent();

			_document.Close();
		}

		private void GenerateContent()
		{
			for (var i = 0; i < _tickets.Count; i++)
			{
				AddFrame(i != 0);

				GeneratePrintedTicket(_tickets[i]);
			}
		}

		private void AddFrame(bool newPage)
		{
			if (newPage)
				_document.NewPage();

			var reader = new PdfReader(GetFullPath(FramePath));

			_contentByte.AddTemplate(_writer.GetImportedPage(reader, 1), 0, 0);
		}

		private void GeneratePrintedTicket(AviaTicket ticket)
		{
			_currentPos = _document.PageSize.Height - _document.TopMargin;

			_currentTicket = ticket;

			GenerateMainBlock();

			GenerateSegmentsBlock();

			GenerateFinanceBlock();

			GeneratePenalties();

			GenerateServices();
		}

		private void GenerateMainBlock()
		{
			AddTemplate(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HeaderPath)));

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFont, 10);

			var xPos = 185;

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _currentTicket.PassengerName, xPos, 695, 0);

			if (_currentTicket.Number.Yes())
			{
				_contentByte.SetFontAndSize(_baseFontBold, 12);
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
					string.Format("{0} {1}", _currentTicket.AirlinePrefixCode, string.Format("{0:0000000000}", _currentTicket.Number)), xPos, 665, 0);
			}

			_contentByte.SetFontAndSize(_baseFont, 10);

			var airline = _currentTicket.Producer != null ? _currentTicket.Producer.Name : _currentTicket.AirlineName;

			if (!string.IsNullOrEmpty(airline))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, airline, xPos, 638, 0);

			xPos = 470;

			_contentByte.SetFontAndSize(_baseFontBold, 12);

			if (!string.IsNullOrEmpty(CompanyName))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CompanyName, xPos, 695, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _currentTicket.IssueDate.ToString("dd MMMM yyyy"), xPos, 665, 0);

			var pnr = _currentTicket.PnrCode;

			if (!string.IsNullOrEmpty(_currentTicket.AirlinePnrCode))
				pnr += " (" + _currentTicket.AirlinePnrCode + ")";

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, pnr, xPos, 638, 0);

			_contentByte.EndText();
		}

		private void GenerateSegmentsBlock()
		{
			var list = new List<FlightSegment>();

			foreach (var segment in _currentTicket.GetTicketedSegments())
			{
				list.Add(segment);

				if (segment.Stopover)
				{
					GenerateSegmentsCaption(list[0]);

					foreach (var flightSegment in list)
						GenerateSegment(flightSegment);

					list = new List<FlightSegment>();
				}
			}

			if (list.Count != 0)
			{
				GenerateSegmentsCaption(list[0]);

				foreach (var flightSegment in list)
					GenerateSegment(flightSegment);
			}

			AddTemplate(GetFullPath(MealInfoPath));
		}


		private void GenerateSegmentsCaption(FlightSegment segment)
		{
			AddTemplate(GetFullPath(SegmentHeaderTemplatePath), GetFullPath(SegmentDataTemplatePath));

			var fromString = string.Empty;

			if (segment.DepartureTime.HasValue)
				fromString += segment.DepartureTime.Value.ToString("dd MMMM yyyy").ToUpper();

			var separator = string.Empty;
			var fromAirport = string.Empty;

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
				string.Format("{0} ({1})", fromAirport, segment.FromAirportCode);

			if (!string.IsNullOrEmpty(fromAirport) && !string.IsNullOrEmpty(fromString))
				fromString = string.Format("{0} - {1}", fromString, fromAirport);

			var caption = string.Format("{0} {1}", ReportRes.TicketPrinter_SegmentCaption, fromString);

			_contentByte.BeginText();

			_contentByte.SetRGBColorFill(0, 65, 132);

			_contentByte.SetFontAndSize(_baseFontBold, 11);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, caption, 300, _currentPos + 28, 0);

			_contentByte.SetRGBColorFill(0, 0, 0);

			_contentByte.EndText();
		}


		private void GenerateSegment(FlightSegment segment)
		{
			var font = new Font(_baseFontCond, 10);

			var spacingHelper = new CellSpacingHelper(2);

			var table = new PdfPTable(4);
			table.DefaultCell.CellEvent = spacingHelper;
			table.DefaultCell.PaddingTop = 3;
			table.DefaultCell.PaddingRight = 3;
			table.DefaultCell.PaddingBottom = 5;
			table.DefaultCell.PaddingLeft = 5;
			table.DefaultCell.Border = Rectangle.NO_BORDER;
			table.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
			table.SetTotalWidth(new[] { 50, 81.5f, 81.5f, 335 });

			if (segment.DepartureTime.HasValue)
				AddCell(table, segment.DepartureTime.Value.ToString("dd.MM.yyyy"), font,
					Element.ALIGN_CENTER);
			else
				table.AddCell(string.Empty);

			AddCell(table, GetFromAirportText(segment) ?? string.Empty, font, Element.ALIGN_LEFT);
			AddCell(table, GetToAirportText(segment) ?? string.Empty, font, Element.ALIGN_LEFT);

			var innerTable = new PdfPTable(9);
			innerTable.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
			innerTable.DefaultCell.Padding = 0;
			innerTable.SetTotalWidth(new float[] { 30, 40, 40, 45, 35, 55, 25, 45, 20 });

			innerTable.DefaultCell.Border = Rectangle.NO_BORDER;

			var flight = string.Empty;

			if (!string.IsNullOrEmpty(segment.FlightNumber))
				flight = string.Format("{0} {1}", segment.Carrier != null ? segment.Carrier.AirlineIataCode : segment.CarrierIataCode,
					segment.FlightNumber);

			AddCell(innerTable, flight, font, Element.ALIGN_CENTER);

			var text = string.Empty;
			var separator = string.Empty;

			if (segment.DepartureTime.HasValue)
			{
				text = segment.DepartureTime.Value.ToString("HH:mm");
				separator = Environment.NewLine;
			}

			if (!string.IsNullOrEmpty(segment.CheckInTerminal))
				text = string.Format("{0}{1}{2} {3}", text, separator, ReportRes.TicketPrinter_Terminal, segment.CheckInTerminal);

			AddCell(innerTable, text, font, Element.ALIGN_CENTER);

			text = string.Empty;
			separator = string.Empty;

			if (segment.ArrivalTime.HasValue)
			{
				text = segment.ArrivalTime.Value.ToString("HH:mm");
				separator = Environment.NewLine;
			}

			if (!string.IsNullOrEmpty(segment.ArrivalTerminal))
				text = string.Format("{0}{1}{2} {3}", text, separator, ReportRes.TicketPrinter_Terminal, segment.ArrivalTerminal);

			AddCell(innerTable, text, font, Element.ALIGN_CENTER);

			AddCell(innerTable, segment.CheckInTime ?? string.Empty, font, Element.ALIGN_CENTER);
			AddCell(innerTable, segment.Duration ?? string.Empty, font, Element.ALIGN_CENTER);

			var serviceClass = string.Empty;

			if (segment.ServiceClass.HasValue && segment.ServiceClass.Value != ServiceClass.Unknown)
				serviceClass = segment.ServiceClass.Value.ToDisplayString();
			else if (!string.IsNullOrEmpty(segment.ServiceClassCode))
				serviceClass = segment.ServiceClassCode;

			AddCell(innerTable, serviceClass, font, Element.ALIGN_CENTER);
			AddCell(innerTable, segment.Luggage ?? string.Empty, font, Element.ALIGN_CENTER);

			var meal = string.Empty;

			if (segment.MealTypes.HasValue)
				meal = segment.MealTypes.Value.ToDisplayString();

			AddCell(innerTable, meal, font, Element.ALIGN_CENTER);
			AddCell(innerTable, segment.Seat ?? string.Empty, font, Element.ALIGN_CENTER);

			table.AddCell(innerTable);

			table.WriteSelectedRows(0, -1, 23.5f, _currentPos, _contentByte);

			_currentPos -= table.GetRowHeight(0);
		}


		private static void AddCell(PdfPTable table, string text, Font font, int horizontalAlignment)
		{
			table.DefaultCell.HorizontalAlignment = horizontalAlignment;

			table.AddCell(new Phrase(text, font));
		}


		private void GenerateFinanceBlock()
		{
			AddTemplate(GetFullPath(CalculationPath));

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFontCond, 10);

			var yPos = _currentPos + 21;

			Money fare = null;

			if (_currentTicket.Fare != null && _currentTicket.Fare.Currency.Code == "UAH")
				fare = _currentTicket.Fare;
			else if (_currentTicket.EqualFare != null && _currentTicket.EqualFare.Currency.Code == "UAH")
				fare = _currentTicket.EqualFare;


			var serviceFee = _currentTicket.Discount != null ? _currentTicket.ServiceFee - _currentTicket.Discount : _currentTicket.ServiceFee;

			//_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(fare), 97, yPos, 0);
			//_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(_currentTicket.FeesTotal), 210, yPos, 0);
			//_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(_currentTicket.Total), 322, yPos, 0);
			//_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(servFee), 434, yPos, 0);
			//_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(_currentTicket.GrandTotal), 545, yPos, 0);


			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(fare), 99, yPos, 0);
			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(_currentTicket.FeesTotal + serviceFee), 322, yPos, 0);
			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(_currentTicket.GrandTotal), 546, yPos, 0);

			_contentByte.EndText();

			var taxes = new StringBuilder();
			var separator = string.Empty;

			foreach (var fee in _currentTicket.Fees)
			{
				taxes.AppendFormat("{0}{1}  {2}", separator, fee.Code, fee.Amount.Amount.ToMoneyString(allowSpaces: false));

				separator = ";   ";
			}


			if (serviceFee.Yes())
			{

				if (taxes.Length > 0)
					taxes.Append(";   ");

				taxes.Append("XP ");
				taxes.Append(serviceFee.Amount.ToMoneyString(allowSpaces: false));
			}


			if (taxes.Length > 0)
			{
				taxes.Insert(0, ReportRes.FeesDetails);

				var font = new Font(_baseFontCond, 10);

				var table = new PdfPTable(1);
				table.DefaultCell.Border = Rectangle.NO_BORDER;
				table.SetTotalWidth(new[] { 553f });

				AddCell(table, taxes.ToString(), font, Element.ALIGN_LEFT);

				table.WriteSelectedRows(0, -1, 23.5f, _currentPos, _contentByte);

				_currentPos -= table.GetRowHeight(0);
			}
		}


		private void GeneratePenalties()
		{
			AddTemplate(GetFullPath(PenaltiesPath));

			if (!_currentTicket.PenalizeOperations.IsNullOrEmpty())
			{
				_contentByte.BeginText();

				_contentByte.SetFontAndSize(_baseFontCond, 10);

				var yPos = _currentPos + 72;

				DisplayPenalizeOperation(PenalizeOperationType.ChangesBeforeDeparture, 175, yPos);
				DisplayPenalizeOperation(PenalizeOperationType.RefundBeforeDeparture, 340, yPos);
				DisplayPenalizeOperation(PenalizeOperationType.NoShowBeforeDeparture, 502, yPos);

				yPos -= 20;

				if (_currentTicket.Segments.Count > 1)
				{
					DisplayPenalizeOperation(PenalizeOperationType.ChangesAfterDeparture, 175, yPos);
					DisplayPenalizeOperation(PenalizeOperationType.RefundAfterDeparture, 340, yPos);
					DisplayPenalizeOperation(PenalizeOperationType.NoShowAfterDeparture, 502, yPos);

					_contentByte.EndText();
				}
				else
				{
					_contentByte.EndText();

					yPos += 2;

					_contentByte.SetLineWidth(0.5f);

					_contentByte.MoveTo(170, yPos);
					_contentByte.LineTo(180, yPos);
					_contentByte.Stroke();

					_contentByte.MoveTo(335, yPos);
					_contentByte.LineTo(345, yPos);
					_contentByte.Stroke();

					_contentByte.MoveTo(497, yPos);
					_contentByte.LineTo(507, yPos);
					_contentByte.Stroke();
				}
			}
		}

		private void GenerateServices()
		{
			var footer = new PdfPTable(1);
			footer.DefaultCell.Border = Rectangle.NO_BORDER;
			footer.SetTotalWidth(new[] { 553f });

			AddCell(footer, ReportRes.TicketPrinter_Footer, new Font(_baseFontCond, 9), Element.ALIGN_LEFT);

			AddTemplate(GetFullPath(ServicesPath), null, footer.GetRowHeight(0));

			footer.WriteSelectedRows(0, -1, 25f, _currentPos + 2, _contentByte);
		}

		private void DisplayPenalizeOperation(PenalizeOperationType type, float xPos, float yPos)
		{
			var operation = _currentTicket.PenalizeOperations.Find(op => op.Type == type);

			if (operation == null)
				return;

			var text = string.Empty;

			switch (operation.Status)
			{
				case PenalizeOperationStatus.NotAllowed:

					if (type == PenalizeOperationType.ChangesBeforeDeparture || type == PenalizeOperationType.ChangesAfterDeparture)
						text = ReportRes.TicketPrinter_ChangesNotAllowed;
					else if (type == PenalizeOperationType.RefundBeforeDeparture || type == PenalizeOperationType.RefundAfterDeparture)
						text = ReportRes.TicketPrinter_RefundNotAllowed;
					else if (type == PenalizeOperationType.NoShowBeforeDeparture || type == PenalizeOperationType.NoShowAfterDeparture)
						text = ReportRes.TicketPrinter_NoShowNotAllowed;

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
						text = string.Format("{0} {1}", ReportRes.TicketPrinter_Charge, operation.Description);

					break;
			}

			if (!string.IsNullOrEmpty(text))
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, xPos, yPos, 0);
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

			var settlement = string.Empty;
			var separator = string.Empty;

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

		private static string GetMoneyString(Money money)
		{
			// need to replace no-break symbol by regular space, since using Myriad Pro it is displayed as artifact in PDF
			return money == null ? string.Empty : money.Amount.ToMoneyString().Replace(' ', ' ');
		}

		private void AddTemplate(string path, string relatedPath = null, float extraHeight = 0)
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
				AddFrame(true);

				_currentPos = _document.PageSize.Height - _document.TopMargin - template.Height;
			}

			_contentByte.AddTemplate(template, 0, _currentPos);
		}

		private static string GetFullPath(string path)
		{
			return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
		}

		private float _currentPos;

		private Document _document;
		private PdfWriter _writer;
		private PdfContentByte _contentByte;
		private IList<AviaTicket> _tickets;

		private readonly BaseFont _baseFont = PdfUtility.GetBaseFont("Myriad Pro", false, false, true);
		private readonly BaseFont _baseFontCond = PdfUtility.GetBaseFont("Myriad Pro Cond", false, false, true);
		private readonly BaseFont _baseFontBold = PdfUtility.GetBaseFont("Myriad Pro", true, false, true);

		private AviaTicket _currentTicket;
	}
}