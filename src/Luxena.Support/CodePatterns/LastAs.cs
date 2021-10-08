using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		#region LastAs<T, T2>(a => a.Name)

		[DebuggerStepThrough]
		public static T2 LastAs<T, T2>(this T[] me, Func<T, T2> evaluator) where T: class
		{
			return me.No() ? default(T2) : me[me.Length - 1].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 LastAs<T, T2>(this Collection<T> me, Func<T, T2> evaluator) where T : class
		{
			return me.No() ? default(T2) : me[me.Count - 1].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 LastAs<T2>(this IList me, Func<object, T2> evaluator)
		{
			return me.No() ? default(T2) : me[me.Count - 1].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 LastAs<T, T2>(this IList<T> me, Func<T, T2> evaluator) where T : class
		{
			return me.No() ? default(T2) : me[me.Count - 1].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 LastAs<T, T2>(this List<T> me, Func<T, T2> evaluator) where T : class
		{
			return me.No() ? default(T2) : me[me.Count - 1].As(evaluator);
		}

		#endregion


		#region LastAs<string, T2>(a => a.Name)

		[DebuggerStepThrough]
		public static T2 LastAs<T2>(this string[] me, Func<string, T2> evaluator) 
		{
			return me.No() ? default(T2) : me[me.Length - 1].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 LastAs<T2>(this Collection<string> me, Func<string, T2> evaluator) 
		{
			return me.No() ? default(T2) : me[me.Count - 1].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 LastAs<T2>(this IList<string> me, Func<string, T2> evaluator) 
		{
			return me.No() ? default(T2) : me[me.Count - 1].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 LastAs<T2>(this List<string> me, Func<string, T2> evaluator) 
		{
			return me.No() ? default(T2) : me[me.Count - 1].As(evaluator);
		}

		#endregion

	}

}
