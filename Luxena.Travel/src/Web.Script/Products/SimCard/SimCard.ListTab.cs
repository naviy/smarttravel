namespace Luxena.Travel
{

	public partial class SimCardListTab
	{

		protected override void CreateCustomColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Number,
				se.Producer,
				se.PassengerName,
			});
		}

	}

}