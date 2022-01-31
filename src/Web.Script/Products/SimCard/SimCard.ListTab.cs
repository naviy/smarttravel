namespace Luxena.Travel
{

	public partial class SimCardListTab
	{

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{

				se.Name,
				se.Number.ToColumn(true, 150, se.GetNameRenderer()),
				se.PnrCode.ToColumn(true, 150, se.GetNameRenderer()),
				se.TourCode.ToColumn(true),

				se.Producer,
				se.PassengerName,
			});
		}

	}

}