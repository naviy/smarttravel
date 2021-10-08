namespace Luxena.Travel
{

	public partial class AirlineMonthCommissionListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.DateFrom.ToColumn(false, 80),
				se.DateTo.ToColumn(true, 80),
				se.Airline,
				se.CommissionPc.ToColumn(false, 80),

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}

}