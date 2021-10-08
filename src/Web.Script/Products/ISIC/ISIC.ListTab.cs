namespace Luxena.Travel
{

	public partial class IsicListTab
	{

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.CardType,
				se.Number1,
				se.Number2,
				se.PassengerName,
			});
		}

	}

}