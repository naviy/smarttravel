namespace Luxena.Travel
{

	public partial class AirlineMonthCommissionEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
				se.Airline,
				se.DateFrom,
				se.CommissionPc,
			}));
		}

	}

}