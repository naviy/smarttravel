using System;

using Luxena.Base.Metamodel;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Счет/квитанция", "Счета/квитанции")]
	[SupervisorPrivileges(List2 = UserRole.Agent, Delete2 = UserRole.Agent)]
	public partial class Invoice : Entity
	{

		[Patterns.Number, EntityName]
		public virtual string Number { get; set; }

		[RU("Договор")]
		[Hidden(false)]
		public virtual string Agreement { get; set; }

		[Patterns.IssueDate]
		public virtual DateTime IssueDate { get; set; }

		[RU("Дата создания"), DateTime2, Utility]
		public virtual DateTime TimeStamp { get; set; }

		[Patterns.Type]
		public virtual InvoiceType Type { get; set; }

		[Hidden(false)]
		public virtual byte[] Content { get; set; }

		public virtual Order Order { get; set; }

		[Patterns.Customer]
		[DataPath("Order.Customer")]
		public virtual Party Customer => Order?.Customer;

		[Patterns.Payer]
		[DataPath("Order.BillTo")]
		[Hidden(true)]
		public virtual Party BillTo => Order.BillTo;

		[Patterns.Payer]
		[DataPath("Order.BillToName")]
		[Hidden(true)]
		public virtual string BillToName => Order?.BillToName;

		[Patterns.Recipient]
		[DataPath("Order.ShipTo")]
		[Hidden(true)]
		public virtual Party ShipTo => Order?.ShipTo;

		[RU("Заказ аннулирован")]
		[DataPath("Order.IsVoid")]
		[Hidden(true)]
		public virtual bool IsOrderVoid => Order.IsVoid;

		[Patterns.Owner]
		[DataPath("Order.Owner")]
		[Hidden(true)]
		public virtual Party Owner => Order.Owner;

		[RU("Выпущен")]
		public virtual Person IssuedBy { get; set; }

		[Patterns.Total]
		public virtual Money Total { get; set; }

		[Patterns.Vat]
		public virtual Money Vat { get; set; }


		public override string ToString() => Number;
	}

}