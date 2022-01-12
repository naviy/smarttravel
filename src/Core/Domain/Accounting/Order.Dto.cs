using System;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Metamodel;
using Luxena.Domain.Contracts;




namespace Luxena.Travel.Domain
{



	//===g






	public partial class OrderDto : EntityContract
	{

		//---g



		public string Type => Class.Of<Order>().Id;

		public string Text => Number;

		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public Party.Reference Customer { get; set; }

		public Party.Reference BillTo { get; set; }

		public Party.Reference ShipTo { get; set; }

		public Party.Reference Intermediary { get; set; }

		public bool IsVoid { get; set; }

		public MoneyDto Discount { get; set; }

		public MoneyDto Vat { get; set; }

		public bool UseServiceFeeOnlyInVat { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto ServiceFee { get; set; }

		public MoneyDto Paid { get; set; }

		public MoneyDto TotalDue { get; set; }

		public MoneyDto VatDue { get; set; }

		public decimal DeliveryBalance { get; set; }


		public DateTime? BonusDate { get; set; }

		public decimal? BonusSpentAmount { get; set; }

		public Party.Reference BonusRecipient { get; set; }


		public Person.Reference AssignedTo { get; set; }

		public BankAccount.Reference BankAccount { get; set; }

		public Party.Reference Owner { get; set; }

		public string Note { get; set; }

		public bool IsPublic { get; set; }
		public bool AllowAddProductsInClosedPeriod { get; set; }

		public bool IsSubjectOfPaymentsControl { get; set; }

		//		public bool SeparateServiceFee { get; set; }

		public OrderItemDto[] Items { get; set; }

		public InvoiceDto[] Invoices { get; set; }

		public PaymentDto[] Payments { get; set; }

		public TaskDto[] Tasks { get; set; }

		public OrderTransferDto[] Transfers { get; set; }

		public OperationPermissions Permissions { get; set; }

		public bool CanCreateTransfer { get; set; }

		public bool CanChangeAssignedTo { get; set; }



		//---g

	}






	public partial class OrderContractService : EntityContractService<Order, Order.Service, OrderDto>
	{

		//---g



		public OrderContractService()
		{

			ContractFromEntity += (r, c) =>
			{

				c.Number = r.Number;
				c.IssueDate = r.IssueDate;

				c.IsVoid = r.IsVoid;

				c.Customer = r.Customer;
				c.ShipTo = r.ShipTo;

				c.BillTo = r.BillTo;

				if (c.BillTo == null && r.BillToName.Yes())
					c.BillTo = new Party.Reference { Name = r.BillToName };


				c.Intermediary = r.Intermediary;

				c.Discount = r.Discount;
				c.Vat = r.Vat;
				c.UseServiceFeeOnlyInVat = r.UseServiceFeeOnlyInVat;
				c.Total = r.Total;
				c.Paid = r.Paid;
				c.TotalDue = r.TotalDue;
				c.ServiceFee = r.ServiceFee;

				c.VatDue = r.VatDue;
				c.DeliveryBalance = r.DeliveryBalance;

				c.BonusDate = r.BonusDate;
				c.BonusSpentAmount = r.BonusSpentAmount;
				c.BonusRecipient = r.BonusRecipient;

				c.AssignedTo = r.AssignedTo;
				c.BankAccount = r.BankAccount;
				c.Owner = r.Owner;

				c.Note = r.Note;
				c.IsPublic = r.IsPublic;
				c.AllowAddProductsInClosedPeriod = r.AllowAddProductsInClosedPeriod;
				c.IsSubjectOfPaymentsControl = r.IsSubjectOfPaymentsControl;

				//				c.SeparateServiceFee = db.Configuration.AviaOrderItemGenerationOption == AviaOrderItemGenerationOption.SeparateServiceFee;

				c.Items = dc.OrderItem.New(r.Items);
				c.Invoices = dc.Invoice.New(r.Invoices);
				c.Payments = r.Payments.Select(a => dc.Payment.New(a).Do(dto => dto.Amount *= a.Sign)).ToArray();
				c.Tasks = dc.Task.New(r.Tasks, q => q.Reverse());

				c.Transfers = r.IncomingTransfers.Select(a => new OrderTransferDto(a, true))
					.Concat(r.OutgoingTransfers.Select(a => new OrderTransferDto(a, false)))
					.OrderBy(a => a.Date)
					.ToArray()
				;


				c.Permissions = new OperationPermissions
				{
					CanDelete = db.Order.CanDelete(r),
					CanUpdate = db.Order.CanUpdate(r)
				};

				c.CanCreateTransfer = db.InternalTransfer.CanCreate();
				c.CanChangeAssignedTo = db.IsGranted(UserRole.Supervisor);

			};



			EntityFromContract += (r, c) =>
			{

				r.IssueDate = c.IssueDate + db;
				r.Number = (c.Number + db) ?? r.Number;

				r.SetCustomer(db, c.Customer + db);
				r.ShipTo = c.ShipTo + db;

				r.BillTo = c.BillTo + db;

				if (r.BillTo != null && c.BillTo != null && c.BillTo.Name.Yes() && r.BillTo.Name != c.BillTo.Name)
				{
					r.BillTo = null;
				}

				r.BillToName = r.BillTo != null || c.BillTo == null ? null : c.BillTo.Name + db;

				r.Intermediary = c.Intermediary + db;

				r.SetDiscount(c.Discount + db);
				r.SetVat(c.Vat + db);
				r.UseServiceFeeOnlyInVat = c.UseServiceFeeOnlyInVat + db;
				r.SetTotal(c.Total + db);

				r.BonusDate = c.BonusDate + db;
				r.BonusSpentAmount = c.BonusSpentAmount + db;
				r.BonusRecipient = c.BonusRecipient + db;

				r.Owner = c.Owner + db;
				r.AssignedTo = c.AssignedTo + db;
				r.BankAccount = c.BankAccount + db;
				r.IsPublic = c.IsPublic + db;
				r.AllowAddProductsInClosedPeriod = c.AllowAddProductsInClosedPeriod + db;
				r.IsSubjectOfPaymentsControl = c.IsSubjectOfPaymentsControl + db;
				r.Note = c.Note + db;

				dc.OrderItem.Update(
					r, r.Items, c.Items,
					r.AddOrderItem, r.RemoveOrderItem
				);

				r.Refresh();
				//r.Update(db);

			};
		}



		public override ItemResponse Update(OrderDto c, RangeRequest prms)
		{

			var alreadyOrderedDocs = (
				from item in c.Items
				let p = item.Product + db
				where p?.Order != null && !Equals(p.Order.Id, c.Id)
				select item
			).ToArray();


			if (alreadyOrderedDocs.Yes())
				return new ItemResponse { Errors = alreadyOrderedDocs };


			return base.Update(c, prms);

		}



		public ItemResponse AddProducts(object orderId, object[] productIds, bool separateServiceFee)
		{

			var order = db.Order.By(orderId);
			db.AssertUpdate(order);

			var products = db.Product.ListByIds(productIds);

			var links = products
				.Where(a => a.Order != null)
				.Select(a => new OrderItemDto { Order = a.Order, Product = a })
				.ToArray();

			if (links.Yes())
				return new ItemResponse { Errors = links };


			order.Add(db, products, separateServiceFee ? ServiceFeeMode.Separate : ServiceFeeMode.Join);

			return new ItemResponse { Item = New(order) };

		}



		//---g

	}






	//===g



}