using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Luxena.Travel.Reports
{
	class PdfPageNumberHelper : PdfPageEventHelper
	{
		public string PageNumberTemplate { get; set; }

		public bool DisplayTotalPageCount { get; set; }

		public Font Font { get; set; }

		private BaseFont BaseFont
		{
			get { return Font.BaseFont; }
		}

		public override void OnOpenDocument(PdfWriter writer, Document document)
		{
			_contentByte = writer.DirectContent;

			_template = _contentByte.CreateTemplate(50, 50);
		}

		public override void OnEndPage(PdfWriter writer, Document document)
		{
			var text = string.Format(PageNumberTemplate, writer.PageNumber);

			_contentByte.BeginText();
			_contentByte.SetFontAndSize(BaseFont, Font.Size);
			_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, text, document.Right - 5, document.Bottom / 2, 0);
			_contentByte.EndText();
			_contentByte.AddTemplate(_template, document.Right - 5, document.Bottom / 2);
		}

		public override void OnCloseDocument(PdfWriter writer, Document document)
		{
			if (!DisplayTotalPageCount)
				return;

			_template.BeginText();
			_template.SetFontAndSize(BaseFont, Font.Size);
			_template.ShowText((writer.PageNumber - 1).ToString());
			_template.EndText();
		}

		private PdfContentByte _contentByte;
		private PdfTemplate _template;
	}
}