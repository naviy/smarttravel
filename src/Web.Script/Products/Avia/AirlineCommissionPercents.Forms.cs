namespace Luxena.Travel
{

	public partial class AirlineCommissionPercentsListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Airline,

				se.Domestic,
				se.International,
				se.InterlineDomestic,
				se.InterlineInternational,
			});
		}

	}


	public partial class AirlineCommissionPercentsEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Airline,

				se.Domestic,
				se.International,
				se.InterlineDomestic,
				se.InterlineInternational,
			}));
		}

	}

}