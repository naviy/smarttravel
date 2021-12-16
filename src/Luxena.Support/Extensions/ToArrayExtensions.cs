using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;




namespace Luxena
{



	//===g






	public static class RegexExtensions
	{

		//---g



		public static TResult[] ToArray<TSource, TResult>(
			this TSource[] source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Length;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this TSource[] source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Length;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this TSource[] source,
			Func<TSource, int, TSource[], TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Length;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static TResult[] ToArray<TSource, TResult>(
			this List<TSource> source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this List<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this List<TSource> source,
			Func<TSource, int, List<TSource>, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}





		//---g



		public static TResult[] ToArray<TSource, TResult>(
			this IList<TSource> source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this IList<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this IList<TSource> source,
			Func<TSource, int, IList<TSource>, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static TResult[] ToArray<TSource, TResult>(
			this IReadOnlyList<TSource> source,
			Func<TSource, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this IReadOnlyList<TSource> source,
			Func<TSource, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static TResult[] ToArray<TSource, TResult>(
			this IReadOnlyList<TSource> source,
			Func<TSource, int, IReadOnlyList<TSource>, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static Match[] ToArray(this MatchCollection col)
		{

			var arr = new Match[col.Count];

			col.CopyTo(arr, 0);

			return arr;

		}



		public static TResult[] ToArray<TResult>(
			this MatchCollection source,
			Func<Match, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static TResult[] ToArray<TResult>(
			this MatchCollection source,
			Func<Match, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static TResult[] ToArray<TResult>(
			this MatchCollection source,
			Func<Match, int, MatchCollection, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i, source);
			}


			return result;

		}



		//---g



		public static Capture[] ToArray(this CaptureCollection col)
		{

			var arr = new Capture[col.Count];

			col.CopyTo(arr, 0);

			return arr;

		}



		public static TResult[] ToArray<TResult>(
			this CaptureCollection source,
			Func<Capture, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i]);
			}


			return result;

		}



		public static TResult[] ToArray<TResult>(
			this CaptureCollection source,
			Func<Capture, int, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


			for (var i = 0; i < len; i++)
			{
				result[i] = selector(source[i], i);
			}


			return result;

		}



		public static TResult[] ToArray<TResult>(
			this CaptureCollection source,
			Func<Capture, int, CaptureCollection, TResult> selector
		)
		{

			if (source == null)
				return null;


			var len = source.Count;

			var result = new TResult[len];


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