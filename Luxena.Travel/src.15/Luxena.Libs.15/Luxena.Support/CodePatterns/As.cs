using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		[DebuggerStepThrough]
		public static T As<T>(this object me) where T : class
		{
			return me as T;
		}

		#region As(a => a.Name)

		[DebuggerStepThrough]
		public static T2 As<T, T2>(this object me, Func<T, T2> evaluator) where T : class
		{
			var o = me as T;
			return o == null ? default(T2) : evaluator(o);
		}

		[DebuggerStepThrough]
		public static T2 As<T, T2>(this T me, Func<T, T2> evaluator) where T : class
		{
			return me == null ? default(T2) : evaluator(me);
		}

		[DebuggerStepThrough]
		public static T2 As<T, T2>(this T? me, Func<T, T2> evaluator) where T : struct
		{
			return me.HasValue ? evaluator(me.Value) : default(T2);
		}


		[DebuggerStepThrough]
		public static T2 As<T, T2>(this T[] me, Func<T[], T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}


		[DebuggerStepThrough]
		public static T2 As<T2>(this ICollection me, Func<ICollection, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}

		[DebuggerStepThrough]
		public static T2 As<T, T2>(this ICollection<T> me, Func<ICollection<T>, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}

		[DebuggerStepThrough]
		public static T2 As<T, T2>(this Collection<T> me, Func<Collection<T>, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}


		[DebuggerStepThrough]
		public static T2 As<T2>(this IDictionary me, Func<IDictionary, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}

		[DebuggerStepThrough]
		public static T2 As<TK, TV, T2>(this IDictionary<TK, TV> me, Func<IDictionary<TK, TV>, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}

		[DebuggerStepThrough]
		public static T2 As<TK, TV, T2>(this Dictionary<TK, TV> me, Func<Dictionary<TK, TV>, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}


		[DebuggerStepThrough]
		public static T2 As<T2>(this IList me, Func<IList, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}

		[DebuggerStepThrough]
		public static T2 As<T, T2>(this IList<T> me, Func<IList<T>, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}

		[DebuggerStepThrough]
		public static T2 As<T, T2>(this List<T> me, Func<List<T>, T2> evaluator)
		{
			return me.No() ? default(T2) : evaluator(me);
		}

		#endregion


		//#region return As(a => a.Name, "Default")

		//[DebuggerStepThrough]
		//public static T2 As<T2>(this object me, Func<object, T2> evaluator, T2 defaults)
		//{
		//	return me == null ? defaults : evaluator(me);
		//}


		//[DebuggerStepThrough]
		//public static T2 As<T, T2>(this T[] me, Func<T[], T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}


		//[DebuggerStepThrough]
		//public static T2 As<T2>(this ICollection me, Func<ICollection, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}

		//[DebuggerStepThrough]
		//public static T2 As<T, T2>(this ICollection<T> me, Func<ICollection<T>, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}

		//[DebuggerStepThrough]
		//public static T2 As<T, T2>(this Collection<T> me, Func<Collection<T>, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}


		//[DebuggerStepThrough]
		//public static T2 As<T2>(this IDictionary me, Func<IDictionary, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}

		//[DebuggerStepThrough]
		//public static T2 As<TK, TV, T2>(this IDictionary<TK, TV> me, Func<IDictionary<TK, TV>, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}

		//[DebuggerStepThrough]
		//public static T2 As<TK, TV, T2>(this Dictionary<TK, TV> me, Func<Dictionary<TK, TV>, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}


		//[DebuggerStepThrough]
		//public static T2 As<T2>(this IList me, Func<IList, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}

		//[DebuggerStepThrough]
		//public static T2 As<T, T2>(this IList<T> me, Func<IList<T>, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}

		//[DebuggerStepThrough]
		//public static T2 As<T, T2>(this List<T> me, Func<List<T>, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}


		//[DebuggerStepThrough]
		//public static T2 As<T2>(this string me, Func<string, T2> evaluator, T2 defaults)
		//{
		//	return me.No() ? defaults : evaluator(me);
		//}

		//#endregion

	}

}
