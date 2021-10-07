namespace Luxena.Travel
{

	public partial class DocumentAccessListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Person,
				se.Owner,
				se.FullDocumentControl,
			});
		}

	}


	public partial class DocumentAccessEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -3;
			Form.add(MainDataPanel(new object[]
			{
				se.Person.ToField(-3),
				se.Owner.ToField(-3),
				se.FullDocumentControl,
			}));
		}

	}

}