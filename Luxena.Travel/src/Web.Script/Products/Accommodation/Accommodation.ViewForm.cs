


namespace Luxena.Travel
{

	public class AccommodationViewForm : ProductViewForm
	{
		public AccommodationViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(AccommodationViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.Accommodation; }
		protected override string GetClassTitle() { return DomainRes.Accommodation; }

		protected override string GetCommonDataHtml()
		{
			AccommodationSemantic v = new SemanticDomain(this).Accommodation;

			string commonDataHtml = 
				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				v.StartDate.ToHtmlTr2(r) +
				v.FinishDate.ToHtmlTr2(r) +

				v.Name.ToHtmlTr2(r, true) +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				GetPassengersHtml(r.Passengers) +
				v.Provider.ToHtmlTr2(r) +

				GetCustomerAndIntermediaryHtml(r.Customer, r.Intermediary) +

				v.Country.ToHtmlTr2(r) +
				v.TourCode.ToHtmlTr2(r) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>";

			return commonDataHtml;
		}


		private AccommodationDto r;
		public override ProductDto Product { get { return r; } set { r = (AccommodationDto)value; } }
	}
}