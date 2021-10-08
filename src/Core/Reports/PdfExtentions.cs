using System;
using System.Globalization;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;

//using NPOI.SS.Util;


namespace Luxena.Travel.Reports
{
	public static class PdfExtentions
	{

		#region PdfContentByte

		public static PdfContentByte AddToBottom(this PdfContentByte content, PdfPTable table)
		{
			var doc = content.PdfDocument;

			table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
			table.WriteSelectedRows(0, -1, doc.LeftMargin, doc.BottomMargin, content);

			return content;
		}

		#endregion


		#region Document

		public static Document SetBottomMargin(this Document doc, float value)
		{
			doc.SetMargins(doc.LeftMargin, doc.RightMargin, doc.TopMargin, value);
			return doc;
		}

		public static Document SetLeftMargin(this Document doc, float value)
		{
			doc.SetMargins(value, doc.RightMargin, doc.TopMargin, doc.BottomMargin);
			return doc;
		}

		public static Document SetRightMargin(this Document doc, float value)
		{
			doc.SetMargins(doc.LeftMargin, value, doc.TopMargin, doc.BottomMargin);
			return doc;
		}

		public static Document SetTopMargin(this Document doc, float value)
		{
			doc.SetMargins(doc.LeftMargin, doc.RightMargin, value, doc.BottomMargin);
			return doc;
		}

		#endregion


		#region Table

		#region AddCell

		public static PdfPTable AddCell(this PdfPTable table, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell);
			
			if (action != null) 
				action(cell);

			table.AddCell(cell);

			return table;
		}

		public static PdfPTable AddCells(this PdfPTable table, int count, Action<PdfPCell> action = null)
		{
			for (var i = 0; i < count; i++)
				AddCell(table, action);

			return table;
		}

		public static PdfPTable AddCell(this PdfPTable table, Font font, string text, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Phrase = new Phrase(text, font)
			};
			if (action != null) action(cell);

			table.AddCell(cell);
			return table;
		}

		public static PdfPTable AddCellC(this PdfPTable table, Font font, string text, Action<PdfPCell> action = null)
		{
			return AddCell(table, font, text, cell => cell.Center().Do(action));
		}

		public static PdfPTable AddCellR(this PdfPTable table, Font font, string text, Action<PdfPCell> action = null)
		{
			return AddCell(table, font, text, cell => cell.Right().Do(action));
		}


		public static PdfPTable AddCell(this PdfPTable table, Font font, DateTime date, Action<PdfPCell> action = null)
		{
			return AddCell(table, font, date.ToString("d.MM.yyyy"), action);
		}

		public static PdfPTable AddCell(this PdfPTable table, Font font, DateTime? value, Action<PdfPCell> action = null)
		{
			return value == null
				? table.AddCell(action)
				: table.AddCell(font, value.Value, action);
		}


		public static PdfPTable AddCell(this PdfPTable table, Font font, string text, int align, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Phrase = new Phrase(text, font),
				HorizontalAlignment = align
			};
			if (action != null) action(cell);

			table.AddCell(cell);
			return table;
		}

		public static PdfPTable AddCell(this PdfPTable table, Font font, string text, int align, int border, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Phrase = new Phrase(text, font),
				HorizontalAlignment = align,
				Border = border
			};
			if (action != null) action(cell);

			table.AddCell(cell);
			return table;
		}


		public static PdfPTable AddCell(this PdfPTable table, float leading, Font font, string text, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Phrase = new Phrase(leading, text, font)
			};
			if (action != null) action(cell);

			table.AddCell(cell);
			return table;
		}

		public static PdfPTable AddCell(this PdfPTable table, float leading, Font font, string text, int align, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Phrase = new Phrase(leading, text, font),
				HorizontalAlignment = align
			};
			if (action != null) action(cell);

			table.AddCell(cell);
			return table;
		}

		public static PdfPTable AddCell(this PdfPTable table, float leading, Font font, string text, int align, int border, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Phrase = new Phrase(leading, text, font),
				HorizontalAlignment = align,
				Border = border
			};
			if (action != null) action(cell);

			table.AddCell(cell);
			return table;
		}


		public static PdfPTable AddMergedCell(this PdfPTable table, int colspan, Font font, string text, Action<PdfPCell> action = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Colspan = colspan,
				Phrase = new Phrase(text, font)
			};
			if (action != null) action(cell);

			table.AddCell(cell);
			return table;
		}

		public static PdfPTable AddCell(this PdfPTable table, Font font, Money money, Action<PdfPCell> action = null)
		{
			return money == null 
				? table.AddCell(action) 
				: table.AddCell(font, money.MoneyString, Element.ALIGN_RIGHT, action);
		}

		public static PdfPTable AddCell(this PdfPTable table, Font font, Font invalidFont, Money money, Currency defaultCurrency, bool allowNonDefaultCurrency, Action<PdfPCell> action = null)
		{
			if (money == null)
				return table.AddCell(action);

			var isDefaultCurrency = money.Currency.Equals(defaultCurrency);
			var isValid = isDefaultCurrency || allowNonDefaultCurrency;

			return table.AddCell(isValid ? font : invalidFont, isDefaultCurrency ? money.ToString() : money.MoneyString, Element.ALIGN_RIGHT, action);
		}

		public static PdfPTable AddDecimal(this PdfPTable table, Font font, decimal amount, bool addZero, Action<PdfPCell> action = null)
		{
			var text = amount == 0 && !addZero ? string.Empty : amount.ToMoneyString();

			return table.AddCell(font, text, Element.ALIGN_RIGHT, action);
		}

		public static PdfPTable AddCell(this PdfPTable table, Font font, int amount, bool addZero, Action<PdfPCell> action = null)
		{
			var text = amount == 0 && !addZero ? string.Empty : amount.ToString(CultureInfo.InvariantCulture);

			return table.AddCell(font, text, Element.ALIGN_RIGHT, action);
		}

		public static PdfPTable AddCell(this PdfPTable table, Font font, int amount, Action<PdfPCell> action = null)
		{
			var text = amount == 0 ? string.Empty : amount.ToString(CultureInfo.InvariantCulture);

			return table.AddCell(font, text, Element.ALIGN_RIGHT, action);
		}

		public static PdfPTable AddImage(this PdfPTable table, string imagePath, Action<PdfPCell> cellAction = null, Action<Image> imageAction = null)
		{
			var cell = new PdfPCell(table.DefaultCell)
			{
				Image = Image.GetInstance(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath))
			};

			if (cellAction != null) cellAction(cell);
			if (imageAction != null) imageAction(cell.Image);

			table.AddCell(cell);
			return table;
		}

		#endregion

		#endregion


		#region Cell

		public static PdfPCell Alignment(this PdfPCell me, int value)
		{
			if (me != null)
				me.HorizontalAlignment = value;
			return me;
		}

		public static PdfPCell Center(this PdfPCell me)
		{
			if (me != null)
				me.HorizontalAlignment = Element.ALIGN_CENTER;
			return me;
		}

		public static PdfPCell Left(this PdfPCell me)
		{
			if (me != null)
				me.HorizontalAlignment = Element.ALIGN_LEFT;
			return me;
		}

		public static PdfPCell Right(this PdfPCell me)
		{
			if (me != null)
				me.HorizontalAlignment = Element.ALIGN_RIGHT;
			return me;
		}


		public static PdfPCell Border(this PdfPCell me)
		{
			if (me != null)
				me.Border = Rectangle.BOX;
			return me;
		}

		public static PdfPCell BorderBottom(this PdfPCell me)
		{
			if (me != null)
				me.Border = Rectangle.BOTTOM_BORDER;
			return me;
		}

		public static PdfPCell BorderLeft(this PdfPCell me)
		{
			if (me != null)
				me.Border = Rectangle.LEFT_BORDER;
			return me;
		}

		public static PdfPCell BorderRight(this PdfPCell me)
		{
			if (me != null)
				me.Border = Rectangle.RIGHT_BORDER;
			return me;
		}

		public static PdfPCell BorderTop(this PdfPCell me)
		{
			if (me != null)
				me.Border = Rectangle.TOP_BORDER;
			return me;
		}


		public static PdfPCell Padding(this PdfPCell me, float value)
		{
			if (me != null)
				me.Padding = value;
			return me;
		}

		public static PdfPCell PaddingBottom(this PdfPCell me, float value)
		{
			if (me != null)
				me.PaddingBottom = value;
			return me;
		}

		public static PdfPCell PaddingLeft(this PdfPCell me, float value)
		{
			if (me != null)
				me.PaddingLeft = value;
			return me;
		}

		public static PdfPCell PaddingRight(this PdfPCell me, float value)
		{
			if (me != null)
				me.PaddingRight = value;
			return me;
		}

		public static PdfPCell PaddingTop(this PdfPCell me, float value)
		{
			if (me != null)
				me.PaddingTop = value;
			return me;
		}

		#endregion


		#region Paragraph

		public static Paragraph Alignment(this Paragraph me, int value)
		{
			if (me != null)
				me.Alignment = value;
			return me;
		}

		public static Paragraph Center(this Paragraph me)
		{
			if (me != null)
				me.Alignment = Element.ALIGN_CENTER;
			return me;
		}

		public static Paragraph Left(this Paragraph me)
		{
			if (me != null)
				me.Alignment = Element.ALIGN_LEFT;
			return me;
		}

		public static Paragraph Right(this Paragraph me)
		{
			if (me != null)
				me.Alignment = Element.ALIGN_RIGHT;
			return me;
		}

		#endregion

	}
}