//using Luxena.Travel.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Domain;

//using NUnit.Framework;


//namespace Luxena.Travel.Tests.Accounting
//{
//	[TestFixture]
//	public class InternalTransferTests
//	{
//		[Test]
//		public void TestCreate()
//		{
//			var customer1 = new Person();
//			var customer2 = new Person();

//			var fromOrder = new Order();
//			fromOrder.SetCustomer(customer1);
//			fromOrder.SetTotal(new Money("UAH", 1000));

//			var toOrder = new Order();
//			toOrder.SetCustomer(customer2);
//			toOrder.SetTotal(new Money("UAH", 5000));

//			var transfer = new InternalTransfer
//			{
//				FromOrder = fromOrder,
//				ToOrder = toOrder,
//				Amount = 1000
//			};

//			Assert.AreEqual(2000, fromOrder.TotalDue.Amount);
//			Assert.AreEqual(4000, toOrder.TotalDue.Amount);

//			transfer.Amount = 5000;

//			Assert.AreEqual(6000, fromOrder.TotalDue.Amount);
//			Assert.AreEqual(0, toOrder.TotalDue.Amount);

//			Assert.AreSame(customer1, transfer.FromParty);
//			Assert.AreSame(customer2, transfer.ToParty);
//		}
//	}

//	[TestFixture]
//	public class OrderTests
//	{
//		[Test]
//		public void TestCreate()
//		{
//			var customer1 = new Person { Name = "Customer 1" };
//			var customer2 = new Person { Name = "Customer 2" };

//			var order = new Order();

//			order.SetCustomer(customer1);

//			order.SetTotal(new Money("UAH", 5000));

//			var payment1 = new CheckPayment();
//			payment1.SetOrder(order);
//			payment1.SetAmount(new Money("UAH", 1000));
//			payment1.Post();

//			var payment2 = new CheckPayment();
//			payment2.SetOrder(order);
//			payment2.SetAmount(new Money("UAH", 2000));
//			payment2.Post();

//			Assert.AreEqual(2000, order.TotalDue.Amount);

//			order.SetCustomer(customer2);

//			Assert.AreEqual(customer2, payment1.Payer);
//			Assert.AreEqual(customer2, payment2.Payer);
//		}
//	}
//}