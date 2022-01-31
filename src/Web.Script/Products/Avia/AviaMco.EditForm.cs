namespace Luxena.Travel
{

	public partial class AviaMcoEditForm
	{

		protected override void CreateControls()
		{
			Form.add(ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{
					se.IssueDate,
					se.FullNumber,
					se.PnrAndTourCodes,

					se.ReissueFor,
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
					se.Description.ToField(350, delegate(FormMember m) { m.Height(35); }),
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}