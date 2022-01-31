using Luxena.Travel.Domain;

using org.apache.poi.ss.usermodel;


namespace Luxena.Travel.Reports
{
	public class CashOrderFormUkrainian : ExcelForm, ICashOrderForm
	{
		public CashOrderFormUkrainian()
		{
			Margins = new[] { 0.2, 0.2, 0.2, 0.2 };
			Widths = new[] { 1.86, 0.86, 0.66, 0.66, 1.61, 1.61, 1.61, 1.11, 1.69, 0.5, 0.5, 1.08, 0.72, 1.58, 1.8, 2.10 };
		}

		public Domain.Domain db { get; set; }

		public byte[] Print(CashInOrderPayment payment)
		{
			_payment = payment;

			return Print();
		}

		protected override void GenerateContent()
		{
			AddCashOrder();

			DrawVerticalLine(9, 0, CurrentRow);

			AddReceipt();
		}

		private Organization Company => db.Configuration.Company;

		private void AddCashOrder()
		{
			AddOrderHeader();
			AddOrderTable();
			AddOrderContent();
		}

		private void AddOrderHeader()
		{
			var style = CreateStyle(8, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			var borderStyle = CloneStyle(style);
			borderStyle.setBorderBottom(DottedBorder);

			NewRow(1.66);
			MergeRegion(CurrentRow, CurrentRow, 5, 8);

			SetCellValue(ReportRes.CreditCashOrderPrinter_Header, 5, style);

			NewRow(0.5);
			MergeRegion(CurrentRow, CurrentRow, 5, 7, borderStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_Form, 5, borderStyle);

			NewRow(0.5);
			MergeRegion(CurrentRow, CurrentRow, 5, 7);

			SetCellValue(ReportRes.CreditCashOrderPrinter_IdCode, 5, style);
			SetCellValue(Company.Code, 8, borderStyle);

			NewRow(0.84);
			MergeRegion(CurrentRow, CurrentRow, 0, 6, borderStyle);

			SetCellValue(GetCompanyText(), 0, borderStyle);

			NewRow(0.34);
		}

		private void AddOrderTable()
		{
			NewRow(0.63);
			MergeRegion(CurrentRow, CurrentRow, 0, 8);

			var style = CreateStyle(13, true, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_BOTTOM);
			SetCellValue(string.Format(ReportRes.CreditCashOrderPrinter_Caption, _payment.DocumentNumber), style);

			NewRow();
			MergeRegion(CurrentRow, CurrentRow, 0, 8);

			style = CreateStyle(10, false, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_BOTTOM);
			SetCellValue(string.Format(ReportRes.CreditCashOrderPrinter_From, _payment.Date.ToString("dd MMMM yyyy")), style);

			style = CreateStyle(9, false, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_CENTER);
			style.setBorderTop(CellStyle.__Fields.BORDER_THIN);
			style.setBorderRight(CellStyle.__Fields.BORDER_THIN);
			style.setBorderBottom(CellStyle.__Fields.BORDER_THIN);
			style.setBorderLeft(CellStyle.__Fields.BORDER_THIN);

			NewRow(0.21);
			NewRow(1.5);

			MergeRegion(CurrentRow, CurrentRow, 0, 2, style);
			MergeRegion(CurrentRow, CurrentRow, 3, 4, style);
			MergeRegion(CurrentRow, CurrentRow, 5, 6, style);
			MergeRegion(CurrentRow, CurrentRow, 7, 8, style);

			SetCellValue(ReportRes.CreditCashOrderPrinter_CorrespondingAccount, 0, style);
			SetCellValue(ReportRes.CreditCashOrderPrinter_AnalizedAccountCode, 3, style);
			SetCellValue(ReportRes.CreditCashOrderPrinter_Total, 5, style);
			SetCellValue(ReportRes.CreditCashOrderPrinter_Code, 7, style);

			NewRow(0.7);

			MergeRegion(CurrentRow, CurrentRow, 0, 2, style);
			MergeRegion(CurrentRow, CurrentRow, 3, 4, style);
			MergeRegion(CurrentRow, CurrentRow, 5, 6, style);
			MergeRegion(CurrentRow, CurrentRow, 7, 8, style);

			SetCellValue(db.Configuration.IncomingCashOrderCorrespondentAccount, 0, style);

			SetCellValue(MoneyFormat(_payment.Amount), 5, style);

			NewRow();
		}

		private void AddOrderContent()
		{
			const double height = 0.65;

			var captionStyle = CreateStyle(10, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			var dataStyle = CreateStyle(10, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			dataStyle.setBorderBottom(DottedBorder);

			NewRow(height);
			MergeRegion(CurrentRow, CurrentRow, 0, 1);
			MergeRegion(CurrentRow, CurrentRow, 2, 8, dataStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_ReceivedFrom, captionStyle);
			SetCellValue(GetReceivedFrom(), 2, dataStyle);

			NewRow(1.47);
			MergeRegion(CurrentRow, CurrentRow, 1, 8, dataStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_Reason, captionStyle);
			SetCellValue(_payment.GetReason(), dataStyle);

			NewRow(1.1);
			MergeRegion(CurrentRow, CurrentRow, 1, 8, dataStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_TotalInWords, captionStyle);

			SetCellValue(_payment.Amount.ToWords(), dataStyle);

			NewRow(height);
			MergeRegion(CurrentRow, CurrentRow, 0, 3);
			MergeRegion(CurrentRow, CurrentRow, 4, 8, dataStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_Vat, captionStyle);
			SetCellValue(GetVatString(), 4, dataStyle);

			NewRow(height);
			MergeRegion(CurrentRow, CurrentRow, 1, 8, dataStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_Notes, captionStyle);

			NewRow(height);
			MergeRegion(CurrentRow, CurrentRow, 0, 3);
			MergeRegion(CurrentRow, CurrentRow, 4, 8, dataStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_MainAccountant, captionStyle);
			SetCellValue(db.Configuration.AccountantDisplayString ?? ReportRes.CreditCashOrderPrinter_NoProvided, 4, dataStyle);

			NewRow(height);
			MergeRegion(CurrentRow, CurrentRow, 0, 3);
			MergeRegion(CurrentRow, CurrentRow, 4, 8, dataStyle);

			SetCellValue(ReportRes.CreditCashOrderPrinter_Cashier, captionStyle);
			SetCellValue(_payment.RegisteredBy.NameForDocuments, 4, dataStyle);
		}

		private void AddReceipt()
		{
			var headerStyle = CreateStyle(14, true, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_BOTTOM);
			var headerStyleSmall = CreateStyle(10, false, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_TOP);

			var captionStyle = CreateStyle(8, true, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			var dataStyle = CreateStyle(8, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_BOTTOM);
			dataStyle.setBorderBottom(DottedBorder);

			CurrentRow = 3;
			MergeRegion(CurrentRow, CurrentRow, 11, 15, dataStyle);
			SetCellValue(GetCompanyText(), 11, dataStyle);

			CurrentRow += 2;
			MergeRegion(CurrentRow, CurrentRow, 11, 15);
			SetCellValue(ReportRes.CreditCashOrderPrinter_Receipt_Caption, 11, headerStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow + 2, 11, 15);
			SetCellValue(ReportRes.CreditCashOrderPrinter_Receipt_Caption1.Fill(_payment.DocumentNumber, _payment.Date.ToString("dd MMMM yyyy")), 11, headerStyleSmall);

			CurrentRow += 3;
			MergeRegion(CurrentRow, CurrentRow, 11, 15);
			SetCellValue(ReportRes.CreditCashOrderPrinter_ReceivedFrom, 11, captionStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow, 11, 15, dataStyle);
			SetCellValue(GetReceivedFrom(), 11, dataStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow, 11, 15);
			SetCellValue(ReportRes.CreditCashOrderPrinter_Reason, 11, captionStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow, 11, 15, dataStyle);
			SetCellValue(_payment.GetReason(), 11, dataStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow, 12, 15, dataStyle);
			SetCellValue(ReportRes.CreditCashOrderPrinter_TotalInWords, 11, captionStyle);

			SetCellValue(_payment.Amount.ToWords(), 12, dataStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow, 11, 12);
			MergeRegion(CurrentRow, CurrentRow, 13, 15, dataStyle);
			SetCellValue(ReportRes.CreditCashOrderPrinter_Vat_Short, 11, captionStyle);
			SetCellValue(GetVatString(), 13, dataStyle);

			++CurrentRow;
			SetCellValue(ReportRes.CreditCashOrderPrinter_MP, 11, captionStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow, 11, 13);
			MergeRegion(CurrentRow, CurrentRow, 14, 15, dataStyle);
			SetCellValue(ReportRes.CreditCashOrderPrinter_MainAccountant, 11, captionStyle);
			SetCellValue(db.Configuration.AccountantDisplayString ?? ReportRes.CreditCashOrderPrinter_NoProvided, 14, dataStyle);

			++CurrentRow;
			MergeRegion(CurrentRow, CurrentRow, 12, 15, dataStyle);
			SetCellValue(ReportRes.CreditCashOrderPrinter_Cashier_Short, 11, captionStyle);
			SetCellValue(_payment.RegisteredBy.NameForDocuments, dataStyle);
		}

		private string GetCompanyText()
		{
			string adress = null;

			if (_payment.Owner != null)
			{
				if (_payment.Owner.LegalAddress.Yes())
					adress = _payment.Owner.LegalAddress;
			}
			else if (Company.LegalAddress.Yes())
				adress = Company.LegalAddress;

			return adress != null ? string.Format("{0} ({1})", Company.NameForDocuments, adress) : Company.NameForDocuments;
		}

		private string GetVatString()
		{
			return _payment.Vat == null ? null : MoneyFormat(_payment.Vat);
		}

		private string GetReceivedFrom()
		{
			return _payment.ReceivedFrom ?? _payment.Payer.NameForDocuments;
		}

		private const short DottedBorder = CellStyle.__Fields.BORDER_DOTTED;

		private CashInOrderPayment _payment;
	}
}