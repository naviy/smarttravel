namespace Luxena.Travel
{

	public partial class AirportListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name,

				se.Code,
				se.Country,
				se.Settlement,
				se.LocalizedSettlement,
				se.Latitude.ToColumn(true),
				se.Longitude.ToColumn(true),

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class AirportEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -1;
			Form.labelWidth = 150;

			Form.add(MainDataPanel(new object[]
			{
				se.Name,

				se.Code,
				se.Country,
				se.Settlement,
				se.LocalizedSettlement,
				se.Latitude,
				se.Longitude,
			}));
		}

	}

}