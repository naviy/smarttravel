using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Luxena.Travel.Reports
{
	public class RegistryReportPdfWriter : IRegistryReportWriter
	{
		public RegistryReportPdfWriter(Stream stream)
		{
			_defaultFontBoldItalic = PdfUtility.GetFont(PdfUtility.Times, 10, true, true);
			_defaultFontBold = PdfUtility.GetFont(PdfUtility.Times, 10, true, false);
			_defaultFont = PdfUtility.GetFont(PdfUtility.Times, 10, false, false);

			_document = new Document(PageSize.A4, 20, 20, 20, 30);

			PdfWriter writer = PdfWriter.GetInstance(_document, stream);

			writer.PageEvent = new PdfPageNumberHelper
			{
				DisplayTotalPageCount = true,
				Font = new Font(_defaultFont)
				{
					Size = 7
				},
				PageNumberTemplate = ReportRes.Common_PageNumberTemplate
			};

			_document.Open();
		}

		public void Close()
		{
			_document.Close();
		}

		public void WriteReportTitle(string title)
		{
			var paragraph = new Paragraph(title, _currentFont)
			{
				Alignment = Element.ALIGN_CENTER,
				SpacingAfter = 10
			};

			_document.Add(paragraph);
		}

		public void WriteFilterString(string filter)
		{
			var paragraph = new Paragraph(filter, _currentFont)
			{
				Alignment = Element.ALIGN_CENTER,
			};

			_document.Add(paragraph);
		}

		public void SetTextAlign(TextAlign align)
		{
			switch (align)
			{
				case TextAlign.Center:
					_currentTextAlign = Element.ALIGN_CENTER;

					break;

				case TextAlign.Left:
					_currentTextAlign = Element.ALIGN_LEFT;

					break;
				case TextAlign.Right:
					_currentTextAlign = Element.ALIGN_RIGHT;

					break;
			}
		}

		public void SetTextFont(int size, bool bold, bool italic)
		{
			if (bold && italic)
				_currentFont = new Font(_defaultFontBoldItalic);
			else if (bold)
				_currentFont = new Font(_defaultFontBold);
			else
				_currentFont = new Font(_defaultFont);

			_currentFont.Size = size;
		}

		public void SetCellBorder(BorderStyle borderStyle)
		{
			var border = Rectangle.NO_BORDER;

			if ((borderStyle & BorderStyle.Top) == BorderStyle.Top)
				border |= Rectangle.TOP_BORDER;

			if ((borderStyle & BorderStyle.Left) == BorderStyle.Left)
				border |= Rectangle.LEFT_BORDER;

			if ((borderStyle & BorderStyle.Right) == BorderStyle.Right)
				border |= Rectangle.RIGHT_BORDER;

			if ((borderStyle & BorderStyle.Bottom) == BorderStyle.Bottom)
				border |= Rectangle.BOTTOM_BORDER;

			_documentTable.DefaultCellBorder = border;
		}

		public void SetCellColor(int red, int green, int blue)
		{
			_documentTable.DefaultCell.BackgroundColor = new Color(red, green, blue);
		}

		public void BeginDocumentTable()
		{
			_document.Add(new Paragraph(17));

			_columnCount = 12;

			_documentTable = new Table(_columnCount)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.NO_BORDER,
				Widths = new[]
				{
					9.5f, 7f, 19.5f, 8f, 8f, 8f, 8f, 8f, 8f, 8f, 8f, 8f
				},
				Width = 100,
				CellsFitPage = true,
				DefaultCellBorderColor = Color.LIGHT_GRAY,
				DefaultVerticalAlignment = Element.ALIGN_TOP,
				DefaultHorizontalAlignment = Element.ALIGN_CENTER
			};
		}

		public void EndDocumentTableContent()
		{
		}

		public void BeginDocumentTableRow()
		{
		}

		public void EndDocumentTableHeaders()
		{
			_documentTable.EndHeaders();
		}

		public void WriteDocumentTableCell(string text)
		{
			AddCell(_documentTable, text);
		}

		public void WriteDocumentTableCell(string text, bool isValid)
		{
			Font font = _currentFont;

			if (!isValid)
			{
				_currentFont = new Font(_currentFont);
				_currentFont.SetColor(255, 0, 0);
			}

			AddCell(_documentTable, text);

			_currentFont = font;
		}

		public void EndDocumentTable()
		{
			_document.Add(_documentTable);
		}

		public void BeginFooter()
		{
			_footerTable = new PdfPTable(2)
			{
				WidthPercentage = 100,
				SpacingBefore = 10
			};

			_footerTable.DefaultCell.Border = Rectangle.NO_BORDER;
			_footerTable.SetTotalWidth(new float[] { 85, 15 });
		}

		public void EndFooter()
		{
			_document.Add(_footerTable);
		}

		public void WriteFooterText(string text)
		{
			_footerTable.AddCell(_currentFont, text);
		}

		public void WriteOperationsTable(string[, ] data)
		{
			var table = new PdfPTable(3)
			{
				WidthPercentage = 100
			};

			table.SetTotalWidth(new float[] {5, 65, 30});
			table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
			table.DefaultCell.BorderColor = Color.LIGHT_GRAY;

			var font = new Font(_defaultFont) { Size = 8 };
			var fontBold = new Font(_defaultFontBold) { Size = 8 };

			table.AddMergedCell(2, fontBold, data[0, 0]);
			table.AddCell(fontBold, data[0, 1], Element.ALIGN_RIGHT);

			table.DefaultCell.Border = Rectangle.NO_BORDER;

			for (int i = 1; i < 4; i++)
			{
				table.AddCell();
				table.AddCell(font, data[i, 0]);
				table.AddCell(font, data[i, 1], Element.ALIGN_RIGHT);
			}

			_footerTable.AddCell(table);
		}

		private void AddCell(Table table, string text)
		{
			var cell = new Cell(new Chunk(text, _currentFont))
			{
				Leading = _currentFont.Size,
				HorizontalAlignment = _currentTextAlign
			};

			table.AddCell(cell);
		}

		private readonly Document _document;

		private Table _documentTable;

		private readonly Font _defaultFont;
		private readonly Font _defaultFontBold;
		private readonly Font _defaultFontBoldItalic;

		private int _currentTextAlign;
		private Font _currentFont;
		private int _columnCount;
		private PdfPTable _footerTable;
	}
}