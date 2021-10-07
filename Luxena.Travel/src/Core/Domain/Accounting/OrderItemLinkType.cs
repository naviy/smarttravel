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
		public virtual bool IsFullDocument => LinkType == OrderItemLinkType.FullDocument;
		public virtual bool IsProductData => LinkType == OrderItemLinkType.ProductData;
		public virtual bool IsServiceFee => LinkType == OrderItemLinkType.ServiceFee;
	}

}