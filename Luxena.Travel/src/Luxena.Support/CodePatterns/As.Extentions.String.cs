namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		#region Format

		public static string AsFormat(this string me, object arg0)
		{
			return me == null ? null : string.Format(me, arg0);
		}

		public static string AsFormat(this string me, object arg0, object arg1)
		{
			return me == null ? null : string.Format(me, arg0, arg1);
		}

		public static string AsFormat(this string me, object arg0, object arg1, object arg2)
		{
			return me == null ? null : string.Format(me, arg0, arg1, arg2);
		}

		public static string AsFormat(this string me, params object[] args)
		{
			return me == null ? null : string.Format(me, args);
		}

		#endregion


		#region Substring

		public static string AsSubstring(this string me, int startIndex)
		{
			return me != null && startIndex >= 0 && startIndex < me.Length
				? me.Substring(startIndex)
				: null;
		}

		public static string AsSubstring(this string me, int startIndex, int length)
		{
			return me != null && startIndex >= 0 && startIndex <= me.Length - length
				? me.Substring(startIndex, length)
				: null;
		}

		#endregion
		
	}

}
