using Ext.grid;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public partial class InsuranceListTab
	{

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);
			args.ForcedProperties = new string[]
			{
				"PassengerDtos", "RequiresProcessing", "IsVoid", "Name"
			};
		}

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.PassengerName,
				se.Producer,
				se.Provider,
				se.Number,
				se.StartDate,
				se.FinishDate,
				se.Country.ToColumn(true),
				se.PnrCode.ToColumn(true),
				se.TourCode.ToColumn(true),
			});
		}

	}

}