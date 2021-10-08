using System;
using System.Collections.Generic;

using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class ConsignmentItemsDto
	{

		public DateTime IssueDate { get; set; }

		public EntityReference Supplier { get; set; }

		public EntityReference Acquirer { get; set; }

		public MoneyDto Discount { get; set; }

		public OrderItemDto[] Items { get; set; } 

	
		public ConsignmentItemsDto()
		{
		}

		public ConsignmentItemsDto(Contracts dc, IList<OrderItem> items, Party acquirer, Party supplier, Money discount, DateTime issueDate)
		{
			IssueDate = issueDate;

			Supplier = supplier;
			Acquirer = acquirer;

			Discount = discount;

			Items = dc.OrderItem.New(items);
		}

		public static ConsignmentItemsDto New(Domain db, Contracts dc, object[] orderIds)
		{
			var orders = db.Order.ListByIds(orderIds);
			var orderItems = new List<OrderItem>();

			var maxDate = DateTime.MinValue;
			var acquirer = orders[0].ShipTo ?? orders[0].Customer;
			var supplier = db.Configuration.Company;
			Money discount = null;

			foreach (var order in orders)
			{
				foreach (var item in order.Items)
				{
					if (order.IsVoid || item.Consignment != null) continue;

					orderItems.Add(item);

					if (item.Product != null && item.Product.IssueDate > maxDate)
						maxDate = item.Product.IssueDate;
				}

				discount += order.Discount.Clone();
			}

			if (maxDate == DateTime.MinValue)
				maxDate = DateTime.Today;

			return new ConsignmentItemsDto(dc, orderItems, acquirer, supplier, discount, maxDate);
		}

	}

}