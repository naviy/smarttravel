namespace Luxena.Travel
{

	public partial class SimCardEditForm
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

					se.Number,
					se.Producer,
					se.IsSale,
				
					se.CustomerAndOrder,
					se.Intermediary,

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}