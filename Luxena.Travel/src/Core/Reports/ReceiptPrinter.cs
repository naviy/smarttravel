using System.Linq;
using System.Text;

using java.io;

using Luxena.Travel.Domain;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;
using org.apache.poi.ss.util;


namespace Luxena.Travel.Reports
{
	public class ReceiptPrinter : IReceiptPrinter
	{
		public Domain.Domain db { get; set; }

		public byte[] Build(Order order, Invoice invoice)
		{
			_order = order;
			_invoice = invoice;

			CreateWorkbook();

			SetColumnWidth(0);
			SetColumnWidth(10);

			GenerateContent(0, 0);
			GenerateContent(0, 10);

			NewRow(15);

			var row = _currentRow;

			var usePageBreak = _variableHeight > MaxVariableHeight;

			if (usePageBreak)
				_sheet.setRowBreak(_currentRow);
			else
				NewRow(10);

			GenerateContent(++_currentRow, 0);

			DrawVerticalLine(usePageBreak ? row : _currentRow, 8);

			if (!usePageBreak)
			{
				DrawHorizotalLine(row, 17);
				DrawLines(row, 8);
			}

			byte[] bytes;

			using (var outputStream = new ByteArrayOutputStream())
			{
				_workbook.write(outputStream);

				bytes = outputStream.toByteArray();
			}

			return bytes;
		}

		private void DrawHorizotalLine(int row, int maxCol)
		{
			var style = _workbook.createCellStyle();
			style.setBorderBottom(CellStyle.__Fields.BORDER_DOTTED);

			for (var i = 0; i <= maxCol; i++)
			{
				var cell = _sheet.getRow(row).createCell(i);
				cell.setCellStyle(style);
			}
		}

		private void DrawVerticalLine(int maxRow, int col)
		{
			var style = _workbook.createCellStyle();
			style.setBorderRight(CellStyle.__Fields.BORDER_DOTTED);

			for (var i = 0; i <= maxRow; i++)
			{
				var cell = _sheet.getRow(i).createCell(col);
				cell.setCellStyle(style);
			}
		}

		private void DrawLines(int row, int col)
		{
			var style = _workbook.createCellStyle();
			style.setBorderBottom(CellStyle.__Fields.BORDER_DOTTED);
			style.setBorderRight(CellStyle.__Fields.BORDER_DOTTED);

			var cell = _sheet.getRow(row).createCell(col);
			cell.setCellStyle(style);
		}

		private void CreateWorkbook()
		{
			_workbook = new HSSFWorkbook();

			_sheet = _workbook.createSheet();

			//_sheet.setFitToPage(true);

			_sheet.setMargin(Sheet.__Fields.TopMargin, 0.2);
			_sheet.setMargin(Sheet.__Fields.RightMargin, 0.2);
			_sheet.setMargin(Sheet.__Fields.BottomMargin, 0.2);
			_sheet.setMargin(Sheet.__Fields.LeftMargin, 0.2);
		}

		private void GenerateContent(int row, int col)
		{
			_currentRow = row - 1;
			_colShift = col;

			_variableHeight = 0;

			AddHeader();

			AddOrderItems();

			AddFooter();
		}

		private void SetColumnWidth(int shift)
		{
			_sheet.setColumnWidth(0 + shift, 7 * 256);
			_sheet.setColumnWidth(1 + shift, (int)(2.5 * 256));
			_sheet.setColumnWidth(2 + shift, 3 * 256);
			_sheet.setColumnWidth(3 + shift, (int)(13.5 * 256));
			_sheet.setColumnWidth(4 + shift, 2 * 256);
			_sheet.setColumnWidth(5 + shift, 6 * 256);
			_sheet.setColumnWidth(6 + shift, (8 * 256));
			_sheet.setColumnWidth(7 + shift, (int)(8.5 * 256));
			_sheet.setColumnWidth(8 + shift, 2 * 256);
			_sheet.setColumnWidth(9 + shift, 2 * 256);
		}

		private void AddHeader()
		{
			var font = _workbook.createFont();
			font.setFontHeightInPoints(6);

			var dataStyle1 = _workbook.createCellStyle();
			dataStyle1.setFont(font);
			dataStyle1.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);
			dataStyle1.setAlignment(CellStyle.__Fields.ALIGN_LEFT);
			dataStyle1.setWrapText(true);

			var dataStyle2 = _workbook.createCellStyle();
			dataStyle2.cloneStyleFrom(dataStyle1);
			dataStyle2.setAlignment(CellStyle.__Fields.ALIGN_RIGHT);

			NewRow(51);

			MergeRegion(_currentRow, _currentRow, 0, 3);
			MergeRegion(_currentRow, _currentRow, 4, 7);

			SetCellValue(GetSupplierInfo(), dataStyle1);
			SetCellValue(GetSupplierBankRequisites(), 4, dataStyle2);

			var captionFont = _workbook.createFont();
			captionFont.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);
			captionFont.setFontHeightInPoints(10);

			var captionStyleCenter = _workbook.createCellStyle();
			captionStyleCenter.setFont(captionFont);
			captionStyleCenter.setAlignment(CellStyle.__Fields.ALIGN_CENTER);

			var clientFont = _workbook.createFont();
			clientFont.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);
			clientFont.setFontHeightInPoints(8);

			var clientStyle = _workbook.createCellStyle();
			clientStyle.setFont(clientFont);
			clientStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);

			var ff = _workbook.createFont();
			ff.setFontHeightInPoints(10);

			var captionStyleBorder = _workbook.createCellStyle();
			captionStyleBorder.cloneStyleFrom(captionStyleCenter);
			captionStyleBorder.setFont(ff);
			captionStyleBorder.setBorderBottom(CellStyle.__Fields.BORDER_THIN);

			NewRow();

			MergeRegion(_currentRow, _currentRow, 0, 7);

			SetCellValue(string.Format(ReportRes.ReceiptPrinter_Receipt, _invoice.Number, _invoice.IssueDate), captionStyleCenter);
			/*
						if (_order.Payments.Count > 0 && _order.Payments[0].ReceivedFrom.Yes())
						{
							NewRow(15);

							_variableHeight += 15;

							SetCellValue(ReportRes.ReceiptPrinter_Payer, clientStyle);

							MergeRegion(_currentRow, _currentRow, 1, 7);

							for (int i = 1; i <= 7; i++)
								CreateCell(i, captionStyleBorder);

							SetCellValue(_order.Payments[0].ReceivedFrom, 1, captionStyleBorder);
						}
			*/

			NewRow(15);

			SetCellValue(ReportRes.ReceiptPrinter_Client, clientStyle);

			for (var i = 1; i <= 7; i++)
				CreateCell(i, captionStyleBorder);

			MergeRegion(_currentRow, _currentRow, 1, 7);

			SetCellValue(
				_order.BillTo != null
					? _order.BillTo.NameForDocuments
					: !string.IsNullOrWhiteSpace(_order.BillToName)
						? _order.BillToName
						: _order.Customer.NameForDocuments,
				1, captionStyleBorder
			);

			NewRow(15);

			MergeRegion(_currentRow, _currentRow, 0, 2);

			SetCellValue(ReportRes.ReceiptPrinter_Order, clientStyle);

			for (var i = 3; i <= 7; i++)
				CreateCell(i, captionStyleBorder);

			MergeRegion(_currentRow, _currentRow, 3, 7);

			SetCellValue(_order.Number, 3, captionStyleBorder);

			NewRow(8);
		}

		private void AddOrderItems()
		{
			AddListHeader();

			AddItems();

			AddListTotals();
		}

		private void AddListHeader()
		{
			var fontBold = _workbook.createFont();
			fontBold.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);
			fontBold.setFontHeightInPoints(8);

			var headerStyle = _workbook.createCellStyle();
			headerStyle.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			headerStyle.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			headerStyle.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			headerStyle.setBorderRight(CellStyle.__Fields.BORDER_THIN);

			headerStyle.setFont(fontBold);
			headerStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);

			NewRow();

			for (var i = 0; i < 5; i++)
				CreateCell(i, headerStyle);

			MergeRegion(_currentRow, _currentRow, 0, 4);

			SetCellValue(ReportRes.ReceiptPrinter_ItemName, 0, headerStyle);

			_currentCol += 4;

			SetCellValue(ReportRes.ReceiptPrinter_Quantity, headerStyle);
			SetCellValue(ReportRes.ReceiptPrinter_Price, headerStyle);
			SetCellValue(ReportRes.ReceiptPrinter_ItemTotal, headerStyle);
		}

		private void AddItems()
		{
			var dataStyle = _workbook.createCellStyle();
			dataStyle.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			dataStyle.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			dataStyle.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			dataStyle.setBorderRight(CellStyle.__Fields.BORDER_THIN);

			var moneyFont = _workbook.createFont();
			moneyFont.setFontHeightInPoints(8);

			var moneyStyle = _workbook.createCellStyle();
			moneyStyle.cloneStyleFrom(dataStyle);
			moneyStyle.setDataFormat(_workbook.createDataFormat().getFormat("#,##0.00"));
			moneyStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_CENTER);
			moneyStyle.setFont(moneyFont);

			var numberFont = _workbook.createFont();
			numberFont.setFontHeightInPoints(8);

			var numberStyle = _workbook.createCellStyle();
			numberStyle.cloneStyleFrom(dataStyle);
			numberStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);
			numberStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_CENTER);
			numberStyle.setFont(numberFont);

			var textFont = _workbook.createFont();
			textFont.setFontHeightInPoints(7);

			var textStyle = _workbook.createCellStyle();
			textStyle.cloneStyleFrom(dataStyle);
			textStyle.setWrapText(true);
			textStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);
			textStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_CENTER);
			textStyle.setFont(textFont);

			foreach (var item in _order.Items)
				_variableHeight += AddListItem(item, textStyle, numberStyle, moneyStyle);
		}

		private int AddListItem(OrderItem item, HSSFCellStyle textStyle, HSSFCellStyle numberStyle, HSSFCellStyle moneyStyle)
		{
			const short rowHeight = 11;

			var height = CalcTextRowCount(item.Text) * rowHeight;

			NewRow((short)height);

			for (var i = 0; i < 5; i++)
				CreateCell(i, textStyle);

			MergeRegion(_currentRow, _currentRow, 0, 4);

			CreateCell(0, textStyle).setCellValue(item.Text.Replace("\r", string.Empty));

			_currentCol += 4;
			CreateCell(numberStyle).setCellValue(item.Quantity);

			var cell = CreateCell(moneyStyle);

			if (item.Price != null)
				cell.setCellValue((double)item.Price.Amount);

			cell = CreateCell(moneyStyle);

			if (item.Total != null)
				cell.setCellValue((double)item.Total.Amount);

			return height;
		}

		private static int CalcTextRowCount(string text)
		{
			return text.Split('\n').Sum(row => row.Length / MaxSymbolCount + 1);
		}

		private void AddListTotals()
		{
			var fontBold = _workbook.createFont();
			fontBold.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);
			fontBold.setFontHeightInPoints(8);

			var moneyStyle = _workbook.createCellStyle();
			moneyStyle.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			moneyStyle.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			moneyStyle.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			moneyStyle.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			moneyStyle.setDataFormat(_workbook.createDataFormat().getFormat("#,##0.00"));
			moneyStyle.setFont(fontBold);

			var textStyle = _workbook.createCellStyle();
			textStyle.setFont(fontBold);
			textStyle.setAlignment(CellStyle.__Fields.ALIGN_RIGHT);
			textStyle.setIndention(1);

			const short height = 14;

			if (_order.Discount != null && _order.Discount.Amount > 0)
			{
				NewRow(height);

				SetCellValue(ReportRes.ReceiptPrinter_Discount, 6, textStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.Discount));

				_variableHeight += height;
			}

			NewRow(height);

			SetCellValue(ReportRes.ReceiptPrinter_OrderTotal, 6, textStyle);
			CreateCell(moneyStyle).setCellValue(GetAmount(_order.Total));

			NewRow(height);

			SetCellValue(ReportRes.ReceiptPrinter_Vat, 6, textStyle);
			CreateCell(moneyStyle).setCellValue(GetAmount(_order.Vat));

			if (!Equals(_order.Total, _order.TotalDue))
			{
				NewRow(height);

				SetCellValue(ReportRes.ReceiptPrinter_Paid, 6, textStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.Paid));

				NewRow(height);

				SetCellValue(ReportRes.ReceiptPrinter_TotalDue, 6, textStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.TotalDue));

				NewRow(height);

				SetCellValue(ReportRes.ReceiptPrinter_Vat, 6, textStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.VatDue));

				_variableHeight += height * 3;
			}
		}

		private void AddFooter()
		{
			var font = _workbook.createFont();
			font.setFontHeightInPoints(7);

			var fontBold = _workbook.createFont();
			fontBold.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);
			fontBold.setFontHeightInPoints(7);

			var textStyle = _workbook.createCellStyle();
			textStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);
			textStyle.setFont(font);
			textStyle.setWrapText(true);

			var boldStyle = _workbook.createCellStyle();
			boldStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);
			boldStyle.setFont(fontBold);
			boldStyle.setWrapText(true);

			var topTextStyle = _workbook.createCellStyle();
			topTextStyle.cloneStyleFrom(textStyle);
			topTextStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);

			var topBoldStyle = _workbook.createCellStyle();
			topBoldStyle.cloneStyleFrom(boldStyle);
			topBoldStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);

			var centerStyle = _workbook.createCellStyle();
			centerStyle.cloneStyleFrom(boldStyle);
			centerStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);

			var borderStyle = _workbook.createCellStyle();
			borderStyle.cloneStyleFrom(boldStyle);
			borderStyle.setBorderBottom(CellStyle.__Fields.BORDER_THIN);

			NewRow();

			NewRow(20);

			MergeRegion(_currentRow, _currentRow, 0, 2);

			SetCellValue(ReportRes.ReceiptPrinter_Total, topBoldStyle);

			if (_order.TotalDue != null)
			{
				MergeRegion(_currentRow, _currentRow, 3, 7);
				SetCellValue(_order.TotalDue.ToWords(), 3, topTextStyle);
			}

			NewRow();

			MergeRegion(_currentRow, _currentRow, 0, 1);
			SetCellValue(ReportRes.ReceiptPrinter_SignedBy, boldStyle);

			MergeRegion(_currentRow, _currentRow, 2, 7);
			SetCellValue(_order.AssignedTo.NameForDocuments, 2, textStyle);

			NewRow();

			SetCellValue(ReportRes.Receipt_Cashier, boldStyle);

			for (var i = 1; i <= 6; i++)
				CreateCell(i, borderStyle);

			MergeRegion(_currentRow, _currentRow, 1, 7);
			SetCellValue(string.Empty, borderStyle);

			NewRow();

			SetCellValue(ReportRes.Receipt_CheckNumber, boldStyle);

			for (var i = 1; i <= 4; i++)
				CreateCell(i, borderStyle);

			MergeRegion(_currentRow, _currentRow, 1, 4);
			SetCellValue(string.Empty, borderStyle);

			SetCellValue(ReportRes.Receipt_Date, 5, centerStyle);
			SetCellValue(_order.IssueDate.ToString("dd.MM.yyyy"), textStyle);
		}

		private string GetSupplierInfo()
		{
			var company = db.Configuration.Company;

			if (company == null)
				return null;

			var supplier = new StringBuilder()
				.AppendLine(company.NameForDocuments);

			if (company.LegalAddress.Yes())
				supplier
					.AppendFormat(ReportRes.ReceiptPrinter_Address, company.LegalAddress)
					.AppendLine();

			if (company.Phone1.Yes())
				supplier
					.AppendFormat(ReportRes.ReceiptPrinter_Phone, company.Phone1)
					.AppendLine();

			if (company.Fax.Yes())
				supplier.AppendFormat(ReportRes.ReceiptPrinter_Fax, company.Fax);

			return supplier.ToString().Replace("\r", string.Empty);
		}

		private string GetSupplierBankRequisites()
		{
			return string.Join("\r\n",
				(_order.BankAccount ?? db.BankAccount.By(a => a.IsDefault)).As(a => a.Description),
				db.Configuration.CompanyDetails.Clip()
			);
		}

		private void NewRow(short height)
		{
			var row = NewRow();
			row.setHeightInPoints(height);
		}

		private HSSFRow NewRow()
		{
			++_currentRow;

			var row = _sheet.getRow(_currentRow) ?? _sheet.createRow(_currentRow);
			_currentCol = -1 + _colShift;

			return row;
		}

		private void MergeRegion(int firstRow, int lastRow, int firstCol, int lastCol)
		{
			_sheet.addMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol + _colShift, lastCol + _colShift));
		}

		private HSSFCell CreateCell(HSSFCellStyle style)
		{
			return CreateCell(++_currentCol - _colShift, style);
		}

		private HSSFCell CreateCell(int col, HSSFCellStyle style)
		{
			_currentCol = col + _colShift;

			var cell = _sheet.getRow(_currentRow).createCell(_currentCol);
			cell.setCellStyle(style);

			return cell;
		}

		private void SetCellValue(string value, HSSFCellStyle style)
		{
			SetCellValue(value, _currentCol + 1 - _colShift, style);
		}

		private void SetCellValue(string value, int col, HSSFCellStyle style)
		{
			_currentCol = col + _colShift;

			var cell = CreateCell(col, style);

			if (value.Yes())
				cell.setCellValue(value);
		}

		private static double GetAmount(Money money)
		{
			return (double)(money != null ? money.Amount : 0);
		}

		private Invoice _invoice;
		private Order _order;

		private HSSFWorkbook _workbook;
		private HSSFSheet _sheet;
		private int _currentRow = -1;
		private int _currentCol = -1;
		private int _colShift;

		private int _variableHeight;

		private const int MaxVariableHeight = 155;

		private const int MaxSymbolCount = 39;
	}
}