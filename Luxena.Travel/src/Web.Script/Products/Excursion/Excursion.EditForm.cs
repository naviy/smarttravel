namespace Luxena.Travel
{

	public partial class ExcursionEditForm
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

					se.StartDate,
					se.FinishDate,
				
					se.CustomerAndOrder,
					se.Intermediary,

					se.TourName,
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