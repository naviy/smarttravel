using System;
using System.Collections.Generic;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public partial class Product : Entity
	{

		public override Type GetClass() { return typeof(Product); }


		public string Name { get; set; }

		public Order Order { get; set; }

		public decimal FeeTotal { get; set; }

		public decimal ServiceFee { get; set; }

		public decimal Total { get; private set; }


		public override string ToString()
		{
			return "Product " + Name + " " + Total;
		}


		public override void Calculate()
		{
			Total = FeeTotal + ServiceFee;
		}

		public override void Flush(bool isDeleted)
		{
			db.SaveToList(this, db.Products, isDeleted);
		}

	}


	public partial class Order : Entity
	{

		public override Type GetClass() { return typeof(Order); }


		public string Number { get; set; }

		public decimal Total { get; private set; }

		public decimal Vat { get; private set; }

		public bool SeparateServiceFee { get; set; }


		public IList<OrderItem> Items { get; set; }


		public override string ToString()
		{
			return "Order " + Number;
		}


		public override Entity Clone(Domain domain)
		{
			var c = (Order)base.Clone(domain);

			c.Items = Items.Clone(domain);

			return c;
		}

		public override void Calculate()
		{
			Total = Items.AsSum(a => a.Total - a.DiscountTotal);
			Vat = Items.AsSum(a => a.Vat - a.DiscountVat);
		}

		public override void Flush(bool isDeleted)
		{
			db.SaveToList(this, db.Orders, isDeleted);
		}

	}


	public enum OrderItemLinkType
	{
		ProductData = 0,
		ServiceFee = 1,
		FullDocument = 2,
	}

	public partial class OrderItem : Entity
	{

		public override Type GetClass() { return typeof(OrderItem); }

	
		public Order Order { get; set; }

		public Product Product { get; set; }

		public virtual OrderItemLinkType? LinkType { get; set; }

		public string Text { get; set; }

		public decimal Total { get; set; }

		public decimal Vat { get; private set; }

		public decimal DiscountTotal { get; set; }

		public decimal DiscountVat { get; set; }

		public IList<OrderItemDiscount> Discounts { get; set; }


		public override string ToString()
		{
			return "OrderItem " + Order.As(a => a.Number) + " " + Text;
		}


		public override Entity Clone(Domain domain)
		{
			var c = (OrderItem)base.Clone(domain);

			c.Discounts = Discounts.Clone(domain);

			return c;
		}


		public override void Calculate()
		{
			if (LinkType == null)
				LinkType = OrderItemLinkType.FullDocument;

			if (Product != null)
			{
				if (LinkType == OrderItemLinkType.FullDocument)
					Total = Product.Total;
				else if (LinkType == OrderItemLinkType.ProductData)
					Total = Product.FeeTotal;
				else if (LinkType == OrderItemLinkType.ServiceFee)
					Total = Product.ServiceFee;
			}

			Vat = Total * 0.2m;

			DiscountTotal = Discounts.AsSum(a => a.Total);
			DiscountVat = Discounts.AsSum(a => a.Vat);
		}

		public override void Flush(bool isDeleted)
		{
			db.SaveToList(this, db.OrderItems, isDeleted);
		}

	}


	public partial class OrderItemDiscount : Entity
	{

		public override Type GetClass() { return typeof(OrderItemDiscount); }

		public OrderItem OrderItem { get; set; }

		public decimal Total { get; set; }

		public decimal Vat { get; private set; }


		public override string ToString()
		{
			return "OrderItemDiscount " + OrderItem.As(a => a.Order.As(b => b.Number) + " " + a.Text) + " " + Total;
		}


		public override void Calculate()
		{
			Vat = Total * 0.2m;
		}

		public override void Flush(bool isDeleted)
		{
			db.SaveToList(this, db.OrderItemDiscounts, isDeleted);
		}

	}


	public partial class Domain: Domain<Domain, Entity, object>
	{
		public List<Product> Products = new List<Product>();
		public List<Order> Orders = new List<Order>();
		public List<OrderItem> OrderItems = new List<OrderItem>();
		public List<OrderItemDiscount> OrderItemDiscounts = new List<OrderItemDiscount>();
	}

}
