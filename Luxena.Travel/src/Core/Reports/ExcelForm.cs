using java.io;

using Luxena.Travel.Domain;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;
using org.apache.poi.ss.util;


namespace Luxena.Travel.Reports
{
	public abstract class ExcelForm
	{
		protected HSSFWorkbook Workbook { get; set; }

		protected HSSFSheet Sheet { get; set; }

		protected int CurrentRow { get; set; }

		protected int CurrentCol { get; set; }

		protected double[] Margins { get; set; }

		protected double[] Widths { get; set; }

		protected byte[] Print()
		{
			CreateWorkbook();

			SetColumnWidths();

			GenerateContent();

			var bytes = GetBytes();

			return bytes;
		}

		protected abstract void GenerateContent();

		protected void CreateWorkbook()
		{
			Workbook = new HSSFWorkbook();

			Sheet = Workbook.createSheet();

			if (Margins != null && Margins.Length == 4)
			{
				Sheet.setMargin(org.apache.poi.ss.usermodel.Sheet.__Fields.TopMargin, Margins[0]);
				Sheet.setMargin(org.apache.poi.ss.usermodel.Sheet.__Fields.RightMargin, Margins[1]);
				Sheet.setMargin(org.apache.poi.ss.usermodel.Sheet.__Fields.BottomMargin, Margins[2]);
				Sheet.setMargin(org.apache.poi.ss.usermodel.Sheet.__Fields.LeftMargin, Margins[3]);
			}

			CurrentRow = -1;
			CurrentCol = -1;
		}

		protected void SetColumnWidths()
		{
			if (Widths == null)
				return;

			for (var index = 0; index < Widths.Length; index++)
				Sheet.setColumnWidth(index, (int) (Widths[index]*_widthCoef));
		}

		protected void MergeRegion(int firstRow, int lastRow, int firstCol, int lastCol)
		{
			Sheet.addMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
		}

		protected void MergeRegion(int firstRow, int lastRow, int firstCol, int lastCol, HSSFCellStyle style)
		{
			for (var i = firstRow; i <= lastRow; i++)
			{
				var row = GetRow(i);

				for (var j = firstCol; j <= lastCol; j++)
				{
					var cell = row.createCell(j);
					cell.setCellStyle(style);
				}
			}

			Sheet.addMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
		}

		private HSSFRow GetRow(int row)
		{
			return Sheet.getRow(row) ?? Sheet.createRow(row);
		}

		protected HSSFRow NewRow()
		{
			++CurrentRow;

			CurrentCol = -1;

			return GetRow(CurrentRow);
		}

		protected void NewRow(double height)
		{
			var row = NewRow();

			row.setHeightInPoints((float) (height*HeightCoef));
		}

		protected HSSFCell CreateCell(HSSFCellStyle style)
		{
			return CreateCell(++CurrentCol, style);
		}

		protected HSSFCell CreateCell(int col, HSSFCellStyle style)
		{
			CurrentCol = col;

			var cell = Sheet.getRow(CurrentRow).createCell(CurrentCol);

			cell.setCellStyle(style);

			return cell;
		}

		protected HSSFCell SetCellValue(string value, HSSFCellStyle style)
		{
			return SetCellValue(value, CurrentCol + 1, style);
		}

		protected HSSFCell SetCellValue(string value, int col, HSSFCellStyle style)
		{
			CurrentCol = col;

			var cell = CreateCell(col, style);

			if (value.Yes())
			{
				value = value.Replace("\r", string.Empty);
				cell.setCellValue(value);
			}

			return cell;
		}

		protected HSSFFont CreateFont(short fontHeight, bool fontBold)
		{
			var font = Workbook.createFont();

			font.setFontHeightInPoints(fontHeight);

			if (fontBold)
				font.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);

			return font;
		}

		protected HSSFCellStyle CreateStyle(HSSFFont font, short halign, short valign)
		{
			var style = Workbook.createCellStyle();

			style.setFont(font);
			style.setVerticalAlignment(valign);
			style.setAlignment(halign);
			style.setWrapText(true);

			return style;
		}

		protected HSSFCellStyle CreateStyle(short fontHeight, bool fontBold, short halign, short valign)
		{
			var font = CreateFont(fontHeight, fontBold);

			return CreateStyle(font, halign, valign);
		}

		protected HSSFCellStyle CloneStyle(HSSFCellStyle source)
		{
			var style = Workbook.createCellStyle();

			style.cloneStyleFrom(source);

			return style;
		}

		protected byte[] GetBytes()
		{
			byte[] bytes;

			using (var outputStream = new ByteArrayOutputStream())
			{
				Workbook.write(outputStream);

				bytes = outputStream.toByteArray();
			}

			return bytes;
		}

		protected void DrawHorizotalLine(int row, int startCol, int endCol)
		{
			var style = Workbook.createCellStyle();

			style.setBorderBottom(CellStyle.__Fields.BORDER_DOTTED);

			for (var i = startCol; i <= endCol; i++)
			{
				var cell = Sheet.getRow(row).createCell(i);

				cell.setCellStyle(style);
			}
		}

		protected void DrawVerticalLine(int col, int startRow, int endRow)
		{
			var style = Workbook.createCellStyle();

			style.setBorderRight(CellStyle.__Fields.BORDER_DOTTED);

			for (var i = startRow; i <= endRow; i++)
			{
				var cell = Sheet.getRow(i).createCell(col);

				cell.setCellStyle(style);
			}
		}

		protected static string MoneyFormat(Money money)
		{
			return money == null ? string.Empty : string.Format("{0} {1}", money.Amount.ToMoneyString(),
				ReportRes.ResourceManager.GetString(string.Format("Common_{0}_Short", money.Currency.Code)));
		}

		protected void SetWidthCoef(double coef)
		{
			_widthCoef = coef;
		}

		private const double HeightCoef = 27.7;
		private double _widthCoef = 5.15*256;
	}
}