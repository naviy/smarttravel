namespace Luxena.Travel
{

	public partial class TransferListTab
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
				se.Provider,
				se.Country.ToColumn(true),
				se.StartDate,

			});
		}

	}

}