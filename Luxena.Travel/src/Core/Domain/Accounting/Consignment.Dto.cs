using System;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class ConsignmentDto : EntityContract
	{

		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public Party.Reference Supplier { get; set; }

		public Party.Reference Acquirer { get; set; }

		public Order.Reference Order { get; set; }

		public MoneyDto Vat { get; set; }

		public MoneyDto GrandTotal { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto Discount { get; set; }

		public string TotalSupplied { get; set; }

		public OrderItemDto[] Items { get; set; }

		public IssuedConsignmentDto[] IssuedConsignments { get; set; }

		public OperationPermissions Permissions { get; set; }

	}


	public partial class ConsignmentContractService : EntityContractService<Consignment, Consignment.Service, ConsignmentDto>
	{
		public ConsignmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;

				c.Supplier = r.Supplier;
				c.Acquirer = r.Acquirer;

				c.GrandTotal = r.GrandTotal;

				c.Vat = r.Vat;
				c.Total = r.Total;

				c.Discount = r.Discount;

				c.TotalSupplied = r.TotalSupplied;

				c.Items = dc.OrderItem.New(r.OrderItems);

				c.Order = r.Order;

				c.IssuedConsignments = dc.IssuedConsignment.New(r.IssuedConsignments);

				c.Permissions = new OperationPermissions
				{
					CanDelete = db.CanDelete(r),
					CanUpdate = db.CanUpdate(r),
				};
			};

			EntityFromContract += (r, c) =>
			{
				r.Order = c.Items.Select(a => db.OrderItem.By(a.Id)?.Order).One();

				r.Number = c.Number + db;

				r.IssueDate = c.IssueDate + db;
				r.Supplier = c.Supplier + db;
				r.Acquirer = c.Acquirer + db;

				var currency =
					db.Currency.By(c.Items.Where(a => a.GrandTotal != null).One(a => a.GrandTotal.Currency))
					?? db.Configuration.DefaultCurrency;

				r.GrandTotal = c.GrandTotal + db ?? new Money(currency);
				r.Vat = c.Vat + db ?? new Money(currency);
				r.Discount = c.Discount + db ?? new Money(currency);

				dc.OrderItem.Update(
					r, r.OrderItems, c.Items,
					null, (cc, item) => item.Text = cc.Text,
					r.AddOrderItem, r.RemoveOrderItem
				);

				db.OnCommit(r, rr =>
				{
					r.TotalSupplied = c.TotalSupplied.Yes() ? c.TotalSupplied + db : r.GetTotalSupplied(db);
					db.Issue(r);
				});
			};
		}
	}

}