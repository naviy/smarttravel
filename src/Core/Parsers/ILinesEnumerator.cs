namespace Luxena.Travel.Parsers
{
	public interface ILinesEnumerator
	{
		bool MoveNext();
		int Number { get; }
		string Current { get; }
		bool EndOfFile { get; }
	}
}