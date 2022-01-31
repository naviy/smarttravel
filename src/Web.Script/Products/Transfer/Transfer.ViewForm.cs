


namespace Luxena.Travel
{

	public class TransferViewForm : ProductViewForm
	{
		public TransferViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(TransferViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.Transfer; }
		protected override string GetClassTitle() { return DomainRes.Transfer; }

		protected override string GetCommonDataHtml()
		{

			TransferSemantic v = new SemanticDomain(this).Transfer;

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
				v.StartDate.ToHtmlTr2(r) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>"
				
			;

		}


		private TransferDto r;
		public override ProductDto Product { get { return r; } set { r = (TransferDto)value; } }
	}
}