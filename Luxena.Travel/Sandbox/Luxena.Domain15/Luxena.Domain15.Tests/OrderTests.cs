using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NUnit.Framework;


namespace Luxena.Domain15.Tests
{

	using Travel.Domain;

	[TestFixture]
	public class OrderTests
	{

		#region New Entities

		[DebuggerStepThrough]
		private static Product NewProduct(int index)
		{
			return new Product { Name = "Product" + index, FeeTotal = index * 8, ServiceFee = index * 2 };
		}

		[DebuggerStepThrough]
		private static Order NewOrder(int index)
		{
			return new Order { Number = "O.15-0000" + index };
		}

		[DebuggerStepThrough]
		private static OrderItem NewOrderItem(int index, Order order = null, decimal[] discounts = null, Product product = null)
		{
			var r = new OrderItem { Order = order, Text = "#" + index, Product = product };

			if (product == null)
				r.Total = index * 10;

			if (discounts != null)
			{
				r.Discounts = discounts
					.Select(a => NewOrderItemDiscount(r, a))
					.ToList();
			}

			return r;
		}

		[DebuggerStepThrough]
		private static OrderItemDiscount NewOrderItemDiscount(OrderItem orderItem, decimal total)
		{
			return new OrderItemDiscount { OrderItem = orderItem, Total = total };
		}


		private static void TestEntryForOrder1(List<Domain.Transaction.Entry> entries, Order order, int itemCount)
		{
			Assert.AreEqual(1, entries.Count);
			Assert.AreSame(order, entries[0].Entity);

			Assert.AreEqual(itemCount, entries[0].Items.Count);

			foreach (var orderItemEntry in entries[0].Items)
			{
				Assert.IsInstanceOf<OrderItem>(orderItemEntry.Entity);
				Assert.AreEqual(0, orderItemEntry.Items.Count);
			}
		}

		private static void TestEntryForOrder1(Domain.Transaction.Entry entry, Order order, params OrderItem[] orderItems)
		{
			Assert.AreSame(order, entry.Entity);

			Assert.AreEqual(orderItems.Length, entry.Items.Count);

			var i = 0;
			foreach (var orderItemEntry in entry.Items)
			{
				Assert.AreSame(orderItems[i], orderItemEntry.Entity);
				Assert.AreEqual(0, orderItemEntry.Items.Count);
				i++;
			}
		}

		private static void TestEntryForOrder1(List<Domain.Transaction.Entry> entries, Order order, params OrderItem[] orderItems)
		{
			Assert.AreEqual(1, entries.Count);
			TestEntryForOrder1(entries[0], order, orderItems);
		}


		private static void TestEntryForOrder1(Domain.Transaction.Entry entry, Order order, params Product[] products)
		{
			Assert.AreSame(order, entry.Entity);
			Assert.AreEqual(products.Length, entry.Items.Count);

			var i = 0;
			foreach (var orderItemEntry in entry.Items)
			{
				Assert.IsInstanceOf<OrderItem>(orderItemEntry.Entity);
				Assert.AreEqual(1, orderItemEntry.Items.Count);
				Assert.AreSame(products[i], orderItemEntry.Items[0].Entity);
				i++;
			}
		}

		private static void TestEntryForOrder1(List<Domain.Transaction.Entry> entries, Order order, params Product[] products)
		{
			Assert.AreEqual(1, entries.Count);
			TestEntryForOrder1(entries[0], order, products);
		}

		#endregion


		[Test]
		public void OrderTests01_1()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				var orderItem1 = NewOrderItem(1);

				db.Commit(() =>
				{
					orderItem1.Order = order1;
					orderItem1.Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem1);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, order1.Items.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);
			}
		}


		[Test]
		public void OrderTests01_2()
		{

			using (var db = new Domain())
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

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem1, orderItem2);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(2, db.OrderItems.Count);
				Assert.AreEqual(2, order1.Items.Count);

				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests01_3()
		{

			using (var db = new Domain())
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

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem1, orderItem2, orderItem3);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
			}
		}


		[Test]
		public void OrderTests01_4()
		{

			using (var db = new Domain())
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

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem1, orderItem2);
				});


				db.Commit(() =>
				{
					orderItem3.Order = order1;
					orderItem3.Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem3);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
			}
		}


		[Test]
		public void OrderTests01_5()
		{

			using (var db = new Domain())
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

					db.Calculated = () =>
					{
						TestEntryForOrder1(db.Entries[0], order1, orderItem1, orderItem2);
						TestEntryForOrder1(db.Entries[1], order2, orderItem3);
					};
				});


				db.Commit(() =>
				{
					orderItem3.Order = order1;
					orderItem3.Save(db);

					db.Calculated = () =>
					{
						TestEntryForOrder1(db.Entries[0], order2, new OrderItem[0]);
						TestEntryForOrder1(db.Entries[1], order1, orderItem3);
					};
				});


				Assert.AreEqual(2, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
			}
		}


		[Test]
		public void OrderTests01_6()
		{

			using (var db = new Domain())
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


				Assert.AreEqual(2, db.Orders.Count);
				Assert.AreEqual(6, db.OrderItems.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
				Assert.AreEqual(60, order2.Total);
				Assert.AreEqual(12, order2.Vat);
			}
		}

		[Test]
		public void OrderTests02_1()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
					NewOrderItem(2, order1).Save(db);
					NewOrderItem(3, order1).Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, 3);
				});


				db.Commit(() =>
				{
					var orderItem3 = order1.Items[2];
					orderItem3.Order = null;
					orderItem3.Save(db);
					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem3);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);

				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests03_1()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
					NewOrderItem(2, order1).Save(db);
					NewOrderItem(3, order1).Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, 3);
				});


				db.Commit(() =>
				{
					var orderItem3 = order1.Items[2];

					orderItem3.Delete(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem3);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(2, db.OrderItems.Count);

				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests03_2()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
					NewOrderItem(2, order1).Save(db);
					NewOrderItem(3, order1).Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, 3);
				});


				db.Commit(() =>
				{
					var orderItem2 = order1.Items[1];
					var orderItem3 = order1.Items[2];

					orderItem3.Delete(db);
					orderItem2.Delete(db);
					orderItem3.Delete(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, orderItem3, orderItem2);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);
			}
		}


		[Test]
		public void OrderTests03_3()
		{

			using (var db = new Domain())
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
					order1.Items[1].Delete(db);
					order1.Items[2].Delete(db);
					NewOrderItem(2, order1).Save(db);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(2, db.OrderItems.Count);

				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests09()
		{

			using (var db = new Domain())
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
					order1.Items[1].Delete(db);
					order1.Items[2].Delete(db);
					order2.Items[2].Delete(db);
				});


				Assert.AreEqual(2, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);
				Assert.AreEqual(30, order2.Total);
				Assert.AreEqual(6, order2.Vat);
			}
		}


		[Test]
		public void OrderTests10()
		{

			using (var db = new Domain())
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
					order1.Items[2].Total = 70;
					order1.Items[2].Save(db);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);

				Assert.AreEqual(100, order1.Total);
				Assert.AreEqual(20, order1.Vat);
			}
		}


		[Test]
		public void OrderTests11()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
					NewOrderItem(2, order1).Save(db);
					var orderItem3 = NewOrderItem(3, order1, new[] { 30m });
					orderItem3.Save(db);
					orderItem3.Discounts.Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(1, db.OrderItemDiscounts.Count);
				Assert.AreEqual(1, db.OrderItems[2].Discounts.Count);


				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests12()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
					NewOrderItem(2, order1).Save(db);
					NewOrderItem(3, order1).Save(db);
				});


				using (var tran = db.BeginWork())
				{
					NewOrderItemDiscount(order1.Items[2], 30).Save(db);
					tran.Calculated = () => { var entries = tran.Entries; };
				}


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(1, db.OrderItemDiscounts.Count);
				Assert.AreEqual(1, db.OrderItems[2].Discounts.Count);


				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests13()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var orderItem3 = NewOrderItem(3, order1, new[] { 30m, 10m });

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
					NewOrderItem(2, order1).Save(db);
					orderItem3.Save(db);
					orderItem3.Discounts.Save(db);
				});

				db.Commit(() =>
				{
					orderItem3.Discounts[1].Delete(db);
					orderItem3.Discounts.Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(1, db.OrderItemDiscounts.Count);
				Assert.AreEqual(1, db.OrderItems[2].Discounts.Count);


				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests14()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var orderItem3 = NewOrderItem(3, order1, new[] { 30m, 10m });

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
					NewOrderItem(2, order1).Save(db);
					orderItem3.Save(db);
					orderItem3.Discounts.Save(db);
				});

				db.Commit(() =>
				{
					orderItem3.Discounts[1].Total = 0;
					orderItem3.Discounts.Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(2, db.OrderItemDiscounts.Count);
				Assert.AreEqual(2, db.OrderItems[2].Discounts.Count);


				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}


		[Test]
		public void OrderTests20_1()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);

				db.Commit(() =>
				{
					product1.Save(db);

					NewOrderItem(1, order1, product: product1).Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);
			}
		}

		[Test]
		public void OrderTests20_2()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);
				var product2 = NewProduct(2);

				db.Commit(() =>
				{
					product1.Save(db);
					product2.Save(db);

					NewOrderItem(1, order1, product: product1).Save(db);
					NewOrderItem(2, order1, product: product2).Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1, product2);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(2, db.OrderItems.Count);
				Assert.AreEqual(2, db.Products.Count);

				Assert.AreEqual(30, order1.Total);
				Assert.AreEqual(6, order1.Vat);
			}
		}

		[Test]
		public void OrderTests20_3()
		{

			using (var db = new Domain())
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

					NewOrderItem(1, order1, product: product1).Save(db);
					NewOrderItem(2, order1, product: product2).Save(db);
					NewOrderItem(3, order1, product: product3).Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1, product2, product3);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(3, db.Products.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
			}
		}


		[Test]
		public void OrderTests20_4()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				db.Commit(() =>
				{
					var product1 = NewProduct(1);
					var product2 = NewProduct(2);
					var product3 = NewProduct(3);

					NewOrderItem(1, order1, product: product1).Save(db);
					NewOrderItem(2, order1, product: product2).Save(db);
					NewOrderItem(3, order1, product: product3).Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1, product2, product3);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(3, db.Products.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
			}
		}


		[Test]
		public void OrderTests20_5()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var order2 = NewOrder(2);

				db.Commit(() =>
				{
					var product11 = NewProduct(1);
					var product12 = NewProduct(2);
					var product13 = NewProduct(3);

					var product21 = NewProduct(1);
					var product22 = NewProduct(2);
					var product23 = NewProduct(3);

					NewOrderItem(1, order1, product: product11).Save(db);
					NewOrderItem(2, order1, product: product12).Save(db);
					NewOrderItem(3, order1, product: product13).Save(db);

					NewOrderItem(1, order2, product: product21).Save(db);
					NewOrderItem(2, order2, product: product22).Save(db);
					NewOrderItem(3, order2, product: product23).Save(db);

					db.Calculated = () =>
					{
						TestEntryForOrder1(db.Entries[0], order1, product11, product12, product13);
						TestEntryForOrder1(db.Entries[1], order2, product21, product22, product23);
					};
				});

				Assert.AreEqual(2, db.Orders.Count);
				Assert.AreEqual(6, db.OrderItems.Count);
				Assert.AreEqual(6, db.Products.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
				Assert.AreEqual(60, order2.Total);
				Assert.AreEqual(12, order2.Vat);
			}
		}


		[Test]
		public void OrderTests20_6()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);

				db.Commit(() =>
				{
					db.Commit(() => NewOrderItem(1, order1, product: product1).Save(db));

					product1.Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);
			}
		}

		[Test]
		public void OrderTests20_7()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);
				var product2 = NewProduct(2);
				var product3 = NewProduct(3);

				db.Commit(() =>
				{
					product1.Save(db);
					product2.Save(db);

					NewOrderItem(1, order1, product: product1).Save(db);
					NewOrderItem(2, order1, product: product2).Save(db);

					db.Commit(() => NewOrderItem(3, order1, product: product3).Save(db));

					product3.Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1, product2, product3);
				});


				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(3, db.Products.Count);

				Assert.AreEqual(60, order1.Total);
				Assert.AreEqual(12, order1.Vat);
			}
		}


		[Test]
		public void OrderTests21()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);
				var product2 = NewProduct(2);
				var product3 = NewProduct(3);

				db.Commit(() =>
				{
					NewOrderItem(1, order1, product: product1).Save(db);
					NewOrderItem(2, order1, product: product2).Save(db);
					NewOrderItem(3, order1, product: product3).Save(db);
				});

				db.Commit(() =>
				{
					product3.FeeTotal = 70 * 0.8m;
					product3.ServiceFee = 70 * 0.2m;
					product3.Save(db);
					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product3);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(3, db.OrderItems.Count);
				Assert.AreEqual(3, db.Products.Count);

				Assert.AreEqual(100, order1.Total);
				Assert.AreEqual(20, order1.Vat);
			}
		}


		[Test]
		public void OrderTests21_2()
		{
			using (var db = new Domain())
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
					NewOrderItem(1, order1, product: product11).Save(db);
					NewOrderItem(2, order1, product: product12).Save(db);
					NewOrderItem(3, order1, product: product13).Save(db);

					NewOrderItem(1, order2, product: product21).Save(db);
					NewOrderItem(2, order2, product: product22).Save(db);
					NewOrderItem(3, order2, product: product23).Save(db);
				});

				db.Commit(() =>
				{
					product13.FeeTotal = 70 * 0.8m;
					product13.ServiceFee = 70 * 0.2m;
					product13.Save(db);

					product23.FeeTotal = 70 * 0.8m;
					product23.ServiceFee = 70 * 0.2m;
					product23.Save(db);

					db.Calculated = db.Calculated = () =>
					{
						TestEntryForOrder1(db.Entries[0], order1, product13);
						TestEntryForOrder1(db.Entries[1], order2, product23);
					};
				});

				Assert.AreEqual(2, db.Orders.Count);
				Assert.AreEqual(6, db.OrderItems.Count);
				Assert.AreEqual(6, db.Products.Count);

				Assert.AreEqual(100, order1.Total);
				Assert.AreEqual(20, order1.Vat);

				Assert.AreEqual(100, order2.Total);
				Assert.AreEqual(20, order2.Vat);
			}
		}


		[Test]
		public void OrderTests24()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1).Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);

				db.Commit(() =>
				{
					order1.Items[0].Delete(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, 1);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(0, db.OrderItems.Count);
				Assert.AreEqual(0, order1.Items.Count);

				Assert.AreEqual(0, order1.Total);
				Assert.AreEqual(0, order1.Vat);
			}
		}

		[Test]
		public void OrderTests25()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1, product: product1).Save(db);
					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);

				db.Commit(() =>
				{
					product1.Order = null;
					product1.Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(0, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);
				Assert.AreEqual(0, order1.Items.Count);

				Assert.AreEqual(0, order1.Total);
				Assert.AreEqual(0, order1.Vat);
			}
		}


		[Test]
		public void OrderTests27_1()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);

				db.Commit(() =>
				{
					NewOrderItem(1, order1, product: product1).Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);

				db.Commit(() =>
				{
					product1.Delete(db);

					db.Calculated = () => { var tree = db.Entries; };
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(0, db.OrderItems.Count);
				Assert.AreEqual(0, db.Products.Count);
				Assert.AreEqual(0, order1.Items.Count);

				Assert.AreEqual(0, order1.Total);
				Assert.AreEqual(0, order1.Vat);
			}
		}


		[Test]
		public void OrderTests27_2()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);
				var product2 = NewProduct(2);

				db.Commit(() =>
				{
					product1.Save(db);
					product2.Save(db);
					NewOrderItem(1, order1, product: product1).Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(2, db.Products.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);

				db.Commit(() =>
				{
					product1.Delete(db);
					NewOrderItem(2, order1, product: product2).Save(db);
					//					product2.Save(db);

					db.Calculated = () => { var tree = db.Entries; };
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(1, order1.Items.Count);
				Assert.AreSame(product2, order1.Items[0].Product);

				Assert.AreEqual(20, order1.Total);
				Assert.AreEqual(4, order1.Vat);
			}
		}


		[Test]
		public void OrderTests27_3()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);
				var product2 = NewProduct(2);

				db.Commit(() =>
				{
					product2.Save(db);
					product1.Save(db);
					NewOrderItem(1, order1, product: product1).Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(2, db.Products.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);

				db.Commit(() =>
				{
					NewOrderItem(2, order1, product: product2).Save(db);
					product1.Delete(db);

					db.Calculated = () => { var tree = db.Entries; };
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(1, order1.Items.Count);
				Assert.AreSame(product2, order1.Items[0].Product);

				Assert.AreEqual(20, order1.Total);
				Assert.AreEqual(4, order1.Vat);
			}
		}


		[Test]
		public void OrderTests27_4()
		{
			using (var db = new Domain())
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
					NewOrderItem(1, order1, product: product1).Save(db);
					product2.Save(db);
					NewOrderItem(2, order1, product: product2).Save(db);
					product1.Save(db);
					product3.Save(db);
					NewOrderItem(3, order1, product: product3).Save(db);
				});

				db.Commit(() =>
				{
					product3.Delete(db);
					product1.Delete(db);

					db.Calculated = () => { var tree = db.Entries; };
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(1, order1.Items.Count);
				Assert.AreSame(product2, order1.Items[0].Product);

				Assert.AreEqual(20, order1.Total);
				Assert.AreEqual(4, order1.Vat);
			}
		}


		[Test]
		public void OrderTests28_1()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);

				db.Commit(() =>
				{
					product1.Order = order1;
					product1.Save(db);

					db.Calculated = () => TestEntryForOrder1(db.Entries, order1, product1);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(1, order1.Items.Count);
				Assert.AreSame(product1, order1.Items[0].Product);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);
			}
		}


		[Test]
		public void OrderTests28_2()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);

				db.Commit(() =>
				{
					order1.SeparateServiceFee = true;
					product1.Order = order1;
					product1.Save(db);

					db.Calculated = () => { var entries = db.Entries; };
				});
				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(2, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(2, order1.Items.Count);
				Assert.AreSame(product1, order1.Items[0].Product);
				Assert.AreSame(product1, order1.Items[1].Product);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);
			}
		}


		[Test]
		public void OrderTests29()
		{
			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);
				var product2 = NewProduct(2);
				var product3 = NewProduct(3);

				db.Commit(() =>
				{
					NewOrderItem(1, order1, product: product1).Save(db);
					NewOrderItem(2, order1, product: product2).Save(db);
					NewOrderItem(3, order1, product: product3).Save(db);
				});

				db.Commit(() =>
				{
					order1.Items.Delete(db);

					db.Calculated = () => { var tree = db.Entries; };
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(0, db.OrderItems.Count);
				Assert.AreEqual(3, db.Products.Count);

				Assert.AreEqual(0, order1.Items.Count);

				Assert.AreEqual(null, product1.Order);
				Assert.AreEqual(null, product2.Order);
				Assert.AreEqual(null, product3.Order);

				Assert.AreEqual(0, order1.Total);
				Assert.AreEqual(0, order1.Vat);
			}
		}


		[Test]
		public void OrderTests30()
		{

			using (var db = new Domain())
			{

				var order1 = NewOrder(1);
				var product1 = NewProduct(1);

				db.Commit(() =>
				{
					product1.Save(db);
					NewOrderItem(1, order1, product: product1).Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(1, db.OrderItems.Count);
				Assert.AreEqual(1, db.Products.Count);

				Assert.AreEqual(10, order1.Total);
				Assert.AreEqual(2, order1.Vat);

				db.Commit(() =>
				{
					order1._OnPreCalculate = r => db.Products[0].Delete(db);
					order1.Save(db);
				});

				Assert.AreEqual(1, db.Orders.Count);
				Assert.AreEqual(0, db.OrderItems.Count);
				Assert.AreEqual(0, db.Products.Count);
				Assert.AreEqual(0, order1.Items.Count);

				Assert.AreEqual(0, order1.Total);
				Assert.AreEqual(0, order1.Vat);
			}
		}


	}

}

