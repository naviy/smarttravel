using System;
using System.Collections.Generic;
using System.Linq;


namespace Luxena.Base.Text
{
	public class CompositeErrorTranslator : IErrorTranslator
	{
		public CompositeErrorTranslator(IEnumerable<IErrorTranslator> translators)
		{
			_translators = translators;
		}

		public Exception Translate(Exception ex)
		{
			return _translators
				.Select(translator => translator.Translate(ex))
				.FirstOrDefault(result => result != null);
		}

		private readonly IEnumerable<IErrorTranslator> _translators;
	}
}