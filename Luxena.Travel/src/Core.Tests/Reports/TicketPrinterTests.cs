using System;
using System.Collections.Generic;
using System.IO;

using Luxena.Base.Domain;
using Luxena.Travel.Bsv;
using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;
using Luxena.Travel.Reports;
using Luxena.Travel.Ufsa;

using NMock2;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Reports
{
	[TestFixture]
	public class TicketPrinterTests
	{
		[SetUp]
		public void SetUp()
		{
			PdfUtility.SetFontsPath(@"..\..\..\web\static\fonts");

			_mockery = new Mockery();
		}

		private static List<AviaTicket> GetTickets()
		{
			var docs = new List<Entity2>();

			docs.AddRange(AirParser.Parse(Res.AirTicket1, new Currency("UAH")));

			var tickets = new List<AviaTicket>();

			foreach (AviaTicket ticket in docs)
			{
				ticket.Owner = new Organization
				{
					Name = "Some travel agency"
				};

				ticket.Segments[0].DepartureTime = new DateTime(2009, 11, 10, 13, 10, 0);
				ticket.Segments[0].CheckInTime = "10:25";
				ticket.Segments[0].ArrivalTerminal = "2E";
				ticket.Segments[0].FromAirport.Country = new Country { Name = "Ukraine" };
				ticket.Segments[0].FromAirport.Settlement = "Kiev";
				ticket.Segments[0].FromAirport.Name = "KIEV BORISPOL very long airport name";

				ticket.Segments[0].ServiceClass = ServiceClass.PremiumEconomy;
				ticket.Segments[0].MealTypes = MealType.Lunch | MealType.Dinner;
				ticket.Segments[0].Seat = "10A";


				ticket.ServiceFee = new Money(new Currency { Code = "UAH" }, 100);
				ticket.GrandTotal = new Money(new Currency { Code = "UAH" }, 123456.22m);

				tickets.Add(ticket);
			}
			return tickets;
		}

		[Test]
		public void TestDefaultTicketPrinter1()
		{
			var tickets = GetTickets();

			var db = _mockery.NewMock<Domain.Domain>();

			Stub.On(db).GetProperty("Configuration").Will(Return.Value(
				new SystemConfiguration
				{
					Company = new Organization { Name = "Фабрика туризму", ActualAddress = "Одеса, вул. Базарна, 45, кв.30", Phone1 = "+38 0482 32-03-20" }
				}));

			using (var stream = new FileStream("~/default_ticket1.pdf".ResolvePath(), FileMode.Create))
			{
				var printer = new DefaultTicketPrinter { db = db };

				printer.Build(stream, tickets);
			}
		}

		/*
				[Test]
				public void TestYanaTicketPrint1()
				{
					var docs = GetTickets();

					foreach (AviaTicket doc in docs)
						doc.GrandTotal = new Money(doc.Total == null || doc.Total.Amount == 0 ? doc.Fare : doc.Total);

					using (var stream = new FileStream("~/ticket1.pdf".ResolvePath(), FileMode.Create))
					{
						var printer = new YanaTicketPrinter
						{
							CompanyInfo = "YANA, 42 SAKSAGANSKOGO STR, KYIV 01033, UKRAINE",
							IataNumber = "723 20135",
							MainDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\header.jpg",
							SegmentHeaderTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\segment_header.jpg ",
							SegmentDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\segment.jpg ",
							FinanceDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\footer.jpg "
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
							MainDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\header.jpg",
							SegmentHeaderTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\segment_header.jpg ",
							SegmentDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\segment.jpg ",
							FinanceDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\footer.jpg "
						};

						printer.Build(stream, tickets);
					}
				}*/

		[Test]
		public void TestBsvTicketPrinter1()
		{
			var tickets = GetTickets();

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Type = PenalizeOperationType.ChangesBeforeDeparture,
				Status = PenalizeOperationStatus.NotAllowed
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Type = PenalizeOperationType.RefundBeforeDeparture,
				Status = PenalizeOperationStatus.Chargeable,
				Description = "50 UAH"
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Type = PenalizeOperationType.NoShowAfterDeparture,
				Status = PenalizeOperationStatus.NotChargeable
			});

			using (var stream = new FileStream(("~/bsv_ticket" + DateTime.Now.ToFileTime() + ".pdf").ResolvePath(), FileMode.Create))
			{
				var printer = new BsvTicketPrinter
				{
					FramePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\frame.pdf",
					HeaderPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\header.pdf",
					SegmentHeaderTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\group header.pdf",
					SegmentDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\group row.pdf",
					MealInfoPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\meal info.pdf",
					CalculationPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\calculation.pdf",
					PenaltiesPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\penalties.pdf",
					ServicesPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\services.pdf",
				};

				printer.Build(stream, tickets);
			}
		}

		[Test]
		public void TestBsvTicketPrinter2()
		{
			var docs = new List<Entity2>();

			var defaultCurrency = new Currency("UAH");

			docs.AddRange(AirParser.Parse(Res.AirTicket1, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket2, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket3, defaultCurrency));
			docs.AddRange(AirParser.Parse(Res.AirTicket4, defaultCurrency));

			var tickets = new List<AviaTicket> { (AviaTicket) docs[docs.Count - 1] };

			using (var stream = new FileStream(("~/bsv_ticket" + DateTime.Now.ToFileTime() + ".pdf").ResolvePath(), FileMode.Create))
			{
				var printer = new BsvTicketPrinter
				{
					FramePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\frame.pdf",
					HeaderPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\header.pdf",
					SegmentHeaderTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\group header.pdf",
					SegmentDataTemplatePath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\group row.pdf",
					MealInfoPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\meal info.pdf",
					CalculationPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\calculation.pdf",
					PenaltiesPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\penalties.pdf",
					ServicesPath = @"..\..\..\..\profiles\bsv\web\static\templates\ticket\services.pdf",
				};

				printer.Build(stream, tickets);
			}
		}

		[Test]
		public void TestUfsaTicketPrinter1()
		{
			var tickets = GetTickets();

			var entities = AirParser.Parse(Res.AirTicket2, new Currency("UAH"));
			tickets.Add((AviaTicket) entities[0]);

			var ticket = tickets[0];
			ticket.GdsPassport = "HK1/P/UKR/E064757/UKR/23APR61/M/25APR17/VLADIMIROV/VITALII";
			ticket.GdsPassportStatus = GdsPassportStatus.Exist;
			ticket.Passenger = new Person();
			ticket.Passenger.AddPassport(new Passport { Number = "E064757" });

			IList<FlightSegment> segments = new List<FlightSegment>(ticket.Segments);

			for (var i = 0; i < 1; i++)
				foreach (var segment in segments)
					ticket.AddSegment((FlightSegment) segment.Clone());

			ticket.AddSegment((FlightSegment) segments[0].Clone());
			ticket.AddSegment((FlightSegment) segments[0].Clone());
			ticket.AddSegment((FlightSegment) segments[0].Clone());
			ticket.AddSegment((FlightSegment) segments[0].Clone());

			var owner = new Organization
			{
				Note = @"Киъв, ул.Золотоворитська, 13
13 Zolotovoritska Str., Kiev",
				Phone1 = "+38 044 111 22 33",
				Email1 = "sale1@ufsa.com.ua"
			};

			tickets.ForEach(t => t.Owner = owner);

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Type = PenalizeOperationType.ChangesBeforeDeparture,
				Status = PenalizeOperationStatus.NotAllowed
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Type = PenalizeOperationType.RefundBeforeDeparture,
				Status = PenalizeOperationStatus.Chargeable,
				Description = "50 UAH"
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Type = PenalizeOperationType.NoShowAfterDeparture,
				Status = PenalizeOperationStatus.NotChargeable
			});

			using (var stream = new FileStream(("~/ufsa_ticket" + DateTime.Now.ToFileTime() + ".pdf").ResolvePath(), FileMode.Create))
			{
				var printer = new UfsaTicketPrinter
				{
					LogoImagePath = @"..\..\..\..\profiles\ufsa\web\static\templates\ticket\ufsa_ticket_logo.png",
					WebSite = "www.ufsa.com.ua",
					Email = "sale@ufsa.com.ua",
					Phone = "+38 044 206 75 74"
				};

				printer.Build(stream, tickets);
			}
		}

		[Test]
		public void TestLuxenaTicketPrinter1()
		{
			var docs = new List<Entity2>();

			docs.AddRange(AirParser.Parse(Res.AirTicket25, new Currency("UAH")));

			var tickets = new List<AviaTicket> { (AviaTicket) docs[0] };

			tickets[0].ServiceFee = tickets[0].EqualFare;
			tickets[0].GrandTotal = tickets[0].EqualFare;

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Ticket = tickets[0],
				Description = "EUR 100.00+100грн",
				Status = PenalizeOperationStatus.Chargeable,
				Type = PenalizeOperationType.ChangesBeforeDeparture
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Ticket = tickets[0],
				Description = "EUR 100.00+налоги",
				Status = PenalizeOperationStatus.Chargeable,
				Type = PenalizeOperationType.RefundBeforeDeparture
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Ticket = tickets[0],
				Description = "не доступно",
				Status = PenalizeOperationStatus.Chargeable,
				Type = PenalizeOperationType.NoShowBeforeDeparture
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Ticket = tickets[0],
				Description = "EUR 100.00+100 грн",
				Status = PenalizeOperationStatus.Chargeable,
				Type = PenalizeOperationType.ChangesAfterDeparture
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Ticket = tickets[0],
				Description = "Не доступно",
				Status = PenalizeOperationStatus.Chargeable,
				Type = PenalizeOperationType.RefundAfterDeparture
			});

			tickets[0].AddPenalizeOperation(new PenalizeOperation
			{
				Ticket = tickets[0],
				Description = "не доступно",
				Status = PenalizeOperationStatus.Chargeable,
				Type = PenalizeOperationType.NoShowAfterDeparture
			});

			tickets[0].Segments[0].MealTypes = MealType.HotMeal;

			using (var stream = new FileStream(("~/luxena_ticket" + DateTime.Now.ToFileTime() + ".pdf").ResolvePath(), FileMode.Create))
			{
				var printer = new LuxenaTicketPrinter
				{
					HeaderPath = @"..\..\..\..\profiles\demo\web\static\templates\ticket\header.pdf",
					SegmentHeaderPartPath = @"..\..\..\..\profiles\demo\web\static\templates\ticket\row_header.pdf",
					SegmentTopPartPath = @"..\..\..\..\profiles\demo\web\static\templates\ticket\row_top.pdf",
					SegmentMiddlePartPath = @"..\..\..\..\profiles\demo\web\static\templates\ticket\row_middle.pdf",
					SegmentBottomPartPath = @"..\..\..\..\profiles\demo\web\static\templates\ticket\row_bottom.pdf",
					FooterPath = @"..\..\..\..\profiles\demo\web\static\templates\ticket\footer.pdf",
					CompanyName = "ТОВ \"Луксена Софт\""
				};

				printer.Build(stream, tickets);
			}
		}

		[Test]
		public void TestGetSegmentLinesCount()
		{
		}


		private Mockery _mockery;
	}
}