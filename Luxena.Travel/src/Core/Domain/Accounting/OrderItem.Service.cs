using System.Collections.Generic;


namespace Luxena.Travel.Domain
{

	public enum ServiceFeeMode { Join, Separate, AlwaysJoin, AlwaysSeparate, }

	partial class OrderItem
	{

		public class Service : Entity2Service<OrderItem>
		{

			#region Read

			public IList<OrderItem> ListByOrderNumber(string orderNumber)
			{
				var order = db.Order.ByNumber(orderNumber);

				return order != null ? order.Items : EmptyList;
			}

			#endregion


			#region Modify

			public Service()
			{
				Calculating += r =>
				{
					if (r.Price == null && r.GrandTotal != null && r.Quantity == 0)
					{
						r.Price = r.GrandTotal.Clone();
						r.Quantity = 1;
					}
				};
			}


			public OrderItem New(Product product, OrderItemLinkType linkType)
			{
				var r = new OrderItem
				{
					Product = product,
					LinkType = linkType
				};

				r.Recalculate(db);

				return r;
			}

			public IList<OrderItem> New<TProduct>(IList<TProduct> documents, ServiceFeeMode serviceFeeMode)
				where TProduct : Product
			{
				var items = new List<OrderItem>();

				foreach (var doc in documents)
					items.AddRange(New(doc, serviceFeeMode));

				return items;
			}

			public IList<OrderItem> New(Product document, ServiceFeeMode serviceFeeMode)
			{
				var separate = serviceFeeMode == ServiceFeeMode.Separate || serviceFeeMode == ServiceFeeMode.AlwaysSeparate;

				if (serviceFeeMode != ServiceFeeMode.AlwaysJoin && serviceFeeMode != ServiceFeeMode.AlwaysSeparate)
				{
					switch (db.Configuration.AviaOrderItemGenerationOption)
					{
						case AviaOrderItemGenerationOption.AlwaysOneOrderItem:
							separate = false;
							break;

						case AviaOrderItemGenerationOption.SeparateServiceFee:
							separate = true;
							break;
					}
				}

				if (separate && document.ServiceFee != null && document.ServiceFee.Amount != 0)
				{
					return new[]
					{
						New(document, OrderItemLinkType.ProductData),
						New(document, OrderItemLinkType.ServiceFee)
					};
				}

				return new[]
				{
					New(document, OrderItemLinkType.FullDocument)
				};
			}


			#endregion


		}

	}

}