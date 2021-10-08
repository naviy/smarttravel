using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Платёж", "Платежи"), DebuggerDisplay("{PaymentForm} {Number}")]
	[AgentPrivileges(
		Delete2 = UserRole.Supervisor,
		Copy2 = UserRole.Administrator
	)]
	public abstract partial class Payment : Entity2
	{

		[EntityType]
		public virtual PaymentForm PaymentForm { get; set; }

		[Patterns.Number, EntityName]
		public string Number { get; set; }

		[Patterns.Date, EntityDate]
		public DateTimeOffset Date { get; set; }

		[RU("Номер документа"), Length(10)]
		public string DocumentNumber { get; set; }

		public virtual string DocumentUniqueCode => null;

		[Patterns.Payer, Required]
		protected Party _Payer;

		[Subject]
		protected Order _Order;

		[RU("Квитанция"), Receipt]
		protected Invoice _Invoice;

		[RU("Дата счета/квитанции")]
		public DateTimeOffset? InvoiceDate => Invoice.As(a => a.IssueDate);

		[NotUiMapped]
		public int Sign => 1;

		[RU("Сумма")]
		public Money Amount { get; set; }

		[Patterns.Vat]
		public Money Vat { get; set; }
		
		[RU("Получен от")]
		public string ReceivedFrom { get; set; }

		[RU("Дата проводки"), Subject]
		public DateTimeOffset? PostedOn { get; set; }

		[RU("Сохранить проведенным"), NotDbMapped, Subject]
		public bool? SavePosted { get; set; }

		[Patterns.Note]
		public string Note { get; set; }

		[Patterns.IsVoid]
		public bool IsVoid { get; set; }

		[Patterns.AssignedTo, Agent]
		protected Person _AssignedTo;

		[RU("Зарегистрирован"), Agent]
		protected Person _RegisteredBy;

		public bool IsPosted => PostedOn.HasValue;

		[ActiveOwner]
		protected Party _Owner;

		public virtual byte[] PrintedDocument { get; set; }

		protected PaymentSystem _PaymentSystem;


		public override string ToString() => Number;


		static partial void Config_(Domain.EntityConfiguration<Payment> entity)
		{
			entity.Association(a => a.Payer);//, a => a.Payments_Payer);
			entity.Association(a => a.Order, a => a.Payments);
			entity.Association(a => a.Invoice, a => a.Payments);
			entity.Association(a => a.AssignedTo);//, a => a.Payments_AssignedTo);
			entity.Association(a => a.RegisteredBy);//, a => a.Payments_RegisteredBy);
			entity.Association(a => a.Owner);//, a => a.Payments_Owner);
			entity.Association(a => a.PaymentSystem);//, a => a.Payments);
		}


		protected override IList<Domain.Entity> GetDependents()
		{
			return new[] { Order };
		}

		protected override void Bind()
		{
			base.Bind();

			ModifyMaster<Order, Payment>(a => a.Order, a => a.Payments);
		}

		public override void CalculateDefaults()
		{
			base.CalculateDefaults();
            Date = DateTime.Today;
		}

		public override void CalculateOnLoad()
		{
			base.CalculateOnLoad();
			SavePosted = PostedOn != null;
		}

		public override void Calculate()
		{
			base.Calculate();

			if (IsSaving())
			{
				if (Number.No())
					Number = db.NewSequence<Payment>();

				if (IsCashOrder && DocumentNumber.No())
				{
					DocumentNumber = db.NewSequence(
						IsCashOutOrder ? "CashOutOrderPayment" : "CashOrderPayment",
						Owner.As(a => a.Id).AsString(),
						num => db.Payments.Any(a => a.DocumentNumber == num && !a.IsVoid)
					);
				}
			}

			if (Order)
				Payer = Order.Customer;

			if (!Amount && !Vat)
			{
				Amount = Order?.TotalDue + db;
				Vat = Order?.VatDue + db;
			}

			if (!RegisteredBy)
				RegisteredBy = db.User?.Person;

			if (!AssignedTo)
				AssignedTo = db.User?.Person;

			if (!Owner)
				Owner = db.DefaultOwner;

			if (LastChangedPropertyName != nameof(PostedOn) && SavePosted != null)
			{
				PostedOn = SavePosted == true ? DateTime.Today : (DateTimeOffset?)null;
			}
			else if (LastChangedPropertyName != nameof(SavePosted))
			{
				SavePosted = PostedOn != null;
			}
		}


		[EntityAction, RU("Анулировать")]
		public void Void(bool? b1) => 
			IsVoid = true;

		[EntityAction, RU("Восстановить")]
		public void Unvoid() => 
			IsVoid = false;

		[EntityAction]
		public string GetNote() => Note;
	}


	partial class Domain
	{
		public DbSet<Payment> Payments { get; set; }
	}

}