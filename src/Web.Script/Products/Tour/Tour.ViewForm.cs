


namespace Luxena.Travel
{

	public class TourViewForm : ProductViewForm
	{
		public TourViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(TourViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.Tour; }
		protected override string GetClassTitle() { return DomainRes.Tour; }


		protected override string GetCommonDataHtml()
		{
			TourSemantic v = new SemanticDomain(this).Tour;

			return 

				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				v.Name.ToHtmlTr2(r, true) +
				GetPnrCodeAndTourCodeHtml() +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				GetPassengersHtml(r.Passengers) +
				v.Provider.ToHtmlTr2(r) +

				GetCustomerAndIntermediaryHtml(r.Customer, r.Intermediary) +


				v.Country.ToHtmlTr2(r) +

				v.StartDate.ToHtmlTr2(r) +
				v.FinishDate.ToHtmlTr2(r) +

				GetHotelHtml(v.HotelName, r.HotelName, r.HotelOffice, r.HotelCode) +
				GetHotelHtml(v.PlacementName, r.PlacementName, r.PlacementOffice, r.PlacementCode) +

				v.AccommodationType.ToHtmlTr2(r) +
				v.CateringType.ToHtmlTr2(r) +


				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>"
			
			;

		}



		private TourDto r;
		public override ProductDto Product { get { return r; } set { r = (TourDto)value; } }
	}
}