namespace Luxena.Travel
{

	public partial class CurrencyDailyRateListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Date,

				se.UAH_EUR,
				se.UAH_RUB,
				se.UAH_USD,
				se.RUB_EUR,
				se.RUB_USD,
				se.EUR_USD,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}

}