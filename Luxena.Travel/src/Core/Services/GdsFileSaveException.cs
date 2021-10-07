using System;
using System.Runtime.Serialization;

namespace Luxena.Travel.Services
{
	public class GdsFileSaveException : Exception
	{
		public GdsFileSaveException()
		{
		}

		public GdsFileSaveException(string message) : base(message)
		{
		}

		public GdsFileSaveException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected GdsFileSaveException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}