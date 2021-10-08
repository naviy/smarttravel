using System;
using System.Collections.Generic;


namespace Luxena
{
	public static class ListExtentions
	{
		public static List<TOutput> Convert<TInput, TOutput>(this IList<TInput> list, Converter<TInput, TOutput> converter)
		{
			var list0 = new List<TInput>(list);
			var list1 = list0.ConvertAll(converter);
			return list1;
		}

		public static T Find<T>(this IList<T> list, Predicate<T> match)
		{
			foreach (T item in list)
				if (match(item))
					return item;

			return default(T);
		}

		public static bool IsNullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		//public static void ForEach<T>(this IList<T> list, Action<T> action)
		//{
		//	if (list == null) return;

		//	foreach (var item in list)
		//	{
		//		action(item); 
		//	}
		//}
	}
}