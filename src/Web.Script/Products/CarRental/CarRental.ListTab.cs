using Ext.grid;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public partial class CarRentalListTab
	{

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);
			args.ForcedProperties = new string[]
			{
				"PassengerDtos", "RequiresProcessing", "IsVoid", "Name", "HotelOffice", "HotelCode", "PlacementOffice", "PlacementCode"
			};
		}

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{
				
				se.Name,

				se.PnrCode.ToColumn(true, 150, se.GetNameRenderer()),
				se.TourCode.ToColumn(true),

				se.PassengerName,

				se.Provider,
				se.StartDate,
				se.FinishDate,
				se.Country.ToColumn(true),

			});
		}

	}

}