using System;


namespace Luxena.Base.Data
{
	public class ForeignKeyViolation : Exception
	{
		public ForeignKeyViolation()
		{
		}

		public ForeignKeyViolation(string message) : base(message)
		{
		}
	}
}