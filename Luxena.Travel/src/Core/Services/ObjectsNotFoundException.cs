using System;


namespace Luxena.Travel.Services
{
	public class ObjectsNotFoundException : Exception
	{
		public ObjectsNotFoundException(string message) : base(message)
		{
		}
	}
}