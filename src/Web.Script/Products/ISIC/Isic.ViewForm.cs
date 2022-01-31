


namespace Luxena.Travel
{

	public class IsicViewForm : ProductViewForm
	{
		public IsicViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(IsicViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.Isic; }
		protected override string GetClassTitle() { return DomainRes.Isic; }


		protected override string GetCommonDataHtml()
		{

			IsicSemantic v = new SemanticDomain(this).Isic;

			return

				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				v.Name.ToHtmlTr2(r, true) +
				GetPnrCodeAndTourCodeHtml() +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				GetPassengerHtml(r.Passenger, r.PassengerName) +

				GetCustomerAndIntermediaryHtml(r.Customer, r.Intermediary) +

				v.CardType.ToHtmlTr2(r, true) +
				v.Number1.ToHtmlTr2(r) +
				v.Number2.ToHtmlTr2(r) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>"
				
			;

		}


		private IsicDto r;
		public override ProductDto Product { get { return r; } set { r = (IsicDto)value; } }
	}

}