using System;
using System.Collections.Generic;
using System.IO;

using Luxena.Base.Domain;
using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;
using Luxena.Travel.Reports;

using NUnit.Framework;

namespace Luxena.Travel.Tests.Reports
{
	[TestFixture]
	public class RegistryReportTest
	{
		[SetUp]
		public void SetUp()
		{
			PdfUtility.SetFontsPath(@"..\..\..\Web\static\fonts");
		}

		[Test]
		public void TestRegistryReport1()
		{
			var documents = new List<AviaDocument>();

			var defaultCurrency = new Currency("UAH");

			var docs = new List<Entity2>();
			docs.AddRange(AirParser.Parse(Res.AirTicket1, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket2, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket3, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket4, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket8, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket14, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket15, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirRefund1, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirRefund2, defaultCurrency));

			foreach (var doc in docs)
			{
				var document = (AviaDocument)doc;

				document.ServiceFee = new Money(new Currency("UAH"), 100);
				document.GrandTotal = new Money(document.Total == null || document.Total.Amount == 0 ? document.Fare : document.Total);

				documents.Add(document);
			}

			documents[0].EqualFare = new Money(new Currency("UAH"), 456852.50m);

			var pdfStream = new FileStream("~/registry_report1.pdf".ResolvePath(), FileMode.Create);
			var xlsStream = new FileStream("~/registry_report1.xls".ResolvePath(), FileMode.Create);

			var report = new RegistryReport
			{
				DateFrom = new DateTime(2009, 10, 01),
				DateTo = new DateTime(2009, 10, 20),
				Products = documents,
				DefaultCurrency = new Currency("UAH"),
				UseDefaultCurrencyForInput = false,
				PaymentType = PaymentType.Invoice
			};

			report.Build(pdfStream, ReportType.Pdf);
			report.Build(xlsStream, ReportType.Excel);
		}

		[Test]
		public void TestRegistryReport2()
		{
			var documents = new List<AviaDocument>();

			var defaultCurrency = new Currency("UAH");

			var docs = new List<Entity2>();
			docs.AddRange(AirParser.Parse(Res.AirTicket1, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket2, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket3, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket4, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket8, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket14, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket15, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirRefund1, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirRefund2, defaultCurrency));

			foreach (AviaDocument document in docs)
			{
				document.ServiceFee = new Money(new Currency("UAH"), 100);
				document.GrandTotal = new Money(document.Total == null || document.Total.Amount == 0 ? document.Fare : document.Total);

				documents.Add(document);
			}

			documents[0].EqualFare = new Money(new Currency("UAH"), 456852.50m);

			var pdfStream = new FileStream("~/registry_report2.pdf".ResolvePath(), FileMode.Create);
			var xlsStream = new FileStream("~/registry_report2.xls".ResolvePath(), FileMode.Create);

			var report = new RegistryReport
			{
				DateFrom = new DateTime(2009, 10, 01),
				DateTo = new DateTime(2009, 10, 20),
				Products = documents,
				DefaultCurrency = new Currency("UAH"),
				UseDefaultCurrencyForInput = false
			};

			report.Build(pdfStream, ReportType.Pdf);
			report.Build(xlsStream, ReportType.Excel);
		}
	}
}