using System.Collections.Generic;


namespace Luxena.Travel.Parsers
{
	public class LinesGroupParser : AbstractLinesParser
	{
		public LinesGroupParser(object[] pattern, ILinesEnumerator lines, AbstractLinesParser parent) :
			base(pattern, lines, parent)
		{
			_result = new List<object[]>();
		}

		public bool TryParse()
		{
			var sequence = new LinesSequenceParser(Pattern, Lines, this);

			var index = _result.Count;

			if (!sequence.TryParse())
				return false;

			_result.Insert(index, sequence.Result);

			while (!Lines.EndOfFile)
			{
				index = _result.Count;

				sequence = new LinesSequenceParser(Pattern, Lines, this);

				if (sequence.TryParse())
				{
					_result.Insert(index, sequence.Result);
				}
				else if (Parent != null && Parent.TryResume())
				{
					break;
				}
				else
				{
					Lines.MoveNext();
				}
			}

			return true;
		}

		public override bool TryResume()
		{
			return TryParse() || (Parent != null && Parent.TryResume());
		}

		public object[] GetResult()
		{
			return _result.ToArray();
		}

		private readonly List<object[]> _result;
	}
}