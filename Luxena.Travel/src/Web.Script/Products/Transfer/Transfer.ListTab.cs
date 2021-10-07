namespace Luxena.Travel
{

	public partial class TransferListTab
	{

		protected override bool UseManyPassengers() { return true; }

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.PassengerName,
				se.Provider,
				se.StartDate,
				se.Country.ToColumn(true),
				se.PnrCode.ToColumn(true),
				se.TourCode.ToColumn(true),
			});
		}

	}

}