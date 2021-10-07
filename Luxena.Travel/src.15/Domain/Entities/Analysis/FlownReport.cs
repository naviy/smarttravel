using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Flown-отчет")]
	public partial class FlownReport
	{
		[Utility]
		public string Id { get; set; }

		// дата продажи билета
		[EntityDate]
		public DateTimeOffset Date { get; set; }

		// статус билета: S - продажа, R - возврат
		[Length(2)]
		public string Op { get; set; }

		// код авиакомпании - не обязательно
		[Length(2)]
		public string AC { get; set; }

		// номер билета
		public AviaDocumentReference TicketNumber { get; set; }

		// заказчик
		[Length(20)]
		public PartyReference Client { get; set; }

		[Length(20)]
		public string Passenger { get; set; }

		// маршрут
		[Length(16)]
		public string Route { get; set; }

		// Валюта
		[Length(3)]
		public string Curr { get; set; }

		[Float(2)]
		public decimal? Fare { get; set; }

		[Float(2)]
		public decimal? Tax { get; set; }

		// за 1-й месяц
		[Float(2)]
		public decimal? Flown1 { get; set; }

		[Float(2)]
		public decimal? Flown2 { get; set; }

		[Float(2)]
		public decimal? Flown3 { get; set; }

		[Float(2)]
		public decimal? Flown4 { get; set; }

		[Float(2)]
		public decimal? Flown5 { get; set; }

		[Float(2)]
		public decimal? Flown6 { get; set; }

		[Float(2)]
		public decimal? Flown7 { get; set; }

		[Float(2)]
		public decimal? Flown8 { get; set; }

		[Float(2)]
		public decimal? Flown9 { get; set; }

		[Float(2)]
		public decimal? Flown10 { get; set; }

		[Float(2)]
		public decimal? Flown11 { get; set; }

		[Float(2)]
		public decimal? Flown12 { get; set; }


		[Length(10)]
		public string TourCode { get; set; }

		[RU("Добор с билета"), Length(14)]
		public AviaDocumentReference CheapTicket { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<FlownReport> sm)
		{
			sm.Patterns((Product a) => new FlownReport
			{
				TourCode = a.TourCode,
			});
		}

	}


	public partial class FlownReportParams : ProductFilter { }


	public class FlownReportQuery : Domain.DbQuery<FlownReportParams, FlownReport>
	{
		public override IEnumerable<FlownReport> Get()
		{
			Params.AllowVoided = true;

			var products = Params.Get(db.AviaDocuments);

			Count = products.Count();

			var query =
				from p in products
				where !p.IsVoid
				orderby p.IssueDate, p.Name

				select new FlownReport
				{
					Id = p.Id,
					Date = p.IssueDate,
					Op = p.IsRefund ? "R" : "S",
					AC = p.Producer.AirlineIataCode,

					TicketNumber = new AviaDocumentReference
					{
						Id = p.Id,
						Name = p.Number,
						_Type = p.Type.ToString(),
					},

					Client = new PartyReference
					{
						Id = p.CustomerId,
						Name = p.Customer.LegalName ?? p.Customer.Name,
						_Type = p.Customer.Type.ToString(),
					},

					Passenger = p.PassengerName,
					Route = p.Itinerary,
					Curr = p.EqualFare.CurrencyId,
					Fare = p.EqualFare.Amount,
					Tax = p.FeesTotal.Amount,
					TourCode = p.TourCode,

					CheapTicket = p.ReissueFor.EqualFare.Amount < p.EqualFare.Amount
						? new AviaDocumentReference
						{
							Id = p.ReissueForId,
							Name = p.ReissueFor.Name,
							_Type = p.ReissueFor.Type.ToString(),
						}
						: null,
				};

			query = query.As(Limit);

			var list = query.ToList();

			if (!Params.IssueMonth.HasValue)
				return list;


			var ticketIds = list.Select(a => a.Id).ToArray();

			var allSegments = db.FlightSegments
				.Where(a => ticketIds.Contains(a.TicketId) && a.CouponAmount.CurrencyId != null)
				.Select(a => new { a.TicketId, a.DepartureTime, a.CouponAmount })
				.ToList();


			var date0 = Params.IssueMonth.As(a => new DateTime(a.Year, a.Month, 1));
			var months = new DateTime[13];
			for (var i = 0; i < months.Length; i++)
			{
				months[i] = date0.AddMonths(i);
			}


			list.ForEach(p =>
			{
				var segments = allSegments.Where(a => a.TicketId == p.Id).ToArray();

				if (segments.No()) return;

				var curr = segments.One().CouponAmount.CurrencyId;

				var rate = db.GetCurrencyRate(p.Date, curr, p.Curr) ?? 1;

				var firstFlownIndex = -1;
				var flownCount = 0;
				var flownSum = 0m;

				var flowns = new decimal[12];
				for (var i = 0; i < flowns.Length; i++)
				{
					var month1 = months[i];
					var month2 = months[i + 1];

					var flown = segments
						.Where(a => a.DepartureTime >= month1 && a.DepartureTime < month2)
						.Sum(a => a.CouponAmount.Amount * rate) ?? 0;

					flownSum += flown;

					if (flown > 0)
					{
						flownCount++;
						if (firstFlownIndex < 0)
							firstFlownIndex = i;
					}

					flowns[i] = flown;
				}

				// Чтобы суммы по купонам сходились с EqualFare
				var fareDelta = (p.Fare ?? 0) - flownSum;
				if (p.Fare != null && fareDelta > 0 && firstFlownIndex >= 0)
				{
					var flownDelta = fareDelta / flownCount;
					flownSum = 0;

					for (var i = 0; i < flowns.Length; i++)
					{
						if (flowns[i] > 0)
							flowns[i] = Math.Round(flowns[i] + flownDelta, 2);

						flownSum += flowns[i];
					}

					flowns[firstFlownIndex] = (p.Fare ?? 0) - flownSum + flowns[firstFlownIndex];
				}

				p.Flown1 = flowns[0];
				p.Flown2 = flowns[1];
				p.Flown3 = flowns[2];
				p.Flown4 = flowns[3];
				p.Flown5 = flowns[4];
				p.Flown6 = flowns[5];
				p.Flown7 = flowns[6];
				p.Flown8 = flowns[7];
				p.Flown9 = flowns[8];
				p.Flown10 = flowns[9];
				p.Flown11 = flowns[10];
				p.Flown12 = flowns[11];
			});

			return list;
		}

	}


	partial class Domain
	{
		public FlownReportQuery FlownReports { get; set; }
	}

}
