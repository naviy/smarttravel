using System;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Luxena.Travel.Reports
{
	public class PdfUtility
	{
		private PdfUtility()
		{
		}

		public const string Times = "Times New Roman";
		public const string ArialNarrow = "Arial Narrow";
		public const string Arial = "Arial";

		public static void SetFontsPath(string path)
		{
			FontFactory.RegisterDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
		}

		public static Font GetFont(string name, int size, bool embedded)
		{
			return FontFactory.GetFont(name, Encoding, embedded ? BaseFont.EMBEDDED : BaseFont.NOT_EMBEDDED, size);
		}

		public static Font GetFont(string name, int size, bool bold, bool italic)
		{
			var fontName = name;

			if (bold)
				fontName += " Bold";
			if (italic)
				fontName += " Italic";

			return FontFactory.GetFont(fontName, Encoding, BaseFont.NOT_EMBEDDED, size);
		}

		public static Font GetFont(string name, int size, bool bold, bool italic, bool embedded)
		{
			var fontName = name;

			if (bold)
				fontName += " Bold";
			if (italic)
				fontName += " Italic";

			return FontFactory.GetFont(fontName, Encoding, embedded ? BaseFont.EMBEDDED : BaseFont.NOT_EMBEDDED, size);
		}

		public static BaseFont GetBaseFont(string name, bool embedded)
		{
			return GetFont(name, 10, embedded).BaseFont;
		}

		public static BaseFont GetBaseFont(string name, bool bold, bool italic)
		{
			return GetFont(name, 10, bold, italic).BaseFont;
		}

		public static BaseFont GetBaseFont(string name, bool bold, bool italic, bool embedded)
		{
			return GetFont(name, 10, bold, italic, embedded).BaseFont;
		}

		private const string Encoding = "Cp1251";
	}
}