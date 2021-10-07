namespace Luxena.Travel
{

	public partial class AirlineServiceClassEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
				se.Airline,
				se.Code,
				se.ServiceClass,
			}));
		}

	}

}