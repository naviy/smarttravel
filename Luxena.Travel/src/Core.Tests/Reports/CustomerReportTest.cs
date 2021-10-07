//using System;
//using System.Collections.Generic;
//using System.IO;

//using Luxena.Base.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Parsers;
//using Luxena.Travel.Reports;

//using NUnit.Framework;

//namespace Luxena.Travel.Tests.Reports
//{
//	[TestFixture]
//	public class CustomerReportTest
//	{
//		#region Setup/Teardown

//		[SetUp]
//		public void SetUp()
//		{
//			PdfUtility.SetFontsPath(@"..\..\..\Web\static\fonts");
//		}

//		#endregion

//		[Test]
//		public void TestCustomerReport1()
//		{
//			var docs1 = new List<Entity2>();

//			var defaultCurrency = new Currency("UAH");

//			docs1.AddRange(AirParser.Parse(Res.AirTicket1, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket2, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket3, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket4, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket8, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket14, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket15, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirRefund1, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirRefund2, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirMco1, defaultCurrency));

//			var docs2 = new List<Entity2>();

//			docs2.AddRange(AirParser.Parse(Res.AirTicket10, defaultCurrency));
//			docs2.AddRange(AirParser.Parse(Res.AirTicket11, defaultCurrency));
//			docs2.AddRange(AirParser.Parse(Res.AirTicket16, defaultCurrency));
//			docs2.AddRange(MirParser.Parse(Res.MirTicket1, defaultCurrency));
//			docs2.AddRange(MirParser.Parse(Res.MirRefund1, defaultCurrency));
//			docs2.AddRange(AirParser.Parse(Res.AirMco2, defaultCurrency));
//			docs2.AddRange(AirParser.Parse(Res.AirMco3, defaultCurrency));

//			var documents = new List<AviaDocument>();

//			var customer = new Organization
//			{
//				Name = Res.CustomerReport_CustomerName
//			};
//			var department = new Department
//			{
//				Name = Res.CustomerReport_CustomerDepartmentName
//			};

//			foreach (AviaDocument document in docs1)
//			{
//				document.GrandTotal =
//					new Money(document.Total == null || document.Total.Amount == 0 ? document.Fare : document.Total);
//				document.SetCustomer(customer);

//				documents.Add(document);
//			}

//			documents[0].GrandTotal = new Money("UAH", 568457.50m);

//			foreach (AviaDocument document in docs2)
//			{
//				document.GrandTotal =
//					new Money(document.Total == null || document.Total.Amount == 0 ? document.Fare : document.Total);
//				document.SetCustomer(department);

//				documents.Add(document);
//			}

//			var stream = new FileStream("~/customer_report_internal1.pdf".ResolvePath(), FileMode.Create);

//			new CustomerReport(
//				new CustomerReportParams
//				{
//					Customer = customer,
//					DateFrom = new DateTime(2009, 7, 1),
//					DateTo = new DateTime(2009, 7, 31),
//					Passenger = "IVANOV",
//					PaymentType = PaymentType.Invoice,
//					Airline = new Airline { Name = "AirFance" }
//				},
//				documents)
//				{
//					DefaultCurrency = new Currency("UAH"),
//					UseDefaultCurrencyForInput = true
//				}
//				.Build(stream);
//		}

//		[Test]
//		public void TestCustomerReport2()
//		{
//			var docs1 = new List<Entity2>();

//			var defaultCurrency = new Currency("UAH");

//			docs1.AddRange(AirParser.Parse(Res.AirTicket1, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket2, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket3, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket4, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket8, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket14, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket15, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirRefund1, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirRefund2, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirMco1, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket10, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket11, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirTicket16, defaultCurrency));
//			docs1.AddRange(MirParser.Parse(Res.MirTicket1, defaultCurrency));
//			docs1.AddRange(MirParser.Parse(Res.MirRefund1, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirMco2, defaultCurrency));
//			docs1.AddRange(AirParser.Parse(Res.AirMco3, defaultCurrency));

//			var documents = new List<AviaDocument>();

//			var customer = new Organization
//			{
//				Name = Res.CustomerReport_CustomerName
//			};

//			foreach (var doc in docs1)
//			{
//				var document = (AviaDocument) doc;

//				document.GrandTotal =
//					new Money(document.Total == null || document.Total.Amount == 0 ? document.Fare : document.Total);
//				document.SetCustomer(customer);

//				documents.Add(document);
//			}

//			documents[0].GrandTotal = new Money("UAH", 568457.50m);


//			var stream = new FileStream("~/customer_report_internal2.pdf".ResolvePath(), FileMode.Create);

//			new CustomerReport(
//				new CustomerReportParams
//				{
//					Customer = customer,
//					DateFrom = new DateTime(2009, 7, 1),
//					DateTo = new DateTime(2009, 7, 31)
//				},
//				documents)
//				{
//					DefaultCurrency = new Currency("UAH"),
//					UseDefaultCurrencyForInput = true
//				}
//				.Build(stream);
//		}
//	}
//}