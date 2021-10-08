using System;


namespace Luxena.Travel.Services
{
	public class DocumentClosedException : Exception
	{
		public DocumentClosedException(string message)	: base(message)
		{
		}
	}
}