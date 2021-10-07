namespace Luxena.Travel
{

	public partial class InternalTransferListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Number,
				se.Date,
				se.FromOrder,
				se.FromParty,
				se.ToOrder,
				se.ToParty,
				se.Amount,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class InternalTransferEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Date,
				EmptyRow(),
				se.FromOrder,
				se.FromParty.ToField(-3),
				se.ToOrder,
				se.ToParty.ToField(-3),
				EmptyRow(),
				se.Amount,
			}));
		}

	}

}