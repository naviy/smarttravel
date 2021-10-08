using System;
using System.IO;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Reports
{
	[TestFixture]
	public class InvoicePrinterTests
	{
		[SetUp]
		public void SetUp()
		{
			//_mockery = new Mockery();
		}

		[Test]
		public void TestInvoicePrinter1()
		{
			/*var company = new Organization
			{
				Name = @"ТОВ ""Аріола Групп""",
				LegalAddress = "04053 м.Київ, вул. Артема 42",
				Phone1 = "(044) 490-28-88",
				Fax = "(044) 490-28-27"
			};

			var invoice = new Order { Number = "I.10-00001", IssueDate = new DateTime(2010, 1, 1), Supplier = company };

			var acquirer = new Organization { Name = @"ТОВ ""Роги та копитця""" };

			invoice.ShipTo = acquirer;
			invoice.BillTo = acquirer;

			//invoice.Agreement = "KK 11234";
			invoice.DaysTillExpiration = 20;
			invoice.SignedBy = new Person { Name = "Соснина Юлия" };

			var uahCurrency = new Currency("UAH");

			invoice.GrandTotal = new Money(uahCurrency, 300);
			invoice.Vat = new Money(uahCurrency, 100);
			invoice.Discount = new Money(uahCurrency, 200);
			invoice.Status = OrderStatus.Opened;

			invoice.AddOrderItem(new OrderItem
			{
				Text = @"
Оплата за авиабилет № 050-3825161623
за маршрутом вфывфы-фывфывф-фывыфы
SHYSHKIN/ANDREYMR",
				Quantity = 1,
				Price = new Money(uahCurrency, 600),
				Discount = new Money(uahCurrency, 50),
				GrandTotal = new Money(uahCurrency, 800)
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"
Оплата за авиабилет № 050-3825166882
MOSKALENKO/YURIYMR",
				Quantity = 1,
				Price = new Money(uahCurrency, 250),
				Discount = new Money(uahCurrency, 15),
				GrandTotal = new Money(uahCurrency, 800)
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = "Invoice item 3",
				Quantity = 1,
				Price = new Money(uahCurrency, 600),
				Discount = new Money(uahCurrency, 20),
				GrandTotal = new Money(uahCurrency, 400)
			});

			var configurationSource = _mockery.NewMock<IConfigurationSource>();

			Stub.On(configurationSource).GetProperty("Configuration").Will(Return.Value(new SystemConfiguration
			{
				Company = company,
				CompanyDetails =
@"ЄДРПОУ 30972992
Р/р 26002380666221  в Київській міській філії АКБ ""Укрсоцбанк"" в м. Києві  МФО 322012
ІПН 309729926594, св-во № 100065709"
			}));

			using (var stream = new FileStream("~/invoice1.xls".ResolvePath(), FileMode.Create))
			{
				byte[] bytes = new InvoicePrinter { ConfigurationSource = configurationSource }
					.Build(invoice, false);

				stream.Write(bytes, 0, bytes.Length);
			}*/
		}

		[Test]
		public void TestInvoicePrinter2()
		{
			/*var company = new Organization
			{
				Name = @"ТОВ ""Аріола Групп""",
				LegalAddress = "04053 м.Київ, вул. Артема 42",
				Phone1 = "(044) 490-28-88",
				Fax = "(044) 490-28-27"
			};

			var invoice = new Invoice();
			invoice.Number = "I.10-00001";
			invoice.IssueDate = new DateTime(2010, 1, 1);
			invoice.Supplier = company;

			invoice.Acquirer = new Organization { Name = @"ТОВ ""Роги та копитця""" }; ;
			invoice.Payer = new Organization { Name = @"ТОВ ""Роги та копитця 2""" }; ;

			invoice.Agreement = "KK 11234";
			invoice.DaysTillExpiration = 20;
			invoice.SignedBy = new Person { Name = "Соснина Юлия" };

			var uahCurrency = new Currency("UAH");

			invoice.Total = new Money(uahCurrency, 2000);
			invoice.Vat = new Money(uahCurrency, 100);
			invoice.Discount = new Money(uahCurrency, 200);
			invoice.IsAviaCompanyAndAgencyDataSplitted = true;
			invoice.Status = InvoiceStatus.Draft;

			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = "Invoice item 1",
				Quantity = 1,
				ServiceFee = new Money(uahCurrency, 200),
				Discount = new Money(uahCurrency, 50),
				Price = new Money(uahCurrency, 600)
			});
			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = "Invoice item 2",
				Quantity = 1,
				ServiceFee = new Money(uahCurrency, 60),
				Price = new Money(uahCurrency, 250),
				Vat = new Money(uahCurrency, 10),
				Discount = new Money(uahCurrency, 15)
			});
			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = "Invoice item 3",
				Quantity = 1,
				ServiceFee = new Money(uahCurrency, 120),
				Price = new Money(uahCurrency, 600),
				Vat = new Money(uahCurrency, 20)
			});

			var configurationSource = _mockery.NewMock<IConfigurationSource>();
			
			Stub.On(configurationSource).GetProperty("Configuration").Will(Return.Value(new SystemConfiguration
			{
				Company = company,
				CompanyDetails =
@"ЄДРПОУ 30972992
Р/р 26002380666221  в Київській міській філії АКБ ""Укрсоцбанк"" в м. Києві  МФО 322012
ІПН 309729926594, св-во № 100065709"
			}));

			using (var stream = new FileStream("~/invoice2.xls".ResolvePath(), FileMode.Create))
			{
				byte[] bytes = new InvoicePrinter { ConfigurationSource = configurationSource }
					.Build(invoice);

				stream.Write(bytes, 0, bytes.Length);
			}*/
		}

		[Test]
		public void TestReceiptPrinter1()
		{
			/*var company = new Organization
			{
				Name = @"ПрАТ ""Універсальне агентство з продажу авіаперевезень""",
				LegalAddress = "Київ 01034, вул. Золотоворітська, 13",
				Phone1 = "+38 044 2067570",
				Fax = "+38 044 2067578"
			};

			var invoice = new Order
			{
				Number = "C.10-00001",
				IssueDate = new DateTime(2010, 1, 1),
				Supplier = company
			};

			var acquirer = new Organization { Name = @"ТОВ ""Роги та копитця""" };

			invoice.ShipTo = acquirer;
			invoice.BillTo = acquirer;

			//invoice.Agreement = "KK 11234";
			invoice.DaysTillExpiration = 20;
			invoice.SignedBy = new Person { Name = "Соснина Юлия" };

			var uahCurrency = new Currency("UAH");

			invoice.GrandTotal = new Money(uahCurrency, 588489.6m);
			invoice.Vat = new Money(uahCurrency, 216.67m);
			invoice.Discount = new Money(uahCurrency, 10);
			invoice.Status = OrderStatus.Opened;

			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592442
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData
				}
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300)
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592442
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData
				}
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300)
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592442
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData
				}
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300)
			});

			invoice.AddPayment(new Payment { ReceivedFrom = "test person" });

			var configurationSource = _mockery.NewMock<IConfigurationSource>();

			Stub.On(configurationSource).GetProperty("Configuration").Will(Return.Value(new SystemConfiguration
			{
				Company = company,
				CompanyDetails =
@"Р/р 26002060408559
в Печерській філії Приватбанку м. Київ
МФО 300711, Код 25264192
252641926599, св. № 100255608"
			}));

			using (var stream = new FileStream("~/receipt1.xls".ResolvePath(), FileMode.Create))
			{
				byte[] bytes = new ReceiptPrinter { ConfigurationSource = configurationSource }
					.Build(invoice, false);

				stream.Write(bytes, 0, bytes.Length);
			}*/
		}

		[Test]
		public void TestCashOrderPrinter1()
		{
			/*var company = new Organization
			{
				Name = @"ТОВ ""Аріола груп""",
				LegalAddress = "Артема 42",
				Code = "30972992",
				Phone1 = "+38 044 2067570",
				Fax = "+38 044 2067578"
			};

			var aviaTicket1 = new AviaTicket
			{
				AirlinePrefixCode = 220,
				Number = 9755592442
			};

			var aviaTicket2 = new AviaTicket
			{
				AirlinePrefixCode = 220,
				Number = 9755592443
			};

			var aviaTicket3 = new AviaTicket
			{
				AirlinePrefixCode = 220,
				Number = 9755592444
			};

			var invoice = new Order
			{
				Number = "C.10-00001",
				IssueDate = new DateTime(2010, 1, 1),
				Supplier = company,
				Owner = company
			};

			var acquirer = new Organization { Name = @"ТОВ ""Роги та копитця""" };

			invoice.ShipTo = acquirer;
			invoice.BillTo = acquirer;

			//invoice.Agreement = "KK 11234";
			invoice.DaysTillExpiration = 20;
			invoice.SignedBy = new Person { Name = "Соснина Юлия" };

			var uahCurrency = new Currency("UAH");

			invoice.GrandTotal = new Money(uahCurrency, 588489.6m);
			invoice.Vat = new Money(uahCurrency, 216.67m);
			invoice.Discount = new Money(uahCurrency, 10);
			invoice.Status = OrderStatus.Opened;

			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592442
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData,
					Document = aviaTicket1
				}
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData,
					Document = aviaTicket1
				}
			});

			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592443
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData,
					Document = aviaTicket2
				}
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData,
					Document = aviaTicket2
				}
			});
			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592444
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new OrderItemAviaLink
				{
					LinkType = OrderItemAviaLinkType.AirlineData,
					Document = aviaTicket3
				}
			});

			invoice.AddOrderItem(new OrderItem
			{
				Text = @"Додаткові послуги",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300)
			});

			invoice.AddPayment(new Payment { ReceivedFrom = "test person" });

			var configurationSource = _mockery.NewMock<IConfigurationSource>();

			Stub.On(configurationSource).GetProperty("Configuration").Will(Return.Value(new SystemConfiguration
			{
				Company = company,
				CompanyDetails =
@"Р/р 26002060408559
в Печерській філії Приватбанку м. Київ
МФО 300711, Код 25264192
252641926599, св. № 100255608"
			}));

			using (var stream = new FileStream("~/cashOrder1.xls".ResolvePath(), FileMode.Create))
			{
				byte[] bytes = new CashOrderPrinter
				{
					ConfigurationSource = configurationSource
				} // { ConfigurationSource = configurationSource }
					.Build(invoice, false);

				stream.Write(bytes, 0, bytes.Length);
			}*/
		}


//		[Test]
//		public void TestConsignmentPrinter1()
//		{
//			var aviaTicket1 = new AviaTicket
//			{
//				AirlinePrefixCode = "220",
//				Number = "9755592442"
//			};

//			var aviaTicket2 = new AviaTicket
//			{
//				AirlinePrefixCode = "220",
//				Number = "9755592443"
//			};
			
//			var consignment = new Consignment
//			{
//				Number = "2014",
//				IssueDate = new DateTime(2010, 11, 19),
//				Supplier = new Organization { Name = @"ТОВ ""Аріола груп""" },
//				TotalSupplied = "Два авіаквитка"
//			};

//			var acquirer = new Organization { Name = @"ТОВ ""Глобус Ессет Менеджмент""" };

//			consignment.Acquirer = acquirer;

//			var uahCurrency = new Currency("UAH");

//			consignment.GrandTotal = new Money(uahCurrency, 5889.6m);
//			consignment.Vat = new Money(uahCurrency, 216.67m);
//			consignment.Discount = new Money(uahCurrency, 16.67m);

//			var item = new OrderItem
//			{
//				Text = @"Оплата за авіаквиток № 220-9755592442
//sadsad
//asdasd
//KOMISARENKO/VOLODYMYR MR",
//				Quantity = 1,
//				Price = new Money(uahCurrency, 14903.6m),
//				Discount = new Money(uahCurrency, 0),
//				GrandTotal = new Money(uahCurrency, 14903.6m),
//			};
//			item.LinkType = OrderItemLinkType.ProductData;
//			item.Product = aviaTicket1;

//			consignment.AddOrderItem(item);

//			item = new OrderItem
//			{
//				Text = @"Сервісний збір",
//				Quantity = 1,
//				Price = new Money(uahCurrency, 1300),
//				Discount = new Money(uahCurrency, 0),
//				GrandTotal = new Money(uahCurrency, 1300),
//			};

//			item.LinkType = OrderItemLinkType.ServiceFee;
//			item.Product = aviaTicket1;

//			consignment.AddOrderItem(item);

//			item = new OrderItem
//			{
//				Text = @"Оплата за авіаквиток № 220-9755592443
//sadsad
//asdasd
//KOMISARENKO/VOLODYMYR MR",
//				Quantity = 1,
//				Price = new Money(uahCurrency, 14903.6m),
//				Discount = new Money(uahCurrency, 0),
//				GrandTotal = new Money(uahCurrency, 14903.6m)
//			};

//			item.LinkType = OrderItemLinkType.ProductData;
//			item.Product = aviaTicket2;

//			consignment.AddOrderItem(item);

//			item = new OrderItem
//			{
//				Text = @"Сервісний збір",
//				Quantity = 1,
//				Price = new Money(uahCurrency, 1300),
//				Discount = new Money(uahCurrency, 0),
//				GrandTotal = new Money(uahCurrency, 1300),
//			};

//			item.LinkType = OrderItemLinkType.ServiceFee;
//			item.Product = aviaTicket2;

//			consignment.AddOrderItem(item);

//			var invItem = new OrderItem
//			{
//				Text = @"Страховка",
//				Quantity = 1,
//				Price = new Money(uahCurrency, 120),
//				Discount = new Money(uahCurrency, 0),
//				GrandTotal = new Money(uahCurrency, 120)
//			};

//			for (var i = 0; i < 15; i++)
//				consignment.AddOrderItem(invItem);

//			using (var stream = new FileStream(string.Format("~/consignment_{0}.xls".ResolvePath(), DateTime.Now.ToFileTime()), FileMode.Create))
//			{
//				var bytes = new ConsignmentForm().Build(consignment, 20);

//				stream.Write(bytes, 0, bytes.Length);
//			}
//		}

		//private Mockery _mockery;
	}
}