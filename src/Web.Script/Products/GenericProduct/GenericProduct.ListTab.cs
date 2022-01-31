using Ext.grid;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public partial class GenericProductListTab
	{

		protected override bool UseManyPassengers() { return true; }


		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);
			args.ForcedProperties = new string[]
			{
				"RequiresProcessing", "IsVoid", "Name", 
			};
		}

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name,
				se.Number.ToColumn(true, 150, se.GetNameRenderer()),
				se.PnrCode.ToColumn(true, 150, se.GetNameRenderer()),
				se.TourCode.ToColumn(true),

				se.PassengerName,
				se.Provider,
				se.GenericType,
				se.StartDate,
				se.FinishDate,
				se.Country.ToColumn(true),
			});
		}

	}

}