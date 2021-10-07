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
		public static IEnumerable<TSource> AsUnion<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return
				first == null ? second :
				second == null ? first :
				first.Union(second);
		}

	}

}
