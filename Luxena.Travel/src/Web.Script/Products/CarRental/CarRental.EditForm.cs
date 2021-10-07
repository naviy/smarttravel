namespace Luxena.Travel
{

	public partial class CarRentalEditForm
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

					se.RowPanel2v(se.StartDate, null, se.FinishDate, null),
			
					se.CustomerAndOrder,
					se.Intermediary,

					se.CarBrand,

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