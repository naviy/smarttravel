//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Data.Entity;
//
//using Luxena.Domain;
//
//
//namespace Luxena.Travel.Domain
//{
//
////	public partial class Product : Entity2
////	{
////
////		public string Name { get; set; }
////
////		public Order Order { get; set; }
////
////		public ICollection<OrderItem> OrderItems { get; set; }
////
////		public decimal FeeTotal { get; set; }
////
////		public decimal ServiceFee { get; set; }
////
////		public decimal Total { get; private set; }
////
////
////		public override string ToString()
////		{
////			return "Product " + Name + " " + Total;
////		}
////
////
////		protected override void Calculate()
////		{
////			base.Calculate();
////
////			Total = FeeTotal + ServiceFee;
////		}
////
////	}
//
//
//	public partial class Order : Entity2
//	{
//
//		[EntityName]
//		public string Number { get; set; }
//
//		public Money Total { get; set; }
//
//		public Money Vat { get; set; }
//
//
//		public ICollection<OrderItem> Items { get; set; }
//
//		public ICollection<Product> Products { get; set; }
//
//
//		public override string ToString()
//		{
//			return "Order " + Number;
//		}
//
//
////		protected override Domain.Entity Clone()
////		{
////			var c = (Order)base.Clone();
////
////			c.Items = Items.Clone(db);
////
////			return c;
////		}
////
////		protected override void Calculate()
////		{
////			base.Calculate();
////
////			Total = Items.AsSum(a => a.Total - a.DiscountTotal);
////			Vat = Items.AsSum(a => a.Vat - a.DiscountVat);
////		}
//
//	}
//
//
//	public enum OrderItemLinkType
//	{
//		ProductData = 0,
//		ServiceFee = 1,
//		FullDocument = 2,
//	}
//
//	public partial class OrderItem : Entity2
//	{
//
//		[Required]
//		public virtual Order Order { get; set; }
//
//		public virtual Product Product { get; set; }
//
//		public OrderItemLinkType? LinkType { get; set; }
//
//		public string Text { get; set; }
//
//		public Money Total { get; set; }
//
//		public Money Vat { get; private set; }
//
//		public Money DiscountTotal { get; set; }
//
//		public Money DiscountVat { get; set; }
//
//
//
//		static partial void Config_(Domain.EntityConfiguration<OrderItem> entity)
//		{
//			entity.Association(a => a.Order, a => a.Items);
//			entity.Association(a => a.Product, a => a.OrderItems);
//		}
//
//		public override string ToString()
//		{
//			return "OrderItem " + Order.As(a => a.Number) + " " + Text;
//		}
//
//		//protected override void Calculate()
//		//{
//		//	base.Calculate();
//
//		//	if (LinkType == null)
//		//		LinkType = OrderItemLinkType.FullDocument;
//
//		//	if (Product != null)
//		//	{
//		//		if (LinkType == OrderItemLinkType.FullDocument)
//		//			Total = Product.Total;
//		//		else if (LinkType == OrderItemLinkType.ProductData)
//		//			Total = Product.FeeTotal;
//		//		else if (LinkType == OrderItemLinkType.ServiceFee)
//		//			Total = Product.ServiceFee;
//		//	}
//
//		//	Vat = Total * 0.2m;
//
//		//	DiscountTotal = Discounts.AsSum(a => a.Total);
//		//	DiscountVat = Discounts.AsSum(a => a.Vat);
//		//}
//
//	}
//
//
//
//	partial class Domain
//	{
//		public DbSet<Order> Orders { get; set; }
//		public DbSet<OrderItem> OrderItems { get; set; }
//	}
//
//}
