using System.ComponentModel.DataAnnotations.Schema;


namespace Luxena.Travel.Domain
{


	public enum OrderItemLinkType
	{
		ProductData = 0,
		ServiceFee = 1,
		FullDocument = 2
	}


	partial class OrderItem
	{
		[NotMapped]
		public bool IsFullDocument => LinkType == OrderItemLinkType.FullDocument;

		[NotMapped]
		public bool IsProductData => LinkType == OrderItemLinkType.ProductData;

		[NotMapped]
		public bool IsServiceFee => LinkType == OrderItemLinkType.ServiceFee;
	}

}
