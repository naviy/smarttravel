namespace Luxena.Travel
{

	public partial class CurrencyListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name,
				se.Code,
				se.NumericCode,
				se.CyrillicCode,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class CurrencyEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
				se.Name,
				se.Code,
				se.NumericCode,
				se.CyrillicCode,
			}));
		}

	}

}