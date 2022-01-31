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
					se.PnrAndTourCodes,
					se.ReissueFor,
					se.PassengerRow,
					se.Provider,
			
					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,

					se.StartDate,

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}