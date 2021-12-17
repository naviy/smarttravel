using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;




namespace Luxena.Travel.Tests
{



	using static Assert;






	//===g






	public class AviaDocumentAsserter : AviaDocumentAsserter<AviaDocument, AviaDocumentAsserter>
	{

		public AviaDocumentAsserter(AviaDocument r) : base(r) { }

	}






	public class AviaDocumentAsserter<TProduct, TThis> : ProductAsserter<TProduct, TThis>
		where TProduct : AviaDocument
		where TThis : AviaDocumentAsserter<TProduct, TThis>
	{

		//---g



		protected AviaDocumentAsserter(TProduct r) : base(r) { }



		//---g



		public TThis AirlinePrefixCode(string expected)
		{
			AreEqual(expected, r.AirlinePrefixCode);
			return (TThis)this;
		}



		public TThis AirlineIataCode(string expected)
		{
			AreEqual(expected, r.AirlineIataCode);
			return (TThis)this;
		}



		public TThis Number(string expected)
		{
			AreEqual(expected, r.Number);
			return (TThis)this;
		}



		//---g






		//---g



		public TThis FlightSegments(params Action<FlightSegmentAsserter>[] asserts)
		{

			var segments = (r as AviaTicket)?.Segments;


			if (segments == null)
			{
				if (asserts.Length > 0)
					AreEqual(asserts.Length, null);
				else
					return (TThis)this;
			}


			AreEqual(asserts.Length, segments.Count);


			for (int i = 0, len = asserts.Length; i < len; i++)
			{
				asserts[i](new FlightSegmentAsserter(segments[i]));
			}


			return (TThis)this;

		}


		//---g

	}






	//===g






	public static partial class Extensions
	{

		//---g



		//public static AviaDocumentAsserter Assert(this AviaDocument r)
		//{
		//	return new AviaDocumentAsserter(r);
		//}



		public static List<AviaDocument> AssertAll(
			this List<AviaDocument> products,
			Action<AviaDocumentAsserter> assert
		)
		{
			products.ForEach(r => assert(new AviaDocumentAsserter(r)));
			return products;
		}




		public static List<AviaDocument> Assert(
			this List<AviaDocument> products,
			params Action<AviaDocumentAsserter>[] asserts
		)
		{

			AreEqual(asserts.Length, products.Count);


			for (int i = 0, len = asserts.Length; i < len; i++)
			{
				asserts[i](new AviaDocumentAsserter(products[i]));
			}


			return products;

		}



		//---g

	}






	//===g



}
