using System;

using Luxena.Travel.Services;


namespace Luxena.Travel
{

	public partial class ClosedPeriodListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.DateFrom,
				se.DateTo,
				se.PeriodState,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class ClosedPeriodEditForm
	{
		protected override void Initialize()
		{
			ClosedPeriodService.GetLastClosedPeriod(delegate(object result)
			{
				if (!Script.IsValue(result)) return;

				Date lastDateTo = ((ClosedPeriodDto)result).DateTo;
				lastDateTo.SetDate(lastDateTo.GetDate() + 1);
				SetCurrentValue("DateFrom", lastDateTo);
			}, null);
		}

		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
				se.DateFrom,
				se.DateTo,
				se.PeriodState,
			}));
		}

	}

}