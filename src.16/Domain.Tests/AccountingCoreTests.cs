using System;
using System.Diagnostics;
using System.Linq;

using Luxena.Domain;
using NUnit.Framework;
// ReSharper disable AccessToModifiedClosure


namespace Luxena.Travel.Domain.Tests.Sandbox
{

	using Travel.Domain;


	public class InitAccountingTestboxAttribute : InitDomainAttribute
	{
		public override void TruncateDatabase(Domain db)
		{
			db.Database.ExecuteSqlCommand(@"
delete from lt_order_item cascade;
delete from lt_product cascade;
delete from lt_payment cascade;
delete from lt_internal_transfer cascade;
delete from lt_order_check cascade;
delete from lt_order cascade;
"
			);
		}
	}


	public class OrderCheckAttribute : CategoryAttribute { }
	public class PaymentAttribute : CategoryAttribute { }
	public class PositionAttribute : CategoryAttribute { }
	public class RecreateOrderItemsAttribute : CategoryAttribute { }
	public class ServiceFeeAttribute : CategoryAttribute { }
	public class TransactionAttribute : CategoryAttribute { }


	[InitAccountingTestbox(UseTruncateDatabase = true)]
	public class AccountingCoreTests : DomainTest
	{

		#region New Entities

		[DebuggerStepThrough]
		private Product NewProduct(int index) =>
			new AviaTicket
			{
				IssueDate = DateTime.Today,
				Name = "Product" + index,
				Number = "000000000" + index,
				EqualFare = new Money("UAH", index * (1000 - 120)),
				ServiceFee = new Money("UAH", index * 120),
			};

		[DebuggerStepThrough]
		private Order NewOrder(int index) =>
			new Order
			{
				IssueDate = DateTime.Today,
				Number = "O.15-0000" + index,
				Customer = db.Customers.FirstOrDefault(),
			};

		[DebuggerStepThrough]
		private OrderItem NewOrderItem(int index, Order order = null) =>
			new OrderItem
			{
				Order = order,
				Text = "#" + index,
				Price = new Money("UAH", 12 * index),
				HasVat = true,
			};

		[DebuggerStepThrough]
		private Payment NewPayment(int index) =>
			new CashInOrderPayment
			{
				Date = DateTime.Today,
				Number = "P.15-0000" + index,
				AssignedTo = db.Agents.One(),
				RegisteredBy = db.Agents.One(),
				Amount = new Money("UAH", index * 1000),
				PostedOn = DateTime.Today,
			};

		[DebuggerStepThrough]
		private InternalTransfer NewInternalTransfer(int index, Order fromOrder, Order toOrder) =>
			new InternalTransfer
			{
				Date = DateTime.Today,
				Number = "P.15-0000" + index,
				FromOrder = fromOrder,
				ToOrder = toOrder,
				Amount = index * 1000,
			};


		[DebuggerStepThrough]
		private OrderItem NewOrderItem(Order order, Product product)
		{
			return new OrderItem
			{
				Order = order,
				Product = product,
			};
		}


		private void Assert_OneOrderItem(Order order, int index, string productId)
		{
			var orderItem = order.Items.By(a => a.ProductId == productId);
			Assert.NotNull(orderItem);
			Assert.AreEqual(true, orderItem.IsFullDocument);
			Assert.AreEqual(index, orderItem.Position);
			Assert.AreEqual(index * 1000, orderItem.Total.Amount);
		}

		private void Assert_TwoOrderItems(Order order, int index, string productId)
		{
			var orderItem1 = order.Items.By(a => a.ProductId == productId && a.IsProductData);
			Assert.NotNull(orderItem1);
			Assert.AreEqual(2 * index - 1, orderItem1.Position);
			Assert.AreEqual(index * 880, orderItem1.Total.Amount);

			var orderItem2 = order.Items.By(a => a.ProductId == productId && a.IsServiceFee);
			Assert.NotNull(orderItem2);
			Assert.AreEqual(2 * index, orderItem2.Position);
			Assert.AreEqual(index * 120, orderItem2.Total.Amount);
		}

		private void Assert_TwoOrderItems(Order order, int positionIndex, int index, string productId)
		{
			var orderItem1 = order.Items.By(a => a.ProductId == productId && a.IsProductData);
			Assert.NotNull(orderItem1);
			Assert.AreEqual(2 * positionIndex - 1, orderItem1.Position);
			Assert.AreEqual(index * 880, orderItem1.Total.Amount);

			var orderItem2 = order.Items.By(a => a.ProductId == productId && a.IsServiceFee);
			Assert.NotNull(orderItem2);
			Assert.AreEqual(2 * positionIndex, orderItem2.Position);
			Assert.AreEqual(index * 120, orderItem2.Total.Amount);
		}


		private void Assert_Order1_Product0_OrderItem0(string orderId)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(0, db.OrderItems.Count());
			Assert.AreEqual(0, db.Products.Count());

			Assert.True(order.Items.No());

			Assert.AreEqual(null, order.Total.Amount);
			Assert.AreEqual(null, order.Vat.Amount);
		}

		private void Assert_Order1_Product1_OrderItem0(string orderId, string productId1 = null)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(0, db.OrderItems.Count());
			Assert.AreEqual(1, db.Products.Count());

			Assert.True(order.Items.No());

			Assert.AreEqual(null, order.Total.Amount);
			Assert.AreEqual(null, order.Vat.Amount);

			if (productId1.Yes())
			{
				var product = db.Products.ById(productId1);
				Assert.NotNull(product);
				Assert.AreEqual(null, product.Order);
			}
		}

		private void Assert_Order1_Product1_OrderItem1(string orderId, string productId1)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(1, db.OrderItems.Count());
			Assert.AreEqual(1, db.Products.Count());

			Assert.AreEqual(1, order.Items?.Count);

			Assert_OneOrderItem(order, 1, productId1);

			Assert.AreEqual(1000, order.Total.Amount);
			Assert.AreEqual(20m, order.Vat.Amount);
		}

		private void Assert_Order1_Product1_OrderItem2(string orderId, string productId1 = null, int productIndex1 = 1)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(2, db.OrderItems.Count());
			Assert.AreEqual(1, db.Products.Count());

			Assert.AreEqual(2, order.Items.Count);

			Assert_TwoOrderItems(order, 1, productIndex1, productId1);

			Assert.AreEqual(productIndex1 * 1000, order.Total.Amount);
			Assert.AreEqual(productIndex1 * 20m, order.Vat.Amount);
		}

		private void Assert_Order1_Product2_OrderItem2(string orderId, string productId1 = null, string productId2 = null)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(2, db.OrderItems.Count());
			Assert.AreEqual(2, db.Products.Count());

			Assert.AreEqual(2, order.Items.Count);

			Assert_OneOrderItem(order, 1, productId1);
			Assert_OneOrderItem(order, 2, productId2);

			Assert.AreEqual(3000, order.Total.Amount);
			Assert.AreEqual(60, order.Vat.Amount);
		}

		private void Assert_Order1_Product2_OrderItem4(string orderId, string productId1 = null, string productId2 = null)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(4, db.OrderItems.Count());
			Assert.AreEqual(2, db.Products.Count());

			Assert.AreEqual(4, order.Items.Count);

			Assert_TwoOrderItems(order, 1, productId1);
			Assert_TwoOrderItems(order, 2, productId2);

			Assert.AreEqual(3000, order.Total.Amount);
			Assert.AreEqual(60, order.Vat.Amount);
		}


		private void Assert_Order1_Product3_OrderItem3(string orderId, string productId1 = null, string productId2 = null, string productId3 = null)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(3, db.OrderItems.Count());
			Assert.AreEqual(3, db.Products.Count());

			Assert.AreEqual(3, order.Items.Count);

			Assert_OneOrderItem(order, 1, productId1);
			Assert_OneOrderItem(order, 2, productId2);
			Assert_OneOrderItem(order, 3, productId2);

			Assert.AreEqual(6000, order.Total.Amount);
			Assert.AreEqual(120, order.Vat.Amount);
		}

		private void Assert_Order1_Product3_OrderItem6(string orderId, string productId1 = null, string productId2 = null, string productId3 = null)
		{
			RecreateDb();

			var order = db.Orders.ById(orderId);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(6, db.OrderItems.Count());
			Assert.AreEqual(3, db.Products.Count());

			Assert.AreEqual(6, order.Items.Count);

			Assert_TwoOrderItems(order, 1, productId1);
			Assert_TwoOrderItems(order, 2, productId2);
			Assert_TwoOrderItems(order, 3, productId3);


			Assert.AreEqual(6000, order.Total.Amount);
			Assert.AreEqual(120, order.Vat.Amount);
		}


		private void Assert_Order2_Product3_OrderItem6(
			string orderId1, string orderId2,
			string productId11 = null, string productId12 = null, string productId13 = null,
			string productId21 = null, string productId22 = null, string productId23 = null
		)
		{
			RecreateDb();

			Assert.AreEqual(2, db.Orders.Count());
			Assert.AreEqual(12, db.OrderItems.Count());
			Assert.AreEqual(6, db.Products.Count());

			var order1 = db.Orders.ById(orderId1);

			Assert.AreEqual(6, order1.Items.Count);

			Assert_TwoOrderItems(order1, 1, productId11);
			Assert_TwoOrderItems(order1, 2, productId12);
			Assert_TwoOrderItems(order1, 3, productId13);

			Assert.AreEqual(6000, order1.Total.Amount);
			Assert.AreEqual(120, order1.Vat.Amount);

			var order2 = db.Orders.ById(orderId2);

			Assert.AreEqual(6, order2.Items.Count);

			Assert_TwoOrderItems(order2, 1, productId21);
			Assert_TwoOrderItems(order2, 2, productId22);
			Assert_TwoOrderItems(order2, 3, productId23);

			Assert.AreEqual(6000, order2.Total.Amount);
			Assert.AreEqual(120, order2.Vat.Amount);
		}


		//private void TestEntryForOrder1(List<Domain.Transaction.Entry> entries, Order order, int itemCount)
		//{
		//	Assert.AreEqual(1, entries.Count);
		//	Assert.AreEqual(order, entries[0].Entity);

		//	Assert.AreEqual(itemCount, entries[0].Items.Count());

		//	foreach (var orderItemEntry in entries[0].Items)
		//	{
		//		Assert.IsInstanceOf<OrderItem>(orderItemEntry.Entity);
		//		Assert.AreEqual(0, orderItemEntry.Items.Count());
		//	}
		//}

		//private void TestEntryForOrder1(Domain.Transaction.Entry entry, Order order, params OrderItem[] orderItems)
		//{
		//	Assert.AreEqual(order, entry.Entity);

		//	Assert.AreEqual(orderItems.Length, entry.Items.Count());

		//	var i = 0;
		//	foreach (var orderItemEntry in entry.Items)
		//	{
		//		Assert.AreSame(orderItems[i], orderItemEntry.Entity);
		//		Assert.AreEqual(0, orderItemEntry.Items.Count());
		//		i++;
		//	}
		//}

		//private void TestEntryForOrder1(List<Domain.Transaction.Entry> entries, Order order, params OrderItem[] orderItems)
		//{
		//	Assert.AreEqual(1, entries.Count);
		//	TestEntryForOrder1(entries[0], order, orderItems);
		//}


		//private void TestEntryForOrder1(Domain.Transaction.Entry entry, Order order, params Product[] products)
		//{
		//	Assert.AreSame(order, entry.Entity);
		//	Assert.AreEqual(products.Length, entry.Items.Count());

		//	var i = 0;
		//	foreach (var orderItemEntry in entry.Items)
		//	{
		//		Assert.IsInstanceOf<OrderItem>(orderItemEntry.Entity);
		//		Assert.AreEqual(1, orderItemEntry.Items.Count());
		//		Assert.AreSame(products[i], orderItemEntry.Items[0].Entity);
		//		i++;
		//	}
		//}

		//private void TestEntryForOrder1(List<Domain.Transaction.Entry> entries, Order order, params Product[] products)
		//{
		//	Assert.AreEqual(1, entries.Count);
		//	TestEntryForOrder1(entries[0], order, products);
		//}

		#endregion


		[Test]
		public void Test01_1()
		{
			var order1 = NewOrder(1);

			var orderItem1 = NewOrderItem(1);

			db.Commit(() =>
			{
				orderItem1.Order = order1;
				orderItem1.Save(db);
			});


			RecreateDb();

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(1, db.OrderItems.Count());

			var order2 = db.Orders.ById(order1.Id);
			Assert.AreEqual(12, order2.Total.Amount);
			Assert.AreEqual(2, order2.Vat.Amount);
			Assert.AreEqual(1, order2.Items.Count);
		}


		[Test]
		public void Test01_1_1()
		{
			var order1 = NewOrder(1);

			db.Commit(() => order1.Save(db));

			RecreateDb();

			order1 = db.Orders.ById(order1.Id);
			var orderItem1 = NewOrderItem(1);

			db.Commit(() =>
			{
				orderItem1.OrderId = order1.Id;
				orderItem1.Save(db);
			});


			RecreateDb();

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(1, db.OrderItems.Count());

			order1 = db.Orders.ById(order1.Id);
			Assert.AreEqual(12, order1.Total.Amount);
			Assert.AreEqual(2, order1.Vat.Amount);
			Assert.AreEqual(1, order1.Items.Count);
		}


		[Test]
		public void Test01_2()
		{
			var order1 = NewOrder(1);

			var orderItem1 = NewOrderItem(1);
			var orderItem2 = NewOrderItem(2);

			db.Commit(() =>
			{
				orderItem1.Order = order1;
				orderItem2.Order = order1;

				orderItem1.Save(db);
				orderItem2.Save(db);
			});


			RecreateDb();

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(2, db.OrderItems.Count());

			order1 = db.Orders.ById(order1.Id);

			Assert.AreEqual(2, order1.Items.Count);
			Assert.AreEqual(36, order1.Total.Amount);
			Assert.AreEqual(6, order1.Vat.Amount);
		}


		[Test]
		public void Test01_3()
		{
			var order1 = NewOrder(1);

			var orderItem1 = NewOrderItem(1);
			var orderItem2 = NewOrderItem(2);
			var orderItem3 = NewOrderItem(3);

			db.Commit(() =>
			{
				orderItem1.Order = order1;
				orderItem2.Order = order1;
				orderItem3.Order = order1;

				orderItem1.Save(db);
				orderItem2.Save(db);
				orderItem3.Save(db);
			});

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(3, db.OrderItems.Count());

			Assert.AreEqual(72, order1.Total.Amount);
			Assert.AreEqual(12, order1.Vat.Amount);
		}


		[Test]
		public void Test01_4()
		{
			var order1 = NewOrder(1);
			var orderItem1 = NewOrderItem(1);
			var orderItem2 = NewOrderItem(2);
			var orderItem3 = NewOrderItem(3);

			db.Commit(() =>
			{
				orderItem1.Order = order1;
				orderItem2.Order = order1;

				orderItem1.Save(db);
				orderItem2.Save(db);
			});


			db.Commit(() =>
			{
				orderItem3.Order = order1;
				orderItem3.Save(db);
			});


			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(3, db.OrderItems.Count());

			Assert.AreEqual(72, order1.Total.Amount);
			Assert.AreEqual(12, order1.Vat.Amount);
		}


		[Test]
		public void Test01_5()
		{
			var order1 = NewOrder(1);
			var order2 = NewOrder(2);
			var orderItem1 = NewOrderItem(1);
			var orderItem2 = NewOrderItem(2);
			var orderItem3 = NewOrderItem(3);

			db.Commit(() =>
			{
				orderItem1.Order = order1;
				orderItem2.Order = order1;
				orderItem3.Order = order2;

				orderItem1.Save(db);
				orderItem2.Save(db);
				orderItem3.Save(db);
			});

			db.Commit(() =>
			{
				orderItem3.Order = order1;
				orderItem3.Save(db);
			});

			Assert.AreEqual(2, db.Orders.Count());
			Assert.AreEqual(3, db.OrderItems.Count());

			Assert.AreEqual(72, order1.Total.Amount);
			Assert.AreEqual(12, order1.Vat.Amount);
		}


		[Test]
		public void Test01_6()
		{
			var order1 = NewOrder(1);
			var order2 = NewOrder(2);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);

				NewOrderItem(1, order2).Save(db);
				NewOrderItem(2, order2).Save(db);
				NewOrderItem(3, order2).Save(db);
			});


			Assert.AreEqual(2, db.Orders.Count());
			Assert.AreEqual(6, db.OrderItems.Count());

			Assert.AreEqual(72, order1.Total.Amount);
			Assert.AreEqual(12, order1.Vat.Amount);
			Assert.AreEqual(72, order2.Total.Amount);
			Assert.AreEqual(12, order2.Vat.Amount);
		}

		[Test]
		public void Test02_1()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);
			});


			var orderItem3 = order1.Items.By(2);

			db.Commit(() =>
			{
				orderItem3.Order = null;

				orderItem3.Save(db);
			});

			Assert.IsNull(orderItem3.Order);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(2, db.OrderItems.Count());

			Assert.AreEqual(36, order1.Total.Amount);
			Assert.AreEqual(6, order1.Vat.Amount);
		}

		[Test]
		public void Test02_2()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);
			});


			var orderItem3 = order1.Items.By(2);

			db.Commit(() =>
			{
				orderItem3.Order = null;
				orderItem3.Save(db);
				orderItem3.Delete(db);
			});

			Assert.IsNull(orderItem3.Order);

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(2, db.OrderItems.Count());

			Assert.AreEqual(36, order1.Total.Amount);
			Assert.AreEqual(6, order1.Vat.Amount);
		}


		[Test]
		public void Test03_1()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);
			});


			db.Commit(() =>
			{
				var orderItem3 = order1.Items.By(2);
				orderItem3.Delete(db);
			});


			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(2, db.OrderItems.Count());

			Assert.AreEqual(36, order1.Total.Amount);
			Assert.AreEqual(6, order1.Vat.Amount);
		}


		[Test]
		public void Test03_2()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);
			});


			db.Commit(() =>
			{
				var orderItem2 = order1.Items.By(1);
				var orderItem3 = order1.Items.By(2);

				orderItem3.Delete(db);
				orderItem2.Delete(db);
				orderItem3.Delete(db);
			});


			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(1, db.OrderItems.Count());

			Assert.AreEqual(12, order1.Total.Amount);
			Assert.AreEqual(2, order1.Vat.Amount);
		}


		[Test]
		public void Test03_3()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);
			});


			db.Commit(() =>
			{
				order1.Items.By(1).Delete(db);
				order1.Items.By(2).Delete(db);
				NewOrderItem(2, order1).Save(db);
			});


			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(2, db.OrderItems.Count());

			Assert.AreEqual(36, order1.Total.Amount);
			Assert.AreEqual(6, order1.Vat.Amount);
		}


		[Test]
		public void Test09()
		{

			var order1 = NewOrder(1);
			var order2 = NewOrder(2);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);

				NewOrderItem(1, order2).Save(db);
				NewOrderItem(2, order2).Save(db);
				NewOrderItem(3, order2).Save(db);
			});

			db.Commit(() =>
			{
				order1.Items.By(1).Delete(db);
				order1.Items.By(2).Delete(db);
				order2.Items.By(2).Delete(db);
			});


			Assert.AreEqual(2, db.Orders.Count());
			Assert.AreEqual(3, db.OrderItems.Count());

			Assert.AreEqual(12, order1.Total.Amount);
			Assert.AreEqual(2, order1.Vat.Amount);
			Assert.AreEqual(36, order2.Total.Amount);
			Assert.AreEqual(6, order2.Vat.Amount);
		}


		[Test]
		public void Test10()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
				NewOrderItem(2, order1).Save(db);
				NewOrderItem(3, order1).Save(db);
			});

			db.Commit(() =>
			{
				order1.Items.By(2).Update(db, a => a.Price.Amount = 84);
			});


			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(3, db.OrderItems.Count());

			Assert.AreEqual(120, order1.Total.Amount);
			Assert.AreEqual(20, order1.Vat.Amount);
		}


		[Test]
		public void Test11()
		{
			var order1 = NewOrder(1);
			var orderItem1 = NewOrderItem(1, order1);

			db.Commit(() => orderItem1.Save(db), useFlush: false);
			db.Commit(() => orderItem1.Delete(db));



			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(0, db.OrderItems.Count());

			Assert.True(order1.Items.No());
		}


		[Test]
		public void Test20_0()
		{
			var product1 = NewProduct(1);

			db.Commit(() => product1.Save(db));

			Assert.AreEqual(1, db.Products.Count());
		}

		[Test]
		public void Test20_1()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				product1.Save(db);
				NewOrderItem(order1, product1).Save(db);
			});

			Assert_Order1_Product1_OrderItem2(order1.Id, productId1: product1.Id);
		}

		[Test]
		public void Test20_2()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);

			db.Commit(() =>
			{
				product1.Save(db);
				product2.Save(db);

				NewOrderItem(order1, product1).Save(db);
				NewOrderItem(order1, product2).Save(db);
			});

			Assert_Order1_Product2_OrderItem4(order1.Id, productId1: product1.Id, productId2: product2.Id);
		}

		[Test]
		public void Test20_3()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var product3 = NewProduct(3);

			db.Commit(() =>
			{
				product1.Save(db);
				product2.Save(db);
				product3.Save(db);

				NewOrderItem(order1, product1).Save(db);
				NewOrderItem(order1, product2).Save(db);
				NewOrderItem(order1, product3).Save(db);
			});

			Assert_Order1_Product3_OrderItem6(order1.Id, productId1: product1.Id, productId2: product2.Id, productId3: product3.Id);
		}

		[Test]
		public void Test20_4()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var product3 = NewProduct(3);

			db.Commit(() =>
			{
				NewOrderItem(order1, product1).Save(db);
				NewOrderItem(order1, product2).Save(db);
				NewOrderItem(order1, product3).Save(db);
			});

			Assert_Order1_Product3_OrderItem6(order1.Id, productId1: product1.Id, productId2: product2.Id, productId3: product3.Id);
		}


		[Test]
		public void Test20_5()
		{
			var order1 = NewOrder(1);
			var order2 = NewOrder(2);

			var product11 = NewProduct(1);
			var product12 = NewProduct(2);
			var product13 = NewProduct(3);

			var product21 = NewProduct(1);
			var product22 = NewProduct(2);
			var product23 = NewProduct(3);

			db.Commit(() =>
			{
				NewOrderItem(order1, product11).Save(db);
				NewOrderItem(order1, product12).Save(db);
				NewOrderItem(order1, product13).Save(db);

				NewOrderItem(order2, product21).Save(db);
				NewOrderItem(order2, product22).Save(db);
				NewOrderItem(order2, product23).Save(db);
			});

			Assert_Order2_Product3_OrderItem6(
				order1.Id, order2.Id,
				productId11: product11.Id, productId12: product12.Id, productId13: product13.Id,
				productId21: product21.Id, productId22: product22.Id, productId23: product23.Id
			);
		}


		[Test]
		public void Test20_6()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				db.Commit(() => NewOrderItem(order1, product1).Save(db));
				product1.Save(db);
			});

			Assert_Order1_Product1_OrderItem2(order1.Id, productId1: product1.Id);
		}


		[Test, Transaction, Position]
		public void Test20_7()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var product3 = NewProduct(3);

			db.Commit(() =>
			{
				product1.Save(db);
				product2.Save(db);

				db.Commit(() => NewOrderItem(order1, product1).Save(db));
				NewOrderItem(order1, product2).Save(db);
				NewOrderItem(order1, product3).Save(db);

				product3.Save(db);
			});

			Assert_Order1_Product3_OrderItem6(order1.Id, productId1: product1.Id, productId2: product2.Id, productId3: product3.Id);
		}

		[Test, Transaction, Position]
		public void Test20_8()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var product3 = NewProduct(3);

			db.Commit(() =>
			{
				NewOrderItem(order1, product2).Save(db);
				NewOrderItem(order1, product3).Save(db);
				db.Commit(() => NewOrderItem(order1, product1).Save(db));
			});

			Assert_Order1_Product3_OrderItem6(order1.Id, productId1: product1.Id, productId2: product2.Id, productId3: product3.Id);
		}

		[Test]
		public void Test21()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var product3 = NewProduct(3);

			db.Commit(() =>
			{
				NewOrderItem(order1, product1).Save(db);
				NewOrderItem(order1, product2).Save(db);
				NewOrderItem(order1, product3).Save(db);
			});

			db.Commit(() =>
			{
				product3.EqualFare.Amount = 3500 - 420;
				product3.ServiceFee.Amount = 420;

				product3.Save(db);
			});

			Assert.AreEqual(6500, order1.Total.Amount);
			Assert.AreEqual(130, order1.Vat.Amount);
		}

		[Test]
		public void Test21_1()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				NewOrderItem(order1, product1).Save(db);
			});

			db.Commit(() =>
			{
				product1.EqualFare.Amount = 1500 - 180;
				product1.ServiceFee.Amount = 180;

				product1.Save(db);
			});

			Assert.AreEqual(1500, order1.Total.Amount);
			Assert.AreEqual(30, order1.Vat.Amount);
		}


		[Test]
		public void Test21_2()
		{
			var order1 = NewOrder(1);
			var order2 = NewOrder(2);
			var product11 = NewProduct(1);
			var product12 = NewProduct(2);
			var product13 = NewProduct(3);
			var product21 = NewProduct(1);
			var product22 = NewProduct(2);
			var product23 = NewProduct(3);

			db.Commit(() =>
			{
				NewOrderItem(order1, product11).Save(db);
				NewOrderItem(order1, product12).Save(db);
				NewOrderItem(order1, product13).Save(db);

				NewOrderItem(order2, product21).Save(db);
				NewOrderItem(order2, product22).Save(db);
				NewOrderItem(order2, product23).Save(db);
			});

			db.Commit(() =>
			{
				product13.EqualFare.Amount = 3500 - 420;
				product13.ServiceFee.Amount = 420;
				product13.Save(db);

				product23.EqualFare.Amount = 3500 - 420;
				product23.ServiceFee.Amount = 420;
				product23.Save(db);
			});

			Assert.AreEqual(6500, order1.Total.Amount);
			Assert.AreEqual(130, order1.Vat.Amount);
			Assert.AreEqual(6500, order2.Total.Amount);
			Assert.AreEqual(130, order2.Vat.Amount);
		}


		[Test]
		public void Test24()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db);
			});

			db.Commit(() =>
			{
				order1.Items.By(0).Delete(db);
			});

			Assert_Order1_Product0_OrderItem0(order1.Id);
		}

		[Test]
		public void Test24_1()
		{
			var order1 = NewOrder(1);

			db.Commit(() =>
			{
				NewOrderItem(1, order1).Save(db).Delete(db);
			});

			Assert_Order1_Product0_OrderItem0(order1.Id);
		}

		[Test]
		public void Test24_2()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				NewOrderItem(order1, product1).Save(db);
			});

			db.Commit(() =>
			{
				product1.Order = null;
				product1.Save(db);
			});

			Assert_Order1_Product1_OrderItem0(order1.Id, productId1: product1.Id);
		}

		[Test]
		public void Test24_3()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);

			try
			{
				db.Commit(() =>
				{
					product1.Save(db);
					product2.Save(db);
					NewOrderItem(order1, product1).Save(db);
				});

				db.Commit(() =>
				{
					product1.Delete(db);
					NewOrderItem(order1, product2).Save(db);
				});
			}
			catch (Exception ex)
			{
				throw;
			}

			Assert_Order1_Product1_OrderItem2(order1.Id, productId1: product2.Id, productIndex1: 2);
		}


		[Test]
		public void Test24_4()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);

			db.Commit(() =>
			{
				product2.Save(db);
				product1.Save(db);
				NewOrderItem(order1, product1).Save(db);
			});

			db.Commit(() =>
			{
				NewOrderItem(order1, product2).Save(db);
				product1.Delete(db);
			});

			Assert_Order1_Product1_OrderItem2(order1.Id, productId1: product2.Id, productIndex1: 2);
		}


		[Test]
		public void Test24_5()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var product3 = NewProduct(3);

			db.Commit(() =>
			{
				product1.Save(db);
				product2.Save(db);
				product3.Save(db);
				NewOrderItem(order1, product1).Save(db);
				product2.Save(db);
				NewOrderItem(order1, product2).Save(db);
				product1.Save(db);
				product3.Save(db);
				NewOrderItem(order1, product3).Save(db);
			});

			db.Commit(() =>
			{
				product3.Delete(db);
				product1.Delete(db);
			});

			Assert_Order1_Product1_OrderItem2(order1.Id, productId1: product2.Id, productIndex1: 2);
		}


		[Test, RecreateOrderItems]
		public void Test28_1()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				product1.Order = order1;
				order1.SeparateServiceFee = false;
				product1.Save(db);
			});

			Assert_Order1_Product1_OrderItem1(order1.Id, productId1: product1.Id);
		}


		[Test, RecreateOrderItems]
		public void Test28_2()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				order1.SeparateServiceFee = true;
				product1.Order = order1;
				product1.Save(db);
			});

			Assert_Order1_Product1_OrderItem2(order1.Id, productId1: product1.Id);
		}

		//[Test]
		//public void Test28_3()
		//{
		//	var order1 = NewOrder(1);
		//	var product1 = NewProduct(1);

		//	try
		//	{
		//		db.Commit(() =>
		//		{
		//			product1.Order = order1;
		//			db.AppConfiguration.AviaOrderItemGenerationOption = ProductOrderItemGenerationOption.AlwaysOneOrderItem;
		//			product1.Save(db);
		//		});

		//	}
		//	finally
		//	{
		//		db.Commit(() => db.AppConfiguration.Update(db, a =>
		//			a.AviaOrderItemGenerationOption = ProductOrderItemGenerationOption.ManualSetting
		//		));
		//	}

		//	Assert_OneOrder_OneProduct_OneOrderItem(order1.Id);
		//}


		[Test, ServiceFee, RecreateOrderItems]
		public void Test28_4()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			product1.EqualFare.Amount = 1000;
			product1.ServiceFee.Amount = 0;

			db.Commit(() =>
			{
				product1.Order = order1;
				product1.Save(db);
			});

			Assert_OneOrderItem(order1, 1, productId: product1.Id);

			db.Commit(() =>
			{
				product1.EqualFare.Amount = 880;
				product1.ServiceFee.Amount = 120;
				product1.Save(db);
			});

			Assert_Order1_Product1_OrderItem2(order1.Id, productId1: product1.Id);
		}

		[Test, ServiceFee, RecreateOrderItems]
		public void Test28_5()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);

			product2.EqualFare.Amount = 2000;
			product2.ServiceFee.Amount = null;

			db.Commit(() =>
			{
				product1.Order = order1;
				product2.Order = order1;
				product1.Save(db);
				product2.Save(db);
			});

			Assert.AreEqual(3, order1.Items.Count);
			Assert.AreEqual(1, order1.Items.By(a => a.Product == product1 && a.IsProductData).Position);
			Assert.AreEqual(2, order1.Items.By(a => a.Product == product1 && a.IsServiceFee).Position);
			Assert.AreEqual(3, order1.Items.By(a => a.Product == product2 && a.IsFullDocument).Position);

			db.Commit(() =>
			{
				product2.EqualFare.Amount = 2000 - 240;
				product2.ServiceFee.Amount = 240;
				product2.Save(db);
			});

			Assert_Order1_Product2_OrderItem4(order1.Id, productId1: product1.Id, productId2: product2.Id);
		}

		[Test, ServiceFee, RecreateOrderItems, Position]
		public void Test28_6()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);

			product1.EqualFare.Amount = 1000;
			product1.ServiceFee.Amount = null;

			db.Commit(() =>
			{
				product1.Order = order1;
				product2.Order = order1;
				product1.Save(db);
				product2.Save(db);
			});

			Assert.AreEqual(3, order1.Items.Count);
			Assert.AreEqual(1, order1.Items.By(a => a.Product == product1 && a.IsFullDocument).Position);
			Assert.AreEqual(2, order1.Items.By(a => a.Product == product2 && a.IsProductData).Position);
			Assert.AreEqual(3, order1.Items.By(a => a.Product == product2 && a.IsServiceFee).Position);

			db.Commit(() =>
			{
				product1.EqualFare.Amount = 880;
				product1.ServiceFee.Amount = 120;
				product1.Save(db);
			});

			Assert_Order1_Product2_OrderItem4(order1.Id, productId1: product1.Id, productId2: product2.Id);
		}


		[Test, Position]
		public void Test28_7()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);

			db.Commit(() =>
			{
				product1.Order = order1;
				product1.Save(db);
			});

			db.Commit(() =>
			{
				product2.Order = order1;
				product2.Save(db);
			});

			Assert_Order1_Product2_OrderItem4(order1.Id, productId1: product1.Id, productId2: product2.Id);
		}


		[Test]
		public void Test29()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var product3 = NewProduct(3);

			db.Commit(() =>
			{
				NewOrderItem(order1, product1).Save(db);
				NewOrderItem(order1, product2).Save(db);
				NewOrderItem(order1, product3).Save(db);
			});

			db.Commit(() =>
			{
				order1.Items.Delete(db);
			});

			Assert.AreEqual(1, db.Orders.Count());
			Assert.AreEqual(0, db.OrderItems.Count());
			Assert.AreEqual(3, db.Products.Count());

			Assert.AreEqual(0, order1.Items.Count);

			Assert.AreEqual(null, product1.Order);
			Assert.AreEqual(null, product2.Order);
			Assert.AreEqual(null, product3.Order);

			Assert.AreEqual(null, order1.Total.Amount);
			Assert.AreEqual(null, order1.Vat.Amount);
		}


		[Test]
		public void Test30_1()
		{
			var order1 = NewOrder(1);

			var orderItem1 = NewOrderItem(1);

			db.Commit(() =>
			{
				orderItem1.Order = order1;
				orderItem1.Save(db);
			});


			orderItem1.Order = null;

			using (var db2 = new Domain())
			{
				var o = (OrderItem)orderItem1.GetDbSet(db2).Find(orderItem1.Id);

				Assert.AreEqual(order1, o.Order);
			}
		}


		[Test]
		public void Test30_2()
		{
			var order1 = NewOrder(1);

			var orderItem1 = NewOrderItem(1);

			db.Commit(() =>
			{
				orderItem1.Order = order1;

				orderItem1.Save(db);
			});

			orderItem1.Order = null;
			order1.Number += "***";


			var o = (OrderItem)orderItem1.GetFromDb();

			Assert.AreEqual(order1, o.Order);


		}

		[Test]
		public void Test31_1()
		{
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				product1.Save(db);
			});

			Assert.AreEqual(1, db.Products.Count());

			db.Commit(() =>
			{
				product1.Name += "qqq";
				product1.Save(db);
			});

			Assert.AreEqual(1, db.Products.Count());
		}


		[Test, Payment]
		public void Test32_1_Payment()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var payment1 = NewPayment(1);

			db.Commit(() =>
			{
				product1.Update(db, a => a.Order = order1);
				product2.Update(db, a => a.Order = order1);
				payment1.Update(db, a => a.Order = order1);
			});

			Assert_Order1_Product2_OrderItem4(order1.Id, productId1: product1.Id, productId2: product2.Id);

			order1 = db.Orders.ById(order1.Id);
			payment1 = db.Payments.ById(payment1.Id);

			Assert.AreEqual(1000, payment1.Amount.Amount);
			Assert.True(payment1.IsPosted);

			Assert.AreEqual(1000, order1.Paid.Amount);
			Assert.AreEqual(2000, order1.TotalDue.Amount);
			Assert.AreEqual(-2000, order1.DeliveryBalance);
		}

		[Test, Payment]
		public void Test32_2_Payment()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var payment1 = NewPayment(1);
			var payment2 = NewPayment(2);

			db.Commit(() =>
			{
				payment1.Update(db, a => a.Order = order1);
				product1.Update(db, a => a.Order = order1);
				product2.Update(db, a => a.Order = order1);
				payment2.Update(db, a => a.Order = order1);
			});

			Assert.AreEqual(3000, order1.Paid.Amount);
			Assert.AreEqual(0, order1.TotalDue.Amount);
			Assert.AreEqual(0, order1.DeliveryBalance);
		}

		[Test, Payment]
		public void Test32_3_Payment()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);
			var product2 = NewProduct(2);
			var payment1 = NewPayment(1);
			var payment2 = NewPayment(2);

			db.Commit(() =>
			{
				product1.Update(db, a => a.Order = order1);
				product2.Update(db, a => a.Order = order1);
				payment1.Update(db, a => a.Order = order1);
			});

			db.Commit(() =>
			{
				payment2.Update(db, a => a.Order = order1);
			});

			Assert.AreEqual(3000, order1.Paid.Amount);
			Assert.AreEqual(0, order1.TotalDue.Amount);
			Assert.AreEqual(0, order1.DeliveryBalance);
		}

		[Test]
		public void Test33_1_InternalTransfer()
		{
			var order1 = NewOrder(1);
			var order2 = NewOrder(2);

			db.Commit(() =>
			{
				NewInternalTransfer(1, order1, order2).Save(db);
			});

			Assert.AreEqual(-1000, order1.Paid.Amount);
			Assert.AreEqual(1000, order2.Paid.Amount);
		}

		[Test, OrderCheck]
		public void Test34_1_OrderCheck()
		{
			var order1 = NewOrder(1);
			var product1 = NewProduct(1);

			db.Commit(() =>
			{
				product1.Update(db, a => a.Order = order1);
			});

			db.Commit(() =>
			{
				new OrderCheck
				{
					Id = "",
					Date = DateTimeOffset.Now,
					Person = db.Persons.One(),
					OrderId = order1.Id,
					CheckType = CheckType.Sale,
					CheckNumber = "00000001111",
					Currency = "UAH",
					CheckAmount = 1000,
					CheckVat = 20,
					PaymentType = CheckPaymentType.Cash,
				}.Save(db);
			});

			Assert.AreEqual(1, order1.Payments.Count);

			var payment1 = order1.Payments.One();

			Assert.AreEqual(1000, payment1.Amount.Amount);
			Assert.True(payment1.IsPosted);


			Assert.AreEqual(0, order1.TotalDue.Amount ?? 0);
			Assert.AreEqual(0, order1.VatDue.Amount ?? 0);
		}

	}

}