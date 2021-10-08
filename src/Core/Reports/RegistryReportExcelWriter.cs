using System.Globalization;
using System.IO;

using java.io;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;

using CellRangeAddress = org.apache.poi.ss.util.CellRangeAddress;


namespace Luxena.Travel.Reports
{

	public class RegistryReportExcelWriter : IRegistryReportWriter
	{
		public RegistryReportExcelWriter(Stream stream)
		{
			_wb = new HSSFWorkbook();

			_sheet = _wb.createSheet(ReportRes.RegistryReport_Title);

			_currentStyle = _wb.createCellStyle();

			_currentStyle.setWrapText(true);
			_currentStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);

			_stream = stream;
		}

		public void Close()
		{
			using (var outputStream = new ByteArrayOutputStream())
			{
				_wb.write(outputStream);

				var bytes = outputStream.toByteArray();

				_stream.Write(bytes, 0, bytes.Length);
			}
		}

		public void WriteReportTitle(string title)
		{
			var row = CreateRow();
			row.setHeightInPoints(35);

			var cell = CreateCell();

			Font font = _wb.createFont();
			font.setFontHeightInPoints(20);

			var style = CloneCurrentStyle();
			style.setAlignment(CellStyle.__Fields.ALIGN_CENTER);
			style.setFont(font);

			cell.setCellStyle(style);

			cell.setCellValue(title);


			_sheet.addMergedRegion(new CellRangeAddress(_currentRowIndex, _currentRowIndex, 0, LastColumnIndex));
		}

		public void WriteFilterString(string filter)
		{
			var row = CreateRow();
			row.setHeightInPoints(40);

			var cell = CreateCell();

			Font font = _wb.createFont();
			font.setFontHeightInPoints(10);

			var style = CloneCurrentStyle();
			style.setAlignment(CellStyle.__Fields.ALIGN_CENTER);
			style.setFont(font);

			cell.setCellStyle(style);

			cell.setCellValue(filter);

			_sheet.addMergedRegion(new CellRangeAddress(_currentRowIndex, _currentRowIndex, 0, LastColumnIndex));
		}

		public void SetTextAlign(TextAlign align)
		{
			_currentStyle = CloneCurrentStyle();

			switch (align)
			{
				case TextAlign.Center:
					_currentStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);

					break;

				case TextAlign.Left:
					_currentStyle.setAlignment(CellStyle.__Fields.ALIGN_LEFT);

					break;
				case TextAlign.Right:
					_currentStyle.setAlignment(CellStyle.__Fields.ALIGN_RIGHT);

					break;
			}
		}

		public void SetTextFont(int size, bool bold, bool italic)
		{
			Font font = _wb.createFont();

			if (bold)
				font.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);

			font.setItalic(italic);

			_currentStyle = CloneCurrentStyle();

			_currentStyle.setFont(font);
		}

		public void SetCellBorder(BorderStyle borderStyle)
		{
			_currentStyle = CloneCurrentStyle();

			_currentStyle.setBorderTop(GetBorder(borderStyle, BorderStyle.Top));
			_currentStyle.setBorderLeft(GetBorder(borderStyle, BorderStyle.Left));
			_currentStyle.setBorderRight(GetBorder(borderStyle, BorderStyle.Right));
			_currentStyle.setBorderBottom(GetBorder(borderStyle, BorderStyle.Bottom));
		}

		public void SetCellColor(int red, int green, int blue)
		{
		}

		public void BeginDocumentTable()
		{
			_sheet.setColumnWidth(0, 15 * 256);
			_sheet.setColumnWidth(1, 11 * 256);
			_sheet.setColumnWidth(2, 26 * 256);
			_sheet.setColumnWidth(3, 11 * 256);
			_sheet.setColumnWidth(4, 10 * 256);
			_sheet.setColumnWidth(5, 11 * 256);
			_sheet.setColumnWidth(6, 11 * 256);
			_sheet.setColumnWidth(7, 11 * 256);
			_sheet.setColumnWidth(8, 10 * 256);
			_sheet.setColumnWidth(9, 10 * 256);
			_sheet.setColumnWidth(10, 12 * 256);
			_sheet.setColumnWidth(11, 20 * 256);
		}

		public void EndDocumentTableHeaders()
		{
		}

		public void EndDocumentTableContent()
		{
			_isDocumentsTableFooter = true;

			_currentStyle = CloneCurrentStyle();

			_currentStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_BOTTOM);
		}

		public void BeginDocumentTableRow()
		{
			var row = CreateRow();

			if (_isDocumentsTableFooter)
				row.setHeightInPoints(20);
		}

		public void WriteDocumentTableCell(string text)
		{
			var cell = CreateCell();

			decimal val;

			if (decimal.TryParse(text, NumberStyles.Any, _culture, out val))
				_currentStyle.setDataFormat(_wb.createDataFormat().getFormat("0.00"));
			else
				_currentStyle.setDataFormat(_wb.createDataFormat().getFormat("text"));

			cell.setCellValue(text);
		}

		public void WriteDocumentTableCell(string text, bool isValid)
		{
			var style = _currentStyle;

			if (!isValid)
			{
				Font font = _wb.createFont();
				font.setColor(Font.__Fields.COLOR_RED);

				_currentStyle = CloneCurrentStyle();
				_currentStyle.setFont(font);
			}

			var cell = CreateCell();

			decimal val;

			if (decimal.TryParse(text, NumberStyles.Any, _culture, out val))
				_currentStyle.setDataFormat(_wb.createDataFormat().getFormat("0.00"));
			else
				_currentStyle.setDataFormat(_wb.createDataFormat().getFormat("text"));

			cell.setCellValue(text);

			_currentStyle = style;
		}

		public void EndDocumentTable()
		{
			_isDocumentsTableFooter = false;
		}

		public void WriteFooterText(string text)
		{
			SetCellBorder(BorderStyle.None);
			SetTextAlign(TextAlign.Left);

			_currentRowIndex += 2;

			CreateRow();

			_currentStyle = CloneCurrentStyle();
			_currentStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_TOP);

			var cell = CreateCell();
			cell.setCellValue(text);

			_sheet.addMergedRegion(new CellRangeAddress(_currentRowIndex, _currentRowIndex + 4, 0, 7));
		}

		public void WriteOperationsTable(string[,] data)
		{
			_currentStyle = CloneCurrentStyle();
			_currentStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_BOTTOM);

			SetCellBorder(BorderStyle.Bottom);

			for (var i = 0; i < 4; i++)
			{
				if (i > 0)
					CreateRow();

				var row = _sheet.getRow(_currentRowIndex);
				row.setHeightInPoints(20);

				_currentCellIndex = 8;

				SetTextAlign(TextAlign.Left);

				var cell = CreateCell();
				cell.setCellValue(data[i, 0]);

				SetTextAlign(TextAlign.Right);

				cell = CreateCell();
				cell.setCellValue(data[i, 1]);

				SetCellBorder(BorderStyle.None);
			}
		}

		public void BeginFooter()
		{

		}

		public void EndFooter()
		{

		}

		private static short GetBorder(BorderStyle borderStyle, BorderStyle border)
		{
			if ((borderStyle & border) == border)
				return CellStyle.__Fields.BORDER_THIN;

			return CellStyle.__Fields.BORDER_NONE;
		}

		private HSSFRow CreateRow()
		{
			++_currentRowIndex;

			var row = _sheet.getRow(_currentRowIndex) ?? _sheet.createRow(_currentRowIndex);

			_currentCellIndex = -1;

			return row;
		}

		private HSSFCell CreateCell()
		{
			var cell = _sheet.getRow(_currentRowIndex).createCell(++_currentCellIndex);

			cell.setCellStyle(_currentStyle);
			return cell;
		}

		private HSSFCellStyle CloneCurrentStyle()
		{
			var style = _wb.createCellStyle();
			style.cloneStyleFrom(_currentStyle);

			return style;
		}

		private static readonly CultureInfo _culture = CultureInfo.GetCultureInfo("ru-RU");

		private int _currentRowIndex = -1;
		private int _currentCellIndex = -1;

		private readonly HSSFWorkbook _wb;
		private readonly HSSFSheet _sheet;

		private HSSFCellStyle _currentStyle;

		private const int LastColumnIndex = 11;
		private readonly Stream _stream;
		private bool _isDocumentsTableFooter;
	}

}