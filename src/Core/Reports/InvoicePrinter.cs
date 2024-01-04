using System;
using System.IO;

using java.io;

using Luxena.Travel.Domain;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;
using org.apache.poi.ss.util;




namespace Luxena.Travel.Reports
{



	//===g






	public class InvoicePrinter : IInvoicePrinter
	{

		//---g


		
		//public static string FileExtension(Domain.Domain db)
		//{

		//	if (_fileExtension != null)
		//		return _fileExtension;


		//	if (db.Resolve<IInvoicePrinter>() is TemplateInvoicePrinter printer)
		//	{

		//		_fileExtension =

		//			Path.GetExtension(
		//				printer.TemplateFileName1 ?? printer.TemplateFileName2 ?? printer.TemplateFileName
		//			)
		//			.TrimStart('.')
		//			.Clip()

		//			?? "xlsx"

		//		;

		//	}
		//	else
		//	{
		//		_fileExtension = "xls";
		//	}


		//	return _fileExtension;

		//}


		//private static string _fileExtension;



		//---g

		public Domain.Domain db { get; set; }


		public int FormNumber { get; set; }

		public string ServiceFeeTitle { get; set; }



		//---g



		private Order _order;
		private string _number;
		private DateTime _issueDate;
		private Person _issuedBy;
		private Party _owner;
		private BankAccount _bankAccount;
		private bool _showPaid;

		private HSSFWorkbook _workbook;
		private HSSFSheet _sheet;
		private int _currentRow = -1;
		private int _currentCol = -1;

		private HSSFCellStyle _moneyStyle;
		private HSSFCellStyle _numberStyle;
		private HSSFCellStyle _textStyle;
		private HSSFCellStyle _unitStyle;



		//---g



		public byte[] Build(
			Order order, 
			string number,
			DateTime issueDate,
			Person issuedBy,
			Party owner,
			BankAccount bankAccount,
			int? formNumber,
			bool showPaid,
			out string fileExtension
		)
		{

			_order = order;
			_number = number;
			_issueDate = issueDate;
			_issuedBy = issuedBy;
			_owner = owner;
			_bankAccount = bankAccount;
			_showPaid = showPaid;


			CreateWorkbook();

			GenerateContent();


			byte[] bytes;

			using (var outputStream = new ByteArrayOutputStream())
			{
				_workbook.write(outputStream);

				bytes = outputStream.toByteArray();
			}



			fileExtension = "xls";

			return bytes;

		}



		private void CreateWorkbook()
		{

			_workbook = new HSSFWorkbook();

			_sheet = _workbook.createSheet();

			_sheet.setMargin(Sheet.__Fields.TopMargin, 0.5);
			_sheet.setMargin(Sheet.__Fields.RightMargin, 0.5);
			_sheet.setMargin(Sheet.__Fields.BottomMargin, 0.5);
			_sheet.setMargin(Sheet.__Fields.LeftMargin, 0.5);

			var dataStyle = _workbook.createCellStyle();
			dataStyle.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			dataStyle.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			dataStyle.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			dataStyle.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			dataStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);

			_moneyStyle = _workbook.createCellStyle();
			_moneyStyle.cloneStyleFrom(dataStyle);
			_moneyStyle.setDataFormat(_workbook.createDataFormat().getFormat("#,##0.00"));

			_numberStyle = _workbook.createCellStyle();
			_numberStyle.cloneStyleFrom(dataStyle);
			_numberStyle.setAlignment(CellStyle.__Fields.ALIGN_RIGHT);

			_textStyle = _workbook.createCellStyle();
			_textStyle.cloneStyleFrom(dataStyle);
			_textStyle.setWrapText(true);
			_textStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);

			_unitStyle = _workbook.createCellStyle();
			_unitStyle.cloneStyleFrom(dataStyle);
			_unitStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);

		}



		private void GenerateContent()
		{

			_sheet.setColumnWidth(0, 5 * 256);
			_sheet.setColumnWidth(1, 17 * 256);
			_sheet.setColumnWidth(2, 32 * 256);
			_sheet.setColumnWidth(3, 4 * 256);
			_sheet.setColumnWidth(4, 9 * 256);
			_sheet.setColumnWidth(5, 14 * 256);
			_sheet.setColumnWidth(6, 14 * 256);


			AddHeader();

			AddOrderItems();

			AddFooter();

		}



		private void AddHeader()
		{

			var captionFont = _workbook.createFont();
			captionFont.setUnderline(Font.__Fields.U_SINGLE);
			captionFont.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);

			var captionStyle = _workbook.createCellStyle();
			captionStyle.setFont(captionFont);
			captionStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);
			captionStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);

			var dataStyle = _workbook.createCellStyle();
			dataStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);
			dataStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);
			dataStyle.setWrapText(true);


			NewRow(120);

			MergeRegion(_currentRow, _currentRow, 2, 6);

			SetCellValue(ReportRes.InvoicePrinter_Supplier, 1, captionStyle);
			SetCellValue(db.Configuration.GetSupplierDetails(db, _order, owner: _owner, bankAccount: _bankAccount), dataStyle);

			++_currentRow;
			NewRow();

			MergeRegion(_currentRow, _currentRow, 2, 6);

			SetCellValue(ReportRes.InvoicePrinter_ShipTo, 1, captionStyle);


			var shipTo = _order.ShipTo ?? _order.Customer;

			if (shipTo != null)
				SetCellValue(shipTo.NameForDocuments, dataStyle);


			++_currentRow;
			NewRow();

			MergeRegion(_currentRow, _currentRow, 2, 6);

			SetCellValue(ReportRes.InvoicePrinter_BillTo, 1, captionStyle);


			if (_order.BillTo == null && _order.BillToName.Yes())
			{
				SetCellValue(_order.BillToName, dataStyle);
			}
			else
			{
				var billTo = _order.BillTo ?? _order.Customer;
				SetCellValue(billTo == null || billTo == shipTo ? ReportRes.InvoicePrinter_Same : billTo.NameForDocuments, dataStyle);
			}

			++_currentRow;

			NewRow();

			MergeRegion(_currentRow, _currentRow, 2, 6);

			SetCellValue(ReportRes.InvoicePrinter_Agreement, 1, captionStyle);

			SetCellValue(_order.Number, dataStyle);

		}



		private void AddOrderItems()
		{

			AddListCaption();


			if (_order.Items.Count == 0)
				return;


			var fontBold = _workbook.createFont();
			fontBold.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);

			var headerStyle = _workbook.createCellStyle();
			headerStyle.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			headerStyle.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			headerStyle.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			headerStyle.setBorderRight(CellStyle.__Fields.BORDER_THIN);

			headerStyle.setFont(fontBold);
			headerStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);

			++_currentRow;


			NewRow();

			MergeRegion(_currentRow, _currentRow, 1, 2);

			SetCellValue(ReportRes.InvoicePrinter_Number, headerStyle);
			SetCellValue(ReportRes.InvoicePrinter_ItemName, headerStyle);
			CreateCell(headerStyle);
			SetCellValue(ReportRes.InvoicePrinter_Units, headerStyle);
			SetCellValue(ReportRes.InvoicePrinter_Quantity, headerStyle);
			SetCellValue(ReportRes.InvoicePrinter_Price, headerStyle);
			SetCellValue(ReportRes.InvoicePrinter_ItemTotal, headerStyle);


			var pos = 1;

			foreach (var item in _order.Items)
			{
				AddListItem(pos++, item);
			}

			
			AddListTotals();

		}



		private void AddListCaption()
		{

			var captionFont = _workbook.createFont();
			captionFont.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);
			captionFont.setFontHeightInPoints(12);

			var captionStyle = _workbook.createCellStyle();
			captionStyle.setFont(captionFont);
			captionStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);

			++_currentRow;

			NewRow();

			MergeRegion(_currentRow, _currentRow, 0, 6);

			var title = ReportRes.InvoicePrinter_Invoice;

			SetCellValue(string.Format(title, _number), captionStyle);

			NewRow();

			MergeRegion(_currentRow, _currentRow, 0, 6);

			SetCellValue(string.Format(ReportRes.InvoicePrinter_IssueDate, _issueDate), captionStyle);

		}



		private void AddListItem(int pos, OrderItem item)
		{

			NewRow(40);

			MergeRegion(_currentRow, _currentRow, 1, 2);

			CreateCell(_numberStyle).setCellValue(pos);


			var title = item.Text.Replace("\r", string.Empty);

			if (ServiceFeeTitle.Yes() && item.IsServiceFee)
				title = ServiceFeeTitle;

			CreateCell(_textStyle).setCellValue(title);
			CreateCell(_textStyle);
			CreateCell(_unitStyle).setCellValue(ReportRes.InvoicePrinter_ItemUnit);
			CreateCell(_numberStyle).setCellValue(item.Quantity);


			var cell = CreateCell(_moneyStyle);

			if (item.Price != null)
				cell.setCellValue((double)item.Price.Amount);


			cell = CreateCell(_moneyStyle);

			if (item.Total != null)
				cell.setCellValue((double)item.Total.Amount);

		}



		private void AddListTotals()
		{

			var fontBold = _workbook.createFont();
			fontBold.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);

			var moneyStyle = _workbook.createCellStyle();
			moneyStyle.cloneStyleFrom(_moneyStyle);
			moneyStyle.setFont(fontBold);
			moneyStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_BOTTOM);

			var totalStyle = _workbook.createCellStyle();
			totalStyle.setFont(fontBold);
			totalStyle.setAlignment(CellStyle.__Fields.ALIGN_RIGHT);
			totalStyle.setIndention(1);
			totalStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_BOTTOM);

			if (_order.Discount != null && _order.Discount.Amount > 0)
			{
				NewRow(24);

				SetCellValue(ReportRes.InvoicePrinter_Discount, 5, totalStyle);
				CreateCell(moneyStyle).setCellValue((double)(_order.Discount.Amount));
			}

			NewRow(24);

			SetCellValue(ReportRes.InvoicePrinter_InvoiceTotalWithVat, 5, totalStyle);
			CreateCell(moneyStyle).setCellValue(GetAmount(_order.Total));


			if (_order.Vat.Yes())
			{
				NewRow(24);

				SetCellValue(ReportRes.InvoicePrinter_Vat, 5, totalStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.Vat));
			}

			if (_showPaid && !Equals(_order.Total, _order.TotalDue))
			{
				NewRow(24);

				SetCellValue(ReportRes.InvoicePrinter_Paid, 5, totalStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.Paid));


				NewRow(24);

				SetCellValue(ReportRes.InvoicePrinter_TotalDue, 5, totalStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.TotalDue));


				NewRow(24);

				SetCellValue(ReportRes.InvoicePrinter_Vat, 5, totalStyle);
				CreateCell(moneyStyle).setCellValue(GetAmount(_order.VatDue));
			}

		}



		private void AddFooter()
		{

			var commonStyle = _workbook.createCellStyle();
			commonStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);

			var font = _workbook.createFont();
			font.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);

			var boldStyle = _workbook.createCellStyle();
			boldStyle.cloneStyleFrom(commonStyle);
			boldStyle.setFont(font);

			_currentRow += 3;

			NewRow();


			var total = _showPaid ? _order.TotalDue : _order.Total;

			if (total != null)
			{
				var totalSuffix = 
					_order.BankAccount?.TotalSuffix.Clip() ??
					(_order.Vat.No() ? $", {DomainRes.Common_WithoutVat}" : null)
				;
				SetCellValue(
					$"{ReportRes.InvoicePrinter_InvoiceTotal} {total.ToWords()}{totalSuffix}",
					boldStyle
				);
			}


			if (db.Configuration.VatRate > 0)
			{
				++_currentRow;
				NewRow();
				SetCellValue(ReportRes.InvoicePrinter_VatLawChanged, commonStyle);
			}

			++_currentRow;
			NewRow();

			SetCellValue(ReportRes.InvoicePrinter_DaysTillExpiration, commonStyle);

			++_currentRow;
			NewRow();

			MergeRegion(_currentRow, _currentRow, 0, 6);

			var style = _workbook.createCellStyle();
			style.setAlignment(CellStyle.__Fields.ALIGN_RIGHT);

			SetCellValue(string.Format(ReportRes.InvoicePrinter_SignedBy, _issuedBy), 0, style);


			var footerDetails = db.Configuration.InvoicePrinter_FooterDetails;

			if (footerDetails.Yes())
			{
				style = _workbook.createCellStyle();
				style.setAlignment(CellStyle.__Fields.ALIGN_LEFT);
				style.setVerticalAlignment(CellStyle.__Fields.ALIGN_GENERAL);
				style.setWrapText(true);
				
				font = _workbook.createFont();
				font.setFontHeight(220);
				//font.setColor(HSSFColor.RED.index);
				style.setFont(font);

				++_currentRow;
				NewRow();
				++_currentRow;
				NewRow(100);
				MergeRegion(_currentRow, _currentRow, 0, 6);
				SetCellValue(footerDetails, style);
			}

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

			_currentCol = -1;

			return row;
		}



		private void MergeRegion(int firstRow, int lastRow, int firstCol, int lastCol)
		{
			_sheet.addMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
		}

		private HSSFCell CreateCell(HSSFCellStyle style)
		{
			return CreateCell(++_currentCol, style);
		}



		private HSSFCell CreateCell(int col, HSSFCellStyle style)
		{
			_currentCol = col;

			var cell = _sheet.getRow(_currentRow).createCell(_currentCol);

			cell.setCellStyle(style);

			return cell;
		}



		private void SetCellValue(string value, HSSFCellStyle style)
		{
			SetCellValue(value, _currentCol + 1, style);
		}



		private void SetCellValue(string value, int col, HSSFCellStyle style)
		{
			_currentCol = col;

			var cell = CreateCell(col, style);

			if (value.Yes())
				cell.setCellValue(value);
		}



		private static double GetAmount(Money money)
		{
			if (money == null)
				return 0;

			return (double) money.Amount;
		}



		//---g

	}






	//===g



}