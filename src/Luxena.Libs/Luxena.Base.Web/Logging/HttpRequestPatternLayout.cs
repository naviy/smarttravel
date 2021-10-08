using log4net.Layout;
using log4net.Util;


namespace Luxena.Base.Web.Logging
{
	public class HttpRequestPatternLayout : PatternLayout
	{
		public HttpRequestPatternLayout()
		{
			var converterInfo = new ConverterInfo
			{
				Name = "userinfo",
				Type = typeof (HttpRequestPatternConverter)
			};

			AddConverter(converterInfo);
		}
	}
}