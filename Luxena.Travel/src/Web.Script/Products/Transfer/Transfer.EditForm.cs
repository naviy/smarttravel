namespace Luxena.Travel
{

	public partial class TransferEditForm
	{

		protected override void CreateControls()
		{
			Form.add(ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{
					se.IssueDate,
					se.ReissueFor,
					se.PassengerRow,
					se.Provider,

					se.StartDate,
			
					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,
					se.PnrAndTourCodes,

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}