using Ext;


namespace Luxena.Travel
{

	public partial class IsicEditForm
	{

		protected override string GetNameBy(object data)
		{
			IsicDto dto = (IsicDto)data;
			return dto.Number1 + " " + dto.Number2;
		}



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

					se.CardType,

					RowPanel(new Component[]
					{
						se.Number1.ToField(114),
						se.Number2.ToField(40, delegate(FormMember m) { m.HideLabel(); }),
					}),

					se.SellerAndOwnerRow,
					se.LegalEntity,

				}),

				se.Finance,

			}));


			Form.add(MainDataPanel(new object[] { se.Note }));

		}

	}

}