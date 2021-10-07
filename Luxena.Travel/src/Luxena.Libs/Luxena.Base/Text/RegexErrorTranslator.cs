using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Luxena.Base.Text
{
	public class RegexErrorTranslator : IErrorTranslator
	{
		public RegexErrorTranslator(IDictionary<string, string> rules, IInterpretationStrategy interpretation)
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

		public static bool Match(string rule, Exception exception)
		{
			var regEx = new Regex(rule,
				RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

			return regEx.IsMatch(exception.ToString());
		}

		private readonly IDictionary<string, string> _rules;
		private readonly IInterpretationStrategy _interpretation;
	}
}