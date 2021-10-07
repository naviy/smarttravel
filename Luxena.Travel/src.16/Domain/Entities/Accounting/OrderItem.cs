using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Позиция заказа", "Позиции заказа")]
	[AgentPrivileges]
	[DebuggerDisplay("{Order} #{Position} ({_objectIndex})  - {Product}.{LinkType.ToString()}")]
	public partial class OrderItem : Entity2
	{

		protected Order _Order;

		protected Product _Product;

		[EntityPosition, Patterns.Number]
		public int Position { get; set; }

		[Patterns.Name, EntityName, Text]
		public string Text { get; set; }

		[RU("Тип")]
		public OrderItemLinkType? LinkType { get; set; }

		[Patterns.Price]
		public Money Price { get; set; }

		[Patterns.Quantity]
		public int Quantity { get; set; }

		[Patterns.Total]
		public Money Total => Price * Quantity;

		[Patterns.Discount]
		public Money Discount { get; set; }

		[RU("К оплате")]
		public Money GrandTotal { get; protected set; }

		public Money GivenVat { get; protected set; }

		public Money TaxedTotal { get; protected set; }

		public bool HasVat { get; set; }

		public Money ServiceFee => 
			Product == null || IsServiceFee ? null : Product.ServiceFee;

		protected Consignment _Consignment;

		public bool IsDelivered => Product?.IsDelivered ?? false;


		private static int _objectCount = 0;
		private readonly int _objectIndex = ++_objectCount;

		public override string ToString()
		{
			return $"{Order} #{Position} ({_objectIndex})";
		}


		public string CheckNameUA
		{
			get
			{
				var p = Product;
				if (p == null || IsServiceFee) return Text;

				var type = Product.ProductTypesByRefundType.By(p.Type) ?? p.Type;

				var title = type.EnumLocalization().Ukrainian.OneOrDefault;


				switch (type)
				{
					case ProductType.AviaTicket:
					case ProductType.AviaMco:

						if (p.Originator == GdsOriginator.Unknown)
							return title + p.Country.As(a => " " + a.Name) +
								(p.PassengerName.As(a => " " + a) ?? p.Passenger.As(a => " " + a.Name));

						return
							(p.IsAviaTicket && p.IsReservation ? "Бронь авіаквитка" : title) +
							(p.Producer ?? p.Provider).As(a => " " + a.AirlineIataCode) +
							p.As((AviaDocument d) => " " + d.Number);

					case ProductType.Accommodation:
					case ProductType.Tour:
						return title + p.Country.As(a => " " + a.Name) +
							(p.PassengerName.As(a => " " + a) ?? p.Passenger.As(a => " " + a.Name));

					case ProductType.CarRental:
					case ProductType.GenericProduct:
					case ProductType.Transfer:
						return p.Name + p.Country.As(a => " " + a.Name) +
							(p.PassengerName.As(a => " " + a) ?? p.Passenger.As(a => " " + a.Name));

					default:
						return title + " " + p.Name;
				}
			}
		}


		static partial void Config_(Domain.EntityConfiguration<OrderItem> entity)
		{
			entity.Association(a => a.Order, a => a.Items);
			entity.Association(a => a.Product);//, a => a.OrderItems);
			entity.Association(a => a.Consignment, a => a.OrderItems);
		}


		protected override IList<Domain.Entity> GetDependents()
		{
			return new[] { Order };
		}

		protected override void Bind()
		{
			base.Bind();

			ModifyMaster<Order, OrderItem>(a => a.Order, a => a.Items);

			if (n.Order == null)
			{
				this.Delete(db);
			}

			var newProduct = n.Product;
			var oldProduct = o.Product;

			if (newProduct != oldProduct)
			{
				if (oldProduct?.Order != null)
					oldProduct.Update(db, a => a.Order = null);

				if (newProduct != null && newProduct.Order != n.Order)
					newProduct.Update(db, a => a.Order = n.Order);
			}
		}

		public override void Calculate()
		{
			base.Calculate();

			if (Quantity == 0)
				Quantity = 1;


			if (Product == null)
			{
				TaxedTotal = Total;
				GrandTotal = TaxedTotal - Discount;
				return;
			}

			const string lang = "ua";

			var currency = Product.Total?.CurrencyId ?? db.DefaultCurrency;

			if (LinkType == null)
				LinkType = OrderItemLinkType.FullDocument;

			switch (LinkType)
			{
				case OrderItemLinkType.ProductData:

					Text = Product.GetOrderItemText(lang);

					Price = Product.Total + Product.Handling.If(a => db.AppConfiguration.UseAviaHandling) + currency;

					if (Product.ServiceFee != null && Product.ServiceFee.Amount != 0)
					{
						GrandTotal = Price + currency;
						Discount = currency;
					}
					else
					{
						GrandTotal = Product.GrandTotal + currency;
						Discount = Product.Discount + currency;
					}

					GivenVat = Product.Vat.If(a => db.AppConfiguration.UseAviaDocumentVatInOrder) + currency;
					TaxedTotal = currency;
					HasVat = GivenVat.Amount > 0;

					break;

				case OrderItemLinkType.ServiceFee:

					var serviceFee = !Product.IsRefund ? Product.ServiceFee : Product.ServiceTotal;

					Text = Texts.ServiceFee[lang];
					Price = serviceFee + currency;
					Discount = Product.Discount + currency;
					GrandTotal = Product.ExtraCharge + currency;

					if (db.AppConfiguration.UseAviaHandling)
					{
						GrandTotal -= Product.Handling;
						Discount += Product.CommissionDiscount;
					}

					TaxedTotal = serviceFee + currency;
					GivenVat = currency;
					HasVat = true;

					break;

				case OrderItemLinkType.FullDocument:

					Text = Product.GetOrderItemText(lang);
					Price = Product.GrandTotal + Product.Discount + currency;
					Discount = Product.Discount + currency;
					GrandTotal = Product.GrandTotal + currency;

					if (Product.Total)
					{
						GivenVat = Product.Vat.If(a => db.AppConfiguration.UseAviaDocumentVatInOrder) + currency;
						TaxedTotal = Product.ServiceFee + currency;
						HasVat = GivenVat.Amount > 0 || TaxedTotal.Amount > 0;
					}

					break;
			}

			if (Product.IsRefund)
			{
				Price *= -1;
				Discount *= -1;
				GrandTotal *= -1;
				GivenVat *= -1;
				TaxedTotal *= -1;
			}
		}


	}


	partial class Domain
	{
		public DbSet<OrderItem> OrderItems { get; set; }
	}

}