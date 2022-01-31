namespace Luxena.Travel
{

	public partial class IsicListTab
	{

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{

				se.Name,
				se.PnrCode.ToColumn(true, 150, se.GetNameRenderer()),
				se.TourCode.ToColumn(true),

				se.PassengerName,

				se.CardType,
				se.Number1,
				se.Number2,

			});
		}

	}

}