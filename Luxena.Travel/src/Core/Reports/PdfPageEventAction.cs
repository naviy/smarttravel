using System;

using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Luxena.Travel.Reports
{

	public class PdfPageEventAction : PdfPageEventHelper
	{
		public PdfPageEventAction(Action onStartPage, Action onEndPage = null)
		{
			_onStartPage = onStartPage;
			_onEndPage = onEndPage;
		}

		public override void OnStartPage(PdfWriter writer, Document document)
		{
			if (_onStartPage != null)
				_onStartPage();
		}

		public override void OnEndPage(PdfWriter writer, Document document)
		{
			if (_onEndPage != null)
				_onEndPage();
		}

		private readonly Action _onStartPage;
		private readonly Action _onEndPage;
	}

}