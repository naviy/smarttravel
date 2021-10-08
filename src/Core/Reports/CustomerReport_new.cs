using System.Collections.Generic;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;
using Luxena.Travel.Domain.Avia;
using Luxena.Travel.Domain.Parties;

namespace Luxena.Travel.Reports
{
	public class CustomerReport
	{
		public CustomerReport(CustomerReportParams reportParams, IList<AviaDocument> documents)
		{
			_params = reportParams;

			_documents = documents ?? new List<AviaDocument>();
		}

		public Currency DefaultCurrency { get; set; }

		public bool UseDefaultCurrencyForInput { get; set; }

		public void Build(Stream stream)
		{
			AggregateData();

			CreatePdfDocument(stream);

			_pdfDocument.Open();

			GenerateReportHeader();

			GenerateReportContent();

			_pdfDocument.Close();
		}

		private void CreatePdfDocument(Stream stream)
		{
			_pdfDocument = new Document(PageSize.A4, 20, 20, 20, 30);

			var writer = PdfWriter.GetInstance(_pdfDocument, stream);

			var font = new Font(_defaultFont)
			{
				Size = 7
			};

			writer.PageEvent = new PdfPageNumberHelper
			{
				DisplayTotalPageCount = true,
				Font = font,
				PageNumberTemplate = ReportRes.Common_PageNumberTemplate
			};
		}

		private void AggregateData()
		{
			foreach (var document in _documents)
				AddDocument(document);
		}

		private void AddDocument(AviaDocument document)
		{
			var client = _params.Customer != null ? document.Customer : document.Order.BillTo;
			
			if (!Equals(client.Id, _params.Client) && !_departments.Contains(client))
				_departments.Add(client);

			if (!_total.ContainsKey(client))
				_total.Add(client, 0);
			if (!_extraCharge.ContainsKey(client))
				_extraCharge.Add(client, 0);
			if (!_grandTotal.ContainsKey(client))
				_grandTotal.Add(client, 0);

			if (document is AviaTicket)
			{
				if (!_tickets.ContainsKey(client))
					_tickets.Add(client, new List<AviaDocument>());

				_tickets[client].Add(document);
			}
			else if (document is AviaMco)
			{
				if (!_mco.ContainsKey(client))
					_mco.Add(client, new List<AviaDocument>());

				_mco[client].Add(document);
			}
			else
			{
				if (!_refunds.ContainsKey(client))
					_refunds.Add(client, new List<AviaDocument>());

				_refunds[client].Add(document);
			}

			var sign = document is AviaRefund ? -1 : 1;

			var total = _total[client];
			var extraCharge = _extraCharge[client];
			var grandTotal = _grandTotal[client];

			AddSum(ref total, document.FeesTotal * sign);
			AddSum(ref extraCharge, document.ExtraCharge * sign);
			AddSum(ref grandTotal, document.GrandTotal * sign);

			_total[client] = total;
			_extraCharge[client] = extraCharge;
			_grandTotal[client] = grandTotal;

			AddSum(ref _totalTotal, document.Total * sign);
			AddSum(ref _extraChargeTotal, document.ExtraCharge * sign);
			AddSum(ref _grandTotalTotal, document.GrandTotal * sign);
		}

		private void AddSum(ref decimal total, Money money)
		{
			if (money == null || !money.Currency.Equals(DefaultCurrency))
				return;

			total += money.Amount;
		}

		private void GenerateReportHeader()
		{
			var font = new Font(_defaultFont) { Size = 20 };

			var paragraph = new Paragraph(
				string.Format(ReportRes.CustomerInternalReport_Title, _params.Client.NameForDocuments), 
				font
			) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 5 };

			_pdfDocument.Add(paragraph);

			if (!string.IsNullOrEmpty(_params.Passenger))
				AddFilterString(ReportRes.Common_Passenger.ToLower(), _params.Passenger);

			if (_params.Airline != null)
				AddFilterString(DomainRes.Airline.ToLower(), _params.Airline.ToString());

			if (_params.PaymentType.HasValue)
				AddFilterString(ReportRes.Common_PaymentType.ToLower(), _params.PaymentType.Value.ToDisplayString());

			string periodOfTimeString;

			if (!_params.DateFrom.HasValue && !_params.DateTo.HasValue)
				periodOfTimeString = ReportRes.Common_AllTime;
			else if (!_params.DateFrom.HasValue)
				periodOfTimeString = string.Format(ReportRes.Common_To, _params.DateTo.Value.ToString("dd.MM.yyyy"));
			else if (!_params.DateTo.HasValue)
				periodOfTimeString = string.Format(ReportRes.Common_From, _params.DateFrom.Value.ToString("dd.MM.yyyy"));
			else
				periodOfTimeString = string.Format(ReportRes.Common_FromTo, _params.DateFrom.Value.ToString("dd.MM.yyyy"),
					_params.DateTo.Value.ToString("dd.MM.yyyy"));

			font.Size = 11;

			paragraph = new Paragraph(15, periodOfTimeString.ToLowerFirstLetter(), font) { Alignment = Element.ALIGN_CENTER };

			_pdfDocument.Add(paragraph);
		}

		private void AddFilterString(string caption, string value)
		{
			var font = new Font(_defaultFont) { Size = 11 };

			var paragraph = new Paragraph(15,
					string.Format("{0} = {1}", caption, value), font) { Alignment = Element.ALIGN_CENTER };

			_pdfDocument.Add(paragraph);
		}

		private void GenerateReportContent()
		{
			if (_documents.Count > 0)
			{
				GenerateCustomerReport(_params.Client);

				_pdfDocument.Add(new Paragraph(20));

				foreach (var department in _departments)
					GenerateCustomerReport(department);
			}

			GenerateTotal();
		}

		private void GenerateCustomerReport(Party client)
		{
			var count = 0;

			if (_tickets.ContainsKey(client))
			{
				GenerateDocumentTable(_tickets[client], client);

				count += _tickets[client].Count;

				_pdfDocument.Add(new Paragraph(10));
			}

			if (_mco.ContainsKey(client))
			{
				GenerateDocumentTable(_mco[client], client);

				count += _mco[client].Count;

				_pdfDocument.Add(new Paragraph(10));
			}

			if (_refunds.ContainsKey(client))
			{
				GenerateDocumentTable(_refunds[client], client);

				count += _refunds[client].Count;

				_pdfDocument.Add(new Paragraph(10));
			}

			GenerateTotal(client, count);
		}

		private void GenerateDocumentTable(List<AviaDocument> documents, Party client)
		{
			if (documents.Count == 0)
				return;

			var table = CreateDocumentTable();

			GenerateDocumentTableHeader(table, client, documents[0].Type, documents.Count);
			GenerateDocumentTableList(documents, table);
			GenerateDocumentTableFooter(documents, table);

			_pdfDocument.Add(table);
		}

		private static Table CreateDocumentTable()
		{
			const int columns = 8;

			var table = new Table(columns)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.NO_BORDER,
				Widths = new[] { 11f, 9f, 8.5f, 20.5f, 21f, 10.5f, 9f, 10.5f },
				Width = 100,
				CellsFitPage = true,
				DefaultCellBorderColor = Color.LIGHT_GRAY,
				DefaultVerticalAlignment = Element.ALIGN_TOP,
				DefaultHorizontalAlignment = Element.ALIGN_LEFT
			};

			return table;
		}

		private void GenerateDocumentTableHeader(Table table, Party client, AviaDocumentType type, int docsCount)
		{
			string customerText;
			string sectionText;

			if (client == _params.Client)
				customerText = _departments.Count == 0 ? string.Format("\"{0}\"", _params.Client.NameForDocuments) : string.Format(ReportRes.CustomerReport_CustomerNameWithoutDepartment, _params.Client.NameForDocuments);
			else
				customerText = string.Format(ReportRes.CustomerReport_DepartmentName, client.NameForDocuments);

			switch (type)
			{
				case AviaDocumentType.Ticket:
					sectionText = ReportRes.CustomerReport_TicketSection;
					break;
				case AviaDocumentType.Mco:
					sectionText = ReportRes.CustomerReport_McoSection;
					break;
				default:
					sectionText = ReportRes.CustomerReport_RefundSection;
					break;
			}

			var font = new Font(_defaultFontBoldItalic) { Size = 9 };

			var documentCount = string.Format(ReportRes.CustomerReport_DocumentCount, docsCount);

			var cell = GetCell(string.Format("{0} - {1} ({2})", customerText, sectionText, documentCount), font);
			cell.Colspan = table.Columns;
			cell.Border = Rectangle.NO_BORDER;

			table.AddCell(cell);

			font = new Font(_defaultFontBold) { Size = 8 };

			AddCell(table, ReportRes.Common_Document, font, Element.ALIGN_CENTER);

			AddCell(table, ReportRes.CustomerReport_Order, font, Element.ALIGN_CENTER);

			AddCell(table, ReportRes.Common_Date, font, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.Common_Passenger, font, Element.ALIGN_CENTER);

			var description = type == AviaDocumentType.Mco ? DomainRes.Common_Description : ReportRes.Common_Itinerary;
			AddCell(table, description, font, Element.ALIGN_CENTER);

			var cost = type == AviaDocumentType.Mco ? ReportRes.CustomerReport_McoCost : ReportRes.CustomerReport_TicketCost;
			AddCell(table, cost, font, Element.ALIGN_CENTER);

			AddCell(table, ReportRes.CustomerReport_ServicreFee, font, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.CustomerReport_GrandTotal, font, Element.ALIGN_CENTER);

			table.EndHeaders();
		}

		private void GenerateDocumentTableList(List<AviaDocument> documents, Table table)
		{
			var font = new Font(_defaultFont) { Size = 8 };

			var invalidCellFont = new Font(font);
			invalidCellFont.SetColor(255, 0, 0);

			foreach (var document in documents)
			{
				var pos = documents.IndexOf(document);

				if (pos == 0)
					table.DefaultCellBorder = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
				else
					table.DefaultCellBorder = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;

				table.DefaultCell.BackgroundColor = pos % 2 == 0 ? new Color(0xF5, 0xF5, 0xF5) : Color.WHITE;

				AddCell(table, document.FullNumber, font, Element.ALIGN_LEFT);

				AddCell(table, document.Order != null ? document.Order.Number : string.Empty, font, Element.ALIGN_LEFT);

				AddCell(table, document.IssueDate.ToString("dd.MM.yyyy"), font, Element.ALIGN_CENTER);
				AddCell(table, document.PassengerName, font, Element.ALIGN_LEFT);

				var text = document.Type == AviaDocumentType.Mco ? ((AviaMco)document).Description : document.Itinerary;
				AddCell(table, text, font, Element.ALIGN_LEFT);

				var isRefund = document.Type == AviaDocumentType.Refund;
				bool isValid;

				AddCell(table, MoneyToString(document.Total, isRefund, out isValid), isValid ? font : invalidCellFont, Element.ALIGN_RIGHT);
				AddCell(table, MoneyToString(document.ExtraCharge, isRefund, out isValid), isValid ? font : invalidCellFont, Element.ALIGN_RIGHT);
				AddCell(table, MoneyToString(document.GrandTotal, isRefund, out isValid), isValid ? font : invalidCellFont, Element.ALIGN_RIGHT);
			}
		}

		private void GenerateDocumentTableFooter(List<AviaDocument> documents, Table table)
		{
			table.DefaultCellBackgroundColor = Color.WHITE;
			table.DefaultHorizontalAlignment = Element.ALIGN_RIGHT;
			table.DefaultCellBorder = Rectangle.BOX;

			decimal total = 0;
			decimal extraCharge = 0;
			decimal grandTotal = 0;

			foreach (var document in documents)
			{
				AddSum(ref total, document.Total);
				AddSum(ref extraCharge, document.ExtraCharge);
				AddSum(ref grandTotal, document.GrandTotal);
			}

			if (documents[0].Type == AviaDocumentType.Refund)
			{
				total *= -1;
				extraCharge *= -1;
				grandTotal *= -1;
			}

			var font = new Font(_defaultFontBoldItalic) { Size = 9 };

			var cell = GetCell(ReportRes.Common_Total, font);
			cell.Colspan = table.Columns - 3;
			table.AddCell(cell);

			AddCell(table, total.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, extraCharge.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, grandTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);
		}

		private void GenerateTotal(Party client, int count)
		{
			if (_departments.Count == 0)
				return;

			var table = new Table(4)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.NO_BORDER,
				DefaultCellBorder = Rectangle.NO_BORDER,
				Widths = new[] { 70f, 10.5f, 9f, 10.5f },
				Width = 100,
				CellsFitPage = true,
				DefaultCellBorderColor = Color.LIGHT_GRAY,
				DefaultVerticalAlignment = Element.ALIGN_TOP,
				DefaultHorizontalAlignment = Element.ALIGN_RIGHT
			};

			_pdfDocument.Add(new Paragraph(5));

			var font = new Font(_defaultFontBold) { Size = 10 };

			var text = client == _params.Client
				? string.Format(ReportRes.CustomerReport_CustomerTotalWithoutDepartment, client.NameForDocuments, count)
				: string.Format(ReportRes.CustomerReport_DepartmentTotal, client.NameForDocuments, count);

			AddCell(table, text, font, Element.ALIGN_RIGHT);
			AddCell(table, _total[client].ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, _extraCharge[client].ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, _grandTotal[client].ToMoneyString(), font, Element.ALIGN_RIGHT);

			_pdfDocument.Add(table);
		}

		private void GenerateTotal()
		{
			_pdfDocument.Add(new Paragraph(20));

			var table = new Table(4)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.TOP_BORDER,
				DefaultCellBorder = Rectangle.NO_BORDER,
				Widths = new[] { 70f, 10.5f, 9f, 10.5f },
				Width = 100,
				CellsFitPage = true,
				DefaultCellBorderColor = Color.LIGHT_GRAY,
				DefaultVerticalAlignment = Element.ALIGN_TOP,
				DefaultHorizontalAlignment = Element.ALIGN_LEFT
			};

			var font = new Font(_defaultFontBold) { Size = 10 };

			var caption = string.Format("{0} - {1}", string.Format(ReportRes.CustomerReport_Total, _params.Client.NameForDocuments),
				string.Format(ReportRes.CustomerReport_DocumentCount, _documents.Count));

			AddCell(table, caption, font, Element.ALIGN_RIGHT);
			AddCell(table, _totalTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, _extraChargeTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, _grandTotalTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);

			_pdfDocument.Add(table);
		}

		private static void AddCell(Table table, string text, Font font, int horizontalAlignment)
		{
			var cell = GetCell(text ?? string.Empty, font);
			cell.HorizontalAlignment = horizontalAlignment;

			table.AddCell(cell);
		}

		private static Cell GetCell(string text, Font font)
		{
			return new Cell(new Chunk(text, font)) { Leading = font.Size };
		}

		private string MoneyToString(Money money, bool isNegative, out bool isValid)
		{
			isValid = true;

			if (money == null)
				return string.Empty;

			if (isNegative)
				money *= -1;

			string text;

			if (!money.Currency.Equals(DefaultCurrency))
			{
				if (UseDefaultCurrencyForInput)
					isValid = false;

				text = money.MoneyString;
			}
			else
				text = money.ToString();

			return text;
		}

		private Document _pdfDocument;

		private readonly IList<AviaDocument> _documents;

		private readonly List<Party> _departments = new List<Party>();
		private readonly Dictionary<Party, List<AviaDocument>> _tickets = new Dictionary<Party, List<AviaDocument>>();
		private readonly Dictionary<Party, List<AviaDocument>> _mco = new Dictionary<Party, List<AviaDocument>>();
		private readonly Dictionary<Party, List<AviaDocument>> _refunds = new Dictionary<Party, List<AviaDocument>>();

		private readonly Dictionary<Party, decimal> _total = new Dictionary<Party, decimal>();
		private readonly Dictionary<Party, decimal> _extraCharge = new Dictionary<Party, decimal>();
		private readonly Dictionary<Party, decimal> _grandTotal = new Dictionary<Party, decimal>();

		private decimal _totalTotal;
		private decimal _extraChargeTotal;
		private decimal _grandTotalTotal;

		private readonly Font _defaultFont = PdfUtility.GetFont(PdfUtility.Times, 10, false, false);
		private readonly Font _defaultFontBoldItalic = PdfUtility.GetFont(PdfUtility.Times, 10, true, true);
		private readonly Font _defaultFontBold = PdfUtility.GetFont(PdfUtility.Times, 10, true, false);

		private readonly CustomerReportParams _params;
	}
}