namespace Luxena.Travel
{

	public partial class ExcursionListTab
	{

		protected override bool UseManyPassengers() { return true; }

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.PassengerName,
				se.StartDate,
				se.FinishDate,
				se.TourName,
				se.Country,
				se.TourCode.ToColumn(true),
			});
		}

	}

}