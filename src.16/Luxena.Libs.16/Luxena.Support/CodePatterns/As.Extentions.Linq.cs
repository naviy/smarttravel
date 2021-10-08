using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		[DebuggerStepThrough]
		public static IEnumerable<TSource> AsConcat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return
				first == null ? second :
				second == null ? first :
				first.Concat(second);
		}

		[DebuggerStepThrough]
		public static int AsCount<TSource>(this ICollection<TSource> me)
		{
			return me?.Count ?? 0;
		}

		[DebuggerStepThrough]
		public static IEnumerable<TResult> AsSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source?.Select(selector);
		}

		[DebuggerStepThrough]
		public static decimal AsSum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			return source?.Sum(selector) ?? 0;
		}

		[DebuggerStepThrough]
		public static decimal? AsSum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			return source == null ? 0 : source.Sum(selector);
		}

		[DebuggerStepThrough]
		public static double AsSum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			return source?.Sum(selector) ?? 0;
		}

		[DebuggerStepThrough]
		public static double? AsSum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			return source == null ? 0 : source.Sum(selector);
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> AsUnion<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return
				first == null ? second :
				second == null ? first :
				first.Union(second);
		}

	}

}
