using System;
using System.Runtime.Serialization;


namespace Luxena.Base.Domain
{

	public class DomainException : Exception
	{
		public DomainException()
		{
		}

		public DomainException(string message)
			: base(message)
		{
		}

		public DomainException(string message, params Object[] args)
			: base(string.Format(message, args))
		{
		}

		public DomainException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

}