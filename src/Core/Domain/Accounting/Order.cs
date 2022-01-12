using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

using Luxena.Base.Domain;
using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("Заказ", "Заказы")]
	[AgentPrivileges(Copy = new object[] { })]
	[DebuggerDisplay("Order {Number}")]
	public partial class Order : Entity2
	{

		//---g



		[Patterns.Number, EntityName]
		public virtual string Number { get; set; }

		[Patterns.IssueDate, EntityDate]
		public virtual DateTime IssueDate { get; set; }

		[Patterns.IsVoid]
		public virtual bool IsVoid { get; set; }

		[Patterns.Customer]
		public virtual Party Customer { get; protected set; }

		public virtual void SetCustomer(Domain db, Party value)
		{
			Customer = value;

			_items.ForEach(a => a.SetOrderReference(db));

			_payments.ForEach(a => a.Refresh());

			_incomingTransfers.ForEach(a => a.Refresh());

			_outgoingTransfers.ForEach(a => a.Refresh());
		}

		[RU("Плательщик"), Suggest(typeof(Customer))]
		public virtual Party BillTo { get; set; }

		[RU("Плательщик")]
		public virtual string BillToName { get; set; }

		[RU("Получатель"), Suggest(typeof(Customer))]
		public virtual Party ShipTo { get; set; }

		[RU("Посредник")]
		public virtual Party Intermediary { get; set; }


		[Patterns.Discount]
		public virtual Money Discount { get; protected set; }

		public virtual void SetDiscount(Money value)
		{
			Discount = value;

			Refresh();
		}

		[Patterns.Vat]
		public virtual Money Vat { get; protected set; }

		public virtual void SetVat(Money value)
		{
			Vat = value;
			Refresh();
		}

		[RU("НДС только от сервисного сбора")]
		public virtual bool UseServiceFeeOnlyInVat { get; set; }

		[Patterns.Total]
		public virtual Money Total { get; protected set; }

		public virtual void SetTotal(Money value)
		{
			Total = value;

			Refresh();
		}

		[RU("Оплачено")]
		public virtual Money Paid => EnsureRefresh(ref _paid);

		[RU("Оплачено нал")]
		public virtual Money CheckPaid => EnsureRefresh(ref _checkPaid);

		[RU("Оплачено б/нал")]
		public virtual Money WirePaid => EnsureRefresh(ref _wirePaid);

		[RU("Оплачено КК")]
		public virtual Money CreditPaid => EnsureRefresh(ref _creditPaid);

		[RU("Оплачено прочее")]
		public virtual Money RestPaid => EnsureRefresh(ref _restPaid);

		[RU("К оплате")]
		public virtual Money TotalDue => EnsureRefresh(ref _totalDue);

		[RU("Оплачен")]
		public virtual bool IsPaid =>
			TotalDue != null && Total != null &&
			(Total.Amount > 0 && TotalDue.Amount <= 0 || Total.Amount < 0 && TotalDue.Amount >= 0);

		[RU("НДС к оплате")]
		public virtual Money VatDue => EnsureRefresh(ref _vatDue);

		[RU("Баланс взаиморасчетов")]
		public virtual decimal DeliveryBalance
		{
			get
			{
				EnsureRefresh();
				return _deliveryBalance;
			}
		}

		[RU("Дата начисления бонусов")]
		public virtual DateTime? BonusDate { get; set; }

		[RU("Списано бонусов")]
		public virtual decimal? BonusSpentAmount { get; set; }

		[RU("Получатель бонусов")]
		public virtual Party BonusRecipient { get; set; }


		[RU("Ответственный")]
		public virtual Person AssignedTo { get; set; }

		public virtual BankAccount BankAccount { get; set; }

		[RU("Владелец")]
		public virtual Party Owner { get; set; }

		[Patterns.Note]
		public virtual string Note { get; set; }

		[RU("Общий доступ")]
		public virtual bool IsPublic { get; set; }

		[RU("Разрешить добавление билетов, даже если заказ находится в закрытом периоде")]
		public virtual bool AllowAddProductsInClosedPeriod { get; set; }

		[RU("Отображать в контроле оплат")]
		public virtual bool IsSubjectOfPaymentsControl { get; set; } = true;

		[RU("Выделять сервисный сбор")]
		[NotMapped]
		public virtual bool SeparateServiceFee { get; set; }

		public virtual IList<OrderItem> Items => _items;

		public virtual IList<Invoice> Invoices => _invoices;

		public virtual int? InvoiceLastIndex { get; set; }
		public virtual int? ConsignmentLastIndex { get; set; }

		public virtual IList<Payment> Payments => _payments;

		public virtual IList<Task> Tasks => _tasks;

		[Patterns.ServiceFee]
		public virtual Money ServiceFee
		{
			get
			{
				Money sum = null;

				foreach (var item in Items)
				{
					sum += item.ServiceFee;
				}

				return sum;
			}
		}

		public virtual IList<InternalTransfer> OutgoingTransfers => _outgoingTransfers;

		public virtual IList<InternalTransfer> IncomingTransfers => _incomingTransfers;

		[RU("Накладные")]
		public virtual string ConsignmentNumbers
			=> Items?.Select(a => a.Consignment).Where(a => a != null).Select(a => a.Number).Distinct().OrderBy(a => a).Join(", ");

		[RU("Счета")]
		public virtual string InvoiceNumbers
			=> Invoices?.Select(a => a.Number).Distinct().OrderBy(a => a).Join(", ");

		[RU("Первый счет")]
		public virtual string FirstInvoiceNumber
			=> Invoices?.Where(a => a.Type == InvoiceType.Invoice).Select(a => a.Number).FirstOrDefault();

		public virtual IEnumerable<Consignment.Reference> ConsignmentRefs
			=> Items?.Select(a => a.Consignment).Where(a => a != null).Distinct().Select(a => (Consignment.Reference)a).ToList();



		//---g



		public virtual void AddOrderItem(Domain db, OrderItem item)
		{

			item.Order = this;

			item.SetOrderReference(db);

			item.Position = _items.Count;

			_items.Add(item);

		}



		public virtual void RemoveOrderItem(Domain db, OrderItem item)
		{

			_items.Remove(item);

			item.ClearOrderReference(db);
			//item.Update(db);

		}



		public virtual IList<OrderItem> ItemsBy(Product doc, Func<OrderItem, bool> match = null)
		{

			if (Items == null) 
				return Array.Empty<OrderItem>();


			return (
				from item in Items
				where Equals(item.Product, doc) && (match == null || match(item))
				select item
			).ToList();

		}



		public virtual void AddPrintedDocument(Invoice item)
		{
			item.Order = this;

			_invoices.Add(item);
		}



		public virtual void AddPayment(Payment payment)
		{
			_payments.Add(payment);

			payment.SetOrder(this);

			Refresh();
		}



		public virtual void RemovePayment(Payment payment)
		{
			_payments.Remove(payment);

			payment.SetOrder(null);

			Refresh();
		}



		public virtual void AddTask(Task task)
		{
			_tasks.Add(task);

			task.SetOrder(this);
		}



		public virtual void RemoveTask(Task task)
		{
			_tasks.Remove(task);

			task.SetOrder(null);
		}



		public virtual void AddOutgoingTransfer(InternalTransfer transfer)
		{
			_outgoingTransfers.Add(transfer);

			transfer.FromOrder = this;

			Refresh();
		}



		public virtual void RemoveOutgoingTransfer(InternalTransfer transfer)
		{
			_outgoingTransfers.Remove(transfer);

			transfer.FromOrder = null;

			Refresh();
		}



		public virtual void AddIncomingTransfer(InternalTransfer transfer)
		{
			_incomingTransfers.Add(transfer);

			transfer.ToOrder = this;

			Refresh();
		}

		public virtual void RemoveIncomingTransfer(InternalTransfer transfer)
		{
			_incomingTransfers.Remove(transfer);

			transfer.ToOrder = null;

			Refresh();
		}



		public virtual bool HasSource(Entity2 entity)
		{
			return Items.Any(item => item.IsLinkedWith(entity));
		}



		public virtual void SetOrderReferences(Domain db)
		{
			_items.ForEach(item => item.SetOrderReference(db));
		}



		public virtual void ClearOrderReferences(Domain db)
		{
			_items.ForEach(item => item.ClearOrderReference(db));
		}

		//		public virtual void Update(Domain db)
		//		{
		//			_items.ForEach(item => item.Update(db));
		//		}



		public override string ToString()
		{
			return Number;
		}



		public virtual void Refresh()
		{
			_refreshPending = true;
		}



		public virtual Money EnsureRefresh(ref Money moneyField)
		{
			EnsureRefresh();
			return moneyField;
		}



		public virtual void EnsureRefresh()
		{

			if (!_refreshPending) 
				return;


			if (Total == null)
			{
				_paid = null;
				_totalDue = null;
				_vatDue = null;
			}
			else
			{

				var currency = Total.Currency;

				_paid = new Money(currency);
				_checkPaid = new Money(currency);
				_wirePaid = new Money(currency);
				_creditPaid = new Money(currency);
				_restPaid = new Money(currency);
				var vat = new Money(currency);


				foreach (var payment in _payments.Where(a => !a.IsVoid && a.IsPosted))
				{

					var money = payment.Sign * payment.Amount;

					_paid += money;


					if (payment.IsCheck)
						_checkPaid += money;
					else if (payment.IsWireTransfer)
						_wirePaid += money;
					else if (payment.IsElectronic)
						_creditPaid += money;
					else
						_restPaid += money;


					vat += payment.Sign * payment.Vat;

				}


				var trasfer = _incomingTransfers.Sum(a => a.Amount) - _outgoingTransfers.Sum(a => a.Amount);


				_paid = _paid + trasfer;
				_restPaid = _restPaid + trasfer;

				_totalDue = Total - _paid;

				_vatDue = Vat - vat;


				var balance = _paid.Clone();


				foreach (var item in _items)
				{
					if (item.IsDelivered)
						balance -= item.GrandTotal;
				}


				_deliveryBalance = balance?.Amount ?? 0;

			}


			_refreshPending = false;

		}



		public virtual void Add(
			Domain db,
			Product document,
			ServiceFeeMode serviceFeeMode = ServiceFeeMode.Separate,
			bool saveDocuments = true
		)
		{
			Add(db, new[] { document }, serviceFeeMode);
		}



		public virtual void Add<TProduct>(
			Domain db,
			IList<TProduct> documents,
			ServiceFeeMode serviceFeeMode = ServiceFeeMode.Separate,
			bool saveDocuments = true
		)
			where TProduct : Product
		{

			var changed = false;

			var orderIsClosed = !db.ClosedPeriod.IsOpened(IssueDate) && !AllowAddProductsInClosedPeriod;
			

			foreach (var document in documents)
			{

				if (document.Order != null && !Equals(document.Order, this))
					throw new DomainException(Exceptions.DocumentAlreadyInOrder, document, document.Order);

				if (!document.IsRefund && orderIsClosed)
					throw new DomainException(Exceptions.OrderIsClosed, document, Number);


				var items = db.OrderItem.New(document, serviceFeeMode);

				if (items.Yes())
				{

					foreach (var item in items)
					{
						AddOrderItem(db, item);
						changed = true;
						db.Save(item);
					}


					if (saveDocuments)
						db.OnCommit(this, document, r => db.Save(r));

				}

			}


			if (changed)
				RecalculateFinanceData(db);


			foreach (var doc in documents)
				doc.SetCustomer(db, Customer);


			if (changed)
				db.OnCommit(this, db.Order.Export);

		}



		public virtual void Remove(Domain db, Product product)
		{
			for (var index = Items.Count - 1; index >= 0; index--)
			{
				var item = Items[index];

				if (item.IsLinkedWith(product))
					RemoveOrderItem(db, item);
			}

			RecalculateFinanceData(db);

			db.Export(this);
		}



		public virtual void Recalculate(Domain db)
		{
			foreach (var item in Items)
				item.Recalculate(db);

			RecalculateFinanceData(db);
		}



		public virtual void RecalculateFinanceData(Domain db)
		{

			var currency = Total.AsCurrency() ?? db.Configuration.DefaultCurrency;

			db.Order.CalcFinanceData(Items, currency, out var total, out var discount, out var vat);


			SetTotal(total);
			SetDiscount(discount);
			SetVat(vat);

			db.Save(this);

		}



		public virtual void SetIsVoid(Domain db, bool value)
		{

			if (IsVoid == value)
				return;


			if (value)
			{
				ClearOrderReferences(db);
			}
			else
			{
				if (db.Order.CanRestore(this, out _))
					SetOrderReferences(db);
				else
					throw new DomainException(Exceptions.Order_CannotRestoreOrder_Msg, Number);
			}


			IsVoid = value;
			//Update(db);

		}



		//---g



		private bool _refreshPending;

		private Money _checkPaid;
		private Money _wirePaid;
		private Money _creditPaid;
		private Money _restPaid;
		private Money _paid;
		private decimal _deliveryBalance;
		private Money _totalDue;
		private Money _vatDue;

		private readonly IList<OrderItem> _items = new List<OrderItem>();
		private readonly IList<Invoice> _invoices = new List<Invoice>();
		private readonly IList<Payment> _payments = new List<Payment>();
		private readonly IList<Task> _tasks = new List<Task>();
		private readonly IList<InternalTransfer> _outgoingTransfers = new List<InternalTransfer>();
		private readonly IList<InternalTransfer> _incomingTransfers = new List<InternalTransfer>();



		//---g

	}






	//===g



}