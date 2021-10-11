using System;
using System.Collections;
using System.Html;

using LxnBase.UI.AutoControls;

using jQueryApi;




namespace Luxena.Travel
{



	public class ReportProvider : IReportProvider
	{

		public ReportProvider()
		{
			jQuery.OnDocumentReady(delegate
			{
				_frames = jQuery.FromHtml("<div style='display:none'><iframe></iframe><iframe></iframe></div>");
				_frames.AppendTo(Document.Body);
				_frames = _frames.Find("iframe");
			});
		}



		public void LoadReport(string url, Dictionary parameters)
		{

			url = Window.Location.Pathname + url.EncodeUri();

			jQueryObject form = jQuery.FromHtml("<form action='" + url + "' method='post'></form>");

			if (Script.IsValue(parameters))
			{
				foreach (DictionaryEntry entry in parameters)
				{
					jQuery.FromHtml("<input type='hidden' name='" + entry.Key + "' />")
						.Value((string)entry.Value)
						.AppendTo(form);
				}
			}


			form.AppendTo(((IFrameElement) _frames[++_current % _frames.Length]).ContentWindow.Document.Body);

			form.Submit().Remove();

		}



		private jQueryObject _frames;
		private int _current;

	}



}