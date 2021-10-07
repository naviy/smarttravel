using System;
using System.Collections.Generic;


namespace Luxena.Base.Text
{
	public class DirectErrorTranslator : IErrorTranslator
	{
		public DirectErrorTranslator(IEnumerable<Type> types)
		{
			_types = new Dictionary<Type, bool>();

			foreach (Type type in types)
				_types.Add(type, true);
		}

		public Exception Translate(Exception exception)
		{
			while (exception != null)
			{
				if (_types.ContainsKey(exception.GetType()))
					return exception;

				exception = exception.InnerException;
			}

			return null;
		}

		private readonly Dictionary<Type, bool> _types;
	}
}