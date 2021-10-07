namespace Luxena.Travel
{

	public partial class ProductPassengerListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Product,
				se.PassengerName,
				se.Passenger,
			});
		}

	}

}