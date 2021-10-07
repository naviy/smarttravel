using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Alliance
{

	public class AllianceTicketPrinter : ITicketPrinter
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

			if (!_pathsCombined)
			{
				HeaderPath = GetFullPath(HeaderPath);
				SegmentHeaderPartPath = GetFullPath(SegmentHeaderPartPath);
				SegmentTopPartPath = GetFullPath(SegmentTopPartPath);
				SegmentMiddlePartPath = GetFullPath(SegmentMiddlePartPath);
				SegmentBottomPartPath = GetFullPath(SegmentBottomPartPath);
				FooterPath = GetFullPath(FooterPath);
				ConditionsPath = GetFullPath(ConditionsPath);

				_pathsCombined = true;
			}

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
			r = ticket;

			GenerateHeader();

			GenerateSegments();

			GenerateFooter();

			AddConditions();
		}

		private void GenerateHeader()
		{
			AddTemplate(HeaderPath, yOffset: -24);
			_currentPos += 75;

			_contentByte.BeginText();

			_contentByte.SetFontAndSize(_baseFont, BaseFontSize);

			var xPos = 185;
			const int yoffset = -19;

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, r.PassengerName, xPos, 752 + yoffset, 0);

			if (r.Number.Yes())
			{
				_contentByte.SetFontAndSize(_baseFontBold, 12);
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
					$"{r.AirlinePrefixCode} {r.Number}", xPos, 728 + yoffset, 0);
			}

			_contentByte.SetFontAndSize(_baseFont, BaseFontSize);

			xPos = 470;

			_contentByte.SetFontAndSize(_baseFontBold, HeaderFieldFontSize);

			var airline = r.Producer != null ? r.Producer.Name : r.AirlineName;
			if (airline.Yes())
				_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, airline, xPos, 752 + yoffset, 0);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, r.PnrCode, xPos, 729 + yoffset, 0);

			_contentByte.EndText();
		}


		private void GenerateSegments()
		{
			var list = new List<FlightSegment>();

			foreach (var segment in r.GetTicketedSegments())
			{
				list.Add(segment);
				if (!segment.Stopover) continue;

				GenerateSegmentHeader(list[0]);

				foreach (var flightSegment in list.Skip(1))
				{
					AddTemplate(SegmentTopPartPath);
					GenerateSegment(flightSegment);
				}

				list.Clear();
			}

			if (list.Count != 0)
			{
				GenerateSegmentHeader(list[0]);

				foreach (var flightSegment in list.Skip(1))
				{
					AddTemplate(SegmentTopPartPath);
					GenerateSegment(flightSegment);
				}
			}
		}

		private void GenerateSegmentHeader(FlightSegment seg)
		{
			AddTemplate(SegmentHeaderPartPath);

			var fromString = string.Empty;

			if (seg.DepartureTime.HasValue)
			{
				fromString +=
					seg.DepartureTime.Value.ToString("dd MMMM yyyy").ToUpper() +
					" / DEPARTURE: " +
					seg.DepartureTime.Value.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture).ToUpper();
			}

			var separator = string.Empty;
			var fromAirport = string.Empty;

			if (seg.FromAirport != null)
			{
				if (seg.FromAirport.Country != null)
				{
					fromAirport = seg.FromAirport.Country.Name;
					separator = ", ";
				}

				if (seg.FromAirport.Settlement.Yes())
					fromAirport += separator + seg.FromAirport.Settlement;
			}

			fromAirport = fromAirport.No() ? seg.FromAirportCode : $"{fromAirport} ({seg.FromAirportCode})";

			if (fromAirport.Yes() && fromString.Yes())
				fromString = $"{fromString} - {fromAirport}";

			var caption = $"{ReportRes.TicketPrinter_SegmentCaption} {fromString}";

			_contentByte.BeginText();

			_contentByte.SetRGBColorFill(0, 65, 132);

			_contentByte.SetFontAndSize(_baseFontBold, SegmentHeaderFontSize);

			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, caption, 300, _currentPos + 35, 0);

			_contentByte.SetRGBColorFill(0, 0, 0);

			_contentByte.EndText();

			GenerateSegment(seg);
		}

		private void GenerateSegment(FlightSegment seg)
		{
			var font = new Font(_baseFontCond, BaseFontSize);

			var tbl = new PdfPTable(15);

			tbl.DefaultCell.PaddingTop = 3;
			tbl.DefaultCell.PaddingRight = 3;
			tbl.DefaultCell.PaddingBottom = 3;
			tbl.DefaultCell.PaddingLeft = 5;
			tbl.DefaultCell.Border = Rectangle.NO_BORDER;
			tbl.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
			tbl.SetTotalWidth(new[] { 113, 6, 104.5f, 6, 104.5f, 6, 39f, 6, 40f, 6, 40f, 6, 40f, 6, 40f });


			var flight = seg.FlightNumber.As(fnum =>
				(seg.Carrier?.AirlineIataCode ?? seg.CarrierIataCode) +
				" " + fnum
			);
			var stop = seg.NumberOfStops > 0 ? $"{seg.NumberOfStops} stop" : "Non Stop";
			tbl.AddCell(font, 
				flight + 
				seg.Operator.As(a => "\r\nOperated by " + a.Name) +
				$"\r\n{stop}" +
				seg.Equipment.As(a => "\r\n" + a.Name)
			);

			tbl.AddCell();
			tbl.AddCell(font,
				GetFromAirportText(seg) +
				seg.DepartureTime.AsString("HH:mm") +
				seg.CheckInTerminal.As(a => "\r\nterminal " + a)
			);
			tbl.AddCell();
			tbl.AddCell(font,
				GetToAirportText(seg) +
				seg.ArrivalTime.AsString("HH:mm") +
				seg.ArrivalTerminal.As(a => "\r\nterminal " + a)
			);


			tbl.AddCell();
			tbl.AddCellC(font, seg.Duration);

			tbl.AddCell();
			tbl.AddCellC(font, seg.Seat);

			tbl.AddCell();
			tbl.AddCellC(font, seg.MealNames("\r\n"));

			tbl.AddCell();
			tbl.AddCellC(font, seg.Luggage);

			tbl.AddCell();
			tbl.AddCellC(font, seg.ServiceClass.If(a => a != ServiceClass.Unknown)?.ToString().ToLower() ?? seg.ServiceClassCode);

			var rowsCount = GetTableLinesCount(tbl);
			float cellHeight = 0;
			for (var i = 0; i < rowsCount; i++)
				cellHeight += AddTemplate(SegmentMiddlePartPath);

			cellHeight += AddTemplate(SegmentBottomPartPath);

			tbl.WriteSelectedRows(0, -1, 13, _currentPos + cellHeight + 7, _contentByte);
		}


		private void GenerateFooter()
		{
			AddTemplate(FooterPath);

			_contentByte.BeginText();

			var yPosCustom = _currentPos - 7;

			_contentByte.SetFontAndSize(_baseFontBold, CustomFontSize);
			_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CompanyFooter, 542, yPosCustom, 0);

			_contentByte.SetFontAndSize(_baseFont, BaseFontSize);

			var yPos = _currentPos + 372;


			//_contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, GetMoneyString(r.GrandTotal), 110, yPos, 0);

			if (r.PenalizeOperations.Yes())
			{
				_contentByte.SetFontAndSize(_baseFontCond, BaseFontSize);

				yPos -= 55;

				DisplayPenalizeOperation(PenalizeOperationType.ChangesBeforeDeparture, 147, yPos);
				DisplayPenalizeOperation(PenalizeOperationType.ChangesAfterDeparture, 300, yPos);
				DisplayPenalizeOperation(PenalizeOperationType.NoShowBeforeDeparture, 485, yPos);

				yPos -= 21;

				//if (r.Segments.Count > 1)
				//{
					DisplayPenalizeOperation(PenalizeOperationType.RefundBeforeDeparture, 147, yPos);
					DisplayPenalizeOperation(PenalizeOperationType.RefundAfterDeparture, 300, yPos);
					DisplayPenalizeOperation(PenalizeOperationType.NoShowAfterDeparture, 485, yPos);
				//}
			}

			_contentByte.EndText();
		}


		private void AddConditions()
		{
			var reader = new PdfReader(ConditionsPath);

			for (var i = 1; i <= reader.NumberOfPages; ++i)
			{
				_document.NewPage();
				var template = _writer.GetImportedPage(reader, i);
				_contentByte.AddTemplate(template, 0, _document.PageSize.Height - template.Height);
			}
		}



		#region Utils

		private string GetFullPath(string path)
		{
			return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
		}

		private static string GetFromAirportText(FlightSegment segment)
		{
			return
				segment.FromAirport != null ? GetAirportText(segment.FromAirport) :
				segment.FromAirportName.Yes() ? segment.FromAirportName :
				segment.FromAirportCode.Yes() ? segment.FromAirportCode :
				null;
		}

		private static string GetToAirportText(FlightSegment segment)
		{
			return
				segment.ToAirport != null ? GetAirportText(segment.ToAirport) :
				segment.ToAirportName.Yes() ? segment.FromAirportName :
				segment.ToAirportCode.Yes() ? segment.FromAirportCode :
				null;
		}

		private static string GetAirportText(Airport airport)
		{
			var builder = new StringBuilder();

			string settlement = null;
			string separator = null;

			if (airport.Settlement.Yes())
			{
				settlement += airport.Settlement;
				separator = ", ";
			}

			if (airport.Country != null)
				settlement += separator + airport.Country.Name;

			if (settlement.Yes() && airport.Name.Yes())
				settlement += ",";

			if (settlement.Yes())
				builder.AppendLine(settlement);

			if (airport.Name.Yes())
				builder.AppendLine(airport.Name);

			return builder.ToString();
		}


		private float AddTemplate(string path, string relatedPath = null/*, float extraHeight = 0*/, float yOffset = 0)
		{
			var reader = new PdfReader(path);

			var template = _writer.GetImportedPage(reader, 1);

			_currentPos -= template.Height + yOffset;

			//var position = _currentPos;

			if (relatedPath.Yes())
			{
				//var relatedTemplate = 
				_writer.GetImportedPage(new PdfReader(relatedPath), 1);
				//position = _currentPos - relatedTemplate.Height;
			}

			//if (position - extraHeight < _document.BottomMargin)
			//{
			//	_currentPos = _document.PageSize.Height - _document.TopMargin - template.Height;
			//}

			_contentByte.AddTemplate(template, 0, _currentPos);

			return template.Height;
		}

		private void DisplayPenalizeOperation(PenalizeOperationType type, float xPos, float yPos)
		{
			var operation = r.PenalizeOperations.Find(a => a.Type == type);

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
					else
						text = "Заборонено/ DENIED";

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

		//private static string GetMoneyString(Money money)
		//{
		//	// need to replace no-break symbol by regular space, since using Myriad Pro it is displayed as artifact in PDF
		//	return money?.Amount.ToMoneyString().Replace(' ', ' ') ?? string.Empty;
		//}

		private static int GetTableLinesCount(PdfPTable table)
		{
			if (table != null && table.TotalHeight > 0)
				return (int)Math.Truncate(table.TotalHeight / BaseFontSize);

			return 3;
		}


		#endregion


		private IList<AviaTicket> _tickets;
		private Document _document;
		private PdfWriter _writer;
		private PdfContentByte _contentByte;
		private bool _pathsCombined;
		private readonly BaseFont _baseFont = PdfUtility.GetBaseFont("Myriad Pro", false, false, true);
		private readonly BaseFont _baseFontCond = PdfUtility.GetBaseFont("Myriad Pro Cond", false, false, true);
		private readonly BaseFont _baseFontBold = PdfUtility.GetBaseFont("Myriad Pro", true, false, true);

		private const int BaseFontSize = 10;
		private const int CustomFontSize = 6;
		private const int SegmentHeaderFontSize = 11;
		private const int HeaderFieldFontSize = 12;

		private float _currentPos;
		private AviaTicket r;

	}

}