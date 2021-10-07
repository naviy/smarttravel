namespace Luxena.Travel.Parsers
{
	public abstract class AbstractLinesParser
	{
		protected AbstractLinesParser(object[] pattern, ILinesEnumerator lines, AbstractLinesParser parent)
		{
			Pattern = pattern;
			Lines = lines;
			Parent = parent;
		}

		public abstract bool TryResume();

		protected object[] Pattern { get; set; }
		protected ILinesEnumerator Lines { get; set; }
		protected AbstractLinesParser Parent { get; set; }
	}
}