using System;

namespace Luxena.Travel.Parsers
{
	public class GdsImportException : Exception
	{
		public GdsImportException(string message) : base(message)
		{
		}

		public GdsImportException(int line, string message) : base(string.Format("Line {0}: {1}", line, message))
		{
		}
	}
}