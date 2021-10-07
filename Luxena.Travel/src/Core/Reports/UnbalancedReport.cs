using System;
using System.Collections.Generic;

using Luxena.Travel.Domain;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;


namespace Luxena.Travel.Reports
{
	public class UnbalancedReport : ExcelForm
	{
		public DateTime? DateTo { get; set; }

		public IEnumerable<PartyBalance> Data { get; set; }

		public bool IncludeOrders { get; set; }

		public byte[] Build()
		{
			Widths = IncludeOrders ? _widthsWithOrders : _widthsWithoutOrders;

			return Print();
		}

		protected override void GenerateContent()
		{
			CreateStyles();

			NewRow();
			CreateCell(_hstyle).setCellValue(DomainRes.Party);
			if (IncludeOrders)
			{
				CreateCell(_hstyle).setCellValue(DomainRes.Order);
				CreateCell(_hstyle).setCellValue(DomainRes.Common_Owner);
			}
			CreateCell(_hstyle).setCellValue("Дата первой операции");
			CreateCell(_hstyle).setCellValue("Дата последней операции");
			CreateCell(_hstyle).setCellValue(DomainRes.OpeningBalance);
			CreateCell(_hstyle).setCellValue("Оказано услуг на сумму");
			CreateCell(_hstyle).setCellValue(DomainRes.Order_Paid);
			CreateCell(_hstyle).setCellValue(DomainRes.Order_DeliveryBalance);

			foreach (var partyBalance in Data)
			{
				_current = IncludeOrders ? _bordered : _normal;

				NewRow();

				CreateCell(_current.TextStyle).setCellValue(partyBalance.Party.Name);

				if (IncludeOrders)
				{
					CreateCell(_current.TextStyle);
					CreateCell(_current.TextStyle);
					MergeRegion(CurrentRow, CurrentRow, 0, 2);
				}

				CreateDateCell(partyBalance.FirstDocumentDate);
				CreateDateCell(partyBalance.LastDocumentDate);

				if (partyBalance.OpeningBalance == null)
					CreateCell(_current.PositiveNumberStyle);
				else
					CreateDecimalCell(partyBalance.OpeningBalance.Balance);

				CreateDecimalCell(partyBalance.Delivered);
				CreateDecimalCell(partyBalance.Paid);
				CreateDecimalCell(partyBalance.Overall);


				if (IncludeOrders && partyBalance.ByOrders != null)
				{
					_current = _normal;

					foreach (var orderBalance in partyBalance.ByOrders)
					{
						NewRow();
						CreateCell(1, _current.TextStyle).setCellValue(orderBalance.Order == null ? "Без заказа" : orderBalance.Order.Name);
						CreateCell(_current.TextStyle).setCellValue(orderBalance.Owner);
						CreateDateCell(orderBalance.FirstDocumentDate);
						CreateDateCell(orderBalance.LastDocumentDate);
						CurrentCol++;
						CreateDecimalCell(orderBalance.Delivered);
						CreateDecimalCell(orderBalance.Paid);
						CreateDecimalCell(orderBalance.Balance);
					}
				}
			}
		}

		private void CreateStyles()
		{
			var numberFormat = Workbook.createDataFormat().getFormat("#,##0.00");

			var redFont = CreateFont(12, false);
			redFont.setColor(IndexedColors.RED.getIndex());

			_hstyle = CreateStyle(12, true, CellStyle.__Fields.ALIGN_CENTER, CellStyle.__Fields.VERTICAL_CENTER);

			_normal = new RowStyles();

			_normal.TextStyle = CreateStyle(12, false, CellStyle.__Fields.ALIGN_LEFT, CellStyle.__Fields.VERTICAL_CENTER);

			_normal.PositiveNumberStyle = CreateStyle(12, false, CellStyle.__Fields.ALIGN_RIGHT, CellStyle.__Fields.VERTICAL_CENTER);
			_normal.PositiveNumberStyle.setDataFormat(numberFormat);

			_normal.NegativeNumberStyle = CreateStyle(redFont, CellStyle.__Fields.ALIGN_RIGHT, CellStyle.__Fields.VERTICAL_CENTER);
			_normal.NegativeNumberStyle.setDataFormat(numberFormat);

			_bordered = new RowStyles();

			_bordered.TextStyle = CloneStyle(_normal.TextStyle);
			_bordered.TextStyle.setBorderTop(2);

			_bordered.PositiveNumberStyle = CloneStyle(_normal.PositiveNumberStyle);
			_bordered.PositiveNumberStyle.setBorderTop(2);

			_bordered.NegativeNumberStyle = CloneStyle(_normal.NegativeNumberStyle);
			_bordered.NegativeNumberStyle.setBorderTop(2);
		}

		private void CreateDecimalCell(decimal value)
		{
			CreateCell(value < 0 ? _current.NegativeNumberStyle : _current.PositiveNumberStyle).setCellValue((double) value);
		}

		private void CreateDateCell(DateTime? value)
		{
			var cell = CreateCell(_current.TextStyle);

			if (value.HasValue)
				cell.setCellValue(value.Value.ToString("dd.MM.yyyy"));
		}

		private class RowStyles
		{
			public HSSFCellStyle TextStyle;
			public HSSFCellStyle PositiveNumberStyle;
			public HSSFCellStyle NegativeNumberStyle;
		}

		private static readonly double[] _widthsWithoutOrders = { 12, 4, 4, 3.5, 3.5, 3.5, 3.5 };
		private static readonly double[] _widthsWithOrders = { 12, 4, 8, 4, 4, 3.5, 3.5, 3.5, 3.5 };

		private HSSFCellStyle _hstyle;
		private RowStyles _normal;
		private RowStyles _bordered;
		private RowStyles _current;
	}
}