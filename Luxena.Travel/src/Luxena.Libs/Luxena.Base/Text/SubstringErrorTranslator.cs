using System;
using System.Collections.Generic;
using System.Linq;


namespace Luxena.Base.Text
{
	public class SubstringErrorTranslator : IErrorTranslator
	{
		public SubstringErrorTranslator(IDictionary<string, string> rules, IInterpretationStrategy interpretation)
		{
			_rules = rules;
			_interpretation = interpretation;
		}

		public Exception Translate(Exception exception)
		{
			return _rules
				.Where(pair => Match(pair.Key, exception))
				.Select(pair => new Exception(_interpretation.Interpret(pair.Value)))
				.FirstOrDefault();
		}

		public bool Match(string rule, Exception exception)
		{
			while (exception != null)
			{
				if (exception.Message.IndexOf(rule, StringComparison.CurrentCultureIgnoreCase) >= 0)
					return true;

				exception = exception.InnerException;
			}

			return false;
		}

		private readonly IDictionary<string, string> _rules;
		private readonly IInterpretationStrategy _interpretation;
	}
}