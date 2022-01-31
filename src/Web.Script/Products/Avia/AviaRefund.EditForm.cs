namespace Luxena.Travel
{

	public partial class AviaRefundEditForm
	{

		public override bool IsRefund { get { return true; } }


		protected override void CreateControls()
		{
			Form.add(ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{
					se.IssueDate,
					se.FullNumber,
					se.PnrAndTourCodes,

					se.RefundedProduct,
					
					se.PassengerName,

					se.Provider.ToField(-3),
					se.CustomerAndOrder,
					se.Intermediary,
					se.Country,
					se.Originator,
					se.BookerRow,
					se.TicketerRow,
					se.TicketingIataOffice,
					se.SellerAndOwnerRow,
					se.LegalEntity,
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}