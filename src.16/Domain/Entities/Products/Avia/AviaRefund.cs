using System.Data.Entity;


namespace Luxena.Travel.Domain
{

	[RU("Возврат авиабилета", "Возвраты авиабилетов")]
	[UA("Повернення авіаквитка")]
	public partial class AviaRefund : AviaDocument
	{
		public override ProductType Type => ProductType.AviaRefund;

		public override bool IsRefund => true;

		[RU("Исходный документ")]
		public AviaDocument RefundedDocument => RefundedProduct as AviaDocument;


		public override string GetOrderItemText(string lang) => 
			Localization(lang) +
			GetOrderItemText2(lang) +
			(RefundedProduct as AviaTicket)?.GetItinerary(true, true, true).As(a => $" {Texts.Itinerary[lang]} {a}");
	}


	partial class Domain
	{
		public DbSet<AviaRefund> AviaRefunds { get; set; }
	}

}