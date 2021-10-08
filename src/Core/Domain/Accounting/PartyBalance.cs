using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Metamodel;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public class PartyBalance
	{

		public EntityReference Party { get; set; }

		public OpeningBalanceDto OpeningBalance { get; set; }

		public DateTime? FirstDocumentDate { get; set; }


		public void SetFirstDocumentDateIfLess(DateTime? date)
		{
			if (FirstDocumentDate.HasValue)
			{
				if (date.HasValue && date.Value < FirstDocumentDate.Value)
					FirstDocumentDate = date;
			}
			else
			{
				FirstDocumentDate = date;
			}
		}

		public DateTime? LastDocumentDate { get; set; }

		public void SetLastDocumentDateIfGreater(DateTime? date)
		{
			if (LastDocumentDate.HasValue)
			{
				if (date.HasValue && date.Value > LastDocumentDate.Value)
					LastDocumentDate = date;
			}
			else
			{
				LastDocumentDate = date;
			}
		}

		public decimal Delivered { get; set; }

		public decimal Paid { get; set; }

		public decimal Overall 
			=> (OpeningBalance?.Balance ?? 0) + Paid - Delivered;

		public OrderBalance[] ByOrders { get; set; }


		public static PartyBalance By(Domain db, Contracts dc, Party party)
		{
			var openingBalance = db.OpeningBalance.Query
				.Where(a => a.Party == party)
				.OrderByDescending(a => a.Date)
				.Take(1)
				.SingleOrDefault();

		//select
		//	order_, sum(case when isrefund then -grandtotal_amount else grandtotal_amount end) delivered, 0 paid
		//from
		//	lt_product
		//where
		//	not isreservation and customer = :party and not isvoid and issuedate >= :from
		//group by
		//	order_

			var byOrders = db.Session.CreateSQLQuery(@"
select
	o.id, o.number_, sum(delivered) delivered, sum(paid) paid
from
	(
		select
			id as order_, total_amount as delivered, 0 as paid
		from
			lt_order
		where
			customer = :party and issuedate >= :from and not isvoid 

		union all

		select
			o.id, sum(-p.grandtotal_amount) delivered, 0 paid
		from
			lt_order o
			inner join lt_product p on o.id = p.order_
		where
			o.customer = :party and o.issuedate >= :from and not o.isvoid and p.isreservation and not p.isvoid
		group by
			o.id

		union all

		select
			p.order_, 0, sum(p.amount_amount)
		from
			lt_order o
			inner join lt_payment p on o.id = p.order_			
		where
			o.customer = :party and p.date_ >= :from and not p.isvoid
		group by
			p.order_

		union all

		select
			fromorder, 0, -sum(amount)
		from
			lt_internal_transfer
		where
			fromparty = :party and date_ >= :from -- and not isvoid
		group by
			fromorder

		union all

		select
			toorder, 0, sum(amount)
		from
			lt_internal_transfer
		where
			toparty = :party and date_ >= :from -- and not isvoid
		group by
			toorder
	) t
	left outer join lt_order o on o.id = t.order_ and not o.isvoid
group by
	o.id, o.number_
having
	sum(delivered) <> sum(paid)
order by
	o.number_ nulls first
				")
				.SetEntity("party", party)
				.SetParameter("from", openingBalance?.Date ?? DateTime.MinValue)
				.List<object[]>()
				.Select(proj => new OrderBalance
				{
					Order = proj[0] == null ? null : new EntityReference { Id = proj[0], Name = (string)proj[1], Type = Class.Of<Order>().Id },
					Delivered = (decimal)proj[2],
					Paid = (decimal)proj[3]
				})
				.ToArray();

			return new PartyBalance
			{
				OpeningBalance = dc.OpeningBalance.New(openingBalance),
				Delivered = byOrders.Sum(a => a.Delivered),
				Paid = byOrders.Sum(a => a.Paid),
				ByOrders = byOrders
			};
		}

		public static IEnumerable<PartyBalance> GetUnbalanced(Domain db, DateTime? to, bool includeOrders)
		{
			var toValue = to ?? DateTime.MaxValue;

			if (includeOrders)
			{
				var orderBalances = db.Session.CreateSQLQuery(@"
select
	p.id partyid, p.name partyname, b.balance openingbalance, ob.ordernumber, ob.orderowner, ob.delivered, ob.paid, ob.mindate, ob.maxdate
from
	lt_party p

	left outer join (
		select
			party, o.number_ ordernumber, ow.name orderowner, sum(delivered) delivered, sum(paid) paid, min(mindate) mindate, max(maxdate) maxdate
		from
			(
				select
					p.customer party, p.order_, sum(case when type = 1 then -p.grandtotal_amount else p.grandtotal_amount end) delivered, 0 paid, min(issuedate) mindate, max(issuedate) maxdate
				from
					lt_product p
					left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = p.customer
				where
					p.issuedate >= coalesce(b.date_, '0001-01-01') and p.issuedate < :to and not p.isreservation and not p.isvoid
				group by
					customer, order_

				union all

				select
					p.payer, p.order_, 0, sum(p.amount_amount), min(p.date_) mindate, max(p.date_) maxdate
				from
					lt_payment p
					left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = p.payer
				where
					not p.isvoid and p.date_ >= coalesce(b.date_, '0001-01-01') and p.date_ < :to
				group by
					p.payer, p.order_

				union all

				select
					t.fromparty, t.fromorder, 0, -sum(t.amount), min(t.date_) mindate, max(t.date_) maxdate
				from
					lt_internal_transfer t
					left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = t.fromparty
				where
					t.date_ >= coalesce(b.date_, '0001-01-01') and t.date_ < :to -- and not t.isvoid
				group by
					t.fromparty, t.fromorder

				union all

				select
					t.toparty, t.toorder, 0, sum(t.amount), min(t.date_) mindate, max(t.date_) maxdate
				from
					lt_internal_transfer t
					left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = t.toparty
				where
					t.date_ >= coalesce(b.date_, '0001-01-01') and t.date_ < :to -- and not t.isvoid
				group by
					t.toparty, t.toorder
			) t
			left outer join lt_order o on o.id = t.order_ and not o.isvoid
			left outer join lt_party ow on ow.id = o.owner
		group by
			party, o.number_, ow.name
	) ob on ob.party = p.id and ob.delivered <> ob.paid

	left outer join (
		select
			b.party, b.balance
		from
			lt_opening_balance b
			inner join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) t on b.party = t.party and b.date_ = t.date_
	) b on b.party = p.id and b.balance <> 0

where
	not (b.balance is null and ob.delivered is null)
order by
	p.name, ob.ordernumber nulls first
					")
					.SetDateTime("to", toValue)
					.List<object[]>();

				var partyBalances = new List<PartyBalance>();
				PartyBalance current = null;
				List<OrderBalance> orders = null;

				foreach (var proj in orderBalances)
				{
					var partyId = proj[0];
					var partyName = (string)proj[1];
					var openingBalance = proj[2];
					var orderNumber = (string)proj[3];
					var orderOwner = (string)proj[4];
					var delivered = proj[5];
					var paid = proj[6];
					var minDate = proj[7];
					var maxDate = proj[8];

					if (current == null || !Equals(current.Party.Id, partyId))
					{
						if (current != null)
						{
							current.ByOrders = orders.ToArray();

							partyBalances.Add(current);
						}

						current = new PartyBalance
						{
							Party = new EntityReference { Id = partyId, Name = partyName },
							OpeningBalance = openingBalance == null ? null : new OpeningBalanceDto { Balance = (decimal)openingBalance }
						};

						orders = new List<OrderBalance>();
					}

					if (delivered == null)
						continue;

					var orderBalance = new OrderBalance
					{
						Order = orderNumber == null ? null : new EntityReference { Name = orderNumber },
						Owner = orderOwner,
						FirstDocumentDate = (DateTime?)minDate,
						LastDocumentDate = (DateTime?)maxDate,
						Delivered = (decimal)delivered,
						Paid = (decimal)paid
					};

					orders.Add(orderBalance);

					if (current.FirstDocumentDate.HasValue)
					{
						if (orderBalance.FirstDocumentDate.HasValue && orderBalance.FirstDocumentDate.Value < current.FirstDocumentDate.Value)
							current.FirstDocumentDate = orderBalance.FirstDocumentDate;
					}
					else
					{
						current.FirstDocumentDate = orderBalance.FirstDocumentDate;
					}

					current.SetFirstDocumentDateIfLess(orderBalance.FirstDocumentDate);
					current.SetLastDocumentDateIfGreater(orderBalance.LastDocumentDate);
					current.Delivered += orderBalance.Delivered;
					current.Paid += orderBalance.Paid;
				}

				return partyBalances;
			}

			return db.Session.CreateSQLQuery(@"
select
	party.name party, b.balance, d.delivered, p.paid, least(d.mindate, p.mindate) mindate, greatest(d.maxdate, p.maxdate) maxdate

from
	lt_party party

	left outer join (
		select
			b.party, b.balance
		from
			lt_opening_balance b
			inner join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) t on b.party = t.party and b.date_ = t.date_
	) b on b.party = party.id

	left outer join (
		select
			p.customer party, sum(case when type = 1 then -p.grandtotal_amount else p.grandtotal_amount end) delivered, min(issuedate) mindate, max(issuedate) maxdate
		from
			lt_product p
			left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = p.customer
		where
			p.issuedate >= coalesce(b.date_, '0001-01-01') and p.issuedate < :to and not p.isreservation and not p.isvoid
		group by
			p.customer
	) d on d.party = party.id

	left outer join (
		select
			party, sum(paid) paid, min(t.mindate) mindate, max(t.maxdate) maxdate
		from
			(
				select
					p.payer party, sum(p.amount_amount) paid, min(p.date_) mindate, max(p.date_) maxdate
				from
					lt_payment p
					left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = p.payer
				where
					not p.isvoid and p.date_ >= coalesce(b.date_, '0001-01-01') and p.date_ < :to
				group by
					p.payer

				union all

				select
					t.fromparty, -sum(t.amount), min(t.date_) mindate, max(t.date_) maxdate
				from
					lt_internal_transfer t
					left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = t.fromparty
				where
					t.date_ >= coalesce(b.date_, '0001-01-01') and t.date_ < :to -- and not t.isvoid
				group by
					t.fromparty

				union all

				select
					t.toparty, sum(t.amount), min(t.date_) mindate, max(t.date_) maxdate
				from
					lt_internal_transfer t
					left outer join (select party, max(date_) date_ from lt_opening_balance where date_ < :to group by party) b on b.party = t.toparty
				where
					t.date_ >= coalesce(b.date_, '0001-01-01') and t.date_ < :to -- and not t.isvoid
				group by
					t.toparty
			) t
		group by
			party
	) p on p.party = party.id

where
	coalesce(d.delivered, 0) <> coalesce(b.balance, 0) + coalesce(p.paid, 0)

order by
	party.name
				")
				.SetDateTime("to", toValue)
				.List<object[]>()
				.Convert(proj => new PartyBalance
				{
					Party = new EntityReference { Name = (string)proj[0] },
					FirstDocumentDate = (DateTime?)proj[4],
					LastDocumentDate = (DateTime?)proj[5],
					OpeningBalance = proj[1] == null ? null : new OpeningBalanceDto { Balance = (decimal)proj[1] },
					Delivered = (decimal?)proj[2] ?? 0,
					Paid = (decimal?)proj[3] ?? 0
				});
		}

	}

}