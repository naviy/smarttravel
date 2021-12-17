using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;




namespace Luxena
{



	//===g






	public static class ToListExtensions
	{

		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this IEnumerable<TSource> source,
			Func<TSource, TResult> selector
		)
		{
			return source?.Select(selector).ToList();
		}



		public static List<TResult> ToList<TSource, TResult>(
			this IEnumerable<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{
			return source?.Select(selector).ToList();
		}



		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this IEnumerable source,
			Func<TSource, TResult> selector
		)
		{
			return source?.Cast<TSource>().Select(selector).ToList();
		}



		public static List<TResult> ToList<TSource, TResult>(
			this IEnumerable source,
			Func<TSource, int, TResult> selector
		)
		{
			return source?.Cast<TSource>().Select(selector).ToList();
		}



		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this ICollection source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			var i = 0;

			foreach (var item in source)
			{
				result[i++] = selector((TSource)item);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this ICollection source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			var i = 0;

			foreach (var item in source)
			{
				result[i++] = selector((TSource)item, i);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this ICollection source,
			Func<TSource, int, ICollection, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			var i = 0;

			foreach (var item in source)
			{
				result[i++] = selector((TSource)item, i, source);
			}


			return result;

		}



		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this ICollection<TSource> source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			var i = 0;

			foreach (var item in source)
			{
				result[i++] = selector(item);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this ICollection<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			var i = 0;

			foreach (var item in source)
			{
				result[i++] = selector(item, i);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this ICollection<TSource> source,
			Func<TSource, int, ICollection<TSource>, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			var i = 0;

			foreach (var item in source)
			{
				result[i++] = selector(item, i, source);
			}


			return result;

		}

		

		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this IReadOnlyList<TSource> source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this IReadOnlyList<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this IReadOnlyList<TSource> source,
			Func<TSource, int, IReadOnlyList<TSource>, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this IList<TSource> source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this IList<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this IList<TSource> source,
			Func<TSource, int, IList<TSource>, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this List<TSource> source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this List<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this List<TSource> source,
			Func<TSource, int, List<TSource>, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static List<TResult> ToList<TSource, TResult>(
			this TSource[] source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Length;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this TSource[] source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Length;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static List<TResult> ToList<TSource, TResult>(
			this TSource[] source,
			Func<TSource, int, TSource[], TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Length;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static Match[] ToList(this MatchCollection col)
		{

			var arr = new Match[col.Count];

			col.CopyTo(arr, 0);

			return arr;

		}



		public static List<TResult> ToList<TResult>(
			this MatchCollection source,
			Func<Match, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static List<TResult> ToList<TResult>(
			this MatchCollection source,
			Func<Match, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static List<TResult> ToList<TResult>(
			this MatchCollection source,
			Func<Match, int, MatchCollection, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static Capture[] ToList(this CaptureCollection col)
		{

			var arr = new Capture[col.Count];

			col.CopyTo(arr, 0);

			return arr;

		}



		public static List<TResult> ToList<TResult>(
			this CaptureCollection source,
			Func<Capture, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static List<TResult> ToList<TResult>(
			this CaptureCollection source,
			Func<Capture, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static List<TResult> ToList<TResult>(
			this CaptureCollection source,
			Func<Capture, int, CaptureCollection, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new List<TResult>(len);


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g

	}






	//===g



}