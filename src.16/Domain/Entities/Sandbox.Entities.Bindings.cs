//using System.Collections.Generic;
//using System.Linq;

//using Luxena.Domain;


//namespace Luxena.Travel.Domain
//{

//	partial class Product
//	{

//		protected override IList<Domain.Entity> GetDependents()
//		{
//			IList<Domain.Entity> dependents = new[] { Order };

//			if (Order != null && Order.Items != null)
//				dependents = dependents.Concat(Order.Items.Where(a => a.Product == this)).ToList();

//			return dependents;
//		}

//		protected override void Bind()
//		{
//			base.Bind();

//			if (n.Order != o.Order)
//			{
//				if (o.Order != null && o.Order.Items != null)
//				{
//					var orderItems = o.Order.Items.Where(a => a.Product == this).ToList();
//					orderItems.Delete(db);
//				}

//				if (n.Order != null)
//				{
//					if (n.Order.Items == null)
//						n.Order.Items = new List<OrderItem>();

//					if (n.Order.Items.All(a => a.Product != this))
//						CreateOrderItems(n).Save(db);
//				}
//			}
//		}

//		static IEnumerable<OrderItem> CreateOrderItems(Product r)
//		{
//			var item1 = new OrderItem { Order = r.Order, Product = r };

//			if (r.Order.SeparateServiceFee)
//			{
//				item1.LinkType = OrderItemLinkType.ProductData;
//				yield return item1;

//				yield return new OrderItem { Order = r.Order, Product = r, LinkType = OrderItemLinkType.ServiceFee, };
//			}
//			else
//				yield return item1;
//		}

//	}


//	partial class OrderItem
//	{

//		protected override IList<Domain.Entity> GetDependents()
//		{
//			return new[] { Order };
//		}

//		protected override void Bind()
//		{
//			base.Bind();

//			ModifyMaster<Order, OrderItem>(
//				a => a.Order, (a, value) => a.Order = value,
//				a => a.Items, (a, value) => a.Items = value
//			);

//			if (n.Order == null)
//			{
//				this.Delete(db);
//			}


//			if (n.Product != o.Product)
//			{
//				if (o.Product != null && o.Product.Order != null)
//				{
//					o.Product.Order = null;
//					o.Product.Save(db);
//				}

//				if (n.Product != null && n.Product.Order != n.Order)
//				{
//					n.Product.Order = n.Order;
//					n.Product.Save(db);
//				}
//			}
//		}

//	}


//	partial class OrderItemDiscount
//	{

//		protected override IList<Domain.Entity> GetDependents()
//		{
//			return new[] { OrderItem };
//		}

//		protected override void Bind()
//		{
//			base.Bind();

//			ModifyMaster<OrderItem, OrderItemDiscount>(
//				a => a.OrderItem, (a, value) => a.OrderItem = value,
//				a => a.Discounts, (a, value) => a.Discounts = value
//			);
//		}

//	}



//}
