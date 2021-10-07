namespace Luxena.Travel
{

	public partial class InsuranceEditForm
	{

		protected override void CreateControls()
		{
			Form.add(ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{
					se.IssueDate,

					se.ReissueFor,
					IsRefund ? se.RefundedProduct : null,
					se.PassengerRow,

					se.Producer,
//					se.Provider,
					se.Number,

					se.RowPanel2v(se.StartDate, null, se.FinishDate, null),
			
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


	public class InsuranceRefundEditForm : InsuranceEditForm
	{
		static InsuranceRefundEditForm()
		{
			RegisterEdit(ClassNames.InsuranceRefund, typeof(InsuranceRefundEditForm));
		}

		protected override string ClassName { get { return ClassNames.InsuranceRefund; } }

		public override bool IsRefund { get { return true; } }
	}


}