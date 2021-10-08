using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{
		
		#region One<T>()

		[DebuggerStepThrough]
		public static T One<T>(this IEnumerable<T> me) where T : class
		{
			return me?.FirstOrDefault();
		}

		[DebuggerStepThrough]
		public static T One<T>(this T[] me) where T : class
		{
			return me.No() ? null : me[0];
		}

		[DebuggerStepThrough]
		public static T One<T>(this Collection<T> me) where T : class
		{
			return me.No() ? null : me[0];
		}

		[DebuggerStepThrough]
		public static object One(this IList me)
		{
			return me.No() ? null : me[0];
		}

		[DebuggerStepThrough]
		public static T One<T>(this IList<T> me) where T : class
		{
			return me.No() ? null : me[0];
		}

		[DebuggerStepThrough]
		public static T One<T>(this List<T> me) where T : class
		{
			return me.No() ? null : me[0];
		}

		#endregion


		#region string One()

		[DebuggerStepThrough]
		public static string One(this string[] me)
		{
			return me.No() ? null : me[0];
		}

		[DebuggerStepThrough]
		public static string One(this Collection<string> me)
		{
			return me.No() ? null : me[0];
		}

		[DebuggerStepThrough]
		public static string One(this IList<string> me)
		{
			return me.No() ? null : me[0];
		}

		[DebuggerStepThrough]
		public static string One(this List<string> me)
		{
			return me.No() ? null : me[0];
		}

		#endregion

		
		#region OneAs<T, T2>(a => a.Name)

		[DebuggerStepThrough]
		public static T2 One<T, T2>(this IEnumerable<T> me, Func<T, T2> evaluator) where T : class
		{
			return me == null ? default(T2) : me.FirstOrDefault().As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T, T2>(this T[] me, Func<T, T2> evaluator) where T : class
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T, T2>(this Collection<T> me, Func<T, T2> evaluator) where T : class
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T2>(this IList me, Func<object, T2> evaluator)
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T, T2>(this IList<T> me, Func<T, T2> evaluator) where T : class
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T, T2>(this List<T> me, Func<T, T2> evaluator) where T : class
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		#endregion


		#region OneAs<string, T2>(a => a.Name)

		[DebuggerStepThrough]
		public static T2 One<T2>(this string[] me, Func<string, T2> evaluator)
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T2>(this Collection<string> me, Func<string, T2> evaluator)
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T2>(this IList<string> me, Func<string, T2> evaluator)
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		[DebuggerStepThrough]
		public static T2 One<T2>(this List<string> me, Func<string, T2> evaluator)
		{
			return me.No() ? default(T2) : me[0].As(evaluator);
		}

		#endregion


		#region AsOne<T, T2>(a => a.Name)

		[DebuggerStepThrough]
		public static T2 AsOne<T, T2>(this IQueryable<T> me, Func<T, T2> evaluator) where T : class
		{
			return me == null ? default(T2) : me.Select(evaluator).FirstOrDefault();
		}

		#endregion



	}

}
