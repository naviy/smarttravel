using System.IO;

namespace Luxena.Travel.Parsers
{
	public class LinesEnumerator : ILinesEnumerator
	{
		public LinesEnumerator(TextReader reader)
		{
			_reader = reader;
		}

		public string ReadLine()
		{
			if (!MoveNext())
				throw new EndOfStreamException();

			return Current;
		}

		public bool MoveNext()
		{
			Current = _reader.ReadLine();

			if (EndOfFile)
				return false;

			++Number;

			return true;
		}

		public int Number { get; private set; }

		public string Current { get; private set; }

		public bool EndOfFile
		{
			get { return Current == null; }
		}

		private readonly TextReader _reader;
	}
}