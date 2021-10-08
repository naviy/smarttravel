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

		#region Else("Default")

		[DebuggerStepThrough]
		public static T Else<T>(this T me, T defaults)
			where T : class
		{
			return me ?? defaults;
		}

		[DebuggerStepThrough]
		public static T? Else<T>(this T? me, T? defaults)
			where T : struct
		{
			return me ?? defaults;
		}


		[DebuggerStepThrough]
		public static T[] Else<T>(this T[] me, T[] defaults)
		{
			return me.No() ? defaults : me;
		}


		[DebuggerStepThrough]
		public static ICollection Else(this ICollection me, ICollection defaults)
		{
			return me.No() ? defaults : me;
		}

		[DebuggerStepThrough]
		public static ICollection<T> Else<T>(this ICollection<T> me, ICollection<T> defaults)
		{
			return me.No() ? defaults : me;
		}

		[DebuggerStepThrough]
		public static Collection<T> Else<T>(this Collection<T> me, Collection<T> defaults)
		{
			return me.No() ? defaults : me;
		}


		[DebuggerStepThrough]
		public static IDictionary Else(this IDictionary me, IDictionary defaults)
		{
			return me.No() ? defaults : me;
		}

		[DebuggerStepThrough]
		public static IDictionary<TK, TV> Else<TK, TV>(this IDictionary<TK, TV> me, IDictionary<TK, TV> defaults)
		{
			return me.No() ? defaults : me;
		}

		[DebuggerStepThrough]
		public static Dictionary<TK, TV> Else<TK, TV>(this Dictionary<TK, TV> me, Dictionary<TK, TV> defaults)
		{
			return me.No() ? defaults : me;
		}


		[DebuggerStepThrough]
		public static IList Else(this IList me, IList defaults)
		{
			return me.No() ? defaults : me;
		}

		[DebuggerStepThrough]
		public static IList<T> Else<T>(this IList<T> me, IList<T> defaults)
		{
			return me.No() ? defaults : me;
		}

		[DebuggerStepThrough]
		public static List<T> Else<T>(this List<T> me, List<T> defaults)
		{
			return me.No() ? defaults : me;
		}


		[DebuggerStepThrough]
		public static string Else(this string me, string defaults)
		{
			return me.No() ? defaults : me;
		}

		#endregion


		#region Else(() => obj.GetName())

		[DebuggerStepThrough]
		public static T Else<T>(this T me, Func<T> defaults)
			where T : class
		{
			return me ?? defaults();
		}

		[DebuggerStepThrough]
		public static T? Else<T>(this T? me, Func<T?> defaults)
			where T : struct
		{
			return me ?? defaults();
		}


		[DebuggerStepThrough]
		public static T[] Else<T>(this T[] me, Func<T[]> defaults)
		{
			return me.No() ? defaults() : me;
		}


		[DebuggerStepThrough]
		public static ICollection Else(this ICollection me, Func<ICollection> defaults)
		{
			return me.No() ? defaults() : me;
		}

		[DebuggerStepThrough]
		public static ICollection<T> Else<T>(this ICollection<T> me, Func<ICollection<T>> defaults)
		{
			return me.No() ? defaults() : me;
		}

		[DebuggerStepThrough]
		public static Collection<T> Else<T>(this Collection<T> me, Func<Collection<T>> defaults)
		{
			return me.No() ? defaults() : me;
		}


		[DebuggerStepThrough]
		public static IDictionary Else(this IDictionary me, Func<IDictionary> defaults)
		{
			return me.No() ? defaults() : me;
		}

		[DebuggerStepThrough]
		public static IDictionary<TK, TV> Else<TK, TV>(this IDictionary<TK, TV> me, Func<IDictionary<TK, TV>> defaults)
		{
			return me.No() ? defaults() : me;
		}

		[DebuggerStepThrough]
		public static Dictionary<TK, TV> Else<TK, TV>(this Dictionary<TK, TV> me, Func<Dictionary<TK, TV>> defaults)
		{
			return me.No() ? defaults() : me;
		}


		[DebuggerStepThrough]
		public static IList Else(this IList me, Func<IList> defaults)
		{
			return me.No() ? defaults() : me;
		}

		[DebuggerStepThrough]
		public static IList<T> Else<T>(this IList<T> me, Func<IList<T>> defaults)
		{
			return me.No() ? defaults() : me;
		}

		[DebuggerStepThrough]
		public static List<T> Else<T>(this List<T> me, Func<List<T>> defaults)
		{
			return me.No() ? defaults() : me;
		}


		[DebuggerStepThrough]
		public static Match Else(this Match me, Func<Match> defaults)
		{
			return me.No() ? defaults() : me;
		}

		[DebuggerStepThrough]
		public static string Else(this string me, Func<string> defaults)
		{
			return me.No() ? defaults() : me;
		}

		#endregion


		#region Else(() => obj.Method1())

		[DebuggerStepThrough]
		public static T Else<T>(this T me, Action defaults)
			where T : class
		{
			if (me == null) 
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static T? Else<T>(this T? me, Action defaults)
			where T : struct
		{
			if (!me.HasValue)
				defaults();
			return me;
		}


		[DebuggerStepThrough]
		public static T[] Else<T>(this T[] me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}


		[DebuggerStepThrough]
		public static ICollection Else(this ICollection me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static ICollection<T> Else<T>(this ICollection<T> me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static Collection<T> Else<T>(this Collection<T> me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}


		[DebuggerStepThrough]
		public static IDictionary Else(this IDictionary me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static IDictionary<TK, TV> Else<TK, TV>(this IDictionary<TK, TV> me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static Dictionary<TK, TV> Else<TK, TV>(this Dictionary<TK, TV> me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}


		[DebuggerStepThrough]
		public static IList Else(this IList me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static IList<T> Else<T>(this IList<T> me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static List<T> Else<T>(this List<T> me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}


		[DebuggerStepThrough]
		public static Match Else(this Match me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		[DebuggerStepThrough]
		public static string Else(this string me, Action defaults)
		{
			if (me.No())
				defaults();
			return me;
		}

		#endregion


	}

}
