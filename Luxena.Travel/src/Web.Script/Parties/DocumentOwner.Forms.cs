namespace Luxena.Travel
{

	public partial class DocumentOwnerListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Owner,
				se.IsActive,
			});
		}

	}


	public partial class DocumentOwnerEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -3;
			Form.add(MainDataPanel(new object[]
			{
				se.Owner.ToField(-3),
				se.IsActive,
			}));
		}

	}

}