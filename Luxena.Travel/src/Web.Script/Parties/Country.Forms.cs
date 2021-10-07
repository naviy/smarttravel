namespace Luxena.Travel
{

	public partial class CountryListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name.ToColumn(false, 150),
				se.TwoCharCode.ToColumn(false, 80),
				se.ThreeCharCode.ToColumn(false, 80),
				se.Note.ToColumn(false, 500),

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class CountryEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -3;

			Form.add(MainDataPanel(new object[]
			{
				se.Name.ToField(-3),
				se.TwoCharCode,
				se.ThreeCharCode,

				EmptyRow(),

				se.Note.ToField(-3, delegate(FormMember m)
				{
					m.Height(200);
				}),
			}));
		}

	}

}