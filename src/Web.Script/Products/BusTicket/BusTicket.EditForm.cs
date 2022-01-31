using Ext;


namespace Luxena.Travel
{

	public partial class BusTicketEditForm
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

					se.Provider,

					RowPanel(new Component[]
					{
						se.DeparturePlace.ToField(-1, delegate(FormMember m) { m.Label(DomainRes.Common_Departure); }),
						TextComponent("/"),
						se.DepartureDate.ToField(-2, delegate(FormMember m) { m.HideLabel(); }),
						se.DepartureTime.ToField(48, delegate(FormMember m) { m.HideLabel(); }),
					}),

					RowPanel(new Component[]
					{
						se.ArrivalPlace.ToField(-1, delegate(FormMember m) { m.Label(DomainRes.Common_Arrival); }),
						TextComponent("/"),
						se.ArrivalDate.ToField(-2, delegate(FormMember m) { m.HideLabel(); }),
						se.ArrivalTime.ToField(48, delegate(FormMember m) { m.HideLabel(); }),
					}),

					se.SeatNumber.ToField(-2),

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


	public class BusTicketRefundEditForm : BusTicketEditForm
	{
		static BusTicketRefundEditForm()
		{
			RegisterEdit(ClassNames.BusTicketRefund, typeof(BusTicketRefundEditForm));
		}

		protected override string ClassName { get { return ClassNames.BusTicketRefund; } }

		public override bool IsRefund { get { return true; } }
	}

}