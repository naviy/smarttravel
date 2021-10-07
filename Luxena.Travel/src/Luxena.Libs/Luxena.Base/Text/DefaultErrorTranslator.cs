using System;


namespace Luxena.Base.Text
{
	public class DefaultErrorTranslator : IErrorTranslator
	{
		public DefaultErrorTranslator(string value, IInterpretationStrategy interpretation)
		{
			_value = value;
			_interpretation = interpretation;
		}

		public Exception Translate(Exception exception)
		{
			return new Exception(_interpretation.Interpret(_value), exception);
		}

		private readonly string _value;
		private readonly IInterpretationStrategy _interpretation;
	}
}