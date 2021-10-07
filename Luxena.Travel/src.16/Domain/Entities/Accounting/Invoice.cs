using System;
using System.Collections.Generic;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Инвойс", "Счета/квитанции")]
	[SupervisorPrivileges(List2 = UserRole.Agent, Delete2 = UserRole.Agent)]
	public partial class Invoice : Entity
	{

		[Patterns.Type, EntityType]
		public InvoiceType Type { get; set; }

		[Patterns.IssueDate, EntityDate]
		public DateTimeOffset IssueDate { get; set; }

		[Patterns.Number, EntityName]
		public virtual string Number { get; set; }

		[RU("Договор")]
		public string Agreement { get; set; }

		[RU("Дата создания"), DateTime2, Utility]
		public DateTimeOffset TimeStamp { get; set; }

		public virtual byte[] Content { get; set; }

		protected Order _Order;

//		[Customer]
//		public Party Customer
//		{
//			get { return Order.As(a => a.Customer); }
//		}
//
//		[Patterns.Payer]
//		public Party BillTo
//		{
//			get { return Order.As(a => a.BillTo); }
//		}
//
//		[Patterns.Payer]
//		public virtual string BillToName
//		{
//			get { return Order.As(a => a.BillToName); }
//		}
//
//		[Patterns.Recipient]
//		[DataPath("Order.ShipTo")]
//		public virtual Party ShipTo
//		{
//			get { return Order.As(a => a.ShipTo); }
//		}
//
//		[RU("Заказ аннулирован")]
//		[DataPath("Order.IsVoid")]
//		public virtual bool IsOrderVoid
//		{
//			get { return Order.IsVoid; }
//		}
//
//		[Patterns.Owner]
//		[DataPath("Order.Owner")]
//		public virtual Party Owner
//		{
//			get { return Order.Owner; }
//		}

		[RU("Выпустил"), Agent]
		protected Person _IssuedBy;

		[Patterns.Total]
		public Money Total { get; set; }

		[Patterns.Vat]
		public Money Vat { get; set; }


		public virtual ICollection<Payment> Payments { get; set; }


		public override string ToString()
		{
			return Number;
		}


		static partial void Config_(Domain.EntityConfiguration<Invoice> entity)
		{
			entity.Association(a => a.Order, a => a.Invoices);
			entity.Association(a => a.IssuedBy);//, a => a.Invoices_IssuedBy);
		}

	}


	partial class Domain
	{
		public DbSet<Invoice> Invoices { get; set; }
	}
}