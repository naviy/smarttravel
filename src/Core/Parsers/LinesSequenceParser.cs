using System.Collections.Generic;


namespace Luxena.Travel.Parsers
{
	public class LinesSequenceParser : AbstractLinesParser
	{
		public LinesSequenceParser(object[] pattern, ILinesEnumerator lines, AbstractLinesParser parent)
			: base(pattern, lines, parent)
		{
			_result = new object[pattern.Length];
		}

		public void Parse()
		{
			while (Lines.MoveNext() && _index < Pattern.Length)
			{
				if (HandleLine() == HandleLineResult.CanPropogate)
				{
					if (Parent != null && Parent.TryResume())
						break;
				}
			}

			if (_index < Pattern.Length)
			{
				for (var j = _index; j < Pattern.Length; ++j)
				{
					var entry = Pattern[j] as string;

					if (entry != null && entry[0] != '?')
						throw new GdsImportException(entry + " is missing");
				}
			}
		}

		public bool TryParse()
		{
			if (HandleLine() != HandleLineResult.Success)
				return false;

			Parse();

			return true;
		}

		public override bool TryResume()
		{
			switch (HandleLine())
			{
				case HandleLineResult.Success:
					return true;

				case HandleLineResult.CanPropogate:
					return Parent != null && Parent.TryResume();
			}

			return false;
		}

		public object[] Result => _result;

		private enum HandleLineResult
		{
			Success,
			CannotPropogate,
			CanPropogate
		}

		private HandleLineResult HandleLine()
		{
			for (var j = _index; j < Pattern.Length; ++j)
			{
				var pattern = Pattern[j] as object[];

				if (pattern == null)
				{
					var str = (string) Pattern[j];
					var isRequired = str[0] != '?';

					if (!isRequired)
						str = str.Substring(1);

					foreach (var part in str.Split('|'))
					{
						if (Lines.Current.StartsWith(part))
						{
							var list = new List<string> { part };

							list.AddRange(Lines.Current.Substring(part.Length).Split(';'));

							_result[j] = list.ToArray();

							_index = j + 1;

							return HandleLineResult.Success;
						}
					}

					if (isRequired)
						return HandleLineResult.CannotPropogate;
				}
				else
				{
					var parser = new LinesGroupParser(pattern, Lines, this);

					++_index;

					if (parser.TryParse())
					{
						_result[j] = parser.GetResult();
						return HandleLineResult.Success;
					}

					--_index;
				}
			}

			return HandleLineResult.CanPropogate;
		}

		private readonly object[] _result;
		private int _index;
	}
}