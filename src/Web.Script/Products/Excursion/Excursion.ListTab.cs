namespace Luxena.Travel
{

	public partial class ExcursionListTab
	{

		protected override bool UseManyPassengers() { return true; }

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{

				se.Name,
				se.PnrCode.ToColumn(true, 150, se.GetNameRenderer()),
				se.TourCode.ToColumn(true),

				se.PassengerName,
				se.Country,
				se.TourName,
				se.StartDate,
				se.FinishDate,

			});
		}

	}

}