namespace Luxena.Travel
{

	public partial class BankAccountListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name,
				se.Description,
				se.IsDefault.ToColumn(false, 90),
				se.Note.ToColumn(true),

				se.CreatedOn.ToColumn(true),
				se.CreatedBy.ToColumn(true),
				se.ModifiedOn.ToColumn(true),
				se.ModifiedBy.ToColumn(true),
			});
		}

	}

}