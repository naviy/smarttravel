using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;


namespace Luxena
{

	public static partial class CodePatternExtentions
	{

		#region By(index)

		[DebuggerStepThrough]
		public static T By<T>(this T[] me, int index)
		{
			return me != null && me.Length > 0 && index < me.Length ? me[index] : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this ICollection<T> me, int index)
		{
			return me != null && me.Count > 0 && index < me.Count ? me.ElementAt(index) : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this Collection<T> me, int index)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : default;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this IDictionary<TK, TV> me, TK key)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : default;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this Dictionary<TK, TV> me, TK key)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : default;
		}

		[DebuggerStepThrough]
		public static object By(this IList me, int index)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : null;
		}

		[DebuggerStepThrough]
		public static T By<T>(this IList<T> me, int index)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this List<T> me, int index)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : default;
		}


		[DebuggerStepThrough]
		public static string By(this Match me, int index)
		{
			return me.Yes() && index >= 0 && index < me.Groups.Count ? me.Groups[index].Value : null;
		}

		[DebuggerStepThrough]
		public static string By(this Match me, string groupName)
		{
			return me.Yes() && groupName.Yes() ? me.Groups[groupName].Value : null;
		}

		[DebuggerStepThrough]
		public static string By(this NameValueCollection me, string key)
		{
			return me.Yes() && key.Yes() ? me[key] : null;
		}

		#endregion


		#region By(index, T defaults)

		[DebuggerStepThrough]
		public static T By<T>(this T[] me, int index, T defaults)
		{
			return me != null && me.Length > 0 && index < me.Length ? me[index] : defaults;
		}

		[DebuggerStepThrough]
		public static T By<T>(this Collection<T> me, int index, T defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this IDictionary<TK, TV> me, TK key, TV defaults)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : defaults;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this Dictionary<TK, TV> me, TK key, TV defaults)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : defaults;
		}

		[DebuggerStepThrough]
		public static object By(this IList me, int index, object defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults;
		}

		[DebuggerStepThrough]
		public static T By<T>(this IList<T> me, int index, T defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults;
		}

		[DebuggerStepThrough]
		public static T By<T>(this List<T> me, int index, T defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults;
		}

		[DebuggerStepThrough]
		public static string By(this NameValueCollection me, string key, string defaults)
		{
			return me.Yes() && key.Yes() ? me[key] : defaults;
		}

		#endregion


		#region By(index, Func<T> defaults)

		[DebuggerStepThrough]
		public static T By<T>(this T[] me, int index, Func<T> defaults)
		{
			return me != null && me.Length > 0 && index < me.Length ? me[index] : defaults != null ? defaults() : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this Collection<T> me, int index, Func<T> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults != null ? defaults() : default;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this IDictionary<TK, TV> me, TK key, Func<TV> defaults)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : defaults != null ? defaults() : default;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this Dictionary<TK, TV> me, TK key, Func<TV> defaults)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : defaults != null ? defaults() : default;
		}

		[DebuggerStepThrough]
		public static object By(this IList me, int index, Func<object> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults?.Invoke();
		}

		[DebuggerStepThrough]
		public static T By<T>(this IList<T> me, int index, Func<T> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults != null ? defaults() : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this List<T> me, int index, Func<T> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults != null ? defaults() : default;
		}

		[DebuggerStepThrough]
		public static string By(this NameValueCollection me, string key, Func<string> defaults)
		{
			return me.Yes() && key.Yes() ? me[key] : defaults?.Invoke();
		}

		#endregion


		#region By(index, Func<this, T> defaults)

		[DebuggerStepThrough]
		public static T By<T>(this T[] me, int index, Func<T[], T> defaults)
		{
			return me != null && me.Length > 0 && index < me.Length ? me[index] : defaults != null ? defaults(me) : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this Collection<T> me, int index, Func<Collection<T>, T> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults != null ? defaults(me) : default;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this IDictionary<TK, TV> me, TK key, Func<IDictionary<TK, TV>, TV> defaults)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : defaults != null ? defaults(me) : default;
		}

		[DebuggerStepThrough]
		public static TV By<TK, TV>(this Dictionary<TK, TV> me, TK key, Func<Dictionary<TK, TV>, TV> defaults)
		{
			return me != null && me.TryGetValue(key, out var value) ? value : defaults != null ? defaults(me) : default;
		}

		[DebuggerStepThrough]
		public static object By(this IList me, int index, Func<IList, object> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults?.Invoke(me);
		}

		[DebuggerStepThrough]
		public static T By<T>(this IList<T> me, int index, Func<IList<T>, T> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults != null ? defaults(me) : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this List<T> me, int index, Func<List<T>, T> defaults)
		{
			return me != null && me.Count > 0 && index < me.Count ? me[index] : defaults != null ? defaults(me) : default;
		}

		[DebuggerStepThrough]
		public static string By(this NameValueCollection me, string key, Func<NameValueCollection, string> defaults)
		{
			return me.Yes() && key.Yes() ? me[key] : defaults?.Invoke(me);
		}

		#endregion


		#region By(T => bool)

		[DebuggerStepThrough]
		public static T By<T>(this IQueryable<T> me, Expression<Func<T, bool>> predicate)
		{
			return me != null && predicate != null ? me.FirstOrDefault(predicate) : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this IEnumerable<T> me, Func<T, bool> predicate)
		{
			return me != null && predicate != null ? me.FirstOrDefault(predicate) : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this T[] me, Predicate<T> predicate)
		{
			return me != null && predicate != null ? Array.Find(me, predicate) : default;
		}

		[DebuggerStepThrough]
		public static T By<T>(this List<T> me, Predicate<T> predicate)
		{
			return me != null && predicate != null ? me.Find(predicate) : default;
		}

		#endregion

	}

}
