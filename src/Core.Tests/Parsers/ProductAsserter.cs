using System;
using System.Collections.Generic;

using Luxena.Travel.Domain;

using NUnit.Framework;




namespace Luxena.Travel.Tests
{



	using static Assert;






	//===g






	public class ProductAsserter: ProductAsserter<Product, ProductAsserter> 
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



		protected TProduct r;



		//---g



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



		//---g

	}






	//===g



}
