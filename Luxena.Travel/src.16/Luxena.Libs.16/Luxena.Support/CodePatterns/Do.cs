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

		#region Do(a => a.Name = "New Name")

		[DebuggerStepThrough]
		public static T Do<T>(this object me, Action<T> action)
			where T : class
		{
			if (me == null) return null;
			var me2 = me as T;
			if (me2 == null) return null;

			if (action != null) action(me2);
			return me2;
		}


		[DebuggerStepThrough]
		public static T Do<T>(this T me, Action<T> action)
			where T : class
		{
			if (me == null) return null;
			if (action != null) action(me);
			return me;
		}

//		[DebuggerStepThrough]
//		public static T Do<T, T2>(this T me, Func<T, T2> action)
//			where T : class
//		{
//			if (me == null) return null;
//			if (action != null) action(me);
//			return me;
//		}

		[DebuggerStepThrough]
		public static T? Do<T>(this T? me, Action<T?> action)
			where T : struct
		{
			if (!me.HasValue) return null;
			if (action != null) action(me);
			return me;
		}


		[DebuggerStepThrough]
		public static T[] Do<T>(this T[] me, Action<T[]> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}


		[DebuggerStepThrough]
		public static ICollection Do(this ICollection me, Action<ICollection> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		[DebuggerStepThrough]
		public static ICollection<T> Do<T>(this ICollection<T> me, Action<ICollection<T>> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		[DebuggerStepThrough]
		public static Collection<T> Do<T>(this Collection<T> me, Action<Collection<T>> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}


		[DebuggerStepThrough]
		public static IDictionary Do(this IDictionary me, Action<IDictionary> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		[DebuggerStepThrough]
		public static IDictionary<TK, TV> Do<TK, TV>(this IDictionary<TK, TV> me, Action<IDictionary<TK, TV>> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		[DebuggerStepThrough]
		public static Dictionary<TK, TV> Do<TK, TV>(this Dictionary<TK, TV> me, Action<Dictionary<TK, TV>> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}


		[DebuggerStepThrough]
		public static IList Do(this IList me, Action<IList> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		[DebuggerStepThrough]
		public static IList<T> Do<T>(this IList<T> me, Action<IList<T>> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		[DebuggerStepThrough]
		public static List<T> Do<T>(this List<T> me, Action<List<T>> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}


		[DebuggerStepThrough]
		public static Match Do(this Match me, Action<Match> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		[DebuggerStepThrough]
		public static string Do(this string me, Action<string> action)
		{
			if (me.No()) return me;
			if (action != null) action(me);
			return me;
		}

		#endregion

	}

}
