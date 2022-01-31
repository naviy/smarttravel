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
					se.RowPanel2v(se.IssueDate, null, se.ReissueFor, null),
					se.Number,
					se.PnrAndTourCodes,

					se.ReissueFor,
					se.PassengerRow,

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