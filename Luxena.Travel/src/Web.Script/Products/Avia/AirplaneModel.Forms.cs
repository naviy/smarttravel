namespace Luxena.Travel
{

	public partial class AirplaneModelListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name,

				se.IataCode,
				se.IcaoCode,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class AirplaneModelEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -1;
			Form.labelWidth = 150;

			Form.add(MainDataPanel(new object[]
			{
				se.Name,

				se.IataCode,
				se.IcaoCode,
			}));
		}

	}

}