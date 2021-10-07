namespace Luxena.Travel
{

	public partial class TourEditForm
	{

		protected override void CreateControls()
		{
			Form.add(ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{
					se.RowPanel2v(se.IssueDate, null, se.ReissueFor, null),
					se.PassengerRow,
					se.Provider,

					se.RowPanel2v(se.StartDate, null, se.FinishDate, null),
			
					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,
					se.PnrAndTourCodes,
					se.HotelRow,
					se.PlacementRow,

					se.RowPanel2(se.AccommodationType, null, se.CateringType, null),

					se.AviaDescription,
					se.TransferDescription,

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}