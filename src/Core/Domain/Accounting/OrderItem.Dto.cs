using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Serialization;
using Luxena.Domain.Contracts;
using Luxena.Travel.Services;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class OrderItemDto : EntityContract
	{
		public OrderItemDto() { }

		public OrderItemDto(Order.Reference order, Product.Reference product)
		{
			Order = order;
			Product = product;
		}


		public Order.Reference Order { get; set; }

		public Consignment.Reference Consignment { get; set; }

		public Product.Reference Product { get; set; }

		public OrderItemLinkType? LinkType { get; set; }


		public string Text { get; set; }

		public string ProductText { get; set; }

		public MoneyDto Price { get; set; }

		public int Quantity { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto Discount { get; set; }

		public MoneyDto GrandTotal { get; set; }

		public MoneyDto GivenVat { get; set; }

		public MoneyDto TaxedTotal { get; set; }

		public bool HasVat { get; set; }

	}


	public partial class OrderItemContractService : EntityContractService<OrderItem, OrderItem.Service, OrderItemDto>
	{
		public OrderItemContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Order = r.Order;
				c.Consignment = r.Consignment;

				c.Product = r.Product;
				c.LinkType = r.LinkType;

				c.Text = r.Text;
				c.ProductText = r.Product?.TextForOrderItem;

				c.Price = r.Price;
				c.Quantity = r.Quantity;
				c.Total = r.Total;
				c.Discount = r.Discount;
				c.GrandTotal = r.GrandTotal;
				c.GivenVat = r.GivenVat;
				c.TaxedTotal = r.TaxedTotal;

				c.HasVat = r.HasVat;
			};

			EntityFromContract += (r, c) =>
			{
				r.Product = c.Product + db;
				r.LinkType = c.LinkType + db;

				r.Text = c.Text + db;
				r.Price = c.Price + db;
				r.Quantity = c.Quantity + db;
				r.Discount = c.Discount + db;
				r.GrandTotal = c.GrandTotal + db;
				r.GivenVat = c.GivenVat + db;
				r.TaxedTotal = c.TaxedTotal + db;

				r.HasVat = c.HasVat + db;
			};
		}

		public OrderItemDto[] ListByOrderNumber(string number)
		{
			return New(db.OrderItem.ListByOrderNumber(number));
		}

		public OrderItemDto[] ListByProducts(object[] productIds)
		{
			var docs = db.Product.ListByIds(productIds);

			if (docs.Count != productIds.Length)
				throw new ObjectsNotFoundException(productIds.Length == 1 ? Exceptions.NoRowById_Translation : Exceptions.ObjectsNotFound_Error);

			return docs
				.Where(a => a.Order != null)
				.Select(a => new OrderItemDto(a.Order, a))
				.ToArray();
		}


		public GenerateOrderItemsResponse Generate(object[] productIds, bool separateServiceFee, string orderId)
		{
			var order = orderId != null ? db.Order.By(orderId) : null;
			var products = db.Product.ListByIds(productIds);

			if (products.Count != productIds.Length)
				throw new ObjectsNotFoundException(productIds.Length == 1 ? Exceptions.NoRowById_Translation : Exceptions.ObjectsNotFound_Error);

			var dtos = new List<OrderItemDto>();
			var links = new List<OrderItemDto>();

			Party customer = null;

			foreach (var product in products)
			{
				if (product.Order == null)
				{
					if (dtos.Count == 0 || customer == null)
						customer = product.Customer;
					else if (!Equals(product.Customer, customer))
						customer = null;

					dtos.AddRange(New(db.OrderItem.New(product, separateServiceFee ? ServiceFeeMode.Separate : ServiceFeeMode.Join)));
				}
				else if (!Equals(product.Order, order))
					links.Add(new OrderItemDto(product.Order, product));
			}
			var response = new GenerateOrderItemsResponse
			{
				Customer = customer,
				Items = dtos.ToArray(),
				OrderItems = links.ToArray()
			};

			return response;
		}

	}


	[DataContract]
	public class GenerateOrderItemsResponse
	{
		public Party.Reference Customer { get; set; }

		public OrderItemDto[] Items { get; set; }

		public OrderItemDto[] OrderItems { get; set; }
	}

}