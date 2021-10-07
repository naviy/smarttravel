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

		#region Nullables

		[DebuggerStepThrough]
		public static bool Yes<T>(this T? me) where T: struct
		{
			return me.HasValue;
		}

		[DebuggerStepThrough]
		public static bool No<T>(this T? me) where T : struct
		{
			return !me.HasValue;
		}


		//[DebuggerStepThrough]
		//public static bool Yes(this decimal? me)
		//{
		//	return me.HasValue && me.Value != 0;
		//}

		//[DebuggerStepThrough]
		//public static bool No(this decimal? me)
		//{
		//	return !me.HasValue || me.Value == 0;
		//}


		//[DebuggerStepThrough]
		//public static bool Yes(this int? me)
		//{
		//	return me.HasValue && me.Value != 0;
		//}

		//[DebuggerStepThrough]
		//public static bool No(this int? me)
		//{
		//	return !me.HasValue || me.Value == 0;
		//}


		//[DebuggerStepThrough]
		//public static bool Yes(this long? me)
		//{
		//	return me.HasValue && me.Value != 0;
		//}

		//[DebuggerStepThrough]
		//public static bool No(this long? me)
		//{
		//	return !me.HasValue || me.Value == 0;
		//}


	
		[DebuggerStepThrough]
		public static bool Yes(this Match me)
		{
			return me != null && me.Success && !string.IsNullOrEmpty(me.Value);
		}

		[DebuggerStepThrough]
		public static bool No(this Match me)
		{
			return me == null || !me.Success || string.IsNullOrEmpty(me.Value);
		}


		[DebuggerStepThrough]
		public static bool Yes(this string me)
		{
			return !string.IsNullOrEmpty(me);
		}

		[DebuggerStepThrough]
		public static bool No(this string me)
		{
			return string.IsNullOrEmpty(me);
		}

		#endregion


		#region Lists

		[DebuggerStepThrough]
		public static bool Yes<T>(this T[] me)
		{
			return me != null && me.Length > 0;
		}

		[DebuggerStepThrough]
		public static bool No<T>(this T[] me)
		{
			return me == null || me.Length == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes(this ICollection me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No(this ICollection me)
		{
			return me == null || me.Count == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes<T>(this ICollection<T> me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No<T>(this ICollection<T> me)
		{
			return me == null || me.Count == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes<T>(this Collection<T> me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No<T>(this Collection<T> me)
		{
			return me == null || me.Count == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes<TK, TV>(this IDictionary<TK, TV> me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No<TK, TV>(this IDictionary<TK, TV> me)
		{
			return me == null || me.Count == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes<TK, TV>(this Dictionary<TK, TV> me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No<TK, TV>(this Dictionary<TK, TV> me)
		{
			return me == null || me.Count == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes(this IList me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No(this IList me)
		{
			return me == null || me.Count == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes<T>(this IList<T> me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No<T>(this IList<T> me)
		{
			return me == null || me.Count == 0;
		}


		[DebuggerStepThrough]
		public static bool Yes<T>(this List<T> me)
		{
			return me != null && me.Count > 0;
		}

		[DebuggerStepThrough]
		public static bool No<T>(this List<T> me)
		{
			return me == null || me.Count == 0;
		}

		#endregion

	}

}
