


namespace Luxena.Travel
{

	public class InsuranceViewForm : ProductViewForm
	{
		public InsuranceViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(InsuranceViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.Insurance; }
		protected override string GetClassTitle() { return DomainRes.Insurance; }

		protected override string GetCommonDataHtml()
		{
			InsuranceSemantic v = new SemanticDomain(this).Insurance;

			string commonDataHtml = 
				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				v.Name.ToHtmlTr2(r, true) +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				GetPassengersHtml(r.Passengers) +
				v.Producer.ToHtmlTr2(r) +
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


		protected override void AddRefund()
		{
			StdAddRefund(r, "InsuranceRefund", new string[]
			{
				"PassengerName", "Passengers", "Producer", "Provider", 
				"Number", "StartDate", "FinishDate",
				"Country",
			});
		}
		

		private InsuranceDto r;
		public override ProductDto Product { get { return r; } set { r = (InsuranceDto)value; } }
	}


	public class InsuranceRefundViewForm : InsuranceViewForm
	{
		public new static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(InsuranceRefundViewForm), type, id, newTab);
		}

		public InsuranceRefundViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		protected override string GetClassName() { return ClassNames.InsuranceRefund; }

		protected override string GetClassTitle() { return DomainRes.InsuranceRefund; }

		public override bool IsRefund { get { return true; } }

	}

}