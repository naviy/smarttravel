using System;


namespace Luxena.Base.Data
{
	public class OperationDeniedException : Exception
	{
		public OperationDeniedException()
		{
		}

		public OperationDeniedException(string message) : base(message)
		{
		}
	}
}