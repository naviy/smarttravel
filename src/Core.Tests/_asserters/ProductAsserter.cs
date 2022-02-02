using System;
using System.Collections.Generic;

using Luxena.Travel.Domain;

using NUnit.Framework;




namespace Luxena.Travel.Tests
{



	using static Assert;






	//===g






	public class ProductAsserter : ProductAsserter<Product, ProductAsserter>
	{

		public ProductAsserter(Product r) : base(r) { }

	}






	public class ProductAsserter<TProduct, TThis>
		where TProduct : Product
		where TThis : ProductAsserter<TProduct, TThis>
	{

		//---g



		protected ProductAsserter(TProduct r)
		{
			this.r = r;
		}



		//---g



		protected readonly TProduct r;



		//---g



		public TThis IssueDate(DateTime expected)
		{
			AreEqual(expected, r.IssueDate);
			return (TThis)this;
		}


		public TThis IssueDate(string expected)
		{
			AreEqual(DateTime.Parse(expected), r.IssueDate);
			return (TThis)this;
		}


		public TThis IssueDate(string expected, IFormatProvider provider)
		{
			AreEqual(DateTime.Parse(expected, provider), r.IssueDate);
			return (TThis)this;
		}



		public TThis PnrCode(string expected)
		{
			AreEqual(expected, r.PnrCode);
			return (TThis)this;
		}



		public TThis PassengerName(string expected)
		{
			AreEqual(expected, r.PassengerName);
			return (TThis)this;
		}



		public TThis TourCode(string expected)
		{
			AreEqual(expected, r.TourCode);
			return (TThis)this;
		}



		//---g



		public TThis Fares(
			string fareCurrency = null,
			decimal? fare = null,
			string equalFareCurrency = null,
			decimal? equalFare = null,
			string totalCurrency = null,
			decimal? total = null
		)
		{

			if (fareCurrency != null)
				AreEqual(fareCurrency, r.Fare.Currency.Code);

			if (fare != null)
				AreEqual(fare.Value, r.Fare.Amount);


			if (equalFareCurrency != null)
				AreEqual(equalFareCurrency, r.EqualFare?.Currency?.Code);

			if (equalFare != null)
				AreEqual(equalFare.Value, r.EqualFare?.Amount);


			if (totalCurrency != null)
				AreEqual(totalCurrency, r.Total.Currency.Code);

			if (total != null)
				AreEqual(total.Value, r.Total.Amount);


			return (TThis)this;

		}



		public TThis Fare(
			string fareCurrency,
			decimal fare
		)
		{
			AreEqual(fareCurrency, r.Fare.Currency.Code);
			AreEqual(fare, r.Fare.Amount);
			return (TThis)this;
		}



		public TThis EqualFare(
			string equalFareCurrency,
			decimal equalFare
		)
		{
			AreEqual(equalFareCurrency, r.EqualFare?.Currency?.Code);
			AreEqual(equalFare, r.EqualFare?.Amount);
			return (TThis)this;
		}



		public TThis Total(
			string totalCurrency,
			decimal total
		)
		{
			AreEqual(totalCurrency, r.Total.Currency.Code);
			AreEqual(total, r.Total.Amount);
			return (TThis)this;
		}



		public TThis FeesTotal(
			string feesTotalCurrency,
			decimal feesTotal
		)
		{
			AreEqual(feesTotalCurrency, r.FeesTotal.Currency.Code);
			AreEqual(feesTotal, r.FeesTotal.Amount);
			return (TThis)this;
		}



		//---g

		

		public TThis TicketingIataOffice(string expected)
		{
			AreEqual(expected, r.TicketingIataOffice);
			return (TThis)this;
		}



		public TThis IataOffice(string expected)
		{
			AreEqual(expected, r.TicketingIataOffice);
			return (TThis)this;
		}



		public TThis BookerOffice(string expected)
		{
			AreEqual(expected, r.BookerOffice);
			return (TThis)this;
		}



		public TThis BookerCode(string expected)
		{
			AreEqual(expected, r.BookerCode);
			return (TThis)this;
		}



		//---g

	}






	//===g






	public static partial class Extensions
	{

		//---g



		public static ProductAsserter Assert(this Product r)
		{
			return new ProductAsserter(r);
		}



		public static List<Product> AssertAll(this List<Product> products, Action<ProductAsserter> assert)
		{
			products.ForEach(r => assert(new ProductAsserter(r)));
			return products;
		}




		public static List<Product> Assert(
			this List<Product> products,
			params Action<ProductAsserter>[] asserts
		)
		{

			AreEqual(asserts.Length, products.Count);


			for (int i = 0, len = asserts.Length; i < len; i++)
			{
				asserts[i](new ProductAsserter(products[i]));
			}


			return products;

		}



		//---g

	}






	//===g



}
