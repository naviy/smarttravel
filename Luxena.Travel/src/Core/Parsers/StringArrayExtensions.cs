namespace Luxena.Travel.Parsers
{
	internal static class StringArrayExtensions
	{
		public static string SafeGet(this string[] s, int i)
		{
			return i >= s.Length ? null : s[i];
		}

		public static string TrimOrNull(this string[] s, int i)
		{
			return i >= s.Length ? null : s[i].TrimOrNull();
		}
	}
}