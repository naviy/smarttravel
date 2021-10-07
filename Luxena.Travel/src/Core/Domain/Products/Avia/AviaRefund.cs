using System;


namespace Luxena.Travel.Domain
{

	public partial class AviaRefund : AviaDocument
	{

		public override ProductType Type => ProductType.AviaRefund;

		public override bool IsRefund => true;

		[RU("Исходный документ")]
		public virtual AviaDocument RefundedDocument => RefundedProduct as AviaDocument;

		public override string Itinerary
		{
			get { return RefundedDocument?.Itinerary; }
			set { }
		}

		public override string GetItinerary(Func<Airport, string> airportToString, bool withSpaces, bool withDates)
		{
			return RefundedDocument?.GetItinerary(airportToString, withSpaces, withDates);
		}


		public override Entity Resolve(Domain db)
		{
			base.Resolve(db);
			var r = this;

			r.RefundedProduct = r.RefundedDocument != null
				? db.AviaDocument.ByNumber(r.RefundedDocument)
				: db.AviaDocument.ByFullNumber(r.Number);

			return r;
		}


		public new class Service : Service<AviaRefund>
		{

		}
		
	}

}