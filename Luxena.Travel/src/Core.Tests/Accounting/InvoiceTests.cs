//using Luxena.Travel.Domain;
//using Luxena.Travel.Managers;

//using NMock2;

//using NUnit.Framework;


//namespace Luxena.Travel.Tests.Accounting
//{
//	[TestFixture]
//	public class InvoiceTests
//	{
//		[SetUp]
//		public void SetUp()
//		{
//			_mocks = new Mockery();

//			_ticket1 = new AviaTicket
//			{
//				Domestic = true,
//				Total = new Money("UAH", 1200),
//				Vat = new Money("UAH", 50),
//				ServiceFee = new Money("UAH", 200),
//				Discount = new Money("UAH", 10),
//				GrandTotal = new Money("UAH", 1390)
//			};

//			_ticket2 = new AviaTicket
//			{
//				Domestic = false,
//				Total = new Money("UAH", 1000),
//				ServiceFee = new Money("UAH", 200),
//				Discount = new Money("UAH", 10),
//				GrandTotal = new Money("UAH", 1190)
//			};

//			_ticket3 = new AviaTicket
//			{
//				Domestic = true,
//				Total = new Money("UAH", 1200),
//				Vat = new Money("UAH", 50),
//				ServiceFee = new Money("UAH", 0),
//				Handling = new Money("UAH", 0),
//				Discount = new Money("UAH", 10),
//				GrandTotal = new Money("UAH", 1190)
//			};

//			_ticket4 = new AviaTicket
//			{
//				Domestic = false,
//				Total = new Money("UAH", 1000),
//				ServiceFee = new Money("UAH", 200),
//				Handling = new Money("UAH", 300),
//				Discount = new Money("UAH", 10),
//				GrandTotal = new Money("UAH", 1490)
//			};

//			var db = _mocks.NewMock<Domain.Domain>();

//			Stub.On(db).GetProperty("Configuration").Will(Return.Value(
//				new SystemConfiguration
//				{
//					VatRate = 20,
//					AviaOrderItemGenerationOption = AviaOrderItemGenerationOption.SeparateServiceFee
//				}));

//			_manager = new OrderManager { db = db };
//		}

//		[Test]
//		public void TestCreateInvoiceItem1()
//		{
//			_manager.db.Configuration.UseAviaDocumentVatInOrder = false;
//			_manager.db.Configuration.UseAviaHandling = false;

//			var items = _manager.CreateOrderItems(_ticket1, true);

//			Assert.AreEqual(2, items.Count);

//			var item = items[0];

//			Assert.AreEqual(new Money("UAH", 1200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 1200), item.Total);
//			Assert.AreEqual(new Money("UAH", 0), item.Discount);
//			Assert.AreEqual(new Money("UAH", 1200), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 0), item.TaxedTotal);
//			Assert.AreEqual(false, item.HasVat);

//			item = items[1];

//			Assert.AreEqual(new Money("UAH", 200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 200), item.Total);
//			Assert.AreEqual(new Money("UAH", 10), item.Discount);
//			Assert.AreEqual(new Money("UAH", 190), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 200), item.TaxedTotal);
//			Assert.AreEqual(true, item.HasVat);
//		}

//		[Test]
//		public void TestCreateInvoiceItem2()
//		{
//			_manager.db.Configuration.UseAviaDocumentVatInOrder = false;
//			_manager.db.Configuration.UseAviaHandling = false;

//			var items = _manager.CreateOrderItems(_ticket1, true);

//			Assert.AreEqual(2, items.Count);

//			var item = items[0];

//			Assert.AreEqual(new Money("UAH", 1200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 1200), item.Total);
//			Assert.AreEqual(new Money("UAH", 0), item.Discount);
//			Assert.AreEqual(new Money("UAH", 1200), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 0), item.TaxedTotal);
//			Assert.IsFalse(item.HasVat);
//		}

//		[Test]
//		public void TestCreateInvoiceItem3()
//		{
//			_manager.db.Configuration.UseAviaDocumentVatInOrder = false;
//			_manager.db.Configuration.UseAviaHandling = false;

//			var items = _manager.CreateOrderItems(_ticket1, true);

//			Assert.AreEqual(2, items.Count);

//			var item = items[0];

//			Assert.AreEqual(new Money("UAH", 1200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 1200), item.Total);
//			Assert.AreEqual(new Money("UAH", 0), item.Discount);
//			Assert.AreEqual(new Money("UAH", 1200), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 0), item.TaxedTotal);
//			Assert.IsFalse(item.HasVat);
//		}

//		[Test]
//		public void TestCreateInvoiceItem4()
//		{
//			_manager.db.Configuration.UseAviaDocumentVatInOrder = false;
//			_manager.db.Configuration.UseAviaHandling = false;

//			var items = _manager.CreateOrderItems(_ticket2, true);

//			Assert.AreEqual(2, items.Count);

//			var item = items[0];

//			Assert.AreEqual(new Money("UAH", 1000), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 1000), item.Total);
//			Assert.AreEqual(new Money("UAH", 0), item.Discount);
//			Assert.AreEqual(new Money("UAH", 1000), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 0), item.TaxedTotal);
//			Assert.IsFalse(item.HasVat);
//		}

//		[Test]
//		public void TestCreateInvoiceItem5()
//		{
//			_manager.db.Configuration.UseAviaDocumentVatInOrder = false;
//			_manager.db.Configuration.UseAviaHandling = true;

//			var items = _manager.CreateOrderItems(_ticket3, true);

//			Assert.AreEqual(1, items.Count);

//			var item = items[0];

//			Assert.AreEqual(new Money("UAH", 1200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 1200), item.Total);
//			Assert.AreEqual(new Money("UAH", 10), item.Discount);
//			Assert.AreEqual(new Money("UAH", 1190), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 0), item.TaxedTotal);
//			Assert.IsFalse(item.HasVat);
//		}

//		[Test]
//		public void TestCreateInvoiceItem6()
//		{
//			_manager.db.Configuration.UseAviaDocumentVatInOrder = false;
//			_manager.db.Configuration.UseAviaHandling = true;

//			var items = _manager.CreateOrderItems(_ticket4, true);

//			Assert.AreEqual(2, items.Count);

//			var item = items[0];

//			Assert.AreEqual(new Money("UAH", 1300), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 1300), item.Total);
//			Assert.AreEqual(new Money("UAH", 0), item.Discount);
//			Assert.AreEqual(new Money("UAH", 1300), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 0), item.TaxedTotal);
//			Assert.IsFalse(item.HasVat);

//			item = items[1];

//			Assert.AreEqual(new Money("UAH", 200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 200), item.Total);
//			Assert.AreEqual(new Money("UAH", 10), item.Discount);
//			Assert.AreEqual(new Money("UAH", 190), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 200), item.TaxedTotal);
//			Assert.AreEqual(true, item.HasVat);
//		}

//		[Test]
//		public void TestCreateInvoiceItem7()
//		{
//			_manager.db.Configuration.UseAviaDocumentVatInOrder = false;
//			_manager.db.Configuration.UseAviaHandling = true;

//			var items = _manager.CreateOrderItems(_ticket1, true);

//			Assert.AreEqual(2, items.Count);

//			var item = items[0];

//			Assert.AreEqual(new Money("UAH", 1200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 1200), item.Total);
//			Assert.AreEqual(new Money("UAH", 0), item.Discount);
//			Assert.AreEqual(new Money("UAH", 1200), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 0), item.TaxedTotal);
//			Assert.IsFalse(item.HasVat);

//			item = items[1];

//			Assert.AreEqual(new Money("UAH", 200), item.Price);
//			Assert.AreEqual(1, item.Quantity);
//			Assert.AreEqual(new Money("UAH", 200), item.Total);
//			Assert.AreEqual(new Money("UAH", 10), item.Discount);
//			Assert.AreEqual(new Money("UAH", 190), item.GrandTotal);
//			Assert.AreEqual(new Money("UAH", 0), item.GivenVat);
//			Assert.AreEqual(new Money("UAH", 200), item.TaxedTotal);
//			Assert.AreEqual(true, item.HasVat);
//		}

//		private Mockery _mocks;
//		private Order.Service _manager;

//		private AviaTicket _ticket1;
//		private AviaTicket _ticket2;
//		private AviaTicket _ticket3;
//		private AviaTicket _ticket4;
//	}
//}