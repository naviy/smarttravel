


namespace Luxena.Travel
{

	public class CarRentalViewForm : ProductViewForm
	{
		public CarRentalViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(CarRentalViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.CarRental; }
		protected override string GetClassTitle() { return DomainRes.CarRental; }

		protected override string GetCommonDataHtml()
		{
			CarRentalSemantic v = new SemanticDomain(this).CarRental;

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

				v.CarBrand.ToHtmlTr2(r) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>"
				
			;

		}


		private CarRentalDto r;
		public override ProductDto Product { get { return r; } set { r = (CarRentalDto)value; } }
	}
}