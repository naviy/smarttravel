namespace Luxena.Travel
{


	public partial class UserVisitListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.StartDate.ToColumn(false),
				se.User,
				se.IP,
				se.SessionId,
				se.Request,
			});
		}

	}



	public partial class UserVisitEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -4;
			Form.add(MainDataPanel(new object[]
			{
				se.StartDate,
				se.User,
				se.IP,
				se.SessionId,
				se.Request,
			}));
		}

	}



}