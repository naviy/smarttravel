using NUnit.Framework;

namespace Luxena.Travel.Tests.Reports
{
	[TestFixture]
	public class ReceiptPrinterTests
	{
		/*[SetUp]
		public void SetUp()
		{
			_mockery = new Mockery();
		}

		[Test]
		public void TestInvoicePrinter1()
		{
			var company = new Organization
			{
				Name = @"ПрАТ ""Універсальне агентство з продажу авіаперевезень""",
				LegalAddress = "Київ 01034, вул. Золотоворітська, 13",
				Phone1 = "+38 044 2067570",
				Fax = "+38 044 2067578"
			};

			var invoice = new Invoice
			{
				Number = "C.10-00001",
				IssueDate = new DateTime(2010, 1, 1),
				Supplier = company
			};

			var acquirer = new Organization { Name = @"ТОВ ""Роги та копитця""" };

			invoice.Acquirer = acquirer;
			invoice.Payer = acquirer;

			//invoice.Agreement = "KK 11234";
			invoice.DaysTillExpiration = 20;
			invoice.SignedBy = new Person { Name = "Соснина Юлия" };

			var uahCurrency = new Currency("UAH");

			invoice.GrandTotal = new Money(uahCurrency, 588489.6m);
			invoice.Vat = new Money(uahCurrency, 216.67m);
			invoice.Discount = new Money(uahCurrency, 10);
			invoice.Status = InvoiceStatus.Draft;

			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592442
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new InvoiceItemAviaLink
				{
					LinkType = InvoiceItemAviaLinkType.AirlineData
				}
			});
			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300)
			});
			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592442
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new InvoiceItemAviaLink
				{
					LinkType = InvoiceItemAviaLinkType.AirlineData
				}
			});
			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300)
			});
			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = @"Оплата за авіаквиток № 220-9755592442
sadsad
asdasd
KOMISARENKO/VOLODYMYR MR",
				Quantity = 1,
				Price = new Money(uahCurrency, 14903.6m),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 14903.6m),
				Source = new InvoiceItemAviaLink
				{
					LinkType = InvoiceItemAviaLinkType.AirlineData
				}
			});
			invoice.AddInvoiceItem(new InvoiceItem
			{
				Text = @"Сервісний збір",
				Quantity = 1,
				Price = new Money(uahCurrency, 1300),
				Discount = new Money(uahCurrency, 0),
				GrandTotal = new Money(uahCurrency, 1300)
			});

			invoice.AddPaymentLink(new PaymentInvoiceLink
				{
					Invoice = invoice,
					Payment = new Payment { ReceivedFrom = "test person" }
				});

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

			using (var stream = new FileStream("~/invoice1.xls", FileMode.Create))
			{
				byte[] bytes = new ReceiptPrinter { ConfigurationSource = configurationSource }
					.Build(invoice);

				stream.Write(bytes, 0, bytes.Length);
			}
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

			using (var stream = new FileStream("~/invoice2.xls", FileMode.Create))
			{
				byte[] bytes = new InvoicePrinter { ConfigurationSource = configurationSource }
					.Build(invoice);

				stream.Write(bytes, 0, bytes.Length);
			}
		}

		private Mockery _mockery;*/
	}
}