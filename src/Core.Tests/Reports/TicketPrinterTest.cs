using NUnit.Framework;


namespace Luxena.Travel.Tests.Reports
{
	[TestFixture]
	public class TicketPrinterTest
	{
		/*[SetUp]
		public void SetUp()
		{
			PdfUtility.Init(@"..\..\..\Web\fonts");

			_mockery = new Mockery();
		}

		private static List<AviaTicket> GetTickets()
		{
			var docs = new List<Entity>();

			docs.AddRange(AirParser.Parse(Res.AirTicket1));

			var tickets = new List<AviaTicket>();

			foreach (Entity doc in docs)
			{
				var ticket = (AviaTicket)doc;

				ticket.Segments[0].DepartureTime = new DateTime(2009, 11, 10, 13, 10, 0);
				ticket.Segments[0].CheckInTime = "10:25";
				ticket.Segments[0].FromAirport.Country = new Country{Name = "Ukraine"};
				ticket.Segments[0].FromAirport.Settlement = "Kiev";
				ticket.Segments[0].FromAirport.Name = "STOCKHOLM ARLANDA gfhdfg gfg";

				ticket.Segments[0].ServiceClass = ServiceClass.PremiumEconomy;
				ticket.Segments[0].MealTypes = MealType.Lunch | MealType.Dinner;
				ticket.Segments[0].Seat = "10A";


				ticket.ServiceFee = new Money(new Currency { Code = "UAH"}, 100);
				ticket.GrandTotal = new Money(new Currency { Code = "UAH" }, 123456.22m);

				tickets.Add(ticket);
			}
			return tickets;
		}

		[Test]
		public void TestDefaultTicketPrinter1()
		{
			List<AviaTicket> tickets = GetTickets();

			IConfigurationSource configurationSource = _mockery.NewMock<IConfigurationSource>();
			Stub.On(configurationSource).GetProperty("Configuration").Will(Return.Value(
				new SystemConfiguration
				{
					Company = new Organization { Name = "Фабрика туризму", ActualAddress = "Одеса, вул. Базарна, 45, кв.30", Phone1 = "+38 0482 32-03-20" }
				}));

			using (var stream = new FileStream("~/default_ticket1.pdf".ResolvePath(), FileMode.Create))
			{
				var printer = new DefaultTicketPrinter { ConfigurationSource = configurationSource };

				printer.Build(stream, tickets);
			}
		}

		/*[Test]
		public void TestYanaTicketPrint1()
		{
			var docs = GetTickets();

			foreach (AviaTicket doc in docs)
				doc.GrandTotal = new Money(doc.Total == null || doc.Total.Amount == 0 ? doc.Fare : doc.Total);

			using (var stream = new FileStream("~/ticket1.pdf".ResolvePath(), FileMode.Create))
			{
				var printer = new YanaTicketPrinter()
				{
					CompanyInfo = "YANA, 42 SAKSAGANSKOGO STR, KYIV 01033, UKRAINE",
					IataNumber = "723 20135",
					MainDataTemplatePath = @"..\..\..\Web\templates\ticket_design\header.jpg",
					SegmentHeaderTemplatePath = @"..\..\..\Web\templates\ticket_design\segment_header.jpg ",
					SegmentDataTemplatePath = @"..\..\..\Web\templates\ticket_design\segment.jpg ",
					FinanceDataTemplatePath = @"..\..\..\Web\templates\ticket_design\footer.jpg "
				};

				printer.Build(stream, docs);
			}
		}

		[Test]
		public void TestYanaTicketPrint2()
		{
			var docs = new List<Entity>();

			docs.AddRange(AirParser.Parse(Res.AirTicket1));
			docs.AddRange(AirParser.Parse(Res.AirTicket2));
			docs.AddRange(AirParser.Parse(Res.AirTicket3));
			docs.AddRange(AirParser.Parse(Res.AirTicket4));

			var tickets = new List<AviaTicket>();

			foreach (Entity doc in docs)
			{
				var ticket = (AviaTicket)doc;

				ticket.GrandTotal = new Money(ticket.Total == null || ticket.Total.Amount == 0 ? ticket.Fare : ticket.Total);

				tickets.Add(ticket);
			}

			using (var stream = new FileStream("~/ticket2.pdf".ResolvePath(), FileMode.Create))
			{
				var printer = new YanaTicketPrinter()
				{
					CompanyInfo = "YANA, 42 SAKSAGANSKOGO STR, KYIV 01033, UKRAINE",
					IataNumber = "723 20135",
					MainDataTemplatePath = @"..\..\..\Web\templates\ticket_design\header.jpg",
					SegmentHeaderTemplatePath = @"..\..\..\Web\templates\ticket_design\segment_header.jpg ",
					SegmentDataTemplatePath = @"..\..\..\Web\templates\ticket_design\segment.jpg ",
					FinanceDataTemplatePath = @"..\..\..\Web\templates\ticket_design\footer.jpg "
				};

				printer.Build(stream, tickets);
			}
		}*/

		/*[Test]
		public void TestBsvTicketPrinter1()
		{
			List<AviaTicket> tickets = GetTickets();

			using (var stream = new FileStream("~/bsv_ticket1.pdf".ResolvePath(), FileMode.Create))
			{
				BsvTicketPrinter printer = new BsvTicketPrinter
				{
					FramePath = @"..\..\..\Web\Templates\ticket_design\frame.pdf",
					HeaderPath = @"..\..\..\Web\Templates\ticket_design\header.pdf",
					SegmentHeaderTemplatePath = @"..\..\..\Web\Templates\ticket_design\group header.pdf",
					SegmentDataTemplatePath = @"..\..\..\Web\Templates\ticket_design\group row.pdf",
					MealInfoPath = @"..\..\..\Web\Templates\ticket_design\meal info.pdf",
					CalculationPath = @"..\..\..\Web\Templates\ticket_design\calculation.pdf",
					FooterPath = @"..\..\..\Web\Templates\ticket_design\footer.pdf"
				};

				printer.Build(stream, tickets);
			}
		}

		[Test]
		public void TestBsvTicketPrinter2()
		{
			var docs = new List<Entity>();

			docs.AddRange(AirParser.Parse(Res.AirTicket1));
			docs.AddRange(AirParser.Parse(Res.AirTicket2));
			docs.AddRange(AirParser.Parse(Res.AirTicket3));
			docs.AddRange(AirParser.Parse(Res.AirTicket4));

			var tickets = new List<AviaTicket>();
			tickets.Add((AviaTicket) docs[docs.Count - 1]);

			using (var stream = new FileStream("~/bsv_ticket2.pdf".ResolvePath(), FileMode.Create))
			{
				BsvTicketPrinter printer = new BsvTicketPrinter
				{
					FramePath = @"..\..\..\Templates\frame.pdf",
					HeaderPath = @"..\..\..\Templates\header.pdf",
					SegmentHeaderTemplatePath = @"..\..\..\Templates\group header.pdf",
					SegmentDataTemplatePath = @"..\..\..\Templates\group row.pdf",
					MealInfoPath = @"..\..\..\Templates\meal info.pdf",
					CalculationPath = @"..\..\..\Templates\calculation.pdf",
					FooterPath = @"..\..\..\Templates\footer.pdf"
				};

				printer.Build(stream, tickets);
			}
		}

		private Mockery _mockery;*/
	}
}