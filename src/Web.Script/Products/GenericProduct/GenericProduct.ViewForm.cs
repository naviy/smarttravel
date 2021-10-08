


namespace Luxena.Travel
{

	public class GenericProductViewForm : ProductViewForm
	{
		public GenericProductViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(GenericProductViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.GenericProduct; }
		protected override string GetClassTitle() { return DomainRes.GenericProduct; }

		protected override string GetCommonDataHtml()
		{
			GenericProductSemantic v = new SemanticDomain(this).GenericProduct;

			string commonDataHtml = 
				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				v.Name.ToHtmlTr2(r, true) +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				GetPassengersHtml(r.Passengers) +
				v.Provider.ToHtmlTr2(r) +

				v.StartDate.ToHtmlTr2(r) +
				v.FinishDate.ToHtmlTr2(r) +

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


		private GenericProductDto r;
		public override ProductDto Product { get { return r; } set { r = (GenericProductDto)value; } }
	}
}