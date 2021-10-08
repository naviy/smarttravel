using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		#region Sure()

		[DebuggerStepThrough]
		public static IEnumerable Sure(this IEnumerable me)
		{
			return me ?? new object[0];
		}

		[DebuggerStepThrough]
		public static IEnumerable<T> Sure<T>(this IEnumerable<T> me)
		{
			return me ?? new T[0];
		}

		[DebuggerStepThrough]
		public static ICollection Sure(this ICollection me)
		{
			return me ?? new object[0];
		}

		[DebuggerStepThrough]
		public static ICollection<T> Sure<T>(this ICollection<T> me)
		{
			return me ?? new T[0];
		}

		//[DebuggerStepThrough]
		//public static Collection<T> Sure<T>(this Collection<T> me, Collection<T> defaults)
		//{
		//	return me.No() ? defaults : me;
		//}


		//[DebuggerStepThrough]
		//public static IDictionary Sure(this IDictionary me)
		//{
		//	return me.No() ? defaults : me;
		//}

		//[DebuggerStepThrough]
		//public static IDictionary<TK, TV> Sure<TK, TV>(this IDictionary<TK, TV> me)
		//{
		//	return me.No() ? defaults : me;
		//}

		//[DebuggerStepThrough]
		//public static Dictionary<TK, TV> Sure<TK, TV>(this Dictionary<TK, TV> me)
		//{
		//	return me.No() ? defaults : me;
		//}


		[DebuggerStepThrough]
		public static IList Sure(this IList me)
		{
			return me ?? new object[0];
		}

		[DebuggerStepThrough]
		public static IList<T> Sure<T>(this IList<T> me)
		{
			return me ?? new T[0];
		}

		[DebuggerStepThrough]
		public static T[] Sure<T>(this T[] me)
		{
			return me ?? new T[0];
		}

		[DebuggerStepThrough]
		public static List<T> Sure<T>(this List<T> me)
		{
			return me ?? new List<T>();
		}

		[DebuggerStepThrough]
		public static string Sure(this string me)
		{
			return me ?? "";
		}

		#endregion

	}

}
