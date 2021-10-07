using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Luxena.Travel
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TSource> AsConcat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first != null && second != null ? first.Concat(second) : first ?? second ?? new TSource[0];
		}

		public static IEnumerable<TSource> AsConcat<TSource>(this IEnumerable<TSource> first, params TSource[] second)
		{
			return first != null && second != null ? first.Concat(second) : first ?? second ?? new TSource[0];
		}



		#region Regex

		public static IEnumerable<string> Select(this MatchCollection me)
		{
			if (me == null)
				yield break;

			foreach (Match match in me)
			{
				yield return match.Value;
			}
		}

		public static IEnumerable<T> Select<T>(this MatchCollection me, Func<Match, T> expression)
		{
			if (me == null || expression == null)
				yield break;

			foreach (Match match in me)
			{
				yield return expression(match);
			}
		}

		public static IEnumerable<string> Select(this CaptureCollection me)
		{
			if (me == null)
				yield break;

			foreach (Capture capture in me)
			{
				yield return capture.Value;
			}
		}

		public static IEnumerable<T> Select<T>(this CaptureCollection me, Func<Capture, T> expression)
		{
			if (me == null || expression == null)
				yield break;

			foreach (Capture capture in me)
			{
				yield return expression(capture);
			}
		}

		#endregion


	}
}