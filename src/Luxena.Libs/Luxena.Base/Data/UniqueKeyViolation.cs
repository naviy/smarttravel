using System;


namespace Luxena.Base.Data
{
	public class UniqueKeyViolation : Exception
	{
		public UniqueKeyViolation()
		{
		}

		public UniqueKeyViolation(string message) : base(message)
		{
		}
	}
}