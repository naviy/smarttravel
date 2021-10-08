using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

using JetBrains.Annotations;




namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		#region Do(a => a.Name = "New Name")

		[DebuggerStepThrough]
		public static T Do<T>(this object me, [InstantHandle] Action<T> action)
			where T : class
		{
			var me2 = me as T;
			if (me2 == null) return null;

			action?.Invoke(me2);
			return me2;
		}


		[DebuggerStepThrough]
		public static T Do<T>(this T me, [InstantHandle] Action<T> action)
			where T : class
		{
			if (me == null) return null;
			action?.Invoke(me);
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
		public static T? Do<T>(this T? me, [InstantHandle] Action<T> action)
			where T : struct
		{
			if (!me.HasValue) return null;
			action?.Invoke(me.Value);
			return me;
		}


		[DebuggerStepThrough]
		public static T[] Do<T>(this T[] me, [InstantHandle] Action<T[]> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}


		[DebuggerStepThrough]
		public static ICollection Do(this ICollection me, [InstantHandle] Action<ICollection> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static ICollection<T> Do<T>(this ICollection<T> me, [InstantHandle] Action<ICollection<T>> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static Collection<T> Do<T>(this Collection<T> me, [InstantHandle] Action<Collection<T>> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}


		[DebuggerStepThrough]
		public static IDictionary Do(this IDictionary me, [InstantHandle] Action<IDictionary> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static IDictionary<TK, TV> Do<TK, TV>(this IDictionary<TK, TV> me, [InstantHandle] Action<IDictionary<TK, TV>> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static Dictionary<TK, TV> Do<TK, TV>(this Dictionary<TK, TV> me, [InstantHandle] Action<Dictionary<TK, TV>> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}


		[DebuggerStepThrough]
		public static IList Do(this IList me, [InstantHandle] Action<IList> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static IList<T> Do<T>(this IList<T> me, [InstantHandle] Action<IList<T>> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static List<T> Do<T>(this List<T> me, [InstantHandle] Action<List<T>> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}


		[DebuggerStepThrough]
		public static Match Do(this Match me, [InstantHandle] Action<Match> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static Group Do(this Group me, [InstantHandle] Action<Group> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		[DebuggerStepThrough]
		public static string Do(this string me, [InstantHandle] Action<string> action)
		{
			if (me.No()) return me;
			action?.Invoke(me);
			return me;
		}

		#endregion

	}

}
