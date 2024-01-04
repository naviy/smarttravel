using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("Платёж", "Платежи"), DebuggerDisplay("{PaymentForm} {Number}")]
	[AgentPrivileges(
		Delete2 = UserRole.Supervisor,
		Copy2 = UserRole.Administrator
	)]
	public abstract partial class Payment : Entity2
	{

		//---g



		[Patterns.Number, EntityName]
		public virtual string Number { get; set; }


		[Patterns.Date]
		public virtual DateTime Date { get; protected set; }

		public virtual void SetDate(Domain db, DateTime value)
		{
			if (!db.ClosedPeriod.IsOpened(value))
				throw new DomainException(Exceptions.DateIsClosed);

			CheckCanModify();

			Date = value;
		}


		public virtual PaymentForm PaymentForm { get; set; }

		[RU("Номер документа")]
		public virtual string DocumentNumber { get; set; }

		[Hidden(false)]
		public virtual string DocumentUniqueCode => null;


		[Patterns.Payer, Required]
		public virtual Party Payer { get; set; }

		public virtual void SetPayer(Party value)
		{
			if (SetPayerInternal(value))
				SetOrderInternal(null);
		}


		public virtual Order Order { get; protected set; }

		public virtual void SetOrder(Order value)
		{
			WrapSetter(() => SetOrderInternal(value));
		}


		[RU("Квитанция"), Suggest(typeof(Receipt))]
		public virtual Invoice Invoice { get; protected set; }

		public virtual void SetInvoice(Invoice value)
		{
			if (Equals(Invoice, value))
				return;

			Invoice = value;

			if (value != null)
				SetOrderInternal(value.Order);
		}


		[RU("Дата счета/квитанции")]
		[DataPath("Invoice.IssueDate")]
		public virtual DateTime? InvoiceDate => Invoice?.IssueDate;

		[Hidden]
		public virtual int Sign => 1;


		[RU("Сумма")]
		public virtual Money Amount { get; protected set; }

		public virtual void SetAmount(Money value)
		{
			if (Equals(Amount, value))
				return;

			Amount = value;

			Order?.Refresh();
		}


		[Patterns.Vat]
		public virtual Money Vat { get; protected set; }

		public virtual void SetVat(Money value)
		{
			if (Equals(Vat, value))
				return;

			Vat = value;

			Order?.Refresh();
		}


		[RU("Получен от"), Hidden(true)]
		public virtual string ReceivedFrom { get; set; }

		[RU("Дата проводки")]
		public virtual DateTime? PostedOn { get; protected set; }


		public virtual void Post()
		{
			SetPostedOn(DateTime.Today.AsUtc());
		}


		public virtual void SetPostedOn(DateTime? value)
		{
			if (PostedOn == value)
				return;

			PostedOn = value;

			Order?.Refresh();
		}


		[RU("Сохранить проведенным"), NotMapped]
		public virtual bool SavePosted
		{
			get => _savePosted;
			set
			{
				_savePosted = value;
				if (value)
					PostedOn = DateTime.Now;
			}
		}


		public virtual void SetPostedOn()
		{
			Post();
		}


		[Patterns.Note, Hidden(true)]
		public virtual string Note { get; set; }


		[Patterns.IsVoid, Hidden(true)]
		public virtual bool IsVoid { get; protected set; }

		public virtual void SetIsVoid(bool value)
		{
			if (IsVoid == value)
				return;

			IsVoid = value;

			Order?.Refresh();
		}


		[Patterns.AssignedTo]
		public virtual Person AssignedTo { get; set; }

		[RU("Зарегистрирован")]
		public virtual Person RegisteredBy { get; set; }

		[Hidden(false)]
		public virtual bool IsPosted => PostedOn.HasValue;

		[Patterns.Owner]
		public virtual Party Owner { get; set; }

		public virtual BankAccount BankAccount { get; set; }


		public virtual byte[] PrintedDocument { get; set; }

		public virtual PaymentSystem PaymentSystem { get; set; }



		private bool _updating;

		private bool _canModifyChecked;
		private bool _savePosted;



		//---g



		public override string ToString()
		{
			return Number;
		}



		//---g



		public virtual void Refresh()
		{

			if (Order != null && Payer == null)
				SetPayerInternal(Order.BillTo ?? Order.Customer);

		}



		private bool SetPayerInternal(Party payer)
		{

			if (Equals(Payer, payer))
				return false;

			CheckCanModify();

			Payer = payer;


			return true;

		}



		private void SetOrderInternal(Order order)
		{

			if (Equals(Order, order))
				return;


			Order?.RemovePayment(this);

			Order = order;


			if (order == null)
				return;


			if (Payer == null)
				Payer = order.BillTo ?? order.Customer;

			order.AddPayment(this);

		}



		private void WrapSetter(Action action)
		{

			if (_updating)
				return;


			_updating = true;

			try
			{
				action();
			}
			finally
			{
				_updating = false;
			}

		}



		private void CheckCanModify(Domain db = null)
		{

			if (_canModifyChecked || db == null)
				return;


			db.Payment.CheckCanUpdate(this);

			_canModifyChecked = true;

		}



		//---g

	}






	//===g



}