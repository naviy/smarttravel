


namespace Luxena.Travel
{

	public class ExcursionViewForm : ProductViewForm
	{
		public ExcursionViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(ExcursionViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.Excursion; }
		protected override string GetClassTitle() { return DomainRes.Excursion; }



		protected override string GetCommonDataHtml()
		{

			ExcursionSemantic v = new SemanticDomain(this).Excursion;

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

				GetCustomerAndIntermediaryHtml(r.Customer, r.Intermediary) +

				v.Country.ToHtmlTr2(r) +

				v.TourName.ToHtmlTr2(r) +
				v.StartDate.ToHtmlTr2(r) +
				v.FinishDate.ToHtmlTr2(r) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>"
			;

		}



		private ExcursionDto r;
		public override ProductDto Product { get { return r; } set { r = (ExcursionDto)value; } }
	}
}