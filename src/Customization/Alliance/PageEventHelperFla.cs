using System;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Luxena.Travel.Alliance
{
	internal class PageEventHelperFla:PdfPageEventHelper
	{
		public PageEventHelperFla(Action onStartPage)
		{
			_onStartPage = onStartPage;
		}

		public override void OnEndPage(PdfWriter writer, Document document)
		{
		}

		public override void OnStartPage(PdfWriter writer, Document document)
		{
			_onStartPage();
		}

		private readonly Action _onStartPage;
	}
}
