using System.Collections.Generic;
using System.Diagnostics;


namespace Luxena.Travel
{
	public static class DictionaryExtensions
	{
		[DebuggerStepThrough]
		public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
		{
			TValue value;
			return dic != null && dic.TryGetValue(key, out value) ? value : default(TValue);
		}

		[DebuggerStepThrough]
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
		{
			TValue value;
			return dic != null && dic.TryGetValue(key, out value) ? value : default(TValue);
		}

		[DebuggerStepThrough]
		public static TResult GetOrDefault<TKey, TValue, TResult>(this Dictionary<TKey, TValue> dic, TKey key)
			where TValue : class
		{
			TValue value;
			return dic.TryGetValue(key, out value) && value is TResult ? (TResult) (object) value : default(TResult);
		}

		[DebuggerStepThrough]
		public static TResult GetOrDefault<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey key)
			where TValue : class
		{
			TValue value;
			return dic.TryGetValue(key, out value) && value is TResult ? (TResult) (object) value : default(TResult);
		}
	}
}