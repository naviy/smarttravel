using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуга", "Все услуги"), Icon("suitcase"), Big]
	[AgentPrivileges]
	[DebuggerDisplay("{Type} {Name}")]
	public abstract partial class Product : Entity2
	{

		//[SemanticSetup]
		//public static void SemanticSetup(SemanticSetup se)
		//{
		//}


		[EntityType]
		public virtual ProductType Type { get; set; }

		[Patterns.Name, EntityName, Length(16)]
		public string Name { get; set; }

		[Patterns.IssueDate, EntityDate]
		public DateTimeOffset IssueDate { get; set; }

		[RU("Продюсер")]
		protected Organization _Producer;

		[RU("Провайдер")]
		protected Organization _Provider;

		[Patterns.ReissueFor]
		protected Product _ReissueFor;

		[RU("Исходный документ")]
		protected Product _RefundedProduct;


		#region Status

		[RU("Это возврат")]
		public virtual bool IsRefund { get { return false; } set { } }

		public virtual bool IsReservation { get { return false; } set { } }

		[RU("Обработан")]
		public bool IsProcessed { get; set; }

		[NotMapped]
		public bool MustBeUnprocessed { get; set; }

		[Patterns.IsVoid]
		public bool IsVoid { get; set; }

		[RU("К обработке")]
		public bool RequiresProcessing { get { return !IsProcessed && !IsVoid; } set { } }

		public bool IsDelivered => !IsReservation && !IsVoid;


		[Patterns.IsPaid]
		public bool IsPaid => Order != null && Order.IsPaid;

		#endregion

		[RU("Маршрут"), Length(16)]
		public string Itinerary { get; set; }


		[Customer]
		protected Party _Customer;

		[Subject]
		protected Order _Order;

		public IEnumerable<OrderItem> GetOrderItems() => Order?.Items?.Where(a => a.Product == this);

		[RU("Посредник")]
		protected Party _Intermediary;


		[Patterns.StartDate]
		public DateTimeOffset? StartDate { get; set; }

		[Patterns.FinishDate]
		public DateTimeOffset? FinishDate { get; set; }


		protected Country _Country;

		[RU("Бронировка")]
		public string PnrCode { get; set; }

		[RU("Туркод")]
		public string TourCode { get; set; }


		[RU("Бронировщик"), Agent]
		protected Person _Booker;

		[RU("Бронировщик: код офиса GDS-агента", ruShort: "код офиса"), Length(8)]
		public string BookerOffice { get; set; }

		[RU("Бронировщик: код GDS-агента", ruShort: "код агента"), Length(8)]
		public string BookerCode { get; set; }


		[RU("Тикетер"), Agent]
		protected Person _Ticketer;

		[RU("Тикетер: код офиса GDS-агента", ruShort: "код офиса"), Length(8)]
		public string TicketerOffice { get; set; }

		[RU("Тикетер: код GDS-агента", ruShort: "код агента"), Length(8)]
		public string TicketerCode { get; set; }

		[RU("IATA офис"), MaxLength(10)]
		public string TicketingIataOffice { get; set; }

		public bool IsTicketerRobot { get; set; }


		[Patterns.Seller, Agent]
		protected Person _Seller;

		[ActiveOwner]
		protected Party _Owner;


		#region Finance

		[RU("Тариф")]
		public Money Fare { get; set; }

		[RU("Экв. тариф"), DefaultMoney, Subject]
		public Money EqualFare { get; set; }

		[RU("Таксы"), DefaultMoney, Subject]
		public Money FeesTotal { get; set; }

		[RU("Штраф за отмену"), DefaultMoney, Subject]
		public Money CancelFee { get; set; }

		[RU("К перечислению провайдеру"), DefaultMoney]
		public Money Total { get; protected set; }


		[Patterns.Vat, DefaultMoney]
		public Money Vat { get; set; }

		[RU("Сервисный сбор"), DefaultMoney, Subject]
		public Money ServiceFee { get; set; }

		[RU("Штраф сервисного сбора"), DefaultMoney, Subject]
		public Money ServiceFeePenalty { get; set; }

		[RU("Доп. доход"), DefaultMoney, Subject]
		public Money Handling { get; set; }

		[RU("Комиссия"), DefaultMoney]
		public Money Commission { get; set; }

		[RU("Скидка от комиссии"), DefaultMoney, Subject]
		public Money CommissionDiscount { get; set; }

		[RU("Скидка"), DefaultMoney, Subject]
		public Money Discount { get; set; }


		[RU("Бонусная скидка"), DefaultMoney, Subject]
		public Money BonusDiscount { get; set; }

		[RU("Бонусное накопление"), DefaultMoney]
		public Money BonusAccumulation { get; set; }


		[RU("Cбор за возврат"), DefaultMoney, Subject]
		public Money RefundServiceFee { get; set; }

		public Money ServiceTotal =>
			ServiceFee - RefundServiceFee - ServiceFeePenalty;


		[RU("К оплате"), DefaultMoney]
		public Money GrandTotal { get; set; }


		public decimal? CancelCommissionPercent { get; set; }

		[RU("Комисия за возврат")]
		public Money CancelCommission { get; set; }

		[RU("% комиссии")]
		public decimal? CommissionPercent { get; set; }


		public Money TotalToTransfer =>
			Total - Commission + CancelCommission;

		public Money Profit =>
			GrandTotal - TotalToTransfer;

		public Money ExtraCharge =>
			GrandTotal - Total;


		private void CalculateFinance()
		{
			var currency = EqualFare?.CurrencyId;
			if (currency.Yes())
			{
				FeesTotal += currency;
				CancelFee += currency;
				ServiceFee += currency;
				Handling += currency;
				Vat += currency;
				Commission += currency;
				CommissionDiscount += currency;
				Discount += currency;
				BonusDiscount += currency;
				RefundServiceFee += currency;
				ServiceFeePenalty += currency;
			}

			Total = EqualFare + FeesTotal - CancelFee;

			GrandTotal = Total + ServiceFee + Handling
				- CommissionDiscount - Discount - BonusDiscount
				- RefundServiceFee - ServiceFeePenalty;
		}

		#endregion


		public PaymentType PaymentType { get; set; }

		[RU("Ставка НДС для услуги")]
		public virtual TaxRate TaxRateOfProduct { get; set; }

		[RU("Ставка НДС для сбора")]
		public virtual TaxRate TaxRateOfServiceFee { get; set; }

		[Patterns.Note]
		public string Note { get; set; }




		[RU("Оригинатор")]
		public GdsOriginator Originator { get; set; }

		[RU("Источник")]
		public ProductOrigin Origin { get; set; }

		[RU("Оригинальный документ")]
		protected GdsFile _OriginalDocument;


		public static implicit operator string (Product me) => me?.Name;

		public override string ToString() => Name;


		//protected override Domain.Entity Clone()
		//{
		//	var c = (Product)base.Clone();

		//	c.Fare = Fare.Clone();
		//	c.EqualFare = EqualFare.Clone();
		//	c.Commission = Commission.Clone();
		//	c.FeesTotal = FeesTotal.Clone();
		//	c.Vat = Vat.Clone();
		//	c.Total = Total.Clone();
		//	c.ServiceFee = ServiceFee.Clone();
		//	c.Handling = Handling.Clone();
		//	c.CommissionDiscount = CommissionDiscount.Clone();
		//	c.Discount = Discount.Clone();

		//	c.CancelFee = CancelFee.Clone();
		//	c.CancelCommission = CancelCommission.Clone();
		//	c.ServiceFeePenalty = ServiceFeePenalty.Clone();
		//	c.RefundServiceFee = RefundServiceFee.Clone();

		//	c.GrandTotal = GrandTotal.Clone();

		//	c.Passengers = Passengers.Clone(db);

		//	return c;
		//}


		#region Passenger

		[Patterns.Passenger]
		public string PassengerName { get; set; }

		public string GetPassengerNames()
		{
			return Passengers.No() ? null : string.Join(", ", Passengers.Select(a => a.PassengerName).OrderBy(a => a));
		}


		[RU("Пассажир из GDS"), Length(20)]
		[NotDbMapped]
		public string GdsPassengerName { get { return _gdsPassengerName.Get(this); } set { _gdsPassengerName.Set(value); } }
		private readonly Lazy<string> _gdsPassengerName = NewLazy(r => r.Passengers.OneAs(a => a.PassengerName));

		[NotDbMapped]
		public string PassengerId { get { return _passengerId.Get(this); } set { _passengerId.Set(value); } }
		private readonly Lazy<string> _passengerId = NewLazy(r => r.Passengers.OneAs(a => a.PassengerId));


		[Patterns.Passenger]
		public Person Passenger { get { return Passengers.OneAs(a => a.Passenger); } }


		private void BindPassenger()
		{
			if (!_passengerId && !_gdsPassengerName) return;

			var psg = Passengers.One() ?? new ProductPassenger();

			psg.Product = this;
			psg.PassengerId = _passengerId;
			psg.PassengerName = _gdsPassengerName;

			psg.Save(db);
		}

		private void CalculatePassenger()
		{
			PassengerName = Passengers.No() ? null :
				Passengers.Select(a =>
					new[] { a.PassengerName, (a.Passenger ?? db.Persons.ById(a.PassengerId))?.Name }.Where(b => b.Yes()).Join(" / ")
				)
				.OrderBy(a => a)
				.Join(", ");
		}

		#endregion


		public virtual ICollection<ProductPassenger> Passengers { get; set; }

		public virtual ICollection<Product> Products_ReissueFor { get; set; }

		public virtual ICollection<Product> Products_RefundedProduct { get; set; }



		static partial void Config_(Domain.EntityConfiguration<Product> entity)
		{
			entity.Association(a => a.Producer);//, a => a.Products_Producer);
			entity.Association(a => a.Provider);//, a => a.Products_Provider);
			entity.Association(a => a.ReissueFor, a => a.Products_ReissueFor);
			entity.Association(a => a.RefundedProduct, a => a.Products_RefundedProduct);
			entity.Association(a => a.Customer);//, a => a.Products_Customer);
			entity.Association(a => a.Order, a => a.Products);
			entity.Association(a => a.Intermediary);//, a => a.Products_Intermediary);
			entity.Association(a => a.Country);//, a => a.Products);
			entity.Association(a => a.Booker);//, a => a.Products_Booker);
			entity.Association(a => a.Ticketer);//, a => a.Products_Ticketer);
			entity.Association(a => a.Seller);//, a => a.Products_Seller);
			entity.Association(a => a.Owner);//, a => a.Products_Owner);
			entity.Association(a => a.OriginalDocument, a => a.Products);
		}


		public override void CalculateDefaults()
		{
			IssueDate = DateTime.Today;
			EqualFare = new Money("UAH");
		}


		protected override IList<Domain.Entity> GetDependents() =>
			new Domain.Entity[] { Order }.AsConcat(GetOrderItems()).ToList();

		protected override void Bind()
		{
			base.Bind();

			BindPassenger();
			//BindCustomer();
			BindOrder();
		}

		void BindOrder()
		{
			if (n.Order != o.Order)
			{
				// Удаляем позиции на эту услугу из старого заказа
				o.GetOrderItems().Delete(db);
			}


			#region Пересоздание OrderItems, если их кол-во нужно изменить

			if (n.Order == null) return;

			if (n.Order.Items == null)
				n.Order.Items = new List<OrderItem>();

			var items = n.GetOrderItems().ToArray();

			if (items.No() ||
				n.ServiceFee && items.All(a => !a.IsServiceFee) ||
				!n.ServiceFee && items.Any(a => a.IsServiceFee))
			{
				var position = items.Yes() ? items.Min(a => a.Position) : 0;
				items.Delete(db);
				CreateOrderItems(n, position: position).Save(db);
			}

			#endregion
		}

		//void BindCustomer()
		//{
		//}

		public override void Calculate()
		{
			base.Calculate();

			if (Order != null)
				Customer = Order.Customer;

			Customer
				.If(a => !a.IsCustomer)
				.Update(db, a => a.IsCustomer = true);

			CalculatePassenger();
			CalculateFinance();
		}

		public IEnumerable<OrderItem> CreateOrderItems(Product r, ServiceFeeMode? serviceFeeMode = null, int position = 0)
		{
			var separate =
				serviceFeeMode == null && r.Order.SeparateServiceFee != false ||
				serviceFeeMode == ServiceFeeMode.Separate ||
				serviceFeeMode == ServiceFeeMode.AlwaysSeparate;

			if (serviceFeeMode != ServiceFeeMode.AlwaysJoin && serviceFeeMode != ServiceFeeMode.AlwaysSeparate)
			{
				switch (db.AppConfiguration.AviaOrderItemGenerationOption)
				{
					case ProductOrderItemGenerationOption.AlwaysOneOrderItem:
						separate = false;
						break;

					case ProductOrderItemGenerationOption.SeparateServiceFee:
						separate = true;
						break;
				}
			}

			if (separate && r.ServiceFee)
			{
				yield return new OrderItem { Order = r.Order, Product = r, LinkType = OrderItemLinkType.ProductData, Position = position };
				yield return new OrderItem { Order = r.Order, Product = r, LinkType = OrderItemLinkType.ServiceFee, Position = position == 0 ? 0 : position + 1 };
			}
			else
				yield return new OrderItem { Order = r.Order, Product = r, Position = position };
		}

		public virtual string GetOrderItemText(string lang) =>
			Localization(lang) + GetOrderItemText2(lang);

		protected string GetOrderItemText2(string lang) =>
			(IsReservation ? null : $" {Name} {Texts.From[lang]} {IssueDate.ToDateString()},{Country.As(a => " " + a.Name + ",")}") +
			GetPassengerNames().As(a => " " + a + ", ");

	}


	partial class Domain
	{
		public DbSet<Product> Products { get; set; }
	}

}