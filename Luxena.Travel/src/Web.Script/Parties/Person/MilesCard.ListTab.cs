namespace Luxena.Travel
{

	public partial class MilesCardListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Owner,
				se.Number,
				se.Organization,
			});
		}

	}

}