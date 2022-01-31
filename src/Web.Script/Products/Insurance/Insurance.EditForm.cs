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

					se.RowPanel2v(se.IssueDate, null, se.ReissueFor, null),
					se.Number,
					se.PnrAndTourCodes,

					IsRefund ? se.RefundedProduct : null,

					se.PassengerRow,

					se.Producer,
//					se.Provider,

					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,
					se.RowPanel2v(se.StartDate, null, se.FinishDate, null),

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