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

					se.RowPanel2v(se.IssueDate, null, se.ReissueFor, null),
					se.PnrAndTourCodes,

					se.PassengerRow,

					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,

					se.TourName,
					se.StartDate,
					se.FinishDate,

					se.SellerAndOwnerRow,
					se.LegalEntity,

				}), 

				se.Finance,

			}));


			Form.add(MainDataPanel(new object[] { se.Note }));

		}

	}

}