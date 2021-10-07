using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Внутренний перевод", "Внутренние переводы")]
	[SupervisorPrivileges]
	public partial class InternalTransfer : Entity2
	{

		[Patterns.Number, EntityName]
		public virtual string Number { get; set; }

		[Patterns.Date, EntityDate]
		public virtual DateTime Date { get; set; }

		[RU("Из заказа")]
		public virtual Order FromOrder
		{
			get { return _fromOrder; }
			set { WrapSetter(() => SetFromOrder(value)); }
		}

		[RU("От контрагента"), Suggest(typeof(Customer)), Required]
		public virtual Party FromParty
		{
			get { return _fromParty; }
			set
			{
				if (Equals(_fromParty, value))
					return;

				_fromParty = value;

				if (_fromOrder != null)
					SetFromOrder(null);
			}
		}

		[RU("В заказ")]
		public virtual Order ToOrder
		{
			get { return _toOrder; }
			set { WrapSetter(() => SetToOrder(value)); }
		}

		[RU("К контрагенту"), Suggest(typeof(Customer)), Required]
		public virtual Party ToParty
		{
			get { return _toParty; }
			set
			{
				if (Equals(_toParty, value))
					return;

				_toParty = value;

				if (_toOrder != null)
					SetToOrder(null);
			}
		}

		[RU("Сумма")]
		public virtual decimal Amount
		{
			get { return _amount; }
			set
			{
				if (_amount == value)
					return;

				_amount = value;

				_fromOrder?.Refresh();

				_toOrder?.Refresh();
			}
		}

		public virtual void Refresh()
		{
			if (_fromOrder != null)
				_fromParty = _fromOrder.Customer;

			if (_toOrder != null)
				_toParty = _toOrder.Customer;
		}

		private void SetFromOrder(Order value)
		{
			if (Equals(_fromOrder, value))
				return;

			_fromOrder?.RemoveOutgoingTransfer(this);

			_fromOrder = value;

			if (_fromOrder == null)
				return;

			_fromParty = _fromOrder.Customer;

			_fromOrder.AddOutgoingTransfer(this);
		}

		private void SetToOrder(Order value)
		{
			if (Equals(_toOrder, value))
				return;

			_toOrder?.RemoveIncomingTransfer(this);

			_toOrder = value;

			if (_toOrder == null)
				return;

			_toParty = _toOrder.Customer;

			_toOrder.AddIncomingTransfer(this);
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

		private Party _fromParty;
		private Order _fromOrder;
		private Party _toParty;
		private Order _toOrder;
		private decimal _amount;
		private bool _updating;
	}

}