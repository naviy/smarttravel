using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Заказ", "Заказы"), Icon("briefcase"), Big]
	[AgentPrivileges(Copy = new object[] { })]
	[DebuggerDisplay("Order {Number}")]
	public partial class Order : Entity2
	{

		[Patterns.Number, EntityName, Length(10), Unique]
		public string Number { get; set; }

		[Patterns.IssueDate, EntityDate]
		public DateTimeOffset IssueDate { get; set; }

		[Patterns.IsVoid]
		public bool IsVoid { get; set; }

		[Customer]
		protected Party _Customer;


		[Patterns.Payer, Customer]
		protected Party _BillTo;

		[Patterns.Payer]
		public string BillToName { get; set; }

		[RU("Получатель"), Customer]
		protected Party _ShipTo;


		[RU("Ответственный"), Agent]
		protected Person _AssignedTo;

		[RU("Владелец")]
		protected Party _Owner;

		protected BankAccount _BankAccount;

		[RU("Общий доступ")]
		public bool IsPublic { get; set; }

		[RU("Отображать в контроле оплат")]
		public bool IsSubjectOfPaymentsControl { get; set; }

		[RU("Выделять сервисный сбор")]
		public bool? SeparateServiceFee { get; set; }

		[RU("НДС только от сервисного сбора")]
		public bool UseServiceFeeOnlyInVat { get; set; }


		[Patterns.Discount]
		public Money Discount { get; protected set; }

		[Patterns.Total]
		public Money Total { get; protected set; }

		[Patterns.Vat]
		public Money Vat { get; protected set; }

		[RU("Оплачено")]
		public Money Paid { get; protected set; }

		[RU("К оплате")]
		public Money TotalDue { get; protected set; }

		[RU("Оплачен")]
		public bool IsPaid
		{
			get { return TotalDue != null && TotalDue.Amount <= 0; }
			protected set { }
		}

		[RU("НДС к оплате")]
		public Money VatDue { get; protected set; }

		[RU("Баланс взаиморасчетов")]
		public decimal DeliveryBalance { get; protected set; }


		[Patterns.ServiceFee]
		public Money ServiceFee => Items.AsSum(a => a.ServiceFee);


		[Patterns.Note]
		public string Note { get; set; }


		public int? InvoiceLastIndex { get; set; }

		public virtual ICollection<OrderItem> Items { get; set; }

		public virtual ICollection<Product> Products { get; set; }


		public virtual ICollection<Invoice> Invoices { get; set; }

		public virtual ICollection<Payment> Payments { get; set; }

		[RU("Входящие внутренние переводы")]
		public virtual ICollection<InternalTransfer> IncomingTransfers { get; set; }

		[RU("Исходящие внутренние переводы")]
		public virtual ICollection<InternalTransfer> OutgoingTransfers { get; set; }


		//public List<OrderItem> ItemsBy(Product doc, Func<OrderItem, bool> match = null)
		//{
		//	if (Items == null) return new OrderItem[0];

		//	return (
		//		from item in Items
		//		where Equals(item.Product, doc) && (match == null || match(item))
		//		select item
		//	).ToList();
		//}


		public override string ToString() => Number;


		static partial void Config_(Domain.EntityConfiguration<Order> entity)
		{
			entity.Association(a => a.Customer);//, a => a.Orders_Customer);
			entity.Association(a => a.BillTo);//, a => a.Orders_BillTo);
			entity.Association(a => a.ShipTo);//, a => a.Orders_ShipTo);
			entity.Association(a => a.AssignedTo);//, a => a.Orders_AssignedTo);
			entity.Association(a => a.BankAccount);//, a => a.Orders);
			entity.Association(a => a.Owner);//, a => a.Orders_Owner);
		}


		public override void Calculate()
		{
			base.Calculate();

			if (!IsSaving()) return;

			var currency = Total?.CurrencyId ?? db.AppConfiguration.DefaultCurrency;

			Discount = Items?.Sum(a => a.Discount) + currency;
			Total = Items?.Sum(a => a.GrandTotal) + currency;
			Vat = currency;

			var taxedTotal = Items?.Where(a => a.HasVat).Sum(a => a.TaxedTotal);
			if (taxedTotal)
				Vat = (taxedTotal - Discount) * db.AppConfiguration.VatRate / (100 + db.AppConfiguration.VatRate);

			var givenVat = Items?.Where(a => a.HasVat).Sum(a => a.GivenVat);
			Vat += givenVat;

			if (Vat?.Amount < 0)
				Vat.Amount = 0;

			Money paymentAmount = currency;
			Money paymentVat = currency;

			Payments?.Where(a => !a.IsVoid && a.IsPosted).ForEach(a =>
			{
				paymentAmount += a.Sign * a.Amount;
				paymentVat += a.Sign * a.Vat;
			});

			Paid = paymentAmount
				+ IncomingTransfers?.Sum(a => a.Amount)
				- OutgoingTransfers?.Sum(a => a.Amount);

			TotalDue = Total - Paid;
			VatDue = Vat - paymentVat;

			var balance = Paid - Items?.Where(a => a.IsDelivered).Sum(a => a.GrandTotal);

			DeliveryBalance = balance?.Amount ?? 0;

			ReorderItems();
		}

		private void ReorderItems()
		{
			var products = Items.Sure()
				.OrderBy(a => a.Position == 0 ? 9999 : a.Position)
				.ThenBy(a => a.Product?.Name)
				.Select(a => a.Product)
				.Distinct()
				.ToList();

			var items = Items?
				.OrderBy(a => a.Product != null ? products.IndexOf(a.Product) : a.Position == 0 ? 9999 : a.Position)
				.ThenBy(a => a.Product?.Name)
				.ThenBy(a => a.LinkType)
				.ToList();

			items.ForEach((a, i) => a.Position = i + 1);

			//Items.ForEach((a, i) =>
			//{
			//	if (a.Position != i + 1)
			//		a.Update(db, aa => a.Position = i + 1);
			//});
		}
	}


	partial class OrderLookup
	{
		// ReSharper disable once RedundantAssignment
		static partial void SelectAndOrderByName(IQueryable<Order> query, ref IEnumerable<OrderLookup> lookupList)
		{
			lookupList =
				from a in query
				orderby a.Number descending, a.IssueDate descending
				select new OrderLookup { Id = a.Id, Number = a.Number };
		}
	}


	partial class Domain
	{
		public DbSet<Order> Orders { get; set; }
	}

}