namespace Luxena.Travel.Domain
{

	[GenericPrivileges(Copy = new object[] { })]
	public partial class OrderItem : Entity2
	{
		[ReadOnly]
		public virtual Order Order { get; set; }

		public virtual Product Product { get; set; }

		[ReadOnly]
		public virtual int Position { get; set; }

		[EntityName]
		public virtual string Text { get; set; }

		public virtual OrderItemLinkType? LinkType { get; set; }

		public virtual Money Price { get; set; }

		public virtual int Quantity { get; set; }

		public virtual Money Total => Price * Quantity;

		public virtual Money Discount { get; set; }

		public virtual Money GrandTotal { get; set; }

		public virtual Money GivenVat { get; set; }

		public virtual Money TaxedTotal { get; set; }

		public virtual bool HasVat { get; set; }

		public virtual Money ServiceFee
		{
			get
			{
				if (Product == null || IsServiceFee)
					return null;

				return Product.ServiceFee;
			}
		}


		public virtual Consignment Consignment { get; set; }

		public virtual bool IsDelivered => Product != null && Product.IsDelivered;

		public virtual bool IsLinkedWith(Entity2 entity)
		{
			return Equals(Product, entity);
		}

		public virtual void Recalculate(Domain db)
		{
			if (Quantity == 0)
				Quantity = 1;

			if (Product == null) return;

			Product = db.Unproxy(Product);

			switch (LinkType)
			{
				case OrderItemLinkType.ProductData:

					Text = Product.TextForOrderItem;

					var price1 = Product.Total;

					if (db.Configuration.UseAviaHandling)
					{
						price1 += Product.Handling - Product.HandlingN;
					}

					Price = price1.Clone();

					if (Product.ServiceFee != null && Product.ServiceFee.Amount != 0)
					{
						GrandTotal = price1.Clone();
						Discount = new Money(Product.Total.Currency);
					}
					else
					{
						GrandTotal = Product.GrandTotal.Clone();
						Discount = Product.Discount.Clone();
					}

					GivenVat = CalculateGivenVat(db);

					TaxedTotal = new Money(Product.Total.Currency);

					HasVat = GivenVat.Amount > 0;

					break;

				case OrderItemLinkType.ServiceFee:

					var serviceFee = !Product.IsRefund ? Product.ServiceFee : Product.ServiceTotal;

					Text = CommonRes.OrderItem_AviaServiceFeeSource;
					Price = serviceFee.Clone();
					Discount = Product.Discount.Clone();
					GrandTotal = Product.ExtraCharge.Clone();

					if (db.Configuration.UseAviaHandling)
					{
						GrandTotal -= Product.Handling - Product.HandlingN;

						Discount += Product.CommissionDiscount;
					}

					TaxedTotal = serviceFee.Clone();

					GivenVat = new Money(Product.Total.Currency);

					HasVat = true;

					break;

				case OrderItemLinkType.FullDocument:

					var price = Product.GrandTotal;

					if (Product.Discount != null)
						price += Product.Discount;

					Text = Product.TextForOrderItem;
					Price = price.Clone();
					Discount = Product.Discount.Clone();
					GrandTotal = Product.GrandTotal.Clone();

					if (Product.Total.Yes())
					{
						GivenVat = CalculateGivenVat(db);

						TaxedTotal = new Money(Product.Total.Currency);

						if (Product.ServiceFee != null)
							TaxedTotal += Product.ServiceFee.Clone();

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

		public virtual void SetOrderReference(Domain db)
		{
			if (Product == null) return;

			Product.SetOrder(db, Order);
			db.OnCommit(Order, Product, r => db.Save(r));
		}

		public virtual void ClearOrderReference(Domain db)
		{
			if (Product != null && Equals(Order, Product.Order) &&
				(!IsServiceFee || Product.Order.ItemsBy(Product, a => !a.IsServiceFee).No()))
			{
				Product.SetOrder(db, null);
				db.OnCommit(Product, r => db.Save(r));
			}
		}

		public override string ToString()
		{
			return Order + " #" + Position;
		}

		
		private Money CalculateGivenVat(Domain db)
		{
			if (db.Configuration.UseAviaDocumentVatInOrder && Product.Vat != null)
				return Product.Vat.Clone();

			return new Money(Product.Total.AsCurrency());
		}

	}

}