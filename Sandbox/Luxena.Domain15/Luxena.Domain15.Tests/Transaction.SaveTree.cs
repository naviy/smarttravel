using System;


using NUnit.Framework;
// ReSharper disable InconsistentNaming
#pragma warning disable 169

namespace Luxena.Domain15.Tests
{

	using Travel.Domain;


	[TestFixture]
	public class TransactionSaveTreeTests
	{

		public class Tester
		{
			public Order order1 = new Order { Number = "O.15-00001" };

			public OrderItem orderItem11 = new OrderItem { Text = "#1" };
			public OrderItem orderItem12 = new OrderItem { Text = "#2" };
			public OrderItem orderItem13 = new OrderItem { Text = "#3" };

			public Order order2 = new Order { Number = "O.15-00002" };

			public OrderItem orderItem21 = new OrderItem { Text = "#1" };
			public OrderItem orderItem22 = new OrderItem { Text = "#2" };
			public OrderItem orderItem23 = new OrderItem { Text = "#3" };


			public void Exec(Func<Domain, Action<Domain.Transaction>> save)
			{
				using (var db = new Domain())
				using (var tran = db.BeginWork())
				{
					var testing = save(db);

					tran.Calculated = () => testing(tran);
				}
			}


			public void AddItemsToOrders1()
			{
				var t = this;
				t.orderItem11.Order = t.order1;
				t.orderItem12.Order = t.order1;
				t.orderItem13.Order = t.order1;

				t.orderItem21.Order = t.order2;
				t.orderItem22.Order = t.order2;
				t.orderItem23.Order = t.order2;
			}


			public void TestOrders1(Domain.Transaction tran)
			{
				Assert.AreEqual(2, tran.Entries.Count);

				TestOrder(tran.Entries[0], order1, true, new[] { orderItem11, orderItem12, orderItem13 });
				TestOrder(tran.Entries[1], order2, true, new[] { orderItem21, orderItem22, orderItem23 });
			}

			public void TestOrder(Domain.Transaction.Entry orderNode, Order order, bool orderActive, OrderItem[] orderItems)
			{
				Assert.AreSame(order, orderNode.Entity);
				Assert.AreEqual(orderActive, orderNode.Active);

				Assert.AreEqual(orderItems.Length, orderNode.Items.Count);
				Assert.AreEqual(orderItems.Length, order.Items.Count);

				var i = 0;
				foreach (var orderItem in orderItems)
				{
					Assert.AreSame(orderItem, order.Items[i]);

					Assert.AreSame(orderItem, orderNode.Items[i].Entity);
					Assert.AreEqual(true, orderNode.Items[i].Active);
					i++;
				}

			}
		}


		[Test]
		public void Test01()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.order1.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.order2.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				return t.TestOrders1;
			});

		}


		[Test]
		public void Test02()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.order1.Save(db);
				t.order2.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				return t.TestOrders1;
			});

		}


		[Test]
		public void Test03()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				t.order1.Save(db);
				t.order2.Save(db);

				return t.TestOrders1;
			});

		}


		[Test]
		public void Test04()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.order1.Save(db);
				t.order2.Save(db);

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				return t.TestOrders1;
			});

		}


		[Test]
		public void Test05()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.order1.Save(db);
				t.order1.Save(db);
				t.order2.Save(db);
				t.order2.Save(db);

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				return t.TestOrders1;
			});

		}


		[Test]
		public void Test06()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.order1.Save(db);
				t.order2.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				t.order1.Save(db);
				t.order2.Save(db);

				return t.TestOrders1;
			});

		}


		[Test]
		public void Test07()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.order1.Save(db);
				t.order2.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.order1.Save(db);
				t.order2.Save(db);

				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				return t.TestOrders1;
			});

		}

		[Test]
		public void Test08()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				return t.TestOrders1;
			});
		}

		[Test]
		public void Test09()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.AddItemsToOrders1();

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				return t.TestOrders1;
			});
		}



		[Test]
		public void Test17()
		{
			var t = new Tester();

			t.Exec(db =>
			{
				t.orderItem11.Order = t.order1;
				t.orderItem22.Order = t.order1;
				t.orderItem23.Order = t.order1;
				t.orderItem23.Order = null;

				t.orderItem21.Order = t.order2;
				t.orderItem22.Order = t.order2;
				t.orderItem23.Order = t.order2;

				t.orderItem12.Order = t.order1;
				t.orderItem13.Order = t.order1;

				t.orderItem11.Save(db);
				t.orderItem12.Save(db);
				t.orderItem13.Save(db);

				t.order1.Save(db);
				t.order2.Save(db);

				t.orderItem21.Save(db);
				t.orderItem22.Save(db);
				t.orderItem23.Save(db);

				t.orderItem12.Save(db);

				t.order1.Save(db);
				t.order2.Save(db);

				t.orderItem13.Save(db);
				t.orderItem23.Save(db);
				t.orderItem22.Save(db);

				return t.TestOrders1;
			});

		}


	}



}

