using Ext.util;




namespace Luxena.Travel
{

	public class PasteboardViewForm : ProductViewForm
	{
		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(PasteboardViewForm), type, id, newTab);
		}

		public PasteboardViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		protected override string GetClassName() { return ClassNames.Pasteboard; }
		protected override string GetClassTitle() { return DomainRes.Pasteboard; }



		protected override string GetCommonDataHtml()
		{
			PasteboardSemantic v = new SemanticDomain(this).Pasteboard;

			return

				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				v.Number.ToHtmlTr2(r, true) +
				GetPnrCodeAndTourCodeHtml() +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +

				GetPassengerHtml(r.Passenger, r.PassengerName) +
				v.Provider.ToHtmlTr2(r) +

				@"<tr><td class='fieldLabel'>" + DomainRes.Common_Departure + @":</td><td class='fieldValue'>" + r.DeparturePlace + " / " + Format.date(r.DepartureDate, "d.m.Y") + " " +r.DepartureTime + @"</td></tr>" +
				@"<tr><td class='fieldLabel'>" + DomainRes.Common_Arrival + @":</td><td class='fieldValue'>" + r.ArrivalPlace + " / " + Format.date(r.ArrivalDate, "d.m.Y") + " " + r.ArrivalTime + @"</td></tr>" +
				v.ServiceClass.ToHtmlTr2(r) +

				v.TrainNumber.ToHtmlTr2(r) +
				v.CarNumber.ToHtmlTr2(r) +
				v.SeatNumber.ToHtmlTr2(r) +

				GetCustomerAndIntermediaryHtml(r.Customer, r.Intermediary) +

				GetOriginHtml(r) +

				GetBookerAndTicketerHtml(v, r) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +
																
				@"</table></div>"
				
			;

		}



		protected override void AddRefund()
		{
			StdAddRefund(r, "PasteboardRefund", new string[]
			{
				"PassengerName", "Passenger", "Number", 
				"DeparturePlace", "DepartureDate", "DepartureTime",
				"ArrivalPlace", "ArrivalDate", "ArrivalTime",
				"ServiceClass", "TrainNumber", "CarNumber", "SeatNumber",
			});
		}


		private PasteboardDto r;
		public override ProductDto Product { get { return r; } set { r = (PasteboardDto)value; } }

	}


	public class PasteboardRefundViewForm : PasteboardViewForm
	{
		public new static void ViewObject(string type, object id, bool newTab)
		{
			ViewProduct(typeof(PasteboardRefundViewForm), type, id, newTab);
		}

		public PasteboardRefundViewForm(string tabId, object id, string type) : base(tabId, id, type) { }

		protected override string GetClassName() { return ClassNames.PasteboardRefund; }

		protected override string GetClassTitle() { return DomainRes.PasteboardRefund; }

		public override bool IsRefund { get { return true; } }

	}

}