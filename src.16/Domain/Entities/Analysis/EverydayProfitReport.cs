using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Ежедневный отчет по выручке")]
	public partial class EverydayProfitReport
	{

		public string Id { get; set; }

		public OrganizationReference Provider { get; set; }

		public ProductType ProductType { get; set; }

		[EntityName]
		public ProductReference Product { get; set; }

		public DateTimeOffset IssueDate { get; set; }

		public PersonReference Seller { get; set; }

		[RU("Пассажиры / Покупатели"), Length(16)]
		public string PassengerName { get; set; }

		public string Itinerary { get; set; }

		public DateTimeOffset? StartDate { get; set; }

		public DateTimeOffset? FinishDate { get; set; }

		public CountryReference Country { get; set; }

		public Money Fare { get; set; }

		[Patterns.Currency]
		public string Currency { get; set; }

		[Patterns.CurrencyRate]
		public decimal? CurrencyRate { get; set; }

		[Float(2)]
		public decimal? EqualFare { get; set; }

		[Float(2)]
		public decimal? FeesTotal { get; set; }

		[Float(2)]
		public decimal? CancelFee { get; set; }

		[Float(2)]
		public decimal? Total { get; set; }

		[Float(2)]
		public decimal? Commission { get; set; }

		[Float(2)]
		public decimal? ServiceFee { get; set; }

		[Float(2)]
		public decimal? Vat { get; set; }

		[Float(2)]
		public decimal? GrandTotal { get; set; }


		public OrderReference Order { get; set; }

		//[Patterns.Payer]
		public PartyReference Payer { get; set; }

		[RU("Счёт")]
		public InvoiceReference Invoice { get; set; }

		[RU("Дата счёта")]
		public DateTimeOffset? InvoiceDate { get; set; }

		[RU("Акт")]
		public InvoiceReference CompletionCertificate { get; set; }

		[RU("Дата акта")]
		public DateTimeOffset? CompletionCertificateDate { get; set; }

		[RU("Оплата")]
		public PaymentReference Payment { get; set; }

		[RU("Дата оплаты")]
		public DateTimeOffset? PaymentDate { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<EverydayProfitReport> sm)
		{
			sm.Patterns((Product a) => new EverydayProfitReport
			{
				Provider = a.Provider,
				ProductType = a.Type,
				IssueDate = a.IssueDate,
				Seller = a.Seller,
				Itinerary = a.Itinerary,
				StartDate=a.StartDate,
				FinishDate = a.FinishDate,
                Fare = a.Fare,
				EqualFare = a.EqualFare,
				FeesTotal = a.FeesTotal,
				CancelFee = a.CancelFee,
				Total = a.Total,
				Commission = a.Commission,
				ServiceFee = a.ServiceFee,
				Vat = a.Vat,
				GrandTotal = a.GrandTotal,
				Order = a.Order,
				Payer = a.Order.BillTo,
			});
		}

	}


	public partial class EverydayProfitReportParams : ProductFilter { }


	public class EverydayProfitReportQuery : Domain.DbQuery<EverydayProfitReportParams, EverydayProfitReport>
	{
		public override IEnumerable<EverydayProfitReport> Get()
		{
			Params.AllowVoided = true;

			var products = Params.Get(db.Products);

			Count = products.Count();

			var query =
				from p in products
				where !p.IsVoid

				let currency0 = p.Fare.CurrencyId
				let currency = p.EqualFare.CurrencyId

				let payer = p.Order.BillTo ?? p.Order.Customer

				let k = p.IsRefund ? -1 : 1

				//let invoice = db.Invoices
				//	.Where(a => a.OrderId == p.OrderId && a.Type == InvoiceType.Invoice)
				//	.OrderByDescending(a => a.IssueDate)
				//	.FirstOrDefault()

				//let act = db.Invoices
				//	.Where(a => a.OrderId == p.OrderId && a.Type == InvoiceType.CompletionCertificate)
				//	.OrderByDescending(a => a.IssueDate)
				//	.FirstOrDefault()

				//let payment = db.Payments
				//	.Where(a => a.OrderId == p.OrderId)
				//	.OrderByDescending(a => a.Date)
				//	.FirstOrDefault()

				select new EverydayProfitReport
				{
					Id = p.Id,
					Provider = new OrganizationReference { Id = p.ProviderId ?? p.ProducerId, Name = p.Provider.Name ?? p.Producer.Name },
					ProductType = p.Type,
					Product = new ProductReference { Id = p.Id, Name = p.Name, _Type = p.Type.ToString() },
					IssueDate = p.IssueDate,
					Seller = new PersonReference { Id = p.SellerId, Name = p.Seller.Name },
					PassengerName = p.PassengerName,
					Itinerary = p.Itinerary,
					StartDate = p.StartDate,
					FinishDate = p.FinishDate,
					Country = new CountryReference { Id = p.CountryId, Name = p.Country.Name },
					Fare = p.Fare,
					Currency = currency,
					CurrencyRate = db.CurrencyDailyRates
						.Where(a => a.Date == p.IssueDate)
						.Select(a => currency0 == "USD" ? a.UAH_USD : currency0 == "EUR" ? a.UAH_EUR : currency0 == "RUB" ? a.UAH_RUB : 1)
						.FirstOrDefault() ?? 1,
					EqualFare = k * p.EqualFare.Amount,
					FeesTotal = k * p.FeesTotal.Amount,
					CancelFee = k * p.CancelFee.Amount,
					Total = k * p.Total.Amount,
					Commission = k * ((p.Commission.Amount ?? 0) - (p.CommissionDiscount.Amount ?? 0)),
					ServiceFee = k * p.ServiceFee.Amount,
					Vat = k * p.Vat.Amount,
					GrandTotal = k * p.GrandTotal.Amount,
					Order = new OrderReference { Id = p.OrderId, Number = p.Order.Number },
					Payer = new PartyReference
					{
						Id = payer.Id,
						Name = payer.Name,
						_Type = payer.Type.ToString(),
					},

					//Invoice = new InvoiceReference { Id = invoice.Id, Number = invoice.Number },
					//InvoiceDate = invoice.IssueDate,
					//CompletionCertificate = new InvoiceReference { Id = act.Id, Number = act.Number },
					//CompletionCertificateDate = act.IssueDate,
					//Payment = new PaymentReference { Id = payment.Id, Number = payment.Number },
					//PaymentDate = payment.Date,

				};

			query = query.As(OrderBy).As(Limit);

			var list = query.ToList();

			list.ForEach(p =>
			{
				db.Invoices
					.Where(a => a.OrderId == p.Order.Id && a.Type == InvoiceType.Invoice)
					.OrderByDescending(a => a.IssueDate)
					.AsOne(a => new { a.Id, a.Number, a.IssueDate })
					.Do(a =>
					{
						p.Invoice = new InvoiceReference(a.Id, a.Number);
						p.InvoiceDate = a.IssueDate;
					});

				db.Invoices
					.Where(a => a.OrderId == p.Order.Id && a.Type == InvoiceType.CompletionCertificate)
					.OrderByDescending(a => a.IssueDate)
					.AsOne(a => new { a.Id, a.Number, a.IssueDate })
					.Do(a =>
					{
						p.CompletionCertificate = new InvoiceReference(a.Id, a.Number);
						p.CompletionCertificateDate = a.IssueDate;
					});

				db.Payments
					.Where(a => a.OrderId == p.Order.Id)
					.OrderByDescending(a => a.Date)
					.AsOne(a => new { a.Id, a.Number, a.Date })
					.Do(a =>
					{
						p.Payment = new PaymentReference(a.Id, a.Number);
						p.PaymentDate = a.Date;
					});
			});

			return list;
		}

	}


	partial class Domain
	{
		public EverydayProfitReportQuery EverydayProfitReports { get; set; }
	}

}
