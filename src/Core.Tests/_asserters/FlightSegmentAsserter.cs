using System;
using System.Collections.Generic;

using Luxena.Travel.Domain;

using NUnit.Framework;




namespace Luxena.Travel.Tests
{



	using static Assert;






	//===g






	public class FlightSegmentAsserter
	{

		//---g



		public FlightSegmentAsserter(FlightSegment r)
		{
			this.r = r;
		}



		//---g



		protected readonly FlightSegment r;



		//---g



		public FlightSegmentAsserter Position(int expected)
		{
			AreEqual(expected, r.Position);
			return this;
		}



		public FlightSegmentAsserter Stopover(bool expected)
		{
			AreEqual(expected, r.Stopover);
			return this;
		}



		public FlightSegmentAsserter CarrierIataCode(string expected)
		{
			AreEqual(expected, r.CarrierIataCode);
			return this;
		}



		public FlightSegmentAsserter FlightNumber(string expected)
		{
			AreEqual(expected, r.FlightNumber);
			return this;
		}



		public FlightSegmentAsserter ServiceClassCode(string expected)
		{
			AreEqual(expected, r.ServiceClassCode);
			return this;
		}



		public FlightSegmentAsserter FromAirport(string expected)
		{
			AreEqual(expected, r.FromAirportCode);
			return this;
		}



		public FlightSegmentAsserter ToAirport(string expected)
		{
			AreEqual(expected, r.ToAirportCode);
			return this;
		}



		public FlightSegmentAsserter FareBasis(string expected)
		{
			AreEqual(expected, r.FareBasis);
			return this;
		}



		public FlightSegmentAsserter Luggage(string expected)
		{
			AreEqual(expected, r.Luggage);
			return this;
		}



		//---g

	}






	//===g






	public static partial class Extensions
	{

		//---g



		public static FlightSegmentAsserter Assert(this FlightSegment r)
		{
			return new FlightSegmentAsserter(r);
		}



		public static List<FlightSegment> AssertAll(this List<FlightSegment> segments, Action<FlightSegmentAsserter> assert)
		{
			segments.ForEach(r => assert(new FlightSegmentAsserter(r)));
			return segments;
		}



		//---g







		//---g

	}






	//===g



}
