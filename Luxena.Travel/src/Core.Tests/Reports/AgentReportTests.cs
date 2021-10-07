//using System;
//using System.Collections.Generic;
//using System.IO;

//using Luxena.Base.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Domain;
//using Luxena.Travel.Parsers;
//using Luxena.Travel.Reports;

//using NUnit.Framework;


//namespace Luxena.Travel.Tests.Reports
//{
//	[TestFixture]
//	public class AgentReportTests
//	{
//		[SetUp]
//		public void SetUp()
//		{
//			PdfUtility.SetFontsPath(@"..\..\..\Web\static\fonts");
//		}

//		[Test]
//		public void TestAgentReport1()
//		{
//			var documents = new List<AviaDocument>();

//			var defaultCurrency = new Currency("UAH");

//			var docs = new List<Entity2>();
//			docs.AddRange(AirParser.Parse(Res.AirTicket1, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirTicket2, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirTicket3, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirTicket4, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirTicket8, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirTicket14, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirTicket15, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirRefund1, defaultCurrency));
//			docs.AddRange(AirParser.Parse(Res.AirRefund2, defaultCurrency));

//			((AviaDocument) docs[0]).EqualFare.Amount = 123456.89m;
//			((AviaDocument) docs[0]).FeesTotal.Amount = 12345.89m;
//			((AviaDocument) docs[2]).IsVoid = true;

//			var owner = new Organization
//			{
//				Name = "Document owner department"
//			};

//			foreach (AviaDocument document in docs)
//			{
//				document.GrandTotal = new Money(document.Total == null || document.Total.Amount == 0 ? document.Fare : document.Total);
//				document.Owner = owner;

//				documents.Add(document);
//			}

//			documents[0].Owner = owner;

//			var order = new Order
//			{
//				Number = "I.10-55555"
//			};

//			order.SetCustomer(new Organization
//			{
//				Name = "Khodchenkova (Yaglych) Svetlana"
//			});

//			order.SetTotal(new Money("USD", 99999));

//			order.AddOrderItem(
//				new OrderItem
//				{
//					Text = @"Оплата за авіаквиток № 566-1632862805
//DUBYTSYKYY/DMYTRO MR"
//				});

//			order.AddOrderItem(
//				new OrderItem
//				{
//					Text = "Сервісний збір"
//				});

//			var payment1 = new CheckPayment
//			{
//				Number = "P.10-00001",
//				//Amount = new Money("UAH", 555555),
//				//Payer = new Organization { Name = "Khodchenkova (Yaglych) Svetlana" },
//				ReceivedFrom = "DUBYTSYKYY/DMYTRO MR"
//			};

//			var payment2 = new CheckPayment
//			{
//				Number = "P.10-00002",
//				//Amount = new Money("UAH", 555555),
//				//Payer = new Organization { Name = "Khodchenkova (Yaglych) Svetlana" },
//				ReceivedFrom = "DUBYTSYKYY/DMYTRO MR"
//			};

//			var payment3 = new CheckPayment
//			{
//				Number = "P.10-00003",
//				//Amount = new Money("UAH", 10000),
//				//Payer = new Organization { Name = "Khodchenkova (Yaglych) Svetlana" },
//				ReceivedFrom = "DUBYTSYKYY/DMYTRO MR"
//			};

//			var payment4 = new CheckPayment
//			{
//				Number = "P.10-00003",
//				//Amount = new Money("USD", 99999),
//				//Payer = new Organization
//				//{
//				//	Name = "Khodchenkova (Yaglych) Svetlana"
//				//},
//				ReceivedFrom = "DUBYTSYKYY/DMYTRO MR"
//			};

//			order.AddPayment(payment1);
//			order.AddPayment(payment2);

//			var stream = new FileStream("~/agent_report.pdf".ResolvePath(), FileMode.Create);

//			new AgentReport
//			{
//				DefaultCurrency = new Currency("UAH"),
//				UseDefaultCurrencyForInput = true,
//				CompanyName = Res.AgentReport_CompanyName,
//				AgentName = Res.AgentReport_AgentName,
//				Date = new DateTime(2009, 1, 1),
//				Documents = documents,
//				ShowAviaHandling = true,
//				Payments = new List<Payment> { payment1, payment2, payment3, payment4 }
//			}
//				.Build(stream);
//		}

//		[Test]
//		public void TestAgentReport2()
//		{
//			var stream = new FileStream("~/agent_report1.pdf".ResolvePath(), FileMode.Create);

//			new AgentReport
//			{
//				CompanyName = Res.AgentReport_CompanyName,
//				AgentName = Res.AgentReport_AgentName,
//				Date = new DateTime(2009, 1, 1),
//				Documents = null
//			}
//			.Build(stream);
//		}
//	}
//}