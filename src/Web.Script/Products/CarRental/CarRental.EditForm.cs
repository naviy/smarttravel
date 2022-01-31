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
					se.RowPanel2v(se.IssueDate, null, se.ReissueFor, null),

					se.PnrAndTourCodes,

					se.PassengerRow,

					se.Provider,

					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,

					se.RowPanel2v(se.StartDate, null, se.FinishDate, null),
					se.CarBrand,

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}),

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}