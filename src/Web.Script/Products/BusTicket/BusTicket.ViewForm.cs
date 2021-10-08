


namespace Luxena.Travel
{

	public class BusTicketViewForm : ProductViewForm
	{
		public BusTicketViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(BusTicketViewForm), type, id, newTab);
		}

		protected override string GetClassName() { return ClassNames.BusTicket; }
		protected override string GetClassTitle() { return DomainRes.BusTicket; }


		protected override string GetCommonDataHtml()
		{
			BusTicketSemantic v = new SemanticDomain(this).BusTicket;

			string commonDataHtml =
				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +


				v.Number.ToHtmlTr2(r, true) +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				GetPassengerHtml(r.Passenger, r.PassengerName) +
				v.Provider.ToHtmlTr2(r) +

				GetCustomerAndIntermediaryHtml(r.Customer, r.Intermediary) +

				GetOriginHtml(r) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +

				@"</table></div>";

			return commonDataHtml;
		}


		protected override void AddRefund()
		{
			StdAddRefund(r, "BusTicketRefund", new string[]
			{
				"PassengerName", "Passenger",  "Provider", "Number",
				"DeparturePlace", "DepartureDate", "DepartureTime",
				"ArrivalPlace", "ArrivalDate", "ArrivalTime",
				"SeatNumber",
			});
		}


		private BusTicketDto r;
		public override ProductDto Product { get { return r; } set { r = (BusTicketDto)value; } }
	}


	public class BusTicketRefundViewForm : BusTicketViewForm
	{
		public new static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(BusTicketRefundViewForm), type, id, newTab);
		}

		public BusTicketRefundViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		protected override string GetClassName() { return ClassNames.BusTicketRefund; }

		protected override string GetClassTitle() { return DomainRes.BusTicketRefund; }

		public override bool IsRefund { get { return true; } }

	}

}