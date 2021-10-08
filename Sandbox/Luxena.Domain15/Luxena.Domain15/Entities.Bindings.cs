using System.Collections.Generic;
using System.Linq;


namespace Luxena.Travel.Domain
{
	
	partial class Product
	{

		public override IList<Entity> GetDependents()
		{
			IList<Entity> dependents = new[] { Order };

			if (Order != null && Order.Items != null)
				dependents = dependents.Concat(Order.Items.Where(a => a.Product == this)).ToList();

			return dependents;
		}

		public override void Bind()
		{
			var newOrder = ((Product)_new).Order;
			var oldOrder = ((Product)_old).Order;

			
			if (newOrder != oldOrder)
			{
				if (oldOrder != null && oldOrder.Items != null)
				{
					var orderItems = oldOrder.Items.Where(a => a.Product == this).ToList();
					orderItems.Delete(db);
				}

				if (newOrder != null)
				{
					if (newOrder.Items == null)
						newOrder.Items = new List<OrderItem>();

					if (newOrder.Items.All(a => a.Product != this))
						CreateOrderItems(db, (Product)_new).Save(db);
				}
			}
		}

		static IEnumerable<OrderItem> CreateOrderItems(Domain db, Product r)
		{
			var item1 = new OrderItem { Order = r.Order, Product = r };

			if (r.Order.SeparateServiceFee)
			{
				item1.LinkType = OrderItemLinkType.ProductData;
				yield return item1;

				yield return new OrderItem { Order = r.Order, Product = r, LinkType = OrderItemLinkType.ServiceFee, };
			}
			else
				yield return item1;
		}

	}

	partial class OrderItem
	{

		public override IList<Entity> GetDependents()
		{
			return new[] { Order };
		}

		public override void Bind()
		{
			ModifyMaster<Order, OrderItem>(
				a => a.Order, (a, value) => a.Order = value,
				a => a.Items, (a, value) => a.Items = value
			);

			var n = (OrderItem)_new;
			var o = (OrderItem)_old;

			if (n.Product != o.Product)
			{
				if (o.Product != null && o.Product.Order != null)
				{
					o.Product.Order = null;
					o.Product.Save(db);
				}
				if (n.Product != null && n.Product.Order != Order)
				{
					n.Product.Order = Order;
					n.Product.Save(db);
				}
			}
		}

	}


	partial class OrderItemDiscount
	{

		public override IList<Entity> GetDependents()
		{
			return new[] { OrderItem };
		}

		public override void Bind()
		{
			ModifyMaster<OrderItem, OrderItemDiscount>(
				a => a.OrderItem, (a, value) => a.OrderItem = value,
				a => a.Discounts, (a, value) => a.Discounts = value
			);
		}

	}



}
