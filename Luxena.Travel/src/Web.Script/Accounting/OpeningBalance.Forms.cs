namespace Luxena.Travel
{

	public partial class OpeningBalanceListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Number,
				se.Date,
				se.Party,
				se.Balance,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class OpeningBalanceEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Number,
				se.Date,
				se.Party.ToField(-3),
				se.Balance,
			}));
		}

	}

}