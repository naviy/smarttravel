using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		#region If(a => a.HasName)

		[DebuggerStepThrough]
		public static T? If<T>(this T? me, Func<T?, bool> match)
			where T : struct
		{
			return me.HasValue && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static T If<T>(this T me, Func<T, bool> match)
			where T : class
		{
			return me != null && match(me) ? me : null;
		}


		[DebuggerStepThrough]
		public static T[] If<T>(this T[] me, Func<T[], bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}


		[DebuggerStepThrough]
		public static ICollection If(this ICollection me, Func<ICollection, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static ICollection<T> If<T>(this ICollection<T> me, Func<ICollection<T>, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static Collection<T> If<T>(this Collection<T> me, Func<Collection<T>, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}


		[DebuggerStepThrough]
		public static IDictionary If(this IDictionary me, Func<IDictionary, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static IDictionary<TK, TV> If<TK, TV>(this IDictionary<TK, TV> me, Func<IDictionary<TK, TV>, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static Dictionary<TK, TV> If<TK, TV>(this Dictionary<TK, TV> me, Func<Dictionary<TK, TV>, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}


		[DebuggerStepThrough]
		public static IList If(this IList me, Func<IList, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static IList<T> If<T>(this IList<T> me, Func<IList<T>, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static List<T> If<T>(this List<T> me, Func<List<T>, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		[DebuggerStepThrough]
		public static Match If(this Match me, Func<Match, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}


		[DebuggerStepThrough]
		public static string If(this string me, Func<string, bool> match)
		{
			return me.Yes() && match(me) ? me : null;
		}

		#endregion


		#region int.If(a => a != 0)

		[DebuggerStepThrough]
		public static bool? If(this bool me, Func<bool, bool> match)
		{
			return match(me) ? (bool?)me : null;
		}

		[DebuggerStepThrough]
		public static decimal? If(this decimal me, Func<decimal, bool> match)
		{
			return match(me) ? (decimal?)me : null;
		}

		[DebuggerStepThrough]
		public static double? If(this double me, Func<double, bool> match)
		{
			return match(me) ? (double?)me : null;
		}

		[DebuggerStepThrough]
		public static int? If(this int me, Func<int, bool> match)
		{
			return match(me) ? (int?)me : null;
		}

		#endregion

	}

}
