using Luxena.Travel.Domain;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;

using HeaderFooter = org.apache.poi.hssf.usermodel.HeaderFooter;


namespace Luxena.Travel.Reports
{

	public class ConsignmentForm : ExcelForm
	{

		public string ServiceFeeTitle { get; set; }


		public byte[] Build(Domain.Domain db, Consignment consignment)
		{
			if (consignment == null)
				return null;

			_consignment = consignment;

			_vatRate = db.Configuration.VatRate;
			_separateBookingFee = db.Configuration.Consignment_SeparateBookingFee;

			Margins = new[] { 0.2, 0.2, 0.2, 0.2 };
			Widths = new[] { 1.5, 2.5, 6.43, 18, 6.0, 7.29, 9.29, 10.29, 0.5, 1.5, 1.5, 0.5, 2.5, 6.43, 18, 6.0, 7.29, 9.29, 10.29, 1.5 };

			_columnOffsetCount = 11;

			SetWidthCoef(1.09 * 256);

			return Print();
		}

		protected override void GenerateContent()
		{
			PrinterSetup();
			AddConsignment();
		}

		private void AddConsignment()
		{
			AddConsignmentHeader();
			AddConsignmentBody();
			AddConsignmentBottom();
			DrawVerticalLine(9, 0, CurrentRow);
		}

		private void PrinterSetup()
		{
			var footer = Sheet.getFooter();
			footer.setRight(string.Format(ReportRes.ConsignmentPrinter_Footer, _consignment.Number, HeaderFooter.page(), HeaderFooter.numPages()));

			var printSetup = Sheet.getPrintSetup();
			printSetup.setLandscape(true);

			SetPageBreaks();
		}

		private void SetPageBreaks()
		{
			var count = _consignment.OrderItems.Count;

			if (count == 5 || count == 6)
				Sheet.setRowBreak(17);
			else if (count == 7)
				Sheet.setRowBreak(14);
			else if (count == 8)
				Sheet.setRowBreak(15);
			else if (count == 15 || count == 16)
				Sheet.setRowBreak(27);
			else if (count == 17)
				Sheet.setRowBreak(24);
			else if (count >= 18)
				Sheet.setRowBreak(25);
		}

		private void AddConsignmentHeader()
		{
			NewRow(0.5);

			var style = CreateStyle(14, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_CENTER);

			NewRow(0.84);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 3);

			SetCellValueMirrored(string.Format(ReportRes.ConsignmentReport_Consignment), 1, style);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_CENTER);

			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderBottom(CellStyle.__Fields.BORDER_MEDIUM);

			var borderStyle = CloneStyle(style);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			borderStyle.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			borderStyle.setBorderBottom(CellStyle.__Fields.BORDER_MEDIUM);

			MergeRegionMirrored(CurrentRow, CurrentRow, 4, 4, borderStyle);


			var numberStyle = CloneStyle(style);
			numberStyle.setFont(CreateFont(8, true));
			SetCellValueMirrored(_consignment.Number, 4, numberStyle);

			borderStyle = CloneStyle(borderStyle);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_NONE);
			var dateParts = $"{_consignment.IssueDate:%d MMMM yyyy}".Split(' ');
			SetCellValueMirrored(dateParts[0], 5, borderStyle);
			borderStyle = CloneStyle(borderStyle);
			borderStyle.setBorderLeft(CellStyle.__Fields.BORDER_NONE);
			SetCellValueMirrored(dateParts[1].ToLower(), 6, borderStyle);
			borderStyle = CloneStyle(borderStyle);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			SetCellValueMirrored(dateParts[2] + ' ' + ReportRes.YearAbbr, 7, borderStyle);

			NewRow(0.4);

			NewRow(0.8);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			borderStyle = CloneStyle(style);

			MergeRegionMirrored(CurrentRow, CurrentRow, 2, 3, borderStyle);
			SetCellValueMirrored(ReportRes.ConsignmentReport_Supplier, 2, style);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderBottom(CellStyle.__Fields.BORDER_NONE);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			SetCellValueMirrored(string.Empty, 1, style);

			style = CreateStyle(10, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			borderStyle = CloneStyle(style);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);

			MergeRegionMirrored(CurrentRow, CurrentRow, 4, 7, borderStyle);
			SetCellValueMirrored(_consignment.Supplier != null ? _consignment.Supplier.NameForDocuments : string.Empty, 4, style);

			NewRow(0.8);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			style.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			borderStyle = CloneStyle(style);

			MergeRegionMirrored(CurrentRow, CurrentRow, 2, 3, borderStyle);
			SetCellValueMirrored(ReportRes.ConsignmentReport_Acquirer, 2, style);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderBottom(CellStyle.__Fields.BORDER_NONE);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderTop(CellStyle.__Fields.BORDER_NONE);
			SetCellValueMirrored(string.Empty, 1, style);

			style = CreateStyle(10, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			style.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			borderStyle = CloneStyle(style);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);

			MergeRegionMirrored(CurrentRow, CurrentRow, 4, 7, borderStyle);
			SetCellValueMirrored(_consignment.Acquirer != null ? _consignment.Acquirer.NameForDocuments : string.Empty, 4, style);

			NewRow(0.2);

			borderStyle = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			borderStyle.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			borderStyle.setBorderBottom(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 7, borderStyle);

			NewRow(0.4);
		}

		private void AddConsignmentBody()
		{
			if (_consignment.OrderItems.Count == 0)
				return;

			NewRow(1.0);

			var style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_TOP);
			var borderStyle = CloneStyle(style);
			borderStyle.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			borderStyle.setBorderBottom(CellStyle.__Fields.BORDER_MEDIUM);
			borderStyle.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			SetCellValueMirrored(CommonRes.Number_Short, 1, borderStyle);

			style = CloneStyle(style);
			style.setAlignment(CellStyle.__Fields.ALIGN_CENTER);
			borderStyle = CloneStyle(style);
			borderStyle.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			borderStyle.setBorderBottom(CellStyle.__Fields.BORDER_MEDIUM);
			borderStyle.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 2, 3, borderStyle);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Item_Name, 2, borderStyle);

			SetCellValueMirrored(string.Empty, 4, borderStyle);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Item_Quantity, 5, borderStyle);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Item_Price, 6, borderStyle);
			borderStyle = CloneStyle(borderStyle);
			borderStyle.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Item_Total, 7, borderStyle);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_TOP);
			style.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			style.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			style.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			style.setBorderTop(CellStyle.__Fields.BORDER_THIN);

			var styleLeftBorder = CloneStyle(style);
			styleLeftBorder.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);

			var styleRightBorder = CloneStyle(style);
			styleRightBorder.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);

			var styleAlignLeft = CloneStyle(style);
			styleAlignLeft.setAlignment(CellStyle.__Fields.ALIGN_LEFT);

			var styleAlignLeftItalic = CloneStyle(styleAlignLeft);
			var font = Workbook.createFont();
			font.setItalic(true);
			styleAlignLeftItalic.setFont(font);

			var pos = 0;

			foreach (var item in _consignment.OrderItems)
			{
				pos++;
				NewRow(1.9);
				SetCellValueMirrored(pos.ToString(), 1, styleLeftBorder);
				MergeRegionMirrored(CurrentRow, CurrentRow, 2, 3, styleAlignLeft);

				bool hasVat;

				SetCellValueOrderItemText(item, styleAlignLeftItalic, styleAlignLeft, out hasVat);

				SetCellValueMirrored(ReportRes.ConsignmentPrinter_Piece, 4, style);
				SetCellValueMirrored(item.Quantity.ToString(), 5, style);

				var bookingFee = item.Product?.BookingFee;
				var withBookingFee = _separateBookingFee && bookingFee.Yes() && (item.IsFullDocument || item.IsProductData);

				var total = hasVat ? item.Price.Amount * 100 / (100 + _vatRate) : item.Price.AsAmount();
				var totalWOVat = hasVat ? item.Total.AsAmount() * 100 / (100 + _vatRate) : item.Total.AsAmount();

				if (withBookingFee)
				{
					total -= bookingFee.Amount;
					totalWOVat -= bookingFee.Amount;
				}

				SetCellValueMirrored(total.ToMoneyString(), 6, style);
				SetCellValueMirrored(totalWOVat.ToMoneyString(), 7, styleRightBorder);

				if (withBookingFee)
				{
					pos++;
					NewRow(1.9);
					SetCellValueMirrored(pos.ToString(), 1, styleLeftBorder);
					MergeRegionMirrored(CurrentRow, CurrentRow, 2, 3, styleAlignLeft);

					SetCellValueMirrored(ReportRes.ConsignmentPrinter_BookingFee, 2, styleAlignLeft);

					SetCellValueMirrored(ReportRes.ConsignmentPrinter_Piece, 4, style);
					SetCellValueMirrored(item.Quantity.ToString(), 5, style);
					SetCellValueMirrored(bookingFee.Amount.ToMoneyString(), 6, style);
					SetCellValueMirrored(bookingFee.Amount.ToMoneyString(), 7, styleRightBorder);
				}
			}

			NewRow(0.8);

			style = CreateStyle(11, true, CellStyle.__Fields.ALIGN_RIGHT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			style.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 6, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Discount, 1, style);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			style.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			SetCellValueMirrored(((_consignment.Discount.Amount * 100) / (100 + _vatRate)).ToMoneyString(), 7, style);
			NewRow(0.8);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			style.setBorderTop(CellStyle.__Fields.BORDER_NONE);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 6, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_TotalWithoutVat, 1, style);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			style.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			SetCellValueMirrored(_consignment.Total.Amount.ToMoneyString(), 7, style);

			NewRow(0.8);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			style.setBorderTop(CellStyle.__Fields.BORDER_NONE);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 6, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Vat, 1, style);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			style.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			style.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			SetCellValueMirrored(_consignment.Vat.Amount.ToMoneyString(), 7, style);

			NewRow(0.8);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			style.setBorderTop(CellStyle.__Fields.BORDER_NONE);
			style.setBorderBottom(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 6, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_TotalWithVat, 1, style);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_THIN);
			style.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			SetCellValueMirrored(_consignment.GrandTotal.Amount.ToMoneyString(), 7, style);
		}

		private void AddConsignmentBottom()
		{
			NewRow(0.2);

			NewRow(2.4);

			var style = CreateStyle(10, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 3, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_TotalSupplied, 1, style);

			var styleItalicUnderline = CreateStyle(13, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			var font = CreateFont(12, false);
			font.setItalic(true);
			font.setUnderline(Font.__Fields.U_SINGLE);
			styleItalicUnderline.setFont(font);
			MergeRegionMirrored(CurrentRow, CurrentRow, 4, 7);
			SetCellValueMirrored(_consignment.TotalSupplied, 4, styleItalicUnderline);

			NewRow(0.5);

			style = CloneStyle(style);
			font = style.getFont(Workbook);
			font.setItalic(true);
			style.setFont(font);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 2);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_OnSum, 1, style);

			styleItalicUnderline = CloneStyle(styleItalicUnderline);
			font = CreateFont(9, false);
			font.setItalic(true);
			font.setUnderline(Font.__Fields.U_SINGLE);
			styleItalicUnderline.setFont(font);
			MergeRegionMirrored(CurrentRow, CurrentRow, 3, 7);
			SetCellValueMirrored(_consignment.GrandTotal.ToWords(), 3, styleItalicUnderline);

			NewRow(0.5);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 4, 7);
			SetCellValueMirrored(string.Format(ReportRes.ConsignmentPrinter_IncludingVat, MoneyFormat(_consignment.Vat)), 4, style);

			var address = _consignment?.Order?.Owner.As(a => a.ActualAddress ?? a.LegalAddress);
			if (address.Yes())
			{
				NewRow(0.5);
				MergeRegionMirrored(CurrentRow, CurrentRow, 1, 7);
				SetCellValueMirrored($"Місце складання: {address}", 1, style);
			}
			else
				NewRow(0.2);

			NewRow(0.8);

			style = CreateStyle(11, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderTop(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 3, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Head, 1, style);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_NONE);
			style.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 4, 7, style);
			//SetCellValueMirrored(ReportRes.ConsignmentPrinter_Accountant, 4, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Head, 4, style);

			NewRow(0.5);
			style = CreateStyle(10, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 7, style);
			NewRow(0.8);

			style = CreateStyle(11, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 3, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Supplied, 1, style);

			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_NONE);
			style.setBorderRight(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 4, 7, style);
			SetCellValueMirrored(ReportRes.ConsignmentPrinter_Acquired, 4, style);

			NewRow(0.2);
			style = CloneStyle(style);
			style.setBorderLeft(CellStyle.__Fields.BORDER_MEDIUM);
			style.setBorderBottom(CellStyle.__Fields.BORDER_MEDIUM);
			MergeRegionMirrored(CurrentRow, CurrentRow, 1, 7, style);

		}

		private void SetCellValueOrderItemText(OrderItem item, HSSFCellStyle styleAlignLeftItalic, HSSFCellStyle styleAlignLeft, out bool hasVat)
		{
			hasVat = item.HasVat;

			var text = item.Text;

			if (item.Product != null && item.IsServiceFee)
			{
				text =
					ServiceFeeTitle.Yes() ? ServiceFeeTitle :
					item.Product.IsAviaTicket ? ReportRes.ConsignmentPrinter_ServiceFee_Ticket :
					item.Product.IsAviaMco ? ReportRes.ConsignmentPrinter_ServiceFee_Mco :
					item.Product.IsRefund
						? ReportRes.ConsignmentPrinter_ServiceFee_Refund
						: ReportRes.ConsignmentPrinter_ServiceFee;
			}

			SetCellValueMirrored(text, 2, styleAlignLeft);
		}

		private void SetCellValueMirrored(string value, int col, HSSFCellStyle style)
		{
			SetCellValue(value, col, style);
			SetCellValue(value, col + _columnOffsetCount, style);
		}

		private void MergeRegionMirrored(int firstRow, int lastRow, int firstCol, int lastCol, HSSFCellStyle style)
		{
			MergeRegion(firstRow, lastRow, firstCol, lastCol, style);
			MergeRegion(firstRow, lastRow, firstCol + _columnOffsetCount, lastCol + _columnOffsetCount, style);
		}

		private void MergeRegionMirrored(int firstRow, int lastRow, int firstCol, int lastCol)
		{
			MergeRegion(firstRow, lastRow, firstCol, lastCol);
			MergeRegion(firstRow, lastRow, firstCol + _columnOffsetCount, lastCol + _columnOffsetCount);
		}

		private int _columnOffsetCount;
		private Consignment _consignment;
		private decimal _vatRate;
		private bool _separateBookingFee;
	}

}
