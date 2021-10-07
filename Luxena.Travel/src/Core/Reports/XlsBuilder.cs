using System.IO;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;


namespace Luxena.Travel.Reports
{
	public class XlsBuilder
	{
		public XlsBuilder()
		{
			_workbook = new HSSFWorkbook();
		}

		public byte[] GetBytes()
		{
			using (var ms = new MemoryStream())
			{
				_workbook.Write(ms);

				return ms.ToArray();
			}
		}

		public XlsBuilder NewSheet(string name)
		{
			_sheet = _workbook.CreateSheet(name);

			return this;
		}

		public XlsBuilder Widths(params double[] values)
		{
			for (var i = 0; i < values.Length; ++i)
				_sheet.SetColumnWidth(i, CalculateColWidth(values[i]));

			return this;
		}

		public IFont NewFont()
		{
			return _workbook.CreateFont();
		}

		public XlsBuilder NewStyle()
		{
			_style = _workbook.CreateCellStyle();
			_style.WrapText = true;

			return this;
		}

		public XlsBuilder NewStyle(IFont font)
		{
			return NewStyle().Font(font);
		}

		public XlsBuilder CloneStyle()
		{
			var style = _workbook.CreateCellStyle();
			style.CloneStyleFrom(_style);

			_style = style;

			return this;
		}

		public XlsBuilder CloneStyle(IFont font)
		{
			return CloneStyle().Font(font);
		}

		public XlsBuilder Font(IFont font)
		{
			_style.SetFont(font);

			return this;
		}

		public XlsBuilder Border(NPOI.SS.UserModel.BorderStyle top, NPOI.SS.UserModel.BorderStyle? right = null, NPOI.SS.UserModel.BorderStyle? bottom = null, NPOI.SS.UserModel.BorderStyle? left = null)
		{
			_style.BorderTop = top;

			if (!right.HasValue)
			{
				right = top;
				bottom = top;
				left = top;
			}
			else if (!bottom.HasValue)
			{
				bottom = top;
				left = right;
			}
			else if (!left.HasValue)
			{
				left = right;
			}

			_style.BorderRight = right.Value;
			_style.BorderBottom = bottom.Value;
			_style.BorderLeft = left.Value;

			return this;
		}

		public XlsBuilder BorderTop(NPOI.SS.UserModel.BorderStyle border)
		{
			_style.BorderTop = border;

			return this;
		}

		public XlsBuilder BorderBottom(NPOI.SS.UserModel.BorderStyle border)
		{
			_style.BorderBottom = border;

			return this;
		}

		public XlsBuilder Right()
		{
			_style.Alignment = HorizontalAlignment.RIGHT;

			return this;
		}

		public XlsBuilder Center()
		{
			_style.Alignment = HorizontalAlignment.CENTER;

			return this;
		}

		public XlsBuilder Top()
		{
			_style.VerticalAlignment = VerticalAlignment.TOP;

			return this;
		}

		public XlsBuilder Middle()
		{
			_style.VerticalAlignment = VerticalAlignment.CENTER;

			return this;
		}

		public XlsBuilder Indention(short value)
		{
			_style.Indention = value;

			return this;
		}

		public XlsBuilder AddRow()
		{
			_row = _sheet.CreateRow(_sheet.PhysicalNumberOfRows == 0 ? 0 : _sheet.LastRowNum + 1);

			_skip = 0;

			return this;
		}

		public XlsBuilder AddRow(double height)
		{
			AddRow();

			_row.HeightInPoints = (float) height;

			return this;
		}

		public XlsBuilder AddCell(string value = null)
		{
			var column = _row.LastCellNum < 0 ? 0 : _row.LastCellNum;

			if (_skip != 0)
			{
				column += _skip;
				_skip = 0;
			}

			_cell = _row.CreateCell(column);

			if (_style != null)
				_cell.CellStyle = _style;

			if (value != null)
				_cell.SetCellValue(value);

			return this;
		}

		public XlsBuilder Merge(int width, int height = 1)
		{
			var first = _cell.ColumnIndex;

			for (var i = 1; i < width; ++i)
				AddCell();

			_sheet.AddMergedRegion(new CellRangeAddress(_row.RowNum, _row.RowNum + height - 1, first, _cell.ColumnIndex));

			return this;
		}

		public XlsBuilder Skip(int num = 1)
		{
			_skip += num;

			return this;
		}

		private static int CalculateColWidth(double width)
		{
			if (width > 1)
				return (int) (441.3793 + 256.0 * (width - 1.0));

			return (int) (441.3793 * width);
		}

		private readonly HSSFWorkbook _workbook;
		private ISheet _sheet;
		private IRow _row;
		private ICell _cell;
		private ICellStyle _style;
		private int _skip;
	}
}