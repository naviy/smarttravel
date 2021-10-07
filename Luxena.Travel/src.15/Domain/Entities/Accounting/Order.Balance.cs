using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Баланс")]
	public partial class OrderBalance
	{
		public string Id { get; set; }

		[Localization(typeof(Order))]
		public OrderReference Order { get; set; }

		[RU("Дата заказа")]
		public DateTimeOffset? IssueDate { get; set; }

		public PartyReference Customer { get; set; }

		[Patterns.Currency, Length(3)]
		public string Currency { get; set; }

		[RU("Оказано услуг на сумму"), Float(2)]
		public decimal Delivered { get; set; }

		[RU("Оплачено"), Float(2)]
		public decimal Paid { get; set; }

		[RU("Баланс"), Float(2)]
		public decimal Balance { get; set; }
	}


	public partial class OrderBalanceParams
	{

		public class Result1
		{
			public string OrderId { get; set; }
			public decimal? Delivered { get; set; }
			public decimal? Paid { get; set; }
		}

		public IQueryable<OrderBalance> Get(Domain db)
		{
			var openingBalance = db.OpeningBalances
				.Where(a => a.PartyId == CustomerId)
				.OrderByDescending(a => a.Date)
				.One();

			var minDate = openingBalance?.Date ?? DateTimeOffset.MinValue;

			var query0 = (
				from a in db.Products
				where a.CustomerId == CustomerId && a.IssueDate >= minDate && !a.IsReservation && !a.IsVoid
				group a by a.OrderId into g
				select new Result1
				{
					OrderId = g.Key,
					Paid = 0,
					Delivered = g.Sum(a => (a.IsRefund ? -1 : 1) * a.GrandTotal.Amount),
				}
			).Concat(
				from a in db.Payments
				where a.PayerId == CustomerId && a.PostedOn >= minDate && !a.IsVoid
				group a by a.OrderId into g
				select new Result1 { OrderId = g.Key, Paid = g.Sum(a => a.Amount.Amount), Delivered = 0, }
			).Concat(
				from a in db.InternalTransfers
				where a.FromPartyId == CustomerId && a.Date >= minDate
				group a by a.FromOrderId into g
				select new Result1 { OrderId = g.Key, Paid = -g.Sum(a => a.Amount), Delivered = 0, }
			).Concat(
				from a in db.InternalTransfers
				where a.ToPartyId == CustomerId && a.Date >= minDate
				group a by a.ToOrderId into g
				select new Result1 { OrderId = g.Key, Paid = g.Sum(a => a.Amount), Delivered = 0, }
			);

			var query1 =
				from a in query0
				group a by a.OrderId into g
				select new
				{
					OrderId = g.Key,
					Delivered = g.Sum(a => a.Delivered) ?? 0,
					Paid = g.Sum(a => a.Paid) ?? 0,
				};

			var query2 =
				from a in query1
				join _o in db.Orders on a.OrderId equals _o.Id into o_
				from o in o_.DefaultIfEmpty()
				let Currency = o.Total.CurrencyId
				let Balance = a.Paid - a.Delivered
				where Balance != 0 && (a.OrderId == null || !o.IsVoid)
				orderby Currency, Balance descending
				select new OrderBalance
				{
					Id = a.OrderId ?? "",
					Order = new OrderReference { Id = a.OrderId, Number = o.Number ?? "Без заказа" },
					IssueDate = o.IssueDate,
					Currency = Currency,
					Delivered = a.Delivered,
					Paid = a.Paid,
					Balance = Balance,
				};

			return query2;

		}

	}

	public class OrderBalanceQuery : Domain.DbQuery<OrderBalanceParams, OrderBalance>
	{
		public override IEnumerable<OrderBalance> Get()
		{
			var orders = Params.Get(db);
			Count = orders.Count();
			return Limit(orders);
		}
	}


	partial class Domain
	{
		public OrderBalanceQuery OrderBalances { get; set; }
	}

}