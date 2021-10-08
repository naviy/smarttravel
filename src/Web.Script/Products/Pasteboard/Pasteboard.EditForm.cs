using Ext;


namespace Luxena.Travel
{

	public partial class PasteboardEditForm
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
					se.Provider.ToField(-3),

					se.Number,

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

					se.ServiceClass.ToField(-2),
					se.RowPanel3c(
						"Поезд / вагон / место",
						se.TrainNumber, null,
						se.CarNumber, null,
						se.SeatNumber, null
					),

					se.CustomerAndOrder,
					se.Intermediary,

					se.Originator,
					se.BookerRow,
					se.BookingFee,
					se.TicketerRow,

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}),

				se.Finance
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}


	public class PasteboardRefundEditForm : PasteboardEditForm
	{
		static PasteboardRefundEditForm()
		{
			RegisterEdit(ClassNames.PasteboardRefund, typeof(PasteboardRefundEditForm));
		}

		protected override string ClassName { get { return ClassNames.PasteboardRefund; } }

		public override bool IsRefund { get { return true; } }
	}

}