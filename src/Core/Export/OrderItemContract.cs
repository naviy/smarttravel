using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract(Name = "Item")]
	public class OrderItemContract
	{

		public OrderItemContract(OrderItem r)
		{
			Description = r.Text;
			Price = r.Price;
			Quantity = r.Quantity;
			Discount = r.Discount;
			Total = r.GrandTotal;
			ProvidedVat = r.GivenVat;
			AmountToVat = r.TaxedTotal;
			Source = r.Product;
			LinkType = r.LinkType;
		}


		[DataMember] public string Description { get; set; }
		[DataMember] public MoneyContract Price { get; set; }
		[DataMember] public int Quantity { get; set; }
		[DataMember] public MoneyContract Discount { get; set; }
		[DataMember] public MoneyContract Total { get; set; }
		[DataMember] public MoneyContract ProvidedVat { get; set; }
		[DataMember] public MoneyContract AmountToVat { get; set; }
		[DataMember] public ProductReference Source { get; set; }
		[DataMember] public OrderItemLinkType? LinkType { get; set; }

	}

}
