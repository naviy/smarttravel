using System.Diagnostics;
using System.IO;
using System.Web;

using log4net.Core;
using log4net.Layout.Pattern;


namespace Luxena.Base.Web.Logging
{
	public class HttpRequestPatternConverter : PatternLayoutConverter
	{
		[DebuggerStepThrough]
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			if (HttpContext.Current != null && HttpContext.Current.User != null)
				writer.Write(string.Format(" - {0}", HttpContext.Current.User.Identity.Name));
		}
	}
}