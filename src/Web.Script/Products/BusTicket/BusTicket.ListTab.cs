namespace Luxena.Travel
{

	public partial class BusTicketListTab
	{

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

				se.DeparturePlace,
				se.DepartureDate.ToColumn(true),
				se.DepartureTime.ToColumn(true),
				se.ArrivalPlace,
				se.ArrivalDate.ToColumn(true),
				se.ArrivalTime.ToColumn(true),

				se.SeatNumber.ToColumn(true),
			});
		}

	}

}