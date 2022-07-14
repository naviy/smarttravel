using System;

using Luxena.Base.Data;
using Luxena.Domain.Contracts;
using Luxena.Travel.Services;


namespace Luxena.Travel.Domain
{



	//===g






	public abstract partial class PaymentDto : EntityContract
	{

		//---g



		public PaymentForm PaymentForm { get; set; }

		public DateTime Date { get; set; }

		public string Number { get; set; }

		public string DocumentNumber { get; set; }

		public Party.Reference Payer { get; set; }

		public MoneyDto Amount { get; set; }

		public MoneyDto Vat { get; set; }

		public string ReceivedFrom { get; set; }

		public DateTime? PostedOn { get; set; }

		public Person.Reference AssignedTo { get; set; }

		public Person.Reference RegisteredBy { get; set; }

		public string Note { get; set; }

		public bool IsVoid { get; set; }

		public Invoice.Reference Invoice { get; set; }

		public Order.Reference Order { get; set; }

		public Party.Reference Owner { get; set; }

		public BankAccount.Reference BankAccount { get; set; }

		public OperationPermissions Permissions { get; set; }



		//---g

	}






	//===g






	public class PaymentContractService<TPayment, TPaymentService, TPaymentDto>
		: EntityContractService<TPayment, TPaymentService, TPaymentDto>
		where TPayment : Payment
		where TPaymentService : Payment.Service<TPayment>
		where TPaymentDto : PaymentDto, new()
	{

		public PaymentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.DocumentNumber = r.DocumentNumber;
				c.Payer = r.Payer;
				c.Amount = r.Amount;
				c.Vat = r.Vat;
				c.PaymentForm = r.PaymentForm;
				c.AssignedTo = r.AssignedTo;
				c.RegisteredBy = r.RegisteredBy;
				c.ReceivedFrom = r.ReceivedFrom;
				c.Note = r.Note;
				c.IsVoid = r.IsVoid;

				c.Invoice = r.Invoice;
				c.Order = r.Order;
				c.Owner = r.Owner;
				c.BankAccount = r.BankAccount;

				c.Date = r.Date;
				c.PostedOn = r.PostedOn;

				c.Permissions = new OperationPermissions
				{
					CanDelete = db.CanDelete(r),
					CanUpdate = db.CanUpdate(r),
				};
			};

			EntityFromContract += (r, c) =>
			{
				r.SetDate(db, c.Date);

				var order = c.Order + db;
				var invoice = c.Invoice + db;

				if (invoice != null && !Equals(invoice.Order, order))
					throw new ArgumentException(Exceptions.InvoiceNotInOrder_Error);

				r.DocumentNumber = c.DocumentNumber + db;

				r.SetAmount(c.Amount + db);
				r.SetVat(c.Vat + db);

				r.ReceivedFrom = c.ReceivedFrom + db;
				r.Note = c.Note + db;
				r.Owner = c.Owner + db;
				r.BankAccount = c.BankAccount + db;
				r.SetPostedOn();
				r.AssignedTo = c.AssignedTo + db;

				r.SetOrder(order);
				r.SetInvoice(invoice);
				r.Payer = c.Payer + db;

				r.AssignedTo |= invoice.As(a => a.IssuedBy) ?? order.As(a => a.AssignedTo) ?? db.Security.Person;
			};
		}


		public TPaymentDto New(Payment r)
		{
			return base.New((TPayment)r);
		}

	}






	//===g






	public partial class PaymentContractService : EntityContractService
	{

		public ItemListResponse ChangeVoidStatus(object[] ids, RangeRequest prms)
		{
			var payments = db.Payment.ChangeVoidStatus(ids);

			return NewItemListResponse(payments, prms, New);
		}


		public ItemListResponse Post(object[] ids, RangeRequest prms)
		{
			var payments = db.Payment.ListByIds(ids);

			if (payments.Count != ids.Length)
				throw new ObjectsNotFoundException(ids.Length == 1 ? Exceptions.NoRowById_Translation : Exceptions.ObjectsNotFound_Error);

			payments.ForEach(a => a.Post());

			var response = db.Payment.GetItemListResponse(payments, prms, New);

			return response;
		}


		public PaymentDto New(Payment r)
		{
			if (r.IsCashInOrder)
				return dc.CashInOrderPayment.New(r);

			if (r.IsCashOutOrder)
				return dc.CashOutOrderPayment.New(r);

			if (r.IsCheck)
				return dc.CheckPayment.New(r);

			if (r.IsElectronic)
				return dc.ElectronicPayment.New(r);

			if (r.IsWireTransfer)
				return dc.WireTransfer.New(r);

			throw new NotImplementedException();
		}

	}






	//===g



}