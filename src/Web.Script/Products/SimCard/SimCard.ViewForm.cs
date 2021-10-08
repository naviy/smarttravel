


namespace Luxena.Travel
{

	public class SimCardViewForm : ProductViewForm
	{
		public SimCardViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(SimCardViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.SimCard; }
		protected override string GetClassTitle() { return DomainRes.SimCard; }


		protected override string GetCommonDataHtml()
		{
			SimCardSemantic v = new SemanticDomain(this).SimCard;

			string commonDataHtml = 
				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				v.Number.ToHtmlTr2(r, true) +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				v.Producer.ToHtmlTr2(r, true) +

				v.IsSale.ToHtmlTr2(r) +

				GetPassengerHtml(r.Passenger, r.PassengerName) +

				GetCustomerAndIntermediaryHtml(r.Customer, r.Intermediary) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>";

			return commonDataHtml;
		}


		private SimCardDto r;
		public override ProductDto Product { get { return r; } set { r = (SimCardDto)value; } }
	}

}